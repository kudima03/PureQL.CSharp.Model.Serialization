using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

public sealed class AndOperatorConverter : JsonConverter<AndOperator>
{
    public override AndOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanOperationJsonModel booleanOperator =
            JsonSerializer.Deserialize<BooleanOperationJsonModel>(ref reader, options)!;

        return booleanOperator.Operator != BooleanOperator.And
            ? throw new JsonException()
            : new AndOperator(booleanOperator.Conditions);
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
