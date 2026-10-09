using System.Text.Json;
using OneOf;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// A union whose cases accept disjoint signatures, so a node selects its case by its
/// <c>operator</c> or, for a leaf, by its shape and declared type.
/// </summary>
internal sealed class UnionDescriptor<TUnion>(
    ModelRegistry registry,
    IReadOnlyList<UnionCase<TUnion>> cases
) : ModelDescriptor(registry, typeof(TUnion))
    where TUnion : class
{
    private readonly Dictionary<string, UnionCase<TUnion>> _bySignature = new Dictionary<
        string,
        UnionCase<TUnion>
    >(StringComparer.Ordinal);

    private bool _sealed;

    public override IReadOnlyCollection<string> Signatures => _bySignature.Keys;

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
            foreach (string signature in descriptor.Signatures)
            {
                if (!_bySignature.TryAdd(signature, unionCase))
                {
                    string first = _bySignature[signature].Type.Name;
                    throw new InvalidOperationException(
                        $"{Type.Name}: '{signature}' matches {first} and {unionCase.Type.Name}"
                    );
                }
            }
        }
    }

    public override object Read(JsonElement element, string path)
    {
        string signature = Discriminators.Of(element, path);
        return _bySignature.TryGetValue(signature, out UnionCase<TUnion>? unionCase)
            ? unionCase.Create(Registry.Find(unionCase.Type).Read(element, path))
            : throw JsonErrors.At(path, $"{Type.Name} does not accept '{signature}'");
    }

    public override void Write(Utf8JsonWriter writer, object value)
    {
        Registry.Write(writer, ((IOneOf)value).Value);
    }
}
