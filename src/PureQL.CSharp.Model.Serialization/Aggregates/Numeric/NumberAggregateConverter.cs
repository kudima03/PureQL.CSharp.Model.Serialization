using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Numeric;

internal sealed class NumberAggregateConverter : JsonConverter<NumberAggregate>
{
    public override NumberAggregate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(
                root,
                options,
                out AverageNumber? averageNumber
            )
                ? new NumberAggregate(averageNumber!)
            : JsonExtensions.TryDeserialize(root, options, out MaxNumber? maxNumber)
                ? new NumberAggregate(maxNumber!)
            : JsonExtensions.TryDeserialize(root, options, out MinNumber? minNumber)
                ? new NumberAggregate(minNumber!)
            : JsonExtensions.TryDeserialize(root, options, out SumNumber? sum)
                ? new NumberAggregate(sum!)
            : throw new JsonException("Unable to determine NumberAggregate type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberAggregate value,
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
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(writer, value.AsT3, options);
        }
        else
        {
            throw new JsonException("Unable to determine NumberAggregate type.");
        }
    }
}
