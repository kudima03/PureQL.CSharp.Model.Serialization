using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal sealed record NullFieldJsonModel
{
    public NullFieldJsonModel(NullField field)
        : this(field.Entity, field.Field, new NullArrayType()) { }

    [JsonConstructor]
    public NullFieldJsonModel(string entity, string field, NullArrayType type)
    {
        Entity = entity ?? throw new JsonException();
        Field = field ?? throw new JsonException();
        Type = type ?? throw new JsonException();
    }

    public string Entity { get; }

    public string Field { get; }

    public NullArrayType Type { get; }
}

internal sealed class NullFieldConverter : JsonConverter<NullField>
{
    public override NullField Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NullFieldJsonModel fieldModel =
            JsonSerializer.Deserialize<NullFieldJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new NullField(fieldModel.Entity, fieldModel.Field);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NullField value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NullFieldJsonModel(value), options);
    }
}
