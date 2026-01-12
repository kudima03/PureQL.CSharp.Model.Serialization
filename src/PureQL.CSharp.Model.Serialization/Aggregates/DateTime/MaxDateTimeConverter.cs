using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.DateTime;

namespace PureQL.CSharp.Model.Serialization.Aggregates.DateTime;

internal sealed class MaxDateTimeConverter : JsonConverter<MaxDateTime>
{
    public override MaxDateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<DateTimeAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != DateTimeAggregateOperatorJsonModel.max_datetime
            ? throw new JsonException()
            : new MaxDateTime(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        MaxDateTime value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeAggregateJsonModel(value), options);
    }
}
