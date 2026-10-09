using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Converts one model type. The value is buffered as a <see cref="JsonDocument"/>,
/// because choosing a variant may need to look ahead into operands.
/// </summary>
internal sealed class PureQLConverter<T>(ModelRegistry registry) : JsonConverter<T>
    where T : class
{
    private readonly ModelRegistry _registry = registry;

    public override T Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        return _registry.Read<T>(document.RootElement, "$");
    }

    public override void Write(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        _registry.Write(writer, value);
    }
}
