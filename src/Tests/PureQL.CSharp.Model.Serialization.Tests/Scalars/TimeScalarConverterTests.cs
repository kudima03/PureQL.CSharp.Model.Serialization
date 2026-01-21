using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record TimeScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public TimeScalarConverterTests()
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
        TimeOnly expected = new TimeOnly(14, 30, 15);

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "value": "{{expected:HH:mm:ss}}"
            }
            """;

        ITimeScalar scalar = JsonSerializer.Deserialize<ITimeScalar>(input, _options)!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        TimeOnly expected = new TimeOnly(14, 30, 15);

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "value": "{{expected:HH:mm:ss}}"
            }
            """;

        string output = JsonSerializer.Serialize<ITimeScalar>(
            new TimeScalar(expected),
            _options
        );

        Assert.Equal(expectedJson, output);
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
                "name": "time"
              },
              "value": "2000-01-01-01"
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "time"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "time"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("uuid")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "value": "14:30"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeScalar>(input, _options)
        );
    }
}
