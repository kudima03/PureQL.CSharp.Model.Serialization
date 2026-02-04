using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record BooleanFieldJsonModel
{
    public BooleanFieldJsonModel(BooleanField field)
        : this(field.Entity, field.Field, (BooleanArrayType)field.Type) { }

    [JsonConstructor]
    public BooleanFieldJsonModel(string entity, string field, BooleanArrayType type)
    {
        Entity = entity ?? throw new JsonException();
        Field = field ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Entity { get; }

    public string Field { get; }

    public BooleanArrayType Type { get; }
}

internal sealed class BooleanFieldConverter : JsonConverter<BooleanField>
{
    public override BooleanField Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<BooleanFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new BooleanField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BooleanFieldJsonModel(value), options);
    }
}
