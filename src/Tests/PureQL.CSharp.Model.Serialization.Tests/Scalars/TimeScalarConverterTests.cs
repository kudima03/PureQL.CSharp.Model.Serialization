using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record TimeScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new TimeScalarConverter(), new TypeConverter<TimeType>() }
    };

    [Fact]
    public void Read()
    {
        TimeOnly expected = new TimeOnly(14, 30, 15);

        string input = /*lang=json,strict*/
            $$"""
            {"type":{"name":"time"},"value":"{{expected:HH:mm:ss}}"}
            """;

        ITimeScalar scalar = JsonSerializer.Deserialize<ITimeScalar>(input, _options)!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        TimeOnly expected = new TimeOnly(14, 30, 15);

        string expectedJson = /*lang=json,strict*/
            $$"""
            {"type":{"name":"time"},"value":"{{expected:HH:mm:ss}}"}
            """;

        string output = JsonSerializer.Serialize<ITimeScalar>(
            new TimeScalar(expected),
            _options
        );

        Assert.Equal(expectedJson, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(""" """)]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":"2000-01-01-01"}"""
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"time"},"value":""}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"time"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value":"14:30"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value":"14:30"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":"14:30"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value":"14:30"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value":"14:30"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value":"14:30"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":"14:30"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }
}
