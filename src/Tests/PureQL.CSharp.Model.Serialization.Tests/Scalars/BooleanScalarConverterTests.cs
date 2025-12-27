using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record BooleanScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new BooleanScalarConverter(), new TypeConverter<BooleanType>() }
    };

    [Fact]
    public void ReadTrue()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"boolean"},"value": true}""";

        IBooleanScalar scalar = JsonSerializer.Deserialize<IBooleanScalar>(
            input,
            _options
        )!;

        Assert.True(scalar.Value);
    }

    [Fact]
    public void ReadFalse()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"boolean"},"value": false}""";

        IBooleanScalar scalar = JsonSerializer.Deserialize<IBooleanScalar>(
            input,
            _options
        )!;

        Assert.False(scalar.Value);
    }

    [Fact]
    public void WriteTrue()
    {
        const string expected =
            /*lang=json,strict*/
            """{"type":{"name":"boolean"},"value":true}""";

        string output = JsonSerializer.Serialize<IBooleanScalar>(
            new BooleanScalar(true),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void WriteFalse()
    {
        const string expected =
            /*lang=json,strict*/
            """{"type":{"name":"boolean"},"value":false}""";

        string output = JsonSerializer.Serialize<IBooleanScalar>(
            new BooleanScalar(false),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"boolean"},"value":""}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"boolean"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(""" """)]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value":true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":true}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }
}
