using System.Text.Json;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Parameters;

public sealed record NumberParameterConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new NumberParameterConverter(), new TypeConverter<NumberType>() }
    };

    [Fact]
    public void ReadName()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"number"},"name": "{{expected}}"}""";

        NumberParameter parameter = JsonSerializer.Deserialize<NumberParameter>(
            input,
            _options
        )!;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void Write()
    {
        const string expected = /*lang=json,strict*/
            """{"name":"auiheyrdsnf","type":{"name":"number"}}""";

        string output = JsonSerializer.Serialize(
            new NumberParameter("auiheyrdsnf"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingNameField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"number"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberParameter>(input, _options)
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
            JsonSerializer.Deserialize<NumberParameter>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name": "auiheyrdsnf"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberParameter>(input, _options)
        );
    }
}
