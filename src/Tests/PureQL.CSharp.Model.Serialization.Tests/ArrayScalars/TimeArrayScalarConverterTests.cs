using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record TimeArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public TimeArrayScalarConverterTests()
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
        IEnumerable<TimeOnly> expected =
        [
            new TimeOnly(14, 30, 15),
            new TimeOnly(15, 30, 15),
            new TimeOnly(14, 40, 16),
        ];

        IEnumerable<string> formattedTimes = expected.Select(x => x.ToString("HH:mm:ss"));

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "value": [
                "{{formattedTimes.First()}}",
                "{{formattedTimes.Skip(1).First()}}",
                "{{formattedTimes.Skip(2).First()}}"
              ]
            }
            """;

        ITimeArrayScalar scalar = JsonSerializer.Deserialize<ITimeArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        IEnumerable<TimeOnly> expectedValues =
        [
            new TimeOnly(14, 30, 15),
            new TimeOnly(15, 30, 15),
            new TimeOnly(14, 40, 16),
        ];

        IEnumerable<string> formattedTimes = expectedValues.Select(x =>
            x.ToString("HH:mm:ss")
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "value": [
                "{{formattedTimes.First()}}",
                "{{formattedTimes.Skip(1).First()}}",
                "{{formattedTimes.Skip(2).First()}}"
              ]
            }
            """;

        string output = JsonSerializer.Serialize<ITimeArrayScalar>(
            new TimeArrayScalar(expectedValues),
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
                "name": "timeArray"
              },
              "value": ["2000-01-01-01"]
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "timeArray"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "timeArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeArrayScalar>(input, _options)
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("uuidArray")]
    [InlineData("rsdftghbstgh")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "value": [
                "14:30"
              ]
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ITimeArrayScalar>(input, _options)
        );
    }
}
