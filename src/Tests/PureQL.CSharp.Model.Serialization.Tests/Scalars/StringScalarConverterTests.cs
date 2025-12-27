using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record StringScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new StringScalarConverter(), new TypeConverter<StringType>() }
    };

    [Fact]
    public void Read()
    {
        string input = /*lang=json,strict*/
            """
            {"type":{"name":"string"},"value":"ianhuedrfiuhaerfd"}
            """;

        IStringScalar scalar = JsonSerializer.Deserialize<IStringScalar>(
            input,
            _options
        )!;

        Assert.Equal("ianhuedrfiuhaerfd", scalar.Value);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {"type":{"name":"string"},"value":"adsihuowbfohuasdfipsduF"}
            """;

        string output = JsonSerializer.Serialize<IStringScalar>(
            new StringScalar("adsihuowbfohuasdfipsduF"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"string"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IStringScalar>(input, _options)
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
            JsonSerializer.Deserialize<IStringScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value":"faijdhnjikabngf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value":"faijdhnjikabngf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":"faijdhnjikabngf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value":"faijdhnjikabngf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":"faijdhnjikabngf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value":"faijdhnjikabngf"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":"faijdhnjikabngf"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IStringScalar>(input, _options)
        );
    }
}
