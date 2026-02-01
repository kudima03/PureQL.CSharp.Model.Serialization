using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record DateArrayScalarJsonModel
{
    public DateArrayScalarJsonModel(IDateArrayScalar scalar)
        : this(new DateArrayType(), scalar.Value) { }

    [JsonConstructor]
    public DateArrayScalarJsonModel(DateArrayType type, IEnumerable<DateOnly> value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public DateArrayType Type { get; }

    public IEnumerable<DateOnly> Value { get; }
}

internal sealed class DateArrayScalarConverter : JsonConverter<IDateArrayScalar>
{
    public override IDateArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateArrayScalarJsonModel scalar =
            JsonSerializer.Deserialize<DateArrayScalarJsonModel>(ref reader, options)!;

        return new DateArrayScalar(scalar.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IDateArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateArrayScalarJsonModel(value), options);
    }
}
