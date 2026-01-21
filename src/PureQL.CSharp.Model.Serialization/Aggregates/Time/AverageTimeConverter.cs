using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Time;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Time;

internal sealed class AverageTimeConverter : JsonConverter<AverageTime>
{
    public override AverageTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<TimeAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != TimeAggregateOperatorJsonModel.average_time
            ? throw new JsonException()
            : new AverageTime(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        AverageTime value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeAggregateJsonModel(value), options);
    }
}
