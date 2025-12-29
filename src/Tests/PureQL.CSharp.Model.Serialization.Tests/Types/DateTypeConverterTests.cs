using System.Text.Json;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Types;

public sealed record DateTypeConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new TypeConverter<DateType>() },
    };

    [Fact]
    public void Read()
    {
        const string input = /*lang=json,strict*/
            """{"name":"date"}""";

        DateType type = JsonSerializer.Deserialize<DateType>(input, _options)!;

        Assert.Equal(type.Name, new DateType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new DateType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """{"name":"date"}""",
            output
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"name":"boolean"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"datetime"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"null"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"number"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"string"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"time"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name":"uuid"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }
}
