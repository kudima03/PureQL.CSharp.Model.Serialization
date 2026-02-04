using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record TimeArrayScalarJsonModel
{
    public TimeArrayScalarJsonModel(ITimeArrayScalar scalar)
        : this(new TimeArrayType(), scalar.Value) { }

    [JsonConstructor]
    public TimeArrayScalarJsonModel(TimeArrayType type, IEnumerable<TimeOnly> value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public TimeArrayType Type { get; }

    public IEnumerable<TimeOnly> Value { get; }
}

internal sealed class TimeArrayScalarConverter : JsonConverter<ITimeArrayScalar>
{
    public override ITimeArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeArrayScalarJsonModel scalar =
            JsonSerializer.Deserialize<TimeArrayScalarJsonModel>(ref reader, options)!;

        return new TimeArrayScalar(scalar.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ITimeArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeArrayScalarJsonModel(value), options);
    }
}
