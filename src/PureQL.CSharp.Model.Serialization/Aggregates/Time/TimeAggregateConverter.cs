using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Time;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Time;

internal sealed class TimeAggregateConverter : JsonConverter<TimeAggregate>
{
    public override TimeAggregate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out MaxTime? maxTime)
                ? new TimeAggregate(maxTime!)
            : JsonExtensions.TryDeserialize(root, options, out MinTime? minTime)
                ? new TimeAggregate(minTime!)
            : JsonExtensions.TryDeserialize(root, options, out AverageTime? averageTime)
                ? new TimeAggregate(averageTime!)
            : throw new JsonException("Unable to determine TimeAggregate type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeAggregate value,
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
            throw new JsonException("Unable to determine TimeAggregate type.");
        }
    }
}
