using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record UuidScalarJsonModel
{
    public UuidScalarJsonModel(IUuidScalar scalar)
        : this(new UuidType(), scalar.Value) { }

    [JsonConstructor]
    public UuidScalarJsonModel(UuidType type, Guid value)
    {
        Type = type;
        Value = value;
    }

    public UuidType Type { get; }

    public Guid Value { get; }
}

public sealed class UuidScalarConverter : JsonConverter<IUuidScalar>
{
    public override IUuidScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidScalarJsonModel scalar = JsonSerializer.Deserialize<UuidScalarJsonModel>(
            ref reader,
            options
        )!;

        return new UuidScalar(scalar.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IUuidScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidScalarJsonModel(value), options);
    }
}
