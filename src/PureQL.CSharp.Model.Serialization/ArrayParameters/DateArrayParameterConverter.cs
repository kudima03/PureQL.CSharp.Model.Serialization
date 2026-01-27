using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record DateArrayParameterJsonModel
{
    public DateArrayParameterJsonModel(DateArrayParameter parameter)
        : this(parameter.Name, (DateArrayType)parameter.Type) { }

    [JsonConstructor]
    public DateArrayParameterJsonModel(string name, DateArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public DateArrayType Type { get; }
}

internal sealed class DateArrayParameterConverter : JsonConverter<DateArrayParameter>
{
    public override DateArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<DateArrayParameterJsonModel>(ref reader, options)!;

        return new DateArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateArrayParameterJsonModel(value), options);
    }
}
