using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization;

internal sealed record FromExpressionJsonModel
{
    public FromExpressionJsonModel(FromExpression expression)
        : this(expression.Entity, expression.Alias) { }

    [JsonConstructor]
    public FromExpressionJsonModel(string entity, string alias)
    {
        Entity = entity ?? throw new JsonException();
        Alias = alias ?? throw new JsonException();
    }

    public string Entity { get; }

    public string Alias { get; }
}

internal sealed class FromExpressionConverter : JsonConverter<FromExpression>
{
    public override FromExpression Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        FromExpressionJsonModel model =
            JsonSerializer.Deserialize<FromExpressionJsonModel>(ref reader, options)!;

        return new FromExpression(model.Entity, model.Alias);
    }

    public override void Write(
        Utf8JsonWriter writer,
        FromExpression value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new FromExpressionJsonModel(value), options);
    }
}
