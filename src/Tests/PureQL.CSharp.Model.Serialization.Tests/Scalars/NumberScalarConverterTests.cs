using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record NumberScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new NumberScalarConverter(), new TypeConverter<NumberType>() }
    };

    [Fact]
    public void Read()
    {
        string input = /*lang=json,strict*/
            """
            {"type":{"name":"number"},"value":0.5800537796011547}
            """;

        INumberScalar scalar = JsonSerializer.Deserialize<INumberScalar>(
            input,
            _options
        )!;

        Assert.Equal(0.5800537796011547, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {"type":{"name":"number"},"value":0.5800537796011547}
            """;

        string output = JsonSerializer.Serialize<INumberScalar>(
            new NumberScalar(0.5800537796011547),
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
        """{"type":{"name":"number"},"value":"35.16.10000"}"""
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"number"},"value":""}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"number"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value":10.12345}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value":10.12345}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":10.12345}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value":10.12345}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":10.12345}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value":10.12345}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":10.12345}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberScalar>(input, _options)
        );
    }
}
