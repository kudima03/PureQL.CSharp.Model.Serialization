using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

internal sealed class OrOperatorConverter : JsonConverter<OrOperator>
{
    public override OrOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanOperationJsonModel booleanOperator =
            JsonSerializer.Deserialize<BooleanOperationJsonModel>(ref reader, options)!;

        return booleanOperator.Operator != BooleanOperator.Or
            ? throw new JsonException()
            : new OrOperator(booleanOperator.Conditions);
    }

    public override void Write(
        Utf8JsonWriter writer,
        OrOperator value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BooleanOperationJsonModel(value), options);
    }
}
