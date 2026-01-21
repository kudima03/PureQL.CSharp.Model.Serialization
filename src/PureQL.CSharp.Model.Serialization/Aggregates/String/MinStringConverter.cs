using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.String;

namespace PureQL.CSharp.Model.Serialization.Aggregates.String;

internal sealed class MinStringConverter : JsonConverter<MinString>
{
    public override MinString Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<StringAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != StringAggregateOperatorJsonModel.min_string
            ? throw new JsonException()
            : new MinString(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        MinString value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StringAggregateJsonModel(value), options);
    }
}
