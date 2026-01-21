using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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

        return JsonExtensions.TryDeserialize(root, options, out BooleanReturning? boolean)
                ? new SelectExpression(boolean!, alias)
            : JsonExtensions.TryDeserialize(root, options, out NumberReturning? number)
                ? new SelectExpression(number!, alias)
            : JsonExtensions.TryDeserialize(root, options, out UuidReturning? uuid)
                ? new SelectExpression(uuid!, alias)
            : JsonExtensions.TryDeserialize(root, options, out DateReturning? date)
                ? new SelectExpression(date!, alias)
            : JsonExtensions.TryDeserialize(root, options, out TimeReturning? time)
                ? new SelectExpression(time!, alias)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeReturning? dateTime
            )
                ? new SelectExpression(dateTime!, alias)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringReturning? stringModel
            )
                ? new SelectExpression(stringModel!, alias)
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
            var v when v.IsT2 => JsonSerializer.SerializeToNode(v.AsT2, options)!,
            var v when v.IsT3 => JsonSerializer.SerializeToNode(v.AsT3, options)!,
            var v when v.IsT4 => JsonSerializer.SerializeToNode(v.AsT4, options)!,
            var v when v.IsT5 => JsonSerializer.SerializeToNode(v.AsT5, options)!,
            var v when v.IsT6 => JsonSerializer.SerializeToNode(v.AsT6, options)!,
            _ => throw new JsonException("Unable to determine SelectExpression type."),
        };

        if (value.Alias is not null)
        {
            node[AliasFieldName] = value.Alias;
        }

        node.WriteTo(writer, options);
    }
}
