using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record BooleanParameterJsonModel
{
    public BooleanParameterJsonModel(BooleanParameter parameter)
        : this(parameter.Name, (BooleanType)parameter.Type) { }

    [JsonConstructor]
    public BooleanParameterJsonModel(string name, BooleanType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public BooleanType Type { get; }
}

public sealed class BooleanParameterConverter : JsonConverter<BooleanParameter>
{
    public override BooleanParameter? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanParameterJsonModel parameter =
            JsonSerializer.Deserialize<BooleanParameterJsonModel>(ref reader, options)!;

        return new BooleanParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BooleanParameterJsonModel(value), options);
    }
}
