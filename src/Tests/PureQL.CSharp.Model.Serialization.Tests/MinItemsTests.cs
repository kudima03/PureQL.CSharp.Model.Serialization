using System.Text.Json;
using System.Text.Json.Nodes;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Checks that every list of every model variant is read only with at least the
/// <c>minItems</c> the schema declares for it: the written JSON of each variant is cut
/// to one item fewer, which must be rejected, and to exactly that many, which must be
/// accepted.
/// </summary>
public sealed record MinItemsTests
{
    private static readonly Dictionary<string, int> MinItems = new Dictionary<
        string,
        int
    >(StringComparer.Ordinal)
    {
        ["aggregate"] = 1,
        ["conditions"] = 2,
        ["distinctBy"] = 1,
        ["groupBy"] = 1,
        ["joins"] = 1,
        ["orderBy"] = 1,
        ["select"] = 1,
        ["subqueries"] = 1,
        ["values"] = 2,
    };

    [Fact]
    public void MinItemsMatchTheSchema()
    {
        using JsonDocument schema = JsonDocument.Parse(
            SpecificationFiles.Read("PureQL-Specification.json")
        );
        Dictionary<string, HashSet<int>> declared = new Dictionary<
            string,
            HashSet<int>
        >(StringComparer.Ordinal);
        Collect(schema.RootElement, declared);

        Assert.Equal(
            MinItems.Keys.Order(StringComparer.Ordinal),
            declared.Keys.Order(StringComparer.Ordinal)
        );
        Assert.All(
            declared,
            property => Assert.Equal([MinItems[property.Key]], property.Value)
        );
    }

    [Theory]
    [MemberData(nameof(ModelTypes))]
    public void ListShorterThanItsMinItemsIsRejected(string typeName)
    {
        Type type = ModelSamples.Types.Single(t => t.FullName == typeName);
        foreach (object value in ModelSamples.Of(type))
        {
            JsonNode written = JsonSerializer.SerializeToNode(
                value,
                type,
                PureQLJson.Options
            )!;
            foreach (JsonArray list in Lists(written))
            {
                string path = list.GetPath();
                int minItems = MinItems[list.GetPropertyName()];

                JsonException exception = Assert.Throws<JsonException>(() =>
                    Read(written, path, minItems - 1, type)
                );
                Assert.StartsWith(
                    $"Expected at least {minItems} item",
                    exception.Message,
                    StringComparison.Ordinal
                );
                Assert.EndsWith(
                    $"(at {path})",
                    exception.Message,
                    StringComparison.Ordinal
                );

                _ = Read(written, path, minItems, type);
            }
        }
    }

    public static TheoryData<string> ModelTypes()
    {
        return SpecificationFiles.Data(ModelSamples.Types.Select(t => t.FullName!));
    }

    /// <summary>Reads a copy of <paramref name="written"/> with one list cut.</summary>
    private static object Read(JsonNode written, string path, int count, Type type)
    {
        JsonNode copy = written.DeepClone();
        JsonArray list = Find(copy, path);
        while (list.Count > count)
        {
            list.RemoveAt(list.Count - 1);
        }

        return JsonSerializer.Deserialize(copy, type, PureQLJson.Options)!;
    }

    private static JsonArray Find(JsonNode root, string path)
    {
        return Lists(root).Single(list => list.GetPath() == path);
    }

    /// <summary>The lists that have a <c>minItems</c>, outermost first.</summary>
    private static IEnumerable<JsonArray> Lists(JsonNode? node)
    {
        if (node is JsonObject item)
        {
            foreach (KeyValuePair<string, JsonNode?> property in item)
            {
                if (
                    property.Value is JsonArray list
                    && MinItems.ContainsKey(property.Key)
                )
                {
                    yield return list;
                }

                foreach (JsonArray nested in Lists(property.Value))
                {
                    yield return nested;
                }
            }
        }
        else if (node is JsonArray items)
        {
            foreach (JsonNode? element in items)
            {
                foreach (JsonArray nested in Lists(element))
                {
                    yield return nested;
                }
            }
        }
    }

    private static void Collect(
        JsonElement element,
        Dictionary<string, HashSet<int>> declared
    )
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (
                    (property.Value.ValueKind == JsonValueKind.Object)
                    && property.Value.TryGetProperty("minItems", out JsonElement min)
                )
                {
                    if (!declared.TryGetValue(property.Name, out HashSet<int>? values))
                    {
                        values = [];
                        declared[property.Name] = values;
                    }

                    _ = values.Add(min.GetInt32());
                }

                Collect(property.Value, declared);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in element.EnumerateArray())
            {
                Collect(item, declared);
            }
        }
    }
}
