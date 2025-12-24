using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record DateTimeScalarJsonModel
{
    public DateTimeScalarJsonModel(IDateTimeScalar scalar)
        : this(new DateTimeType(), scalar.Value) { }

    [JsonConstructor]
    public DateTimeScalarJsonModel(DateTimeType type, DateTime? value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public DateTimeType Type { get; }

    public DateTime? Value { get; }
}

public sealed class DateTimeScalarConverter : JsonConverter<IDateTimeScalar>
{
    public override IDateTimeScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeScalarJsonModel scalar =
            JsonSerializer.Deserialize<DateTimeScalarJsonModel>(ref reader, options)!;

        return new DateTimeScalar(scalar.Value!.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IDateTimeScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeScalarJsonModel(value), options);
    }
}
