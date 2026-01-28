using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record NumberArrayScalarJsonModel
{
    public NumberArrayScalarJsonModel(INumberArrayScalar scalar)
        : this(new NumberArrayType(), scalar.Value) { }

    [JsonConstructor]
    public NumberArrayScalarJsonModel(NumberArrayType type, IEnumerable<double> value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public NumberArrayType Type { get; }

    public IEnumerable<double> Value { get; }
}

internal sealed class NumberArrayScalarConverter : JsonConverter<INumberArrayScalar>
{
    public override INumberArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberArrayScalarJsonModel model =
            JsonSerializer.Deserialize<NumberArrayScalarJsonModel>(ref reader, options)!;

        return new NumberArrayScalar(model.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        INumberArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumberArrayScalarJsonModel(value), options);
    }
}
