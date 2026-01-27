using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record UuidArrayParameterJsonModel
{
    public UuidArrayParameterJsonModel(UuidArrayParameter parameter)
        : this(parameter.Name, (UuidArrayType)parameter.Type) { }

    [JsonConstructor]
    public UuidArrayParameterJsonModel(string name, UuidArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public UuidArrayType Type { get; }
}

internal sealed class UuidArrayParameterConverter : JsonConverter<UuidArrayParameter>
{
    public override UuidArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<UuidArrayParameterJsonModel>(ref reader, options)!;

        return new UuidArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidArrayParameterJsonModel(value), options);
    }
}
