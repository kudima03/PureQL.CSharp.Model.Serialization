using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record UuidParameterJsonModel
{
    public UuidParameterJsonModel(UuidParameter parameter)
        : this(parameter.Name, (UuidType)parameter.Type) { }

    [JsonConstructor]
    public UuidParameterJsonModel(string name, UuidType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }

    public UuidType Type { get; }
}

public sealed class UuidParameterConverter : JsonConverter<UuidParameter>
{
    public override UuidParameter? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidParameterJsonModel parameter =
            JsonSerializer.Deserialize<UuidParameterJsonModel>(ref reader, options)!;

        return new UuidParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidParameterJsonModel(value), options);
    }
}
