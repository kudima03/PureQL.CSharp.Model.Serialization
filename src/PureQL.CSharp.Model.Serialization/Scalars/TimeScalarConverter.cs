using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record TimeScalarJsonModel
{
    public TimeScalarJsonModel(ITimeScalar scalar)
        : this(new TimeType(), scalar.Value) { }

    [JsonConstructor]
    public TimeScalarJsonModel(TimeType type, TimeOnly? value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public TimeType Type { get; }

    public TimeOnly? Value { get; }
}

public sealed class TimeScalarConverter : JsonConverter<ITimeScalar>
{
    public override ITimeScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeScalarJsonModel scalar = JsonSerializer.Deserialize<TimeScalarJsonModel>(
            ref reader,
            options
        )!;

        return new TimeScalar(scalar.Value!.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ITimeScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeScalarJsonModel(value), options);
    }
}
