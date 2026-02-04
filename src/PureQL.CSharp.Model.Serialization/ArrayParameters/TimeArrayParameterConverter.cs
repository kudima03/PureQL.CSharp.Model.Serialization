using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record TimeArrayParameterJsonModel
{
    public TimeArrayParameterJsonModel(TimeArrayParameter parameter)
        : this(parameter.Name, (TimeArrayType)parameter.Type) { }

    [JsonConstructor]
    public TimeArrayParameterJsonModel(string name, TimeArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public TimeArrayType Type { get; }
}

internal sealed class TimeArrayParameterConverter : JsonConverter<TimeArrayParameter>
{
    public override TimeArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<TimeArrayParameterJsonModel>(ref reader, options)!;

        return new TimeArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeArrayParameterJsonModel(value), options);
    }
}
