using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;

namespace PureQL.CSharp.Model.Serialization.Arithmetics;

internal sealed class MultiplyConverter : JsonConverter<Multiply>
{
    public override Multiply Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        ArithmeticOperatorJsonModel arithmeticOperator =
            JsonSerializer.Deserialize<ArithmeticOperatorJsonModel>(ref reader, options)!;

        return arithmeticOperator.Operator != ArithmeticOperator.Multiply
            ? throw new JsonException()
            : new Multiply(arithmeticOperator.Arguments);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Multiply value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new ArithmeticOperatorJsonModel(value), options);
    }
}
