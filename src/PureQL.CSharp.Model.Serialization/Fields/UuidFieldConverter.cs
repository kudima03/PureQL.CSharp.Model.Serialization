using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record UuidFieldJsonModel
{
    public UuidFieldJsonModel(UuidField field)
        : this(field.Entity, field.Field, (UuidType)field.Type) { }

    [JsonConstructor]
    public UuidFieldJsonModel(string entity, string field, UuidType type)
    {
        Entity = entity;
        Field = field;
        Type = type;
    }

    public string Entity { get; }

    public string Field { get; }

    public UuidType Type { get; }
}

public sealed class UuidFieldConverter : JsonConverter<UuidField>
{
    public override UuidField? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<UuidFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new UuidField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidFieldJsonModel(value), options);
    }
}
