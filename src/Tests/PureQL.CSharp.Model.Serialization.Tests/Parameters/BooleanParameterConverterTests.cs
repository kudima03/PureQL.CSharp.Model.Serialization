using System.Text.Json;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Parameters;

public sealed record BooleanParameterConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new BooleanParameterConverter(),
            new TypeConverter<BooleanType>()
        }
    };

    [Fact]
    public void ReadName()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"boolean"},"name": "{{expected}}"}""";

        BooleanParameter parameter = JsonSerializer.Deserialize<BooleanParameter>(
            input,
            _options
        )!;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void Write()
    {
        const string expected = /*lang=json,strict*/
            """{"name":"auiheyrdsnf","type":{"name":"boolean"}}""";

        string output = JsonSerializer.Serialize(
            new BooleanParameter("auiheyrdsnf"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingNameField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"boolean"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanParameter>(input, _options)
        );
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanParameter>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"name": "auiheyrdsnf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"name": "auiheyrdsnf"}"""
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
            JsonSerializer.Deserialize<BooleanParameter>(input, _options)
        );
    }
}
