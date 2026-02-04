using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record UuidArrayScalarJsonModel
{
    public UuidArrayScalarJsonModel(IUuidArrayScalar scalar)
        : this(new UuidArrayType(), scalar.Value) { }

    [JsonConstructor]
    public UuidArrayScalarJsonModel(UuidArrayType type, IEnumerable<Guid> value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public UuidArrayType Type { get; }

    public IEnumerable<Guid> Value { get; }
}

internal sealed class UuidArrayScalarConverter : JsonConverter<IUuidArrayScalar>
{
    public override IUuidArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidArrayScalarJsonModel scalar =
            JsonSerializer.Deserialize<UuidArrayScalarJsonModel>(ref reader, options)!;

        return new UuidArrayScalar(scalar.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IUuidArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidArrayScalarJsonModel(value), options);
    }
}
