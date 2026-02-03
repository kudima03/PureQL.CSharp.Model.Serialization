using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record BooleanArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanArrayScalarConverterTests()
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
              "type": {
                "name": "booleanArray"
              },
              "value": [
                true,
                false,
                true
              ]
            }
            """;

        IBooleanArrayScalar scalar = JsonSerializer.Deserialize<IBooleanArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal([true, false, true], scalar.Value);
    }

    [Fact]
    public void Write()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "booleanArray"
                  },
                  "value": [
                    true,
                    false,
                    true
                  ]
                }
                """;

        string output = JsonSerializer.Serialize<IBooleanArrayScalar>(
            new BooleanArrayScalar([true, false, true]),
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
                "name": "booleanArray"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "booleanArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanArrayScalar>(input, _options)
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
            JsonSerializer.Deserialize<IBooleanArrayScalar>(input, _options)
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
    [InlineData("dateArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("stringArray")]
    [InlineData("numberArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "value": [
                true,
                false,
                true
              ]
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IBooleanArrayScalar>(input, _options)
        );
    }
}
