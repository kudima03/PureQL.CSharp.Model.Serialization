using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record DateScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new DateScalarConverter(), new TypeConverter<DateType>() },
    };

    [Fact]
    public void Read()
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);

        string input = /*lang=json,strict*/
            $$"""
            {"type":{"name":"date"},"value":"{{expectedDate:yyyy-MM-dd}}"}
            """;

        IDateScalar scalar = JsonSerializer.Deserialize<IDateScalar>(input, _options)!;

        Assert.Equal(expectedDate, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);

        string expected = /*lang=json,strict*/
            $$"""
            {"type":{"name":"date"},"value":"{{expectedDate:yyyy-MM-dd}}"}
            """;

        string output = JsonSerializer.Serialize<IDateScalar>(
            new DateScalar(DateOnly.FromDateTime(DateTime.Now)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value":"2000-01-01-01"}"""
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"datetime"},"value":""}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"datetime"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value":"2000-01-01"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":"2000-01-01"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
        );
    }
}
