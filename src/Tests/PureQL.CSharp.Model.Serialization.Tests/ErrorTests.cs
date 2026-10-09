using System.Text.Json;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.RowExpressions;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record ErrorTests
{
    private const string Field =
        /*lang=json,strict*/
        """{"source":"o","field":"f","type":{"name":"decimal"}}""";

    [Theory]
    [InlineData(
        /*lang=json,strict*/
        """{"source":"o","source":"p","field":"f","type":{"name":"decimal"}}""",
        "Duplicate property 'source'"
    )]
    [InlineData(
        /*lang=json,strict*/
        """{"source":"o","field":"f","type":{"name":"decimal"},"alias":"a"}""",
        "Unexpected property 'alias'"
    )]
    [InlineData(
        /*lang=json,strict*/
        """{"source":"o","field":"f","type":{"name":"decimal","length":2}}""",
        "Unexpected type property 'length'"
    )]
    [InlineData(
        /*lang=json,strict*/
        """{"source":"o","field":"f","type":{"nullable":true}}""",
        "Missing type name"
    )]
    [InlineData( /*lang=json,strict*/
        """{"source":"o","field":"f"}""",
        "Missing property 'type'"
    )]
    [InlineData(
        /*lang=json,strict*/
        """{"source":"o","field":"f","type":"decimal"}""",
        "Expected a type object"
    )]
    [InlineData(
        /*lang=json,strict*/
        """{"source":"o","field":"f","type":{"name":"string"}}""",
        "DecimalRow does not accept 'field string'"
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":1}""",
        "Expected a string"
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"like"}""",
        "DecimalRow does not accept 'like'"
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"x"}""",
        "Expected an operator node or a reference"
    )]
    [InlineData("""[]""", "Expected an object, found Array")]
    [InlineData( /*lang=json,strict*/
        """{"operator":"add","values":{}}""",
        "Expected an array, found Object"
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"add"}""",
        "Missing property 'values'"
    )]
    public void MalformedNodeIsRejectedWithItsPath(string json, string message)
    {
        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<DecimalRow>(json)
        );

        Assert.StartsWith(message, exception.Message, StringComparison.Ordinal);
        Assert.Contains("(at $", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("""{"operator":"equal","right":{{F}}}""", "Missing property 'left'")]
    [InlineData(
        """{"operator":"equal","left":[],"right":{{F}}}""",
        "Cannot determine the type"
    )]
    [InlineData(
        """{"operator":"equal","left":{"operator":1},"right":{{F}}}""",
        "Cannot determine the type"
    )]
    [InlineData(
        """{"operator":"equal","left":{"operator":"if"},"right":{{F}}}""",
        "Cannot determine the type"
    )]
    [InlineData(
        """{"operator":"equal","left":{"operator":"coalesce","values":[]},"right":{{F}}}""",
        "Cannot determine the type"
    )]
    [InlineData(
        """{"operator":"equal","left":{"operator":"like"},"right":{{F}}}""",
        "Cannot determine the type"
    )]
    [InlineData(
        """{"operator":"equal","left":{"type":{"name":"stringList"}},"right":{{F}}}""",
        "Cannot determine the type"
    )]
    [InlineData(
        """{"operator":"greaterThan","left":{"type":{"name":"uuid"}},"right":{{F}}}""",
        "GreaterThanRow does not accept Uuid"
    )]
    public void OperandOfUnknownTypeIsRejected(string json, string message)
    {
        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<BooleanRow>(
                json.Replace("{{F}}", Field, StringComparison.Ordinal)
            )
        );

        Assert.StartsWith(message, exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(
        """{"operator":"if","condition":{{T}},"then":{{K}},"else":{{K}}}""",
        "decimal",
        typeof(EqualDecimalGroup)
    )]
    [InlineData(
        """{"operator":"coalesce","values":[{{K}},{{K}}]}""",
        "decimal",
        typeof(EqualDecimalGroup)
    )]
    [InlineData(
        """{"operator":"min","selector":{{F}}}""",
        "decimal",
        typeof(EqualDecimalGroup)
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"count"}""",
        "integer",
        typeof(EqualDecimalGroup)
    )]
    [InlineData(
        """{"operator":"concat","values":[{{S}},{{S}}]}""",
        "string",
        typeof(EqualStringGroup)
    )]
    [InlineData(
        """{"operator":"dateAddDays","left":{"type":{"name":"date"},"value":"2024-01-01"},"right":{{I}}}""",
        "date",
        typeof(EqualDateGroup)
    )]
    [InlineData(
        """{"operator":"timeAddSeconds","left":{"type":{"name":"time"},"value":"10:00:00"},"right":{{I}}}""",
        "time",
        typeof(EqualTimeGroup)
    )]
    [InlineData(
        """{"operator":"datetimeAddSeconds","left":{"type":{"name":"datetime"},"value":"2024-01-01T10:00:00Z"},"right":{{I}}}""",
        "datetime",
        typeof(EqualDatetimeGroup)
    )]
    [InlineData(
        """{"operator":"any","predicate":{{T}}}""",
        "boolean",
        typeof(EqualBooleanGroup)
    )]
    [InlineData( /*lang=json,strict*/
        """{"key":0,"type":{"name":"uuid"}}""",
        "uuid",
        typeof(EqualUuidGroup)
    )]
    public void EqualVariantFollowsTheLeftOperand(string left, string type, Type variant)
    {
        string json =
            """{"operator":"equal","left":"""
            + left.Replace(
                    "{{T}}",
                    /*lang=json,strict*/
                    """{"type":{"name":"boolean"},"value":true}""",
                    StringComparison.Ordinal
                )
                .Replace(
                    "{{K}}",
                    /*lang=json,strict*/
                    """{"key":0,"type":{"name":"decimal"}}""",
                    StringComparison.Ordinal
                )
                .Replace(
                    "{{S}}",
                    /*lang=json,strict*/
                    """{"type":{"name":"string"},"value":"s"}""",
                    StringComparison.Ordinal
                )
                .Replace(
                    "{{I}}",
                    /*lang=json,strict*/
                    """{"type":{"name":"integer"},"value":1}""",
                    StringComparison.Ordinal
                )
                .Replace("{{F}}", Field, StringComparison.Ordinal)
            + $$$""","right":{"type":{"name":"{{{type}}}","nullable":true},"value":null}}""";

        EqualGroup equal = PureQLJson.Deserialize<EqualGroup>(json);

        Assert.IsType(variant, equal.Value);
    }

    [Fact]
    public void NullValueIsRejectedOnWrite()
    {
        AddDecimalRow add = new AddDecimalRow([null!]);

        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(add));
    }

    [Fact]
    public void UnknownJoinTypeIsRejectedOnWrite()
    {
        JoinEntity join = new JoinEntity(
            (JoinType)42,
            "users",
            new BooleanRow(new FieldBoolean("users", "active"))
        );

        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(join));
    }

    [Theory]
    [InlineData(
        /*lang=json,strict*/
        """{"type":"cross","entity":"u","on":{"type":{"name":"boolean"},"value":true}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"entity":"u","on":{"type":{"name":"boolean"},"value":true}}"""
    )]
    public void JoinWithoutAKnownTypeIsRejected(string json)
    {
        _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<Join>(json));
    }

    [Fact]
    public void GroupKeyIndexMustBeAnInteger()
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<DecimalGroup>(
                /*lang=json,strict*/
                """{"key":"0","type":{"name":"decimal"}}"""
            )
        );
    }
}
