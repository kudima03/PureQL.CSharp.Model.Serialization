using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.String;

namespace PureQL.CSharp.Model.Serialization.Aggregates.String;

internal sealed class StringAggregateConverter : JsonConverter<StringAggregate>
{
    public override StringAggregate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out MaxString? maxString)
                ? new StringAggregate(maxString!)
            : JsonExtensions.TryDeserialize(root, options, out MinString? minString)
                ? new StringAggregate(minString!)
            : throw new JsonException("Unable to determine StringAggregate type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringAggregate value,
        JsonSerializerOptions options
    )
    {
        if (value.IsT0)
        {
            JsonSerializer.Serialize(writer, value.AsT0, options);
        }
        else if (value.IsT1)
        {
            JsonSerializer.Serialize(writer, value.AsT1, options);
        }
        else
        {
            throw new JsonException("Unable to determine StringAggregate type.");
        }
    }
}
