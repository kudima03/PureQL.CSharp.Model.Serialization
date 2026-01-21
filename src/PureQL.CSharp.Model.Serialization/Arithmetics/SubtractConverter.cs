using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;

namespace PureQL.CSharp.Model.Serialization.Arithmetics;

internal sealed class SubtractConverter : JsonConverter<Subtract>
{
    public override Subtract Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        ArithmeticOperatorJsonModel arithmeticOperator =
            JsonSerializer.Deserialize<ArithmeticOperatorJsonModel>(ref reader, options)!;

        return arithmeticOperator.Operator != ArithmeticOperator.Subtract
            ? throw new JsonException()
            : new Subtract(arithmeticOperator.Arguments);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Subtract value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new ArithmeticOperatorJsonModel(value), options);
    }
}
