using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record BoolArrayScalarJsonModel
{
    public BoolArrayScalarJsonModel(IBooleanArrayScalar scalar)
        : this(new BooleanArrayType(), scalar.Value) { }

    [JsonConstructor]
    public BoolArrayScalarJsonModel(BooleanArrayType type, IEnumerable<bool> value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public BooleanArrayType Type { get; }

    public IEnumerable<bool> Value { get; }
}

internal sealed class BooleanArrayScalarConverter : JsonConverter<IBooleanArrayScalar>
{
    public override IBooleanArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BoolArrayScalarJsonModel scalar =
            JsonSerializer.Deserialize<BoolArrayScalarJsonModel>(ref reader, options)!;

        return new BooleanArrayScalar(scalar.Value!);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IBooleanArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BoolArrayScalarJsonModel(value), options);
    }
}
