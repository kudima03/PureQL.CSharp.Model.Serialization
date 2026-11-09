using System.Text;
using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Checks the signature a union chooses its case by: the operator or leaf kind, the
/// declared type and, for <c>equal</c>, <c>notEqual</c>, <c>in</c> and the
/// comparisons, the family of the type the operand declares, whatever the order of the
/// properties.
/// </summary>
public sealed record SignatureTests
{
    [Theory]
    [InlineData("integer", Families.Decimal)]
    [InlineData("decimal", Families.Decimal)]
    [InlineData("string", Families.String)]
    [InlineData("boolean", Families.Boolean)]
    [InlineData("date", Families.Date)]
    [InlineData("time", Families.Time)]
    [InlineData("datetime", Families.Datetime)]
    [InlineData("uuid", Families.Uuid)]
    public void ComparisonsCarryTheFamilyOfTheirOperand(string typeName, int family)
    {
        int equal = Signature.Of(Kinds.Equal, TypeIds.Boolean, family);
        int @in = Signature.Of(Kinds.In, TypeIds.Boolean, family);
        string leaf = $$$"""{"type":{"name":"{{{typeName}}}"}}""";
        string nullable = $$$"""{"type":{"name":"{{{typeName}}}","nullable":true}}""";
        string node = $$$"""{"operator":"if","type":{"name":"{{{typeName}}}"}}""";
        string first = $$$"""{"type":{"nullable":true,"name":"{{{typeName}}}"},"value":null}""";
        const string type = "\"type\":{\"name\":\"boolean\"}";

        foreach (string operand in new[] { leaf, nullable, node, first })
        {
            Assert.Equal(equal, Scan($$"""{"operator":"equal",{{type}},"left":{{operand}}}"""));
            Assert.Equal(equal, Scan($$"""{"left":{{operand}},{{type}},"operator":"equal"}"""));
            Assert.Equal(@in, Scan($$"""{"operator":"in",{{type}},"operand":{{operand}}}"""));
            Assert.Equal(@in, Scan($$"""{"operand":{{operand}},"left":{},"operator":"in",{{type}}}"""));
        }
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"source":"o","field":"f","type":{"name":"decimal","nullable":true}}""",
        Kinds.Field,
        TypeIds.DecimalNullable
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"integerList"},"param":"ids"}""",
        Kinds.Param,
        TypeIds.IntegerList
    )]
    [InlineData( /*lang=json,strict*/
        """{"key":"k","type":{"name":"uuid"}}""",
        Kinds.Key,
        TypeIds.Uuid
    )]
    [InlineData( /*lang=json,strict*/
        """{"subquery":"s","field":"f","type":{"name":"date","nullable":true}}""",
        Kinds.Column,
        TypeIds.DateNullable
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time","nullable":true},"value":null}""",
        Kinds.Literal,
        TypeIds.TimeNullable
    )]
    [InlineData( /*lang=json,strict*/
        """{"values":[],"type":{"name":"decimal"},"operator":"add"}""",
        Kinds.Add,
        TypeIds.Decimal
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"datetimeDiffSeconds","type":{"name":"decimal"},"left":{}}""",
        Kinds.DatetimeDiffSeconds,
        TypeIds.Decimal
    )]
    public void NodesAreToldApartByKindAndDeclaredType(string json, int kind, int type)
    {
        Assert.Equal(Signature.Of(kind, type), Scan(json));
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"operator":"equal","type":{"name":"boolean"},"left":{"operator":"add"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"equal","type":{"name":"boolean"},"left":{"type":{"name":"like"}}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"equal","type":{"name":"boolean"},"left":{"type":{"name":1}}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"equal","type":{"name":"boolean"},"left":{"type":"integer"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"in","type":{"name":"boolean"},"operand":[]}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"equal","type":{"name":"boolean"},"left":{"type":{"nullable":true}}}"""
    )]
    public void OperandsWithoutADeclaredTypeHaveNoFamily(string json)
    {
        JsonException error = Assert.Throws<JsonException>(() => Scan(json));

        Assert.StartsWith(
            "Cannot determine the type for BooleanRow",
            error.Message,
            StringComparison.Ordinal
        );
    }

    private static int Scan(string json)
    {
        Utf8JsonReader reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        _ = reader.Read();
        int signature = new ReadContext().Signature(ref reader, "BooleanRow");
        Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
        return signature;
    }
}
