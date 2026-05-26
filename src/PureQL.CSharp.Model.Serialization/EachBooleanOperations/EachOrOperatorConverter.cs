using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.EachBooleanOperations;

namespace PureQL.CSharp.Model.Serialization.EachBooleanOperations;

internal sealed class EachOrOperatorConverter : JsonConverter<EachOrOperator>
{
    public override EachOrOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachAndOrOperatorJsonModel model =
            JsonSerializer.Deserialize<EachAndOrOperatorJsonModel>(ref reader, options)!;

        return model.Operator != EachBooleanOperatorName.eachOr
            ? throw new JsonException()
            : new EachOrOperator(model.Conditions);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachOrOperator value,
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
