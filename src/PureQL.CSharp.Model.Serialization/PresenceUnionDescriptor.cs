using System.Text.Json;
using OneOf;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// A two-case union chosen by whether a property is present: the query forms by
/// <c>groupBy</c>, <c>from</c> and <c>join</c> by <c>subquery</c>.
/// </summary>
internal sealed class PresenceUnionDescriptor<TUnion>(
    ModelRegistry registry,
    string property,
    UnionCase<TUnion> present,
    UnionCase<TUnion> absent
) : ModelDescriptor(registry, typeof(TUnion))
    where TUnion : class
{
    public override IReadOnlyCollection<string> Signatures => [];

    public override object Read(JsonElement element, string path)
    {
        UnionCase<TUnion> unionCase =
            (element.ValueKind == JsonValueKind.Object)
            && element.TryGetProperty(property, out _)
                ? present
                : absent;
        return unionCase.Create(Registry.Find(unionCase.Type).Read(element, path));
    }

    public override void Write(Utf8JsonWriter writer, object value)
    {
        Registry.Write(writer, ((IOneOf)value).Value);
    }
}
