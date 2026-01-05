using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Types;

public sealed record StringTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringTypeConverterTests()
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
              "name": "string"
            }
            """;

        StringType type = JsonSerializer.Deserialize<StringType>(input, _options)!;

        Assert.Equal(type.Name, new StringType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new StringType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """
            {
              "name": "string"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("uuid")]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "name": "{{type}}"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringType>(input, _options)
        );
    }
}
