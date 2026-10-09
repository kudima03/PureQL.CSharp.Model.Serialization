using System.Text.Json;
using PureQL.CSharp.Model.Literals;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record LiteralFormatTests
{
    [Theory]
    [InlineData("2024-01-31T18:30:00+03:00", "2024-01-31T18:30:00+03:00")]
    [InlineData("2024-01-01T00:00:00+00:00", "2024-01-01T00:00:00Z")]
    [InlineData("2024-01-01T00:00:00-00:30", "2024-01-01T00:00:00-00:30")]
    [InlineData("2024-02-01T00:00:00.123456+05:30", "2024-02-01T00:00:00.123456+05:30")]
    [InlineData("2024-02-01T00:00:00.120000000Z", "2024-02-01T00:00:00.12Z")]
    [InlineData("2024-01-01T00:00:00+14:00", "2024-01-01T00:00:00+14:00")]
    [InlineData("2024-12-31T23:59:59.5-23:59", "2025-01-01T23:58:59.5Z")]
    [InlineData("2024-01-01T10:00:00+23:59", "2023-12-31T10:01:00Z")]
    public void DatetimeIsWrittenWithAnOffset(string input, string output)
    {
        LiteralDatetime literal = Datetime(input);

        Assert.Equal(Datetime(output).Value, literal.Value);
        Assert.Equal(output, ValueText(PureQLJson.Serialize(literal)));
    }

    [Theory]
    [InlineData("0001-01-01T00:00:00+00:01")]
    [InlineData("9999-12-31T23:59:59-00:01")]
    [InlineData("2024-01-01T00:00:00.1234567891Z")]
    [InlineData("2024-01-01T00:00:00.12345678Z")]
    [InlineData("2024-01-01T00:00:00.Z")]
    [InlineData("2024-01-01T00:00:00+24:00")]
    [InlineData("2024-01-01T00:00:00+05:60")]
    [InlineData("2024-01-01T00:00:00-00:00")]
    [InlineData("2024-01-01T00:00:00 +05:00")]
    [InlineData("2024-01-01T00:00:00")]
    [InlineData("2024-01-01X00:00:00Z")]
    [InlineData("2024-01-01T00:00Z")]
    public void DatetimeOutsideTheFormatOrRangeIsRejected(string input)
    {
        _ = Assert.Throws<JsonException>(() => Datetime(input));
    }

    [Theory]
    [InlineData("00:00:00", "00:00:00")]
    [InlineData("09:30:15.5", "09:30:15.5")]
    [InlineData("23:59:59.999999900", "23:59:59.9999999")]
    [InlineData("12:00:00.000000000", "12:00:00")]
    public void TimeKeepsSignificantFractionDigits(string input, string output)
    {
        LiteralTime literal = PureQLJson.Deserialize<LiteralTime>(
            Json("time", $"\"{input}\"")
        );

        Assert.Equal(Json("time", $"\"{output}\""), PureQLJson.Serialize(literal));
    }

    [Theory]
    [InlineData("23:59:59.999999999")]
    [InlineData("24:00:00")]
    [InlineData("12:60:00")]
    [InlineData("12:00:60")]
    [InlineData("12-00-00")]
    [InlineData("12:00:00.")]
    [InlineData("1:00:00")]
    public void TimeOutsideTheFormatOrPrecisionIsRejected(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralTime>(Json("time", $"\"{input}\""))
        );
    }

    [Theory]
    [InlineData("0000-01-01")]
    [InlineData("2023-02-29")]
    [InlineData("2024-13-01")]
    [InlineData("2024-00-01")]
    [InlineData("2024-01-00")]
    [InlineData("2024/01/01")]
    [InlineData("2024-1-01")]
    public void DateOutsideTheFormatOrRangeIsRejected(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralDate>(Json("date", $"\"{input}\""))
        );
    }

    [Fact]
    public void DateIsWrittenWithFourDigitYear()
    {
        LiteralDate literal = PureQLJson.Deserialize<LiteralDate>(
            Json("date", "\"0042-02-28\"")
        );

        Assert.Equal(new DateOnly(42, 2, 28), literal.Value);
        Assert.Equal(Json("date", "\"0042-02-28\""), PureQLJson.Serialize(literal));
    }

    [Fact]
    public void UuidIsWrittenInLowerCase()
    {
        LiteralUuid literal = PureQLJson.Deserialize<LiteralUuid>(
            Json("uuid", "\"3F2A6C1E-8B4D-4E2A-9C1F-1A2B3C4D5E6F\"")
        );

        Assert.Equal(
            Json("uuid", "\"3f2a6c1e-8b4d-4e2a-9c1f-1a2b3c4d5e6f\""),
            PureQLJson.Serialize(literal)
        );
    }

    [Theory]
    [InlineData("3f2a6c1e-8b4d-4e2a-9c1f-1a2b3c4d5e6")]
    [InlineData("3f2a6c1e+8b4d-4e2a-9c1f-1a2b3c4d5e6f")]
    [InlineData("3f2a6c1e-8b4d-4e2a-9c1f-1a2b3c4d5e6g")]
    public void MalformedUuidIsRejected(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralUuid>(Json("uuid", $"\"{input}\""))
        );
    }

    [Theory]
    [InlineData("5", 5L)]
    [InlineData("5.0", 5L)]
    [InlineData("-9223372036854775808", long.MinValue)]
    [InlineData("1e3", 1000L)]
    public void IntegerAcceptsIntegralNumbers(string input, long value)
    {
        LiteralInteger literal = PureQLJson.Deserialize<LiteralInteger>(
            Json("integer", input)
        );

        Assert.Equal(value, literal.Value);
    }

    [Theory]
    [InlineData("5.5")]
    [InlineData("9223372036854775808")]
    [InlineData("1e30")]
    [InlineData("\"5\"")]
    public void IntegerRejectsOtherValues(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralInteger>(Json("integer", input))
        );
    }

    [Theory]
    [InlineData("1.50", "1.50")]
    [InlineData("1.5e-07", "0.00000015")]
    [InlineData("-3", "-3")]
    public void DecimalKeepsItsValue(string input, string output)
    {
        LiteralDecimal literal = PureQLJson.Deserialize<LiteralDecimal>(
            Json("decimal", input)
        );

        Assert.Equal(Json("decimal", output), PureQLJson.Serialize(literal));
    }

    [Theory]
    [InlineData("1e400")]
    [InlineData("\"1.5\"")]
    public void DecimalRejectsOtherValues(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralDecimal>(Json("decimal", input))
        );
    }

    [Fact]
    public void NullableLiteralIsWrittenWithNullValue()
    {
        LiteralStringNullable literal = PureQLJson.Deserialize<LiteralStringNullable>(
            /*lang=json,strict*/
            """{"type":{"name":"string","nullable":true},"value":null}"""
        );

        Assert.Equal(
            /*lang=json,strict*/
            """{"type":{"name":"string","nullable":true},"value":null}""",
            PureQLJson.Serialize(literal)
        );
    }

    private static string ValueText(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("value").GetString()!;
    }

    private static LiteralDatetime Datetime(string value)
    {
        return PureQLJson.Deserialize<LiteralDatetime>(Json("datetime", $"\"{value}\""));
    }

    private static string Json(string type, string value)
    {
        return $$"""{"type":{"name":"{{type}}"},"value":{{value}}}""";
    }
}
