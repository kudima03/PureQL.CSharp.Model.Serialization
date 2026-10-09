using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// A canonical text of a query that is equal for semantically equal queries: keys are
/// sorted, schema defaults are dropped (<c>nullable: false</c>, <c>direction: asc</c>,
/// <c>distinct: false</c>, and <c>over</c> with the default of its query), numbers are
/// compared by value, a <c>datetime</c> by its instant (the offset is notation only),
/// fractions of a second without trailing zeros, and uuids without regard to case.
/// </summary>
internal static partial class JsonSemantics
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
        Write(builder, node, false);
        return builder.ToString();
    }

    private static void Write(StringBuilder builder, JsonNode? node, bool grouped)
    {
        switch (node)
        {
            case JsonObject value:
                bool isQuery = value.ContainsKey("from") && value.ContainsKey("select");
                bool context = isQuery ? value.ContainsKey("groupBy") : grouped;
                string defaultOver = context ? "group" : "all";
                _ = builder.Append('{');
                bool first = true;
                foreach (
                    KeyValuePair<string, JsonNode?> property in value.OrderBy(
                        p => p.Key,
                        StringComparer.Ordinal
                    )
                )
                {
                    if (
                        IsDefault(property, "nullable", "false")
                        || IsDefault(property, "distinct", "false")
                        || IsDefault(property, "direction", "\"asc\"")
                        || IsDefault(property, "over", $"\"{defaultOver}\"")
                    )
                    {
                        continue;
                    }

                    _ = builder
                        .Append(first ? "" : ",")
                        .Append('"')
                        .Append(property.Key)
                        .Append("\":");
                    Write(builder, property.Value, context);
                    first = false;
                }

                _ = builder.Append('}');
                break;
            case JsonArray array:
                _ = builder.Append('[');
                for (int i = 0; i < array.Count; i++)
                {
                    _ = builder.Append(i == 0 ? "" : ",");
                    Write(builder, array[i], grouped);
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
            case JsonValue value when value.GetValueKind() == JsonValueKind.String:
                _ = builder.Append(
                    JsonSerializer.Serialize(Text(value.GetValue<string>()))
                );
                break;
            default:
                _ = builder.Append(node?.ToJsonString() ?? "null");
                break;
        }
    }

    private static bool IsDefault(
        KeyValuePair<string, JsonNode?> property,
        string name,
        string json
    )
    {
        return property.Key == name && property.Value?.ToJsonString() == json;
    }

    private static string Text(string text)
    {
        Match datetime = DatetimePattern().Match(text);
        if (datetime.Success)
        {
            DateTime local = DateTime.ParseExact(
                datetime.Groups[1].Value,
                "yyyy-MM-dd'T'HH:mm:ss",
                CultureInfo.InvariantCulture
            );
            TimeSpan offset =
                datetime.Groups[3].Value == "Z"
                    ? TimeSpan.Zero
                    : TimeSpan.Parse(
                        datetime.Groups[3].Value.TrimStart('+'),
                        CultureInfo.InvariantCulture
                    );
            DateTime utc = local - offset;
            return $"instant {utc:yyyy-MM-dd'T'HH:mm:ss}{Fraction(datetime.Groups[2].Value)}";
        }

        Match time = TimePattern().Match(text);
        return time.Success
                ? $"time {time.Groups[1].Value}{Fraction(time.Groups[2].Value)}"
            : UuidPattern().IsMatch(text) ? text.ToLowerInvariant()
            : text;
    }

    private static string Fraction(string fraction)
    {
        string digits = fraction.TrimStart('.').TrimEnd('0');
        return digits.Length == 0 ? "" : $".{digits}";
    }

    [GeneratedRegex(
        @"^([0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2})(\.[0-9]+)?(Z|[+-][0-9]{2}:[0-9]{2})$"
    )]
    private static partial Regex DatetimePattern();

    [GeneratedRegex(@"^([0-9]{2}:[0-9]{2}:[0-9]{2})(\.[0-9]+)?$")]
    private static partial Regex TimePattern();

    [GeneratedRegex(
        "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$"
    )]
    private static partial Regex UuidPattern();
}
