using System.Text.Json;
using OneOf;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// A union chosen by which property is present: the query forms by <c>groupBy</c>, then
/// <c>aggregate</c>, <c>from</c> and <c>join</c> by <c>subquery</c>. The first listed
/// property that is present selects its case, and the fallback case is used when none
/// is.
/// </summary>
internal sealed class PresenceUnionDescriptor<TUnion>(
    ModelRegistry registry,
    IReadOnlyList<(string Property, UnionCase<TUnion> Case)> present,
    UnionCase<TUnion> fallback
) : ModelDescriptor(registry, typeof(TUnion))
    where TUnion : class
{
    public override IReadOnlyCollection<string> Signatures => [];

    public override object Read(JsonElement element, string path)
    {
        UnionCase<TUnion> unionCase = fallback;
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach ((string property, UnionCase<TUnion> candidate) in present)
            {
                if (element.TryGetProperty(property, out _))
                {
                    unionCase = candidate;
                    break;
                }
            }
        }

        return unionCase.Create(Registry.Find(unionCase.Type).Read(element, path));
    }

    public override void Write(Utf8JsonWriter writer, object value)
    {
        Registry.Write(writer, ((IOneOf)value).Value);
    }
}
