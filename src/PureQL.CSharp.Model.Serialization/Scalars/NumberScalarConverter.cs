using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record NumberScalarJsonModel
{
    public NumberScalarJsonModel(INumberScalar scalar)
        : this(new NumberType(), scalar.Value) { }

    [JsonConstructor]
    public NumberScalarJsonModel(NumberType type, double value)
    {
        Type = type;
        Value = value;
    }

    public NumberType Type { get; }

    public double Value { get; }
}

public sealed class NumberScalarConverter : JsonConverter<INumberScalar>
{
    public override INumberScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberScalarJsonModel model = JsonSerializer.Deserialize<NumberScalarJsonModel>(
            ref reader,
            options
        )!;

        return new NumberScalar(model.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        INumberScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumberScalarJsonModel(value), options);
    }
}
