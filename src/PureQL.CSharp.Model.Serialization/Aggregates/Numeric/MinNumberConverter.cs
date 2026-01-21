using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Numeric;

internal sealed class MinNumberConverter : JsonConverter<MinNumber>
{
    public override MinNumber Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumericAggregateJsonModel aggregate =
            JsonSerializer.Deserialize<NumericAggregateJsonModel>(ref reader, options)!;

        return aggregate.Operator != NumericAggregateOperatorJsonModel.min_number
            ? throw new JsonException()
            : new MinNumber(aggregate.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        MinNumber value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumericAggregateJsonModel(value), options);
    }
}
