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
    public BoolScalarJsonModel(BooleanType type, bool value)
    {
        Type = type;
        Value = value;
    }

    public BooleanType Type { get; }

    public bool Value { get; }
}

public sealed class BooleanScalarConverter : JsonConverter<IBooleanScalar>
{
    public override IBooleanScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BoolScalarJsonModel scalar = JsonSerializer.Deserialize<BoolScalarJsonModel>(
            ref reader,
            options
        )!;

        return scalar.Type.Name != new BooleanType().Name
            ? throw new JsonException()
            : new BooleanScalar(scalar.Value);
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
