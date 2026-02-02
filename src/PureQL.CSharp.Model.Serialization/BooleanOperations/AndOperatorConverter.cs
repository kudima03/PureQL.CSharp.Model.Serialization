using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

internal sealed class AndOperatorConverter : JsonConverter<AndOperator>
{
    public override AndOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanOperationJsonModel booleanOperator =
            JsonSerializer.Deserialize<BooleanOperationJsonModel>(ref reader, options)!;

        return booleanOperator.Operator != BooleanOperator.And ? throw new JsonException()
            : booleanOperator.Conditions!.Value.TryPickT0(
                out IEnumerable<BooleanReturning>? collection,
                out _
            )
                ? new AndOperator(collection)
            : booleanOperator.Conditions!.Value.TryPickT1(
                out BooleanArrayReturning? array,
                out _
            )
                ? new AndOperator(array)
            : throw new JsonException("Unable to determine AndOperator type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        AndOperator value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BooleanOperationJsonModel(value), options);
    }
}
