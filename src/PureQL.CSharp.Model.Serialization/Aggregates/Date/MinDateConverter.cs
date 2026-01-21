using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Date;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Date;

internal sealed class MinDateConverter : JsonConverter<MinDate>
{
    public override MinDate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<DateAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != DateAggregateOperatorJsonModel.min_date
            ? throw new JsonException()
            : new MinDate(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        MinDate value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateAggregateJsonModel(value), options);
    }
}
