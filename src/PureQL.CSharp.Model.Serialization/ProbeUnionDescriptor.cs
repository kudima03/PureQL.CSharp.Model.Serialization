using System.Text.Json;
using OneOf;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// A union whose cases differ only by operand type, such as the variants of
/// <c>equal</c>. The case is chosen by the family that <c>probe.*</c> reports for the
/// operand, or for the node itself when no operand is named (<c>orderBy</c> keys).
/// </summary>
internal sealed class ProbeUnionDescriptor<TUnion>(
    ModelRegistry registry,
    string? operand,
    IReadOnlyList<UnionCase<TUnion>> cases
) : ModelDescriptor(registry, typeof(TUnion))
    where TUnion : class
{
    private readonly Dictionary<TypeFamily, UnionCase<TUnion>> _byFamily =
        cases.ToDictionary(unionCase => unionCase.Family!.Value);

    private readonly HashSet<string> _signatures = new HashSet<string>(
        StringComparer.Ordinal
    );

    private bool _sealed;

    public override IReadOnlyCollection<string> Signatures => _signatures;

    public override void Seal()
    {
        if (_sealed)
        {
            return;
        }

        _sealed = true;
        foreach (UnionCase<TUnion> unionCase in cases)
        {
            ModelDescriptor descriptor = Registry.Find(unionCase.Type);
            descriptor.Seal();
            _signatures.UnionWith(descriptor.Signatures);
        }
    }

    public override object Read(JsonElement element, string path)
    {
        JsonElement target = element;
        string targetPath = path;
        if (operand is not null)
        {
            targetPath = $"{path}.{operand}";
            if (
                (element.ValueKind != JsonValueKind.Object)
                || !element.TryGetProperty(operand, out target)
            )
            {
                throw JsonErrors.At(path, $"Missing property '{operand}'");
            }
        }

        TypeFamily family =
            Discriminators.Probe(target)
            ?? throw JsonErrors.At(
                targetPath,
                $"Cannot determine the type for {Type.Name}"
            );
        return _byFamily.TryGetValue(family, out UnionCase<TUnion>? unionCase)
            ? unionCase.Create(Registry.Find(unionCase.Type).Read(element, path))
            : throw JsonErrors.At(targetPath, $"{Type.Name} does not accept {family}");
    }

    public override void Write(Utf8JsonWriter writer, object value)
    {
        Registry.Write(writer, ((IOneOf)value).Value);
    }
}
