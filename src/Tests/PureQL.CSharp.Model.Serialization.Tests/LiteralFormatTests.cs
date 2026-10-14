using System.Text.Json;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record LiteralFormatTests
{
    [Theory]
    [InlineData("2024-01-31T18:30:00Z")]
    [InlineData("2024-02-01T00:00:00.123456Z")]
    [InlineData("2024-02-01T00:00:00.5Z")]
    [InlineData("0001-01-01T00:00:00Z")]
    [InlineData("9999-12-31T23:59:59.999999Z")]
    public void DatetimeIsReadAndWrittenInUtc(string text)
    {
        LiteralDatetime literal = Datetime(text);

        Assert.Equal(TimeSpan.Zero, literal.Value.Offset);
        Assert.Equal(text, ValueText(PureQLJson.Serialize(literal)));
    }

    [Fact]
    public void DatetimeWithAnOffsetIsWrittenInUtc()
    {
        LiteralDatetime literal = new LiteralDatetime(
            new DateTimeOffset(2024, 1, 31, 18, 30, 0, TimeSpan.FromHours(3))
        );

        Assert.Equal("2024-01-31T15:30:00Z", ValueText(PureQLJson.Serialize(literal)));
    }

    [Theory]
    [InlineData("2024-01-31T18:30:00+03:00")]
    [InlineData("2024-01-01T00:00:00+00:00")]
    [InlineData("2024-01-01T00:00:00-00:00")]
    [InlineData("0000-01-01T00:00:00Z")]
    [InlineData("2024-01-01T00:00:00.1234567Z")]
    [InlineData("2024-01-01T00:00:00.1234567891Z")]
    [InlineData("2024-01-01T00:00:00.50Z")]
    [InlineData("2024-01-01T00:00:00.000000Z")]
    [InlineData("2024-01-01T00:00:00.Z")]
    [InlineData("2024-01-01T00:00:00 Z")]
    [InlineData("2024-01-01T00:00:00")]
    [InlineData("2024-01-01X00:00:00Z")]
    [InlineData("2024-01-01T00:00Z")]
    [InlineData("2024-01-01t00:00:00Z")]
    [InlineData("2024-01-01T24:00:00Z")]
    public void DatetimeOutsideTheFormatOrRangeIsRejected(string input)
    {
        _ = Assert.Throws<JsonException>(() => Datetime(input));
    }

    [Fact]
    public void DatetimeFinerThanAMicrosecondIsRejectedOnWrite()
    {
        LiteralDatetime literal = new LiteralDatetime(
            new DateTimeOffset(2024, 1, 31, 18, 30, 0, TimeSpan.Zero).AddTicks(1)
        );

        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(literal));
    }

    [Theory]
    [InlineData("00:00:00")]
    [InlineData("09:30:15.5")]
    [InlineData("09:30:15.000001")]
    [InlineData("23:59:59.999999")]
    public void TimeIsReadAndWrittenToTheMicrosecond(string text)
    {
        LiteralTime literal = PureQLJson.Deserialize<LiteralTime>(
            Json("time", $"\"{text}\"")
        );

        Assert.Equal(Json("time", $"\"{text}\""), PureQLJson.Serialize(literal));
    }

    [Theory]
    [InlineData("23:59:59.9999999")]
    [InlineData("23:59:59.999999999")]
    [InlineData("12:00:00.50")]
    [InlineData("12:00:00.000000")]
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

    [Fact]
    public void TimeFinerThanAMicrosecondIsRejectedOnWrite()
    {
        LiteralTime literal = new LiteralTime(
            new TimeOnly(12, 0, 0).Add(TimeSpan.FromTicks(1))
        );

        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(literal));
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
        LiteralUuid literal = new LiteralUuid(
            Guid.Parse("3F2A6C1E-8B4D-4E2A-9C1F-1A2B3C4D5E6F")
        );

        Assert.Equal(
            Json("uuid", "\"3f2a6c1e-8b4d-4e2a-9c1f-1a2b3c4d5e6f\""),
            PureQLJson.Serialize(literal)
        );
    }

    [Fact]
    public void UuidInUpperCaseIsRejected()
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralUuid>(
                Json("uuid", "\"3F2A6C1E-8B4D-4E2A-9C1F-1A2B3C4D5E6F\"")
            )
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
    [InlineData("0")]
    [InlineData("-3")]
    [InlineData("1.5")]
    [InlineData("-0.00000015")]
    [InlineData("9999999999999999999999999999")]
    [InlineData("0.9999999999999999999999999999")]
    [InlineData("-999999999999999999999999.9999")]
    public void DecimalIsReadAndWrittenAsAString(string text)
    {
        LiteralDecimal literal = PureQLJson.Deserialize<LiteralDecimal>(
            Json("decimal", $"\"{text}\"")
        );

        Assert.Equal(
            decimal.Parse(text, System.Globalization.CultureInfo.InvariantCulture),
            literal.Value
        );
        Assert.Equal(Json("decimal", $"\"{text}\""), PureQLJson.Serialize(literal));
    }

    [Theory]
    [InlineData("1.50", "1.5")]
    [InlineData("-0.0", "0")]
    [InlineData("100", "100")]
    public void DecimalIsWrittenWithoutTrailingZeros(string value, string text)
    {
        LiteralDecimal literal = new LiteralDecimal(
            decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture)
        );

        Assert.Equal(Json("decimal", $"\"{text}\""), PureQLJson.Serialize(literal));
    }

    [Theory]
    [InlineData("1.5")]
    [InlineData("1e3")]
    [InlineData("\"1.50\"")]
    [InlineData("\"01\"")]
    [InlineData("\"-0\"")]
    [InlineData("\"0.0\"")]
    [InlineData("\"1e3\"")]
    [InlineData("\"1.\"")]
    [InlineData("\".5\"")]
    [InlineData("\"+1\"")]
    [InlineData("\" 1\"")]
    [InlineData("\"0.\"")]
    [InlineData("\"\"")]
    [InlineData("\"-\"")]
    [InlineData("\"10000000000000000000000000000\"")]
    [InlineData("\"1.000000000000000000000000000001\"")]
    [InlineData("\"0.00000000000000000000000000001\"")]
    public void DecimalRejectsOtherSpellings(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<LiteralDecimal>(Json("decimal", input))
        );
    }

    [Fact]
    public void DecimalBeyondTwentyEightDigitsIsRejectedOnWrite()
    {
        LiteralDecimal literal = new LiteralDecimal(decimal.MaxValue);

        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(literal));
    }

    [Fact]
    public void ListLiteralRepeatingAValueIsRejected()
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<ListLiteralInteger>(
                /*lang=json,strict*/
                """{"type":{"name":"integerList"},"value":[1,2,1]}"""
            )
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
