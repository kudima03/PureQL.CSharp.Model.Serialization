using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;
using Json.Schema;
using OneOf;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Validates the JSON written for every variant of every model type against the schema
/// definition the type mirrors, so the written shape is checked against the
/// specification rather than only against the reader.
/// </summary>
public sealed partial record SchemaConformanceTests
{
    private static readonly Lazy<Schema> Specification = new Lazy<Schema>(() =>
        new Schema()
    );

    [Theory]
    [MemberData(nameof(ValidFiles))]
    public void SchemaAcceptsValidFixture(string name)
    {
        Assert.True(
            Specification.Value.IsValid(null, SpecificationFiles.Read(name)),
            name
        );
    }

    [Theory]
    [MemberData(nameof(InvalidFiles))]
    public void SchemaRejectsInvalidFixture(string name)
    {
        Assert.False(
            Specification.Value.IsValid(null, SpecificationFiles.Read(name)),
            name
        );
    }

    [Theory]
    [MemberData(nameof(ModelTypes))]
    public void WrittenJsonMatchesTheSchemaDefinition(string typeName)
    {
        Type type = ModelSamples.Types.Single(t => t.FullName == typeName);
        foreach (object value in ModelSamples.Of(type))
        {
            string json = JsonSerializer.Serialize(value, type, PureQLJson.Options);
            object leaf = value;
            while (leaf is IOneOf union)
            {
                leaf = union.Value;
            }

            foreach (string? definition in Definitions(type, leaf.GetType()))
            {
                Assert.True(
                    Specification.Value.IsValid(definition, json),
                    $"{type.Name} ({leaf.GetType().Name}) is not a valid "
                        + $"{definition ?? "query"}: {json}"
                );
            }
        }
    }

    [Theory]
    [InlineData("add.decimal@row", true, "values", "{{D}},{{D}}")]
    [InlineData("add.decimal@row", false, "value", "{{D}},{{D}}")]
    [InlineData("add.decimal@row", false, "values", "{{D}}")]
    [InlineData("add.integer@row", false, "values", "{{D}},{{D}}")]
    [InlineData("add.decimal@group", false, "values", "{{D}},{{D}}")]
    public void DefinitionCheckTellsValidFromInvalid(
        string definition,
        bool valid,
        string property,
        string operands
    )
    {
        string field = /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""";
        string json =
            $$"""{"operator":"add","{{property}}":["""
            + operands.Replace("{{D}}", field, StringComparison.Ordinal)
            + "]}";

        Assert.Equal(valid, Specification.Value.IsValid(definition, json));
    }

    public static TheoryData<string> ValidFiles()
    {
        return SpecificationFiles.Data(SpecificationFiles.Valid());
    }

    public static TheoryData<string> InvalidFiles()
    {
        return SpecificationFiles.Data(SpecificationFiles.Invalid());
    }

    public static TheoryData<string> ModelTypes()
    {
        return SpecificationFiles.Data(ModelSamples.Types.Select(t => t.FullName!));
    }

    /// <summary>
    /// The definitions a value is checked against: that of its declared type, when the
    /// schema has one, and that of the record it holds, which always has one.
    /// </summary>
    private static IEnumerable<string?> Definitions(Type declared, Type record)
    {
        if (
            Specification.Value.TryFind(declared, out string? union)
            && declared != record
        )
        {
            yield return union;
        }

        yield return Specification.Value.TryFind(record, out string? definition)
            ? definition
            : throw new InvalidOperationException(
                $"No schema definition for {record.Name}"
            );
    }

    private sealed partial class Schema
    {
        private readonly BuildOptions _options = new BuildOptions
        {
            SchemaRegistry = new SchemaRegistry(),
        };

        private readonly Dictionary<string, string> _byTypeName = [];

        private readonly Dictionary<string, JsonSchema> _schemas = [];

        private readonly string _id;

        public Schema()
        {
            string text = SpecificationFiles.Read("PureQL-Specification.json");
            JsonSchema root = JsonSchema.FromText(text, _options);
            using JsonDocument document = JsonDocument.Parse(text);
            _id = document.RootElement.GetProperty("$id").GetString()!;
            _schemas[""] = root;
            foreach (
                JsonProperty definition in document
                    .RootElement.GetProperty("$defs")
                    .EnumerateObject()
            )
            {
                string typeName = string.Concat(
                    Separator()
                        .Split(definition.Name)
                        .Select(p => char.ToUpperInvariant(p[0]) + p[1..])
                );
                _byTypeName[typeName] = definition.Name;
            }
        }

        public bool TryFind(Type type, [NotNullWhen(true)] out string? definition)
        {
            string name = type.Name;
            if (name == nameof(PureQLQuery))
            {
                definition = "";
                return true;
            }

            Match list = ListPattern().Match(name);
            string mapped = name switch
            {
                _ when list.Success => $"List{list.Groups[1].Value}",
                _ when name.StartsWith("SelectItemGroup", StringComparison.Ordinal) =>
                    "SelectItemGroup",
                _ when name.StartsWith(
                        "SelectItemProjection",
                        StringComparison.Ordinal
                    ) => "SelectItemProjection",
                _ when name.StartsWith("GroupKey", StringComparison.Ordinal) =>
                    "GroupKey",
                "FromEntity" or "FromSubquery" => "From",
                "JoinEntity" or "JoinSubquery" => "Join",
                _ => name,
            };
            return _byTypeName.TryGetValue(mapped, out definition);
        }

        public bool IsValid(string? definition, string json)
        {
            string key = definition ?? "";
            if (!_schemas.TryGetValue(key, out JsonSchema? schema))
            {
                schema = JsonSchema.FromText(
                    $$"""{"$ref":"{{_id}}#/$defs/{{definition}}"}""",
                    _options
                );
                _schemas[key] = schema;
            }

            using JsonDocument instance = JsonDocument.Parse(
                json,
                new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip,
                    MaxDepth = 256,
                }
            );
            return schema.Evaluate(instance.RootElement).IsValid;
        }

        [GeneratedRegex("[.@]")]
        private static partial Regex Separator();

        [GeneratedRegex(
            "^List(?:Literal|Param|SubqueryColumn)?(Boolean|Date|Datetime|Decimal|Integer|String|Time|Uuid)$"
        )]
        private static partial Regex ListPattern();
    }
}
