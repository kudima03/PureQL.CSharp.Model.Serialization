using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record DateScalarJsonModel
{
    public DateScalarJsonModel(IDateScalar scalar)
        : this(new DateType(), scalar.Value) { }

    [JsonConstructor]
    public DateScalarJsonModel(DateType type, DateOnly? value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public DateType Type { get; }

    public DateOnly? Value { get; }
}

internal sealed class DateScalarConverter : JsonConverter<IDateScalar>
{
    public override IDateScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateScalarJsonModel scalar = JsonSerializer.Deserialize<DateScalarJsonModel>(
            ref reader,
            options
        )!;

        return new DateScalar(scalar.Value!.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IDateScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateScalarJsonModel(value), options);
    }
}
