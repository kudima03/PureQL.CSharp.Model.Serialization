using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record NullScalarConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new NullScalarConverter(), new TypeConverter<NullType>() },
    };

    [Fact]
    public void Read()
    {
        string input = /*lang=json,strict*/
            """
            {"type":{"name":"null"}}
            """;

        INullScalar scalar = JsonSerializer.Deserialize<INullScalar>(input, _options)!;

        Assert.Equal(new NullScalar(), scalar);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {"type":{"name":"null"}}
            """;

        string output = JsonSerializer.Serialize<INullScalar>(new NullScalar(), _options);

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"dateTime"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"}}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"}}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INullScalar>(input, _options)
        );
    }
}
