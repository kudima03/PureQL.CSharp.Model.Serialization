using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;

namespace PureQL.CSharp.Model.Serialization.Arithmetics;

internal sealed class DivideConverter : JsonConverter<Divide>
{
    public override Divide Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        ArithmeticOperatorJsonModel arithmeticOperator =
            JsonSerializer.Deserialize<ArithmeticOperatorJsonModel>(ref reader, options)!;

        return arithmeticOperator.Operator != ArithmeticOperator.Divide
            ? throw new JsonException()
            : new Divide(arithmeticOperator.Arguments);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Divide value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new ArithmeticOperatorJsonModel(value), options);
    }
}
