using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record DateTimeScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new DateTimeScalarConverter(), new TypeConverter<DateTimeType>() },
    };

    [Fact]
    public void Read()
    {
        DateTime expected = DateTime.Now;

        string input = /*lang=json,strict*/
            $$"""
            {"type":{"name":"datetime"},"value":"{{expected:O}}"}
            """;

        IDateTimeScalar scalar = JsonSerializer.Deserialize<IDateTimeScalar>(
            input,
            _options
        )!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        DateTime expectedValue = DateTime.Now;

        string expected = /*lang=json,strict*/
        $$"""
            {"type":{"name":"datetime"},"value":{{JsonSerializer.Serialize(
                expectedValue
            )}}}
            """;

        string output = JsonSerializer.Serialize<IDateTimeScalar>(
            new DateTimeScalar(expectedValue),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnBadFormat()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"datetime"},"value":"35.16.10000 25:00:00"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"datetime"},"value":""}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"datetime"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value":"22.12.2025 9:52:31"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":"22.12.2025 9:52:31"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }
}
