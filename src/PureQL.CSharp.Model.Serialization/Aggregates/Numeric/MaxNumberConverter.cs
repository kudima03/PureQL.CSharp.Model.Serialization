using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Numeric;

internal sealed class MaxNumberConverter : JsonConverter<MaxNumber>
{
    public override MaxNumber Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumericAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<NumericAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != NumericAggregateOperatorJsonModel.max_number
            ? throw new JsonException()
            : new MaxNumber(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        MaxNumber value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumericAggregateJsonModel(value), options);
    }
}
