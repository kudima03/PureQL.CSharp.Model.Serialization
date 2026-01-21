using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record BoolScalarJsonModel
{
    public BoolScalarJsonModel(IBooleanScalar scalar)
        : this(new BooleanType(), scalar.Value) { }

    [JsonConstructor]
    public BoolScalarJsonModel(BooleanType type, bool? value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public BooleanType Type { get; }

    public bool? Value { get; }
}

internal sealed class BooleanScalarConverter : JsonConverter<IBooleanScalar>
{
    public override IBooleanScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BoolScalarJsonModel scalar = JsonSerializer.Deserialize<BoolScalarJsonModel>(
            ref reader,
            options
        )!;

        return new BooleanScalar(scalar.Value!.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IBooleanScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BoolScalarJsonModel(value), options);
    }
}
