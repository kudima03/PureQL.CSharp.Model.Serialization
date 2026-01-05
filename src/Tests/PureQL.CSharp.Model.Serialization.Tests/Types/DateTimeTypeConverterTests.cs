using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Types;

public sealed record DateTimeTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeTypeConverterTests()
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
              "name": "datetime"
            }
            """;

        DateTimeType type = JsonSerializer.Deserialize<DateTimeType>(input, _options)!;

        Assert.Equal(type.Name, new DateTimeType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new DateTimeType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """
            {
              "name": "datetime"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("uuid")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("date")]
    [InlineData("string")]
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
            JsonSerializer.Deserialize<DateTimeType>(input, _options)
        );
    }
}
