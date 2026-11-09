using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.TotalExpressions;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record DefaultValueTests
{
    [Fact]
    public void NonNullTypeIsWrittenWithoutNullable()
    {
        Assert.Equal(
            /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""",
            PureQLJson.Serialize(new FieldDecimal("o", "f"))
        );
    }

    [Fact]
    public void NullableFalseIsRejected()
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<FieldDecimal>(
                /*lang=json,strict*/
                """{"source":"o","field":"f","type":{"name":"decimal","nullable":false}}"""
            )
        );
    }

    [Theory]
    [InlineData(
        AggregateOver.Group, /*lang=json,strict*/
        """{"operator":"count","type":{"name":"integer"}}"""
    )]
    [InlineData(
        AggregateOver.All, /*lang=json,strict*/
        """{"operator":"count","type":{"name":"integer"},"over":"all"}"""
    )]
    public void GroupOverIsOmitted(AggregateOver over, string json)
    {
        CountGroup count = new CountGroup(null, over);

        Assert.Equal(json, PureQLJson.Serialize(count));
        Assert.Equal(over, PureQLJson.Deserialize<CountGroup>(json).Over);
    }

    [Fact]
    public void ExplicitDefaultOverIsRejected()
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<CountGroup>( /*lang=json,strict*/
                """{"operator":"count","type":{"name":"integer"},"over":"group"}"""
            )
        );
    }

    [Fact]
    public void OverIsRejectedOutsideAGroup()
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<CountProjection>( /*lang=json,strict*/
                """{"operator":"count","type":{"name":"integer"},"over":"all"}"""
            )
        );
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<CountTotal>( /*lang=json,strict*/
                """{"operator":"count","type":{"name":"integer"},"over":"all"}"""
            )
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"operator":"count","type":{"name":"integer"},"over":"window"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"count","type":{"name":"integer"},"over":1}"""
    )]
    public void OtherOverIsRejected(string json)
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<CountGroup>(json)
        );
    }

    [Theory]
    [InlineData(SortDirection.Asc, "")]
    [InlineData(SortDirection.Desc, ",\"direction\":\"desc\"")]
    public void AscendingDirectionIsOmitted(SortDirection direction, string suffix)
    {
        string expression = /*lang=json,strict*/
            """{"key":"user_id","type":{"name":"integer"}}""";
        OrderItemGroup item = new OrderItemGroup(new KeyInteger("user_id"), direction);

        string json = PureQLJson.Serialize(item);

        Assert.Equal($$"""{"expression":{{expression}}{{suffix}}}""", json);
        Assert.Equal(direction, PureQLJson.Deserialize<OrderItemGroup>(json).Direction);
    }

    [Fact]
    public void AscendingDirectionIsRejected()
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<OrderItemGroup>( /*lang=json,strict*/
                """{"expression":{"key":"user_id","type":{"name":"integer"}},"direction":"asc"}"""
            )
        );
    }

    [Fact]
    public void AbsentClausesAreOmitted()
    {
        PureQLQuery query = new MainPlainQueryWithoutPagination(
            new FromEntity("orders"),
            [new SelectItemProjection("flag", new FieldBoolean("orders", "flag"))]
        );

        Assert.Equal(
            /*lang=json,strict*/
            """{"from":{"entity":"orders"},"select":[{"alias":"flag","expression":{"source":"orders","field":"flag","type":{"name":"boolean"}}}]}""",
            PureQLJson.Serialize(query)
        );
    }

    [Fact]
    public void RoundIsToldApartByItsDeclaredType()
    {
        string operand = /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""";
        string digits = /*lang=json,strict*/
            """{"type":{"name":"integer"},"value":2}""";

        DecimalRow withDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"operator":"round","type":{"name":"decimal"},"operand":{{operand}},"digits":{{digits}}}"""
        );
        DecimalRow withoutDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"operator":"round","type":{"name":"integer"},"operand":{{operand}}}"""
        );

        _ = Assert.IsType<RoundDecimalRow>(withDigits.Value);
        _ = Assert.IsType<RoundIntegerRow>(withoutDigits.Value);
    }

    [Theory]
    [InlineData("integer", true)]
    [InlineData("decimal", false)]
    public void DigitsBelongToADecimalRound(string type, bool withDigits)
    {
        string operand = /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""";
        string count = /*lang=json,strict*/
            """{"type":{"name":"integer"},"value":2}""";
        string suffix = withDigits ? $",\"digits\":{count}" : "";

        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<DecimalRow>(
                $$"""{"operator":"round","type":{"name":"{{type}}"},"operand":{{operand}}{{suffix}}}"""
            )
        );
    }
}
