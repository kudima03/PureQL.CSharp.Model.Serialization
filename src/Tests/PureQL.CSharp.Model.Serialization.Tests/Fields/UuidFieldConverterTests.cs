using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record UuidFieldConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidFieldConverterTests()
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
                "name": "uuid"
              },
              "entity": "{{expected}}",
              "field": "test"
            }
            """;

        UuidField field = JsonSerializer.Deserialize<UuidField>(input, _options)!;

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
                "name": "uuid"
              },
              "entity": "test",
              "field": "{{expected}}"
            }
            """;

        UuidField field = JsonSerializer.Deserialize<UuidField>(input, _options)!;

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
                    "name": "uuid"
                  }
                }
                """;

        string output = JsonSerializer.Serialize(
            new UuidField("auiheyrdsnf", "jinaudferv"),
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
                "name": "uuid"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidField>(input, _options)
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
                "name": "uuid"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidField>(input, _options)
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
            JsonSerializer.Deserialize<UuidField>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("")]
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
            JsonSerializer.Deserialize<UuidField>(input, _options)
        );
    }
}
