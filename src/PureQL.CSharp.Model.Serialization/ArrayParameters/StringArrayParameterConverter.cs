using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record StringArrayParameterJsonModel
{
    public StringArrayParameterJsonModel(StringArrayParameter parameter)
        : this(parameter.Name, (StringArrayType)parameter.Type) { }

    [JsonConstructor]
    public StringArrayParameterJsonModel(string name, StringArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public StringArrayType Type { get; }
}

internal sealed class StringArrayParameterConverter : JsonConverter<StringArrayParameter>
{
    public override StringArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<StringArrayParameterJsonModel>(
                ref reader,
                options
            )!;

        return new StringArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new StringArrayParameterJsonModel(value),
            options
        );
    }
}
