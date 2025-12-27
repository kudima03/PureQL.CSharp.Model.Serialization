using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record DateTimeParameterJsonModel
{
    public DateTimeParameterJsonModel(DateTimeParameter parameter)
        : this(parameter.Name, (DateTimeType)parameter.Type) { }

    [JsonConstructor]
    public DateTimeParameterJsonModel(string name, DateTimeType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public DateTimeType Type { get; }
}

public sealed class DateTimeParameterConverter : JsonConverter<DateTimeParameter>
{
    public override DateTimeParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeParameterJsonModel parameter =
            JsonSerializer.Deserialize<DateTimeParameterJsonModel>(ref reader, options)!;

        return new DateTimeParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeParameterJsonModel(value), options);
    }
}
