using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record DateTimeArrayScalarJsonModel
{
    public DateTimeArrayScalarJsonModel(IDateTimeArrayScalar scalar)
        : this(new DateTimeArrayType(), scalar.Value) { }

    [JsonConstructor]
    public DateTimeArrayScalarJsonModel(
        DateTimeArrayType type,
        IEnumerable<DateTime> value
    )
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public DateTimeArrayType Type { get; }

    public IEnumerable<DateTime> Value { get; }
}

internal sealed class DateTimeArrayScalarConverter : JsonConverter<IDateTimeArrayScalar>
{
    public override IDateTimeArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeArrayScalarJsonModel scalar =
            JsonSerializer.Deserialize<DateTimeArrayScalarJsonModel>(
                ref reader,
                options
            )!;

        return new DateTimeArrayScalar(scalar.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IDateTimeArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new DateTimeArrayScalarJsonModel(value),
            options
        );
    }
}
