using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization;

internal enum JoinTypeJsonModel
{
    None,
    Left,
    Right,
    Inner,
    Full,
}

internal sealed class JoinConverter : JsonConverter<Join>
{
    private const string TypePropertyName = "type";
    private const string EntityPropertyName = "entity";
    private const string OnPropertyName = "on";

    public override Join Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        if (!root.TryGetProperty(TypePropertyName, out JsonElement typeElement))
        {
            throw new JsonException("Missing 'type' property.");
        }

        if (!root.TryGetProperty(EntityPropertyName, out JsonElement entityElement))
        {
            throw new JsonException("Missing 'entity' property.");
        }

        if (!root.TryGetProperty(OnPropertyName, out JsonElement onElement))
        {
            throw new JsonException("Missing 'on' property.");
        }

        JoinTypeJsonModel typeModel =
            JsonSerializer.Deserialize<JoinTypeJsonModel>(
                typeElement.GetRawText(),
                options
            );

        if (typeModel == JoinTypeJsonModel.None)
        {
            throw new JsonException("Invalid join type.");
        }

        string entity =
            entityElement.GetString() ?? throw new JsonException("Null entity.");

        JoinType joinType = (JoinType)(typeModel - 1);
        OneOf<BooleanReturning, BooleanArrayReturning> on =
            JsonSerializer.Deserialize<OneOf<BooleanReturning, BooleanArrayReturning>>(
                onElement.GetRawText(),
                options
            );

        return on.TryPickT0(out BooleanReturning? boolReturning, out _)
            ? new Join(joinType, entity, boolReturning)
            : on.TryPickT1(out BooleanArrayReturning? boolArrayReturning, out _)
                ? new Join(joinType, entity, boolArrayReturning)
                : throw new JsonException("Unable to determine Join.On type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Join value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartObject();
        writer.WritePropertyName(TypePropertyName);

        JoinTypeJsonModel typeModel = (JoinTypeJsonModel)(value.Type + 1);
        JsonSerializer.Serialize(writer, typeModel, options);

        writer.WritePropertyName(EntityPropertyName);
        writer.WriteStringValue(value.Entity);

        writer.WritePropertyName(OnPropertyName);
        JsonSerializer.Serialize(
            writer,
            value.On,
            options
        );

        writer.WriteEndObject();
    }
}
