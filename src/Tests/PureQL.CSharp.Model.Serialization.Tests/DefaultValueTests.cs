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
        OrderItemGroup item = new OrderItemGroupExpression(
            new KeyInteger("user_id"),
            direction
        );

        string json = PureQLJson.Serialize(item);

        Assert.Equal($$"""{"expression":{{expression}}{{suffix}}}""", json);
        Assert.Equal(
            direction,
            Assert.IsType<OrderItemGroupExpression>(
                PureQLJson.Deserialize<OrderItemGroup>(json).Value
            ).Direction
        );
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
    public void RoundIsToldApartByItsDeclaredTypeAndDigits()
    {
        string @decimal = /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""";
        string integer = /*lang=json,strict*/
            """{"source":"o","field":"n","type":{"name":"integer"}}""";
        string digits = /*lang=json,strict*/
            """{"type":{"name":"integer"},"value":-2}""";

        DecimalRow withDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"operator":"round","type":{"name":"decimal"},"operand":{{@decimal}},"digits":{{digits}}}"""
        );
        DecimalRow integerWithDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"digits":{{digits}},"operator":"round","type":{"name":"integer"},"operand":{{integer}}}"""
        );
        DecimalRow withoutDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"operator":"round","type":{"name":"integer"},"operand":{{@decimal}},"mode":"halfEven"}"""
        );

        _ = Assert.IsType<RoundDecimalRow>(withDigits.Value);
        _ = Assert.IsType<RoundWithDigitsIntegerRow>(integerWithDigits.Value);
        RoundWithoutDigitsIntegerRow round = Assert.IsType<RoundWithoutDigitsIntegerRow>(
            withoutDigits.Value
        );
        Assert.Equal(RoundingMode.HalfEven, round.Mode);
    }

    [Theory]
    [InlineData("decimal", "decimal", false)]
    [InlineData("integer", "integer", false)]
    [InlineData("decimal", "integer", true)]
    public void RoundIsRejectedWhereNoSignatureFits(
        string operandType,
        string type,
        bool withDigits
    )
    {
        string operand =
            $$$"""{"source":"o","field":"f","type":{"name":"{{{operandType}}}"}}""";
        string count = /*lang=json,strict*/
            """{"type":{"name":"integer"},"value":2}""";
        string suffix = withDigits ? $",\"digits\":{count}" : "";

        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<DecimalRow>(
                $$"""{"operator":"round","type":{"name":"{{type}}"},"operand":{{operand}}{{suffix}}}"""
            )
        );
    }

    [Theory]
    [InlineData(RoundingMode.HalfAwayFromZero, "")]
    [InlineData(RoundingMode.HalfEven, ",\"mode\":\"halfEven\"")]
    public void HalfAwayFromZeroIsOmitted(RoundingMode mode, string suffix)
    {
        RoundWithoutDigitsIntegerRow round = new RoundWithoutDigitsIntegerRow(
            new FieldDecimal("o", "f"),
            mode
        );

        string json = PureQLJson.Serialize(round);

        Assert.EndsWith($"}}{suffix}}}", json, StringComparison.Ordinal);
        Assert.Equal(mode, PureQLJson.Deserialize<RoundWithoutDigitsIntegerRow>(json).Mode);
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"operator":"round","type":{"name":"integer"},"operand":{"source":"o","field":"f","type":{"name":"decimal"}},"mode":"halfAwayFromZero"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"concatAll","type":{"name":"string"},"selector":{"source":"o","field":"f","type":{"name":"string"}},"separator":{"type":{"name":"string"},"value":","},"orderBy":[{"expression":{"source":"o","field":"f","type":{"name":"string"}}}],"distinct":false}"""
    )]
    public void ExplicitDefaultsOfNewOptionsAreRejected(string json)
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<ExprGroup>(json)
        );
    }
}
