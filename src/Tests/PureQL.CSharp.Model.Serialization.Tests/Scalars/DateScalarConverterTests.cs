using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record DateScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateScalarConverterTests()
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
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{expectedDate:yyyy-MM-dd}}"
            }
            """;

        IDateScalar scalar = JsonSerializer.Deserialize<IDateScalar>(input, _options)!;

        Assert.Equal(expectedDate, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{expectedDate:yyyy-MM-dd}}"
            }
            """;

        string output = JsonSerializer.Serialize<IDateScalar>(
            new DateScalar(DateOnly.FromDateTime(DateTime.Now)),
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
              "value": "2000-01-01-01"
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
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
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
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
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
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
              "value": "2000-01-01"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateScalar>(input, _options)
        );
    }
}
