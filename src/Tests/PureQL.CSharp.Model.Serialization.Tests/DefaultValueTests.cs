using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.RowExpressions;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record DefaultValueTests
{
    [Fact]
    public void NonNullTypeIsWrittenWithoutNullable()
    {
        FieldDecimal field = PureQLJson.Deserialize<FieldDecimal>(
            /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal","nullable":false}}"""
        );

        Assert.Equal(
            /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""",
            PureQLJson.Serialize(field)
        );
    }

    [Theory]
    [InlineData(
        AggregateOver.Group, /*lang=json,strict*/
        """{"operator":"count"}"""
    )]
    [InlineData(
        AggregateOver.All, /*lang=json,strict*/
        """{"operator":"count","over":"all"}"""
    )]
    public void GroupOverIsOmitted(AggregateOver over, string json)
    {
        CountGroup count = new CountGroup(null, over);

        Assert.Equal(json, PureQLJson.Serialize(count));
        Assert.Equal(over, PureQLJson.Deserialize<CountGroup>(json).Over);
    }

    [Fact]
    public void ExplicitDefaultOverIsRead()
    {
        Assert.Equal(
            AggregateOver.Group,
            PureQLJson
                .Deserialize<CountGroup>( /*lang=json,strict*/
                    """{"operator":"count","over":"group"}"""
                )
                .Over
        );
    }

    [Fact]
    public void FixedOverIsAcceptedButNotWritten()
    {
        CountProjection count = PureQLJson.Deserialize<CountProjection>(
            /*lang=json,strict*/
            """{"operator":"count","over":"all"}"""
        );

        Assert.Equal( /*lang=json,strict*/
            """{"operator":"count"}""",
            PureQLJson.Serialize(count)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"operator":"count","over":"group"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"count","over":"window"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"operator":"count","over":1}"""
    )]
    public void OtherOverIsRejected(string json)
    {
        _ = Assert.Throws<System.Text.Json.JsonException>(() =>
            PureQLJson.Deserialize<CountProjection>(json)
        );
    }

    [Theory]
    [InlineData(SortDirection.Asc, "")]
    [InlineData(SortDirection.Desc, ",\"direction\":\"desc\"")]
    public void AscendingDirectionIsOmitted(SortDirection direction, string suffix)
    {
        string expression = /*lang=json,strict*/
            """{"key":0,"type":{"name":"integer"}}""";
        OrderItemGroup item = new OrderItemGroup(
            new ValueGroup(
                new DecimalNullableGroup(new KeyAsDecimalNullable(new KeyInteger(0)))
            ),
            direction
        );

        string json = PureQLJson.Serialize(item);

        Assert.Equal($$"""{"expression":{{expression}}{{suffix}}}""", json);
        Assert.Equal(direction, PureQLJson.Deserialize<OrderItemGroup>(json).Direction);
    }

    [Fact]
    public void FalseDistinctAndAbsentClausesAreOmitted()
    {
        MainPlainQuery query = new MainPlainQuery(
            new From(new FromEntity("orders")),
            [
                new Model.SelectItems.SelectItemProjection(
                    new Model.SelectItems.SelectItemProjectionNonNullable(
                        new Model.SelectItems.SelectItemProjectionBoolean(
                            "flag",
                            new BooleanProjection(new FieldBoolean("orders", "flag"))
                        )
                    )
                ),
            ]
        );

        Assert.Equal(
            /*lang=json,strict*/
            """{"from":{"entity":"orders"},"select":[{"alias":"flag","type":{"name":"boolean"},"expression":{"source":"orders","field":"flag","type":{"name":"boolean"}}}]}""",
            PureQLJson.Serialize(new PureQLQuery(query))
        );
    }

    [Fact]
    public void RoundWithDigitsIsToldApartByDigits()
    {
        string value = /*lang=json,strict*/
            """{"source":"o","field":"f","type":{"name":"decimal"}}""";
        string digits = /*lang=json,strict*/
            """{"type":{"name":"integer"},"value":2}""";

        DecimalRow withDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"operator":"round","value":{{value}},"digits":{{digits}}}"""
        );
        DecimalRow withoutDigits = PureQLJson.Deserialize<DecimalRow>(
            $$"""{"operator":"round","value":{{value}}}"""
        );

        _ = Assert.IsType<RoundDecimalDigitsRow>(withDigits.AsT4.Value);
        _ = Assert.IsType<RoundIntegerRow>(withoutDigits.AsT4.Value);
    }
}
