using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record BooleanScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanScalarConverterTests()
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
    public void ReadTrue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        IBooleanScalar scalar = JsonSerializer.Deserialize<IBooleanScalar>(
            input,
            _options
        )!;

        Assert.True(scalar.Value);
    }

    [Fact]
    public void ReadFalse()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": false
            }
            """;

        IBooleanScalar scalar = JsonSerializer.Deserialize<IBooleanScalar>(
            input,
            _options
        )!;

        Assert.False(scalar.Value);
    }

    [Fact]
    public void WriteTrue()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
                """;

        string output = JsonSerializer.Serialize<IBooleanScalar>(
            new BooleanScalar(true),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void WriteFalse()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
                """;

        string output = JsonSerializer.Serialize<IBooleanScalar>(
            new BooleanScalar(false),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
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
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
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
              "value": true
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanScalar>(input, _options)
        );
    }
}
