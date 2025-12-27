using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record StringParameterJsonModel
{
    public StringParameterJsonModel(StringParameter parameter)
        : this(parameter.Name, (StringType)parameter.Type) { }

    [JsonConstructor]
    public StringParameterJsonModel(string name, StringType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public StringType Type { get; }
}

public sealed class StringParameterConverter : JsonConverter<StringParameter>
{
    public override StringParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringParameterJsonModel parameter =
            JsonSerializer.Deserialize<StringParameterJsonModel>(ref reader, options)!;

        return new StringParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StringParameterJsonModel(value), options);
    }
}
