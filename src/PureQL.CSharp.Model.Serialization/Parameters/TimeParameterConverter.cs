using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record TimeParameterJsonModel
{
    public TimeParameterJsonModel(TimeParameter parameter)
        : this(parameter.Name, (TimeType)parameter.Type) { }

    [JsonConstructor]
    public TimeParameterJsonModel(string name, TimeType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public TimeType Type { get; }
}

public sealed class TimeParameterConverter : JsonConverter<TimeParameter>
{
    public override TimeParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeParameterJsonModel parameter =
            JsonSerializer.Deserialize<TimeParameterJsonModel>(ref reader, options)!;

        return new TimeParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeParameterJsonModel(value), options);
    }
}
