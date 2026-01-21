using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;

namespace PureQL.CSharp.Model.Serialization.Arithmetics;

internal sealed class AddConverter : JsonConverter<Add>
{
    public override Add Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        ArithmeticOperatorJsonModel arithmeticOperator =
            JsonSerializer.Deserialize<ArithmeticOperatorJsonModel>(ref reader, options)!;

        return arithmeticOperator.Operator != ArithmeticOperator.Add
            ? throw new JsonException()
            : new Add(arithmeticOperator.Arguments);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Add value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new ArithmeticOperatorJsonModel(value), options);
    }
}
