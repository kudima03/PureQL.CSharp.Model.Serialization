using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.DateTime;

namespace PureQL.CSharp.Model.Serialization.Aggregates.DateTime;

internal sealed class AverageDateTimeConverter : JsonConverter<AverageDateTime>
{
    public override AverageDateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<DateTimeAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != DateTimeAggregateOperatorJsonModel.average_datetime
            ? throw new JsonException()
            : new AverageDateTime(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        AverageDateTime value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeAggregateJsonModel(value), options);
    }
}
