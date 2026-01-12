using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Date;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Date;

internal sealed class DateAggregateConverter : JsonConverter<DateAggregate>
{
    public override DateAggregate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out MaxDate? maxDate)
                ? new DateAggregate(maxDate!)
            : JsonExtensions.TryDeserialize(root, options, out MinDate? minDate)
                ? new DateAggregate(minDate!)
            : JsonExtensions.TryDeserialize(root, options, out AverageDate? averageDate)
                ? new DateAggregate(averageDate!)
            : throw new JsonException("Unable to determine DateAggregate type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateAggregate value,
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
            throw new JsonException("Unable to determine Arithmetic type.");
        }
    }
}
