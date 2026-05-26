using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.EachBooleanOperations;

namespace PureQL.CSharp.Model.Serialization.EachBooleanOperations;

internal sealed class EachNotOperatorConverter : JsonConverter<EachNotOperator>
{
    public override EachNotOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachNotOperatorJsonModel model =
            JsonSerializer.Deserialize<EachNotOperatorJsonModel>(ref reader, options)!;

        return model.Operator != EachNotOperatorName.eachNot
            ? throw new JsonException()
            : new EachNotOperator(model.Condition);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachNotOperator value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new EachNotOperatorJsonModel(value), options);
    }
}
