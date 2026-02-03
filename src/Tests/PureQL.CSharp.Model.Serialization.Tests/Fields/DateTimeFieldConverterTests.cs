using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record DateTimeFieldConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeFieldConverterTests()
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
    public void ReadEntity()
    {
        const string expected = "test";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "entity": "{{expected}}",
              "field": "test"
            }
            """;

        DateTimeField field = JsonSerializer.Deserialize<DateTimeField>(input, _options)!;

        Assert.Equal(expected, field.Entity);
    }

    [Fact]
    public void ReadField()
    {
        const string expected = "test";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "entity": "test",
              "field": "{{expected}}"
            }
            """;

        DateTimeField field = JsonSerializer.Deserialize<DateTimeField>(input, _options)!;

        Assert.Equal(expected, field.Field);
    }

    [Fact]
    public void Write()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "entity": "auiheyrdsnf",
                  "field": "jinaudferv",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
                """;

        string output = JsonSerializer.Serialize(
            new DateTimeField("auiheyrdsnf", "jinaudferv"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingEntityField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "field": "jinaudferv",
              "type": {
                "name": "datetimeArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeField>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingFieldField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "entity": "auiheyrdsnf",
              "type": {
                "name": "datetimeArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeField>(input, _options)
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
            JsonSerializer.Deserialize<DateTimeField>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("dateArray")]
    [InlineData("booleanArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("")]
    [InlineData("asdasdasdasd")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "entity": "auiheyrdsnf",
              "field": "jinaudferv"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeField>(input, _options)
        );
    }
}
