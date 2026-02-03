using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record DateTimeFieldJsonModel
{
    public DateTimeFieldJsonModel(DateTimeField field)
        : this(field.Entity, field.Field, (DateTimeArrayType)field.Type) { }

    [JsonConstructor]
    public DateTimeFieldJsonModel(string entity, string field, DateTimeArrayType type)
    {
        Entity = entity ?? throw new JsonException();
        Field = field ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Entity { get; }

    public string Field { get; }

    public DateTimeArrayType Type { get; }
}

internal sealed class DateTimeFieldConverter : JsonConverter<DateTimeField>
{
    public override DateTimeField Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<DateTimeFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new DateTimeField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeFieldJsonModel(value), options);
    }
}
