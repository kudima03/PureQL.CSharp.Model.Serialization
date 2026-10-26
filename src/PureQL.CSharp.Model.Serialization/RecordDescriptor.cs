using System.Text.Json;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>A record written as one JSON object.</summary>
internal sealed class RecordDescriptor<T>(
    ModelRegistry registry,
    IEnumerable<string> signatures,
    string? operatorName,
    IType? operatorType,
    NullableCause nullableCause,
    Func<ObjectReader, T> read,
    Action<ObjectWriter, T> write
) : ModelDescriptor(registry, typeof(T))
    where T : class
{
    private readonly HashSet<string> _signatures = new HashSet<string>(
        signatures,
        StringComparer.Ordinal
    );

    public override IReadOnlyCollection<string> Signatures => _signatures;

    public override object Read(JsonElement element, string path)
    {
        if (_signatures.Count > 0)
        {
            string signature = Discriminators.Of(element, path);
            if (!_signatures.Contains(signature))
            {
                throw JsonErrors.At(path, $"{Type.Name} does not accept '{signature}'");
            }
        }

        ObjectReader reader = new ObjectReader(Registry, element, path);
        if (operatorName is not null)
        {
            reader.Skip("operator");
            reader.Skip("type");
        }

        T value = read(reader);
        reader.EnsureAllRead();
        EnsureNullableCause(element, path);
        return value;
    }

    public override void Write(Utf8JsonWriter writer, object value)
    {
        writer.WriteStartObject();
        if (operatorName is not null)
        {
            writer.WriteString("operator", operatorName);
        }

        ObjectWriter objectWriter = new ObjectWriter(Registry, writer);
        if (operatorType is not null)
        {
            objectWriter.Type(operatorType);
        }

        write(objectWriter, (T)value);
        writer.WriteEndObject();
    }

    /// <summary>
    /// Rejects a nullable declaration that the operands of the node do not give it: it
    /// is wider than the exact type the schema requires.
    /// </summary>
    private void EnsureNullableCause(JsonElement element, string path)
    {
        if (
            operatorType is not { Nullable: true }
            || (nullableCause == NullableCause.Always)
        )
        {
            return;
        }

        bool[] operands =
        [
            .. Discriminators.Operands(element).Select(Discriminators.IsNullable),
        ];
        bool nullable =
            nullableCause == NullableCause.EveryOperand
                ? operands.All(o => o)
                : operands.Any(o => o) || element.TryGetProperty("predicate", out _);
        if (!nullable)
        {
            throw JsonErrors.At(
                path,
                $"{Type.Name} is declared nullable, but its operands make it non-null"
            );
        }
    }
}
