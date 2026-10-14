using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Checks that <c>Discriminators.Probe</c> reports the family of the type that a node
/// declares, whether it is a leaf or an operator.
/// </summary>
public sealed record ProbeTests
{
    [Theory]
    [InlineData("integer", "Decimal")]
    [InlineData("decimal", "Decimal")]
    [InlineData("string", "String")]
    [InlineData("boolean", "Boolean")]
    [InlineData("date", "Date")]
    [InlineData("time", "Time")]
    [InlineData("datetime", "Datetime")]
    [InlineData("uuid", "Uuid")]
    public void LeavesAndOperatorsReportTheFamilyOfTheirDeclaredType(
        string typeName,
        string familyName
    )
    {
        TypeFamily family = Enum.Parse<TypeFamily>(familyName);
        Assert.Equal(family, Probe($$$"""{"type":{"name":"{{{typeName}}}"}}"""));
        Assert.Equal(
            family,
            Probe($$$"""{"type":{"name":"{{{typeName}}}","nullable":true}}""")
        );
        Assert.Equal(
            family,
            Probe($$$"""{"operator":"if","type":{"name":"{{{typeName}}}"}}""")
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"operator":"add"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"like"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":1}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":"integer"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """[]"""
    )]
    public void NodesWithoutADeclaredTypeHaveNoFamily(string json)
    {
        Assert.Null(Probe(json));
    }

    private static TypeFamily? Probe(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        return Discriminators.Probe(document.RootElement);
    }
}
