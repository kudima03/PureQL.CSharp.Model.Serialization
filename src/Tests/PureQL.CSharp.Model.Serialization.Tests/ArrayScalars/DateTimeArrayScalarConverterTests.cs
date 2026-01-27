using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record DateTimeArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeArrayScalarConverterTests()
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
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x => x.ToString("O"));

        string input = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        IDateTimeArrayScalar scalar = JsonSerializer.Deserialize<IDateTimeArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        IEnumerable<DateTime> expectedValues =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expectedValues.Select(x => x.ToString("O"));

        string expected = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        string output = JsonSerializer.Serialize<IDateTimeArrayScalar>(
            new DateTimeArrayScalar(expectedValues),
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
                "name": "datetimeArray"
              },
              "value": "35.16.10000 25:00:00"
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "datetimeArray"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "datetimeArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeArrayScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
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
              "value": ["22.12.2025 9:52:31"]
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateTimeArrayScalar>(input, _options)
        );
    }
}
