using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record TimeFieldJsonModel
{
    public TimeFieldJsonModel(TimeField field)
        : this(field.Entity, field.Field, (TimeType)field.Type) { }

    [JsonConstructor]
    public TimeFieldJsonModel(string entity, string field, TimeType type)
    {
        Entity = entity;
        Field = field;
        Type = type;
    }

    public string Entity { get; }

    public string Field { get; }

    public TimeType Type { get; }
}

public sealed class TimeFieldConverter : JsonConverter<TimeField>
{
    public override TimeField? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<TimeFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new TimeField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeFieldJsonModel(value), options);
    }
}
