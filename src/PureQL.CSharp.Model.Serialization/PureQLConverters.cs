using System.Collections;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Every converter needed to serialize and deserialize a <see cref="PureQLQuery"/>,
/// with one converter per model type, so any part of a query can be converted on its
/// own as well.
/// </summary>
public sealed record PureQLConverters : IEnumerable<JsonConverter>
{
    public IEnumerator<JsonConverter> GetEnumerator()
    {
        return ModelRegistry.Instance.Converters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
