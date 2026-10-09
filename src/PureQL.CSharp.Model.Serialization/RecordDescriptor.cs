using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>A record written as one JSON object.</summary>
internal sealed class RecordDescriptor<T>(
    ModelRegistry registry,
    IEnumerable<string> signatures,
    string? operatorName,
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
        }

        T value = read(reader);
        reader.EnsureAllRead();
        return value;
    }

    public override void Write(Utf8JsonWriter writer, object value)
    {
        writer.WriteStartObject();
        if (operatorName is not null)
        {
            writer.WriteString("operator", operatorName);
        }

        write(new ObjectWriter(Registry, writer), (T)value);
        writer.WriteEndObject();
    }
}
