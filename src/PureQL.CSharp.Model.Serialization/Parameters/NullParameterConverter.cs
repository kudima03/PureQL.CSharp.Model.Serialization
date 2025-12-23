using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record NullParameterJsonModel
{
    public NullParameterJsonModel(NullParameter parameter)
        : this(parameter.Name, (NullType)parameter.Type) { }

    [JsonConstructor]
    public NullParameterJsonModel(string name, NullType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }

    public NullType Type { get; }
}

public sealed class NullParameterConverter : JsonConverter<NullParameter>
{
    public override NullParameter? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NullParameterJsonModel parameter =
            JsonSerializer.Deserialize<NullParameterJsonModel>(ref reader, options)!;

        return new NullParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NullParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NullParameterJsonModel(value), options);
    }
}
