using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.DateTime;

namespace PureQL.CSharp.Model.Serialization.Aggregates.DateTime;

internal sealed class DateTimeAggregateConverter : JsonConverter<DateTimeAggregate>
{
    public override DateTimeAggregate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out MaxDateTime? maxDateTime)
                ? new DateTimeAggregate(maxDateTime!)
            : JsonExtensions.TryDeserialize(root, options, out MinDateTime? minDateTime)
                ? new DateTimeAggregate(minDateTime!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out AverageDateTime? averageDateTime
            )
                ? new DateTimeAggregate(averageDateTime!)
            : throw new JsonException("Unable to determine DateTimeAggregate type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeAggregate value,
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
        else if (value.IsT2)
        {
            JsonSerializer.Serialize(writer, value.AsT2, options);
        }
        else
        {
            throw new JsonException("Unable to determine DateTimeAggregate type.");
        }
    }
}
