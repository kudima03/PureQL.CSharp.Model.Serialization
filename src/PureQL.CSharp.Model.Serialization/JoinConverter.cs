using System.Text.Json;
using System.Text.Json.Serialization;
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

internal sealed record JoinJsonModel
{
    public JoinJsonModel(Join join)
        : this((JoinTypeJsonModel)(join.Type + 1), join.Entity, join.On) { }

    [JsonConstructor]
    public JoinJsonModel(JoinTypeJsonModel type, string entity, BooleanReturning on)
    {
        Type = type;
        Entity = entity;
        On = on;
    }

    public JoinTypeJsonModel Type { get; }

    public string Entity { get; }

    public BooleanReturning On { get; }
}

internal sealed class JoinConverter : JsonConverter<Join>
{
    public override Join Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        JoinJsonModel model = JsonSerializer.Deserialize<JoinJsonModel>(
            ref reader,
            options
        )!;

        return new Join((JoinType)(model.Type - 1), model.Entity, model.On);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Join value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new JoinJsonModel(value), options);
    }
}
