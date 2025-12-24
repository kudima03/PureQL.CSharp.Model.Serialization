using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record UuidScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new UuidScalarConverter(), new TypeConverter<UuidType>() },
    };

    [Fact]
    public void Read()
    {
        Guid expected = Guid.NewGuid();

        string input = /*lang=json,strict*/
            $$"""
            {"type":{"name":"uuid"},"value":"{{expected}}"}
            """;

        IUuidScalar scalar = JsonSerializer.Deserialize<IUuidScalar>(input, _options)!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        Guid expected = Guid.NewGuid();

        string expectedJson = /*lang=json,strict*/
            $$"""
            {"type":{"name":"uuid"},"value":"{{expected}}"}
            """;

        string output = JsonSerializer.Serialize<IUuidScalar>(
            new UuidScalar(expected),
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
        """{"type":{"name":"uuid"},"value":"afdkjgnhajlkhisfdbng"}"""
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"uuid"},"value":""}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """{"type":{"name":"uuid"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value":"86246f01-f5be-4925-a699-ed1f988b0a7c"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }
}
