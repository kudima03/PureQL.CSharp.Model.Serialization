using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Checks that <c>Discriminators.Probe</c> reports the family that the schema's
/// <c>probe.*</c> definitions assign to every leaf type and operator.
/// </summary>
public sealed record ProbeParityTests
{
    private static readonly string[] Spine = ["if", "coalesce", "min", "max", "average"];

    [Fact]
    public void LeafTypesAndFixedOperatorsMatchTheSchema()
    {
        foreach (
            (TypeFamily family, string[] typeNames, string[] operators) in SchemaProbes()
        )
        {
            foreach (string typeName in typeNames)
            {
                Assert.Equal(family, Probe($$$"""{"type":{"name":"{{{typeName}}}"}}"""));
            }

            foreach (string op in operators)
            {
                Assert.Equal(family, Probe($$$"""{"operator":"{{{op}}}"}"""));
            }
        }
    }

    [Fact]
    public void SpineOperatorsFollowTheirOperand()
    {
        string uuid = /*lang=json,strict*/
            """{"type":{"name":"uuid"}}""";

        Assert.Equal(TypeFamily.Uuid, Probe("""{"operator":"if","then":""" + uuid + "}"));
        Assert.Equal(
            TypeFamily.Uuid,
            Probe("""{"operator":"coalesce","values":[""" + uuid + "]}")
        );
        Assert.Equal(
            TypeFamily.Uuid,
            Probe("""{"operator":"min","selector":""" + uuid + "}")
        );
        Assert.Equal(
            TypeFamily.Uuid,
            Probe("""{"operator":"max","selector":""" + uuid + "}")
        );
        Assert.Equal(
            TypeFamily.Uuid,
            Probe("""{"operator":"average","selector":""" + uuid + "}")
        );
        Assert.Null(
            Probe( /*lang=json,strict*/
                """{"operator":"coalesce","values":{}}"""
            )
        );
        Assert.Null(
            Probe( /*lang=json,strict*/
                """{"operator":"if"}"""
            )
        );
    }

    [Fact]
    public void EverySchemaOperatorHasAFamilyOrFollowsAnOperand()
    {
        HashSet<string> known =
        [
            .. Spine,
            .. SchemaProbes().SelectMany(p => p.Operators),
        ];
        using JsonDocument schema = JsonDocument.Parse(
            SpecificationFiles.Read("PureQL-Specification.json")
        );
        IEnumerable<string> operators = schema
            .RootElement.GetProperty("$defs")
            .EnumerateObject()
            .Select(d => d.Value)
            .Where(d =>
                d.ValueKind == JsonValueKind.Object
                && d.TryGetProperty("properties", out JsonElement p)
                && p.TryGetProperty("operator", out JsonElement o)
                && o.TryGetProperty("const", out _)
            )
            .Select(d =>
                d.GetProperty("properties")
                    .GetProperty("operator")
                    .GetProperty("const")
                    .GetString()!
            )
            .Distinct();

        Assert.All(operators, op => Assert.Contains(op, known));
        Assert.Null(
            Probe( /*lang=json,strict*/
                """{"operator":"like"}"""
            )
        );
    }

    private static TypeFamily? Probe(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        return Discriminators.Probe(document.RootElement);
    }

    private static List<(
        TypeFamily Family,
        string[] TypeNames,
        string[] Operators
    )> SchemaProbes()
    {
        using JsonDocument schema = JsonDocument.Parse(
            SpecificationFiles.Read("PureQL-Specification.json")
        );
        List<(TypeFamily, string[], string[])> probes = [];
        foreach (
            JsonProperty definition in schema
                .RootElement.GetProperty("$defs")
                .EnumerateObject()
        )
        {
            if (!definition.Name.StartsWith("probe.", StringComparison.Ordinal))
            {
                continue;
            }

            TypeFamily family = Enum.Parse<TypeFamily>(
                definition.Name["probe.".Length..],
                true
            );
            string[] typeNames = [];
            string[] operators = [];
            foreach (
                JsonElement branch in definition
                    .Value.GetProperty("anyOf")
                    .EnumerateArray()
            )
            {
                string[] required =
                [
                    .. branch
                        .GetProperty("required")
                        .EnumerateArray()
                        .Select(r => r.GetString()!),
                ];
                JsonElement properties = branch.GetProperty("properties");
                if (required is ["type"])
                {
                    typeNames = Names(
                        properties
                            .GetProperty("type")
                            .GetProperty("properties")
                            .GetProperty("name")
                    );
                }
                else if (required is ["operator"])
                {
                    operators = Names(properties.GetProperty("operator"));
                }
            }

            probes.Add((family, typeNames, operators));
        }

        return probes;
    }

    private static string[] Names(JsonElement enumeration)
    {
        return
        [
            .. enumeration
                .GetProperty("enum")
                .EnumerateArray()
                .Select(e => e.GetString()!),
        ];
    }
}
