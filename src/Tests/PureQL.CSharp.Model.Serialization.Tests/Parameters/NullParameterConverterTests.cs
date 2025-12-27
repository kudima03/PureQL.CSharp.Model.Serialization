using System.Text.Json;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Parameters;

public sealed record NullParameterConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new NullParameterConverter(), new TypeConverter<NullType>() }
    };

    [Fact]
    public void ReadName()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"null"},"name": "{{expected}}"}""";

        NullParameter parameter = JsonSerializer.Deserialize<NullParameter>(
            input,
            _options
        )!;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void Write()
    {
        const string expected = /*lang=json,strict*/
            """{"name":"auiheyrdsnf","type":{"name":"null"}}""";

        string output = JsonSerializer.Serialize(
            new NullParameter("auiheyrdsnf"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingNameField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"null"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NullParameter>(input, _options)
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
            JsonSerializer.Deserialize<NullParameter>(input, _options)
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
            JsonSerializer.Deserialize<NullParameter>(input, _options)
        );
    }
}
