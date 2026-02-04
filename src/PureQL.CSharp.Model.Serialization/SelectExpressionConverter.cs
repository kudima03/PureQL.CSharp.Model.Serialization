using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization;

internal sealed class SelectExpressionConverter : JsonConverter<SelectExpression>
{
    private const string AliasFieldName = "alias";

    public override SelectExpression Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        string? alias = null;
        if (root.TryGetProperty(AliasFieldName, out JsonElement aliasElement))
        {
            alias = aliasElement.GetString();
        }

        return JsonExtensions.TryDeserialize(
                root,
                options,
                out SingleValueReturning? single
            )
                ? new SelectExpression(single!, alias)
            : JsonExtensions.TryDeserialize(root, options, out ArrayReturning? array)
                ? new SelectExpression(array!, alias)
            : throw new JsonException("Unable to determine SelectExpression type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        SelectExpression value,
        JsonSerializerOptions options
    )
    {
        JsonNode node = value switch
        {
            var v when v.IsT0 => JsonSerializer.SerializeToNode(v.AsT0, options)!,
            var v when v.IsT1 => JsonSerializer.SerializeToNode(v.AsT1, options)!,
            _ => throw new JsonException("Unable to determine SelectExpression type."),
        };

        if (value.Alias is not null)
        {
            node[AliasFieldName] = value.Alias;
        }

        node.WriteTo(writer, options);
    }
}
