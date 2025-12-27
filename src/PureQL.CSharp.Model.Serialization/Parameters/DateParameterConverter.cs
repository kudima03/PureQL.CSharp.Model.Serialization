using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal sealed record DateParameterJsonModel
{
    public DateParameterJsonModel(DateParameter parameter)
        : this(parameter.Name, (DateType)parameter.Type) { }

    [JsonConstructor]
    public DateParameterJsonModel(string name, DateType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public DateType Type { get; }
}

public sealed class DateParameterConverter : JsonConverter<DateParameter>
{
    public override DateParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateParameterJsonModel parameter =
            JsonSerializer.Deserialize<DateParameterJsonModel>(ref reader, options)!;

        return new DateParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateParameterJsonModel(value), options);
    }
}
