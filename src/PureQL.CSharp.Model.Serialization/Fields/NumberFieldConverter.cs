using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record NumberFieldJsonModel
{
    public NumberFieldJsonModel(NumberField field)
        : this(field.Entity, field.Field, (NumberType)field.Type) { }

    [JsonConstructor]
    public NumberFieldJsonModel(string entity, string field, NumberType type)
    {
        Entity = entity ?? throw new JsonException();
        Field = field ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Entity { get; }

    public string Field { get; }

    public NumberType Type { get; }
}

public sealed class NumberFieldConverter : JsonConverter<NumberField>
{
    public override NumberField Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<NumberFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new NumberField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumberFieldJsonModel(value), options);
    }
}
