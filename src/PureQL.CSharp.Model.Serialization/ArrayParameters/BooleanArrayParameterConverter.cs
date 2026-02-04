using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record BooleanArrayParameterJsonModel
{
    public BooleanArrayParameterJsonModel(BooleanArrayParameter parameter)
        : this(parameter.Name, (BooleanArrayType)parameter.Type) { }

    [JsonConstructor]
    public BooleanArrayParameterJsonModel(string name, BooleanArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public BooleanArrayType Type { get; }
}

internal sealed class BooleanArrayParameterConverter
    : JsonConverter<BooleanArrayParameter>
{
    public override BooleanArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<BooleanArrayParameterJsonModel>(
                ref reader,
                options
            )!;

        return new BooleanArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new BooleanArrayParameterJsonModel(value),
            options
        );
    }
}
