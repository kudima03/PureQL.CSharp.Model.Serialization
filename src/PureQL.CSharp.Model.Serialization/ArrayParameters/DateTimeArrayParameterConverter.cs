using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayParameters;

internal sealed record DateTimeArrayParameterJsonModel
{
    public DateTimeArrayParameterJsonModel(DateTimeArrayParameter parameter)
        : this(parameter.Name, (DateTimeArrayType)parameter.Type) { }

    [JsonConstructor]
    public DateTimeArrayParameterJsonModel(string name, DateTimeArrayType type)
    {
        Name = name ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Name { get; }

    public DateTimeArrayType Type { get; }
}

internal sealed class DateTimeArrayParameterConverter
    : JsonConverter<DateTimeArrayParameter>
{
    public override DateTimeArrayParameter Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeArrayParameterJsonModel parameter =
            JsonSerializer.Deserialize<DateTimeArrayParameterJsonModel>(
                ref reader,
                options
            )!;

        return new DateTimeArrayParameter(parameter.Name);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeArrayParameter value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new DateTimeArrayParameterJsonModel(value),
            options
        );
    }
}
