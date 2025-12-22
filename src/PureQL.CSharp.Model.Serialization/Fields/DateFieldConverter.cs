using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record DateFieldJsonModel
{
    public DateFieldJsonModel(DateField field)
        : this(field.Entity, field.Field, (DateType)field.Type) { }

    [JsonConstructor]
    public DateFieldJsonModel(string entity, string field, DateType type)
    {
        Entity = entity;
        Field = field;
        Type = type;
    }

    public string Entity { get; }

    public string Field { get; }

    public DateType Type { get; }
}

public sealed class DateFieldConverter : JsonConverter<DateField>
{
    public override DateField? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<DateFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new DateField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateFieldJsonModel(value), options);
    }
}
