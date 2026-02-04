using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record NullArrayParameterJsonModel
{
    public NullArrayParameterJsonModel(NullArrayParameter parameter)
        : this(parameter.Name, (NullArrayType)parameter.Type) { }

    [JsonConstructor]
    public NullArrayParameterJsonModel(string name, NullArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public NullArrayType Type { get; }
}

internal sealed class NullArrayParameterConverter : JsonConverter<NullArrayParameter>
{
    public override NullArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NullArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<NullArrayParameterJsonModel>(ref reader, options)!;

        return new NullArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NullArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NullArrayParameterJsonModel(value), options);
    }
}
