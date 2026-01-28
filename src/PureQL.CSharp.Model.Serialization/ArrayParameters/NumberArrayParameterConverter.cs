using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record NumberArrayParameterJsonModel
{
    public NumberArrayParameterJsonModel(NumberArrayParameter parameter)
        : this(parameter.Name, (NumberArrayType)parameter.Type) { }

    [JsonConstructor]
    public NumberArrayParameterJsonModel(string name, NumberArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public NumberArrayType Type { get; }
}

internal sealed class NumberArrayParameterConverter : JsonConverter<NumberArrayParameter>
{
    public override NumberArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<NumberArrayParameterJsonModel>(
                ref reader,
                options
            )!;

        return new NumberArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new NumberArrayParameterJsonModel(value),
            options
        );
    }
}
