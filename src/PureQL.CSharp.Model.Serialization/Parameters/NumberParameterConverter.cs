using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record NumberParameterJsonModel
{
    public NumberParameterJsonModel(NumberParameter parameter)
        : this(parameter.Name, (NumberType)parameter.Type) { }

    [JsonConstructor]
    public NumberParameterJsonModel(string name, NumberType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public NumberType Type { get; }
}

public sealed class NumberParameterConverter : JsonConverter<NumberParameter>
{
    public override NumberParameter? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberParameterJsonModel parameter =
            JsonSerializer.Deserialize<NumberParameterJsonModel>(ref reader, options)!;

        return new NumberParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumberParameterJsonModel(value), options);
    }
}
