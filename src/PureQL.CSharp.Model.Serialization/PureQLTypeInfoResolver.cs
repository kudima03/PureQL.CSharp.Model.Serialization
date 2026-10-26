using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Resolves the <see cref="JsonTypeInfo"/> of every model type without reflection, so
/// that a <see cref="PureQLQuery"/> or any part of it can be serialized in an
/// application published with Native AOT or trimming: set it as the
/// <see cref="JsonSerializerOptions.TypeInfoResolver"/> (ahead of a source-generated
/// context, through <see cref="JsonTypeInfoResolver.Combine"/>, when the query is part
/// of a larger document) and use the <see cref="JsonTypeInfo"/> overloads of
/// <see cref="JsonSerializer"/>. A type that is not a model type resolves to
/// <see langword="null"/>, which leaves it to the next resolver in the chain.
/// </summary>
public sealed record PureQLTypeInfoResolver : IJsonTypeInfoResolver
{
    public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        return ModelRegistry.Instance.CreateTypeInfo(type, options);
    }
}
