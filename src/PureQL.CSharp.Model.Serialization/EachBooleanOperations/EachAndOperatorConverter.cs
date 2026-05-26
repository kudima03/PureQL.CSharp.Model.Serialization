using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.EachBooleanOperations;

namespace PureQL.CSharp.Model.Serialization.EachBooleanOperations;

internal sealed class EachAndOperatorConverter : JsonConverter<EachAndOperator>
{
    public override EachAndOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachAndOrOperatorJsonModel model =
            JsonSerializer.Deserialize<EachAndOrOperatorJsonModel>(ref reader, options)!;

        return model.Operator != EachBooleanOperatorName.eachAnd
            ? throw new JsonException()
            : new EachAndOperator(model.Conditions);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachAndOperator value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachAndOrOperatorJsonModel(value),
            options
        );
    }
}
