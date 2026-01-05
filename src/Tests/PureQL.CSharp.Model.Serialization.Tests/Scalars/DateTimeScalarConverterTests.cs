using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record DateTimeScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeScalarConverterTests()
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
        DateTime expected = DateTime.Now;

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": "{{expected:O}}"
            }
            """;

        IDateTimeScalar scalar = JsonSerializer.Deserialize<IDateTimeScalar>(
            input,
            _options
        )!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        DateTime expectedValue = DateTime.Now;

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;

        string output = JsonSerializer.Serialize<IDateTimeScalar>(
            new DateTimeScalar(expectedValue),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    [InlineData( /*lang=json,strict*/
            """
            {
              "type": {
                "name": "datetime"
              },
              "value": "35.16.10000 25:00:00"
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "datetime"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "datetime"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("string")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "value": "22.12.2025 9:52:31"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeScalar>(input, _options)
        );
    }
}
