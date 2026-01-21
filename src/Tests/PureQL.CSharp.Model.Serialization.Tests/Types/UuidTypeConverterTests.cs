using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Types;

public sealed record UuidTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidTypeConverterTests()
    {
        _options = new JsonSerializerOptions()
        {
            NewLine = "\n",
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        foreach (JsonConverter converter in new PureQLConverters())
        {
            _options.Converters.Add(converter);
        }
    }

    [Fact]
    public void Read()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "name": "uuid"
            }
            """;

        UuidType type = JsonSerializer.Deserialize<UuidType>(input, _options)!;

        Assert.Equal(type.Name, new UuidType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new UuidType(), _options);

        Assert.Equal(
            /*lang=json,strict*/
            """
            {
              "name": "uuid"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("time")]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "name": "{{type}}"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidType>(input, _options)
        );
    }
}
