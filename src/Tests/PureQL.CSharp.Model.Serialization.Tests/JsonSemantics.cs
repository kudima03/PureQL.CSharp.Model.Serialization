using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// A canonical text of a query that is equal for semantically equal queries: keys are
/// sorted and numbers are compared by value. Every value has one spelling in the
/// specification, so nothing else is normalized: defaults are never written and
/// <c>datetime</c>, <c>time</c>, <c>decimal</c> and <c>uuid</c> values are compared as
/// text.
/// </summary>
internal static class JsonSemantics
{
    public static string Canonical(string json)
    {
        JsonNode node = JsonNode.Parse(
            json,
            null,
            new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                MaxDepth = 256,
            }
        )!;
        StringBuilder builder = new StringBuilder();
        Write(builder, node);
        return builder.ToString();
    }

    private static void Write(StringBuilder builder, JsonNode? node)
    {
        switch (node)
        {
            case JsonObject value:
                _ = builder.Append('{');
                bool first = true;
                foreach (
                    KeyValuePair<string, JsonNode?> property in value.OrderBy(
                        p => p.Key,
                        StringComparer.Ordinal
                    )
                )
                {
                    _ = builder
                        .Append(first ? "" : ",")
                        .Append('"')
                        .Append(property.Key)
                        .Append("\":");
                    Write(builder, property.Value);
                    first = false;
                }

                _ = builder.Append('}');
                break;
            case JsonArray array:
                _ = builder.Append('[');
                for (int i = 0; i < array.Count; i++)
                {
                    _ = builder.Append(i == 0 ? "" : ",");
                    Write(builder, array[i]);
                }

                _ = builder.Append(']');
                break;
            case JsonValue value when value.GetValueKind() == JsonValueKind.Number:
                _ = builder.Append(
                    decimal.Parse(
                            value.ToJsonString(),
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture
                        )
                        .ToString("G29", CultureInfo.InvariantCulture)
                );
                break;
            default:
                _ = builder.Append(node?.ToJsonString() ?? "null");
                break;
        }
    }
}
