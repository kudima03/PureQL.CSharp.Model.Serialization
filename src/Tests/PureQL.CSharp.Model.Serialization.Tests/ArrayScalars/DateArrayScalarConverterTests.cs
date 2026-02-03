using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record DateArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateArrayScalarConverterTests()
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
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": [
                "{{formattedDates.First()}}",
                "{{formattedDates.Skip(1).First()}}",
                "{{formattedDates.Skip(2).First()}}"
              ]
            }
            """;

        IDateArrayScalar scalar = JsonSerializer.Deserialize<IDateArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(expectedDates, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": [
                "{{formattedDates.First()}}",
                "{{formattedDates.Skip(1).First()}}",
                "{{formattedDates.Skip(2).First()}}"
              ]
            }
            """;

        string output = JsonSerializer.Serialize<IDateArrayScalar>(
            new DateArrayScalar(expectedDates),
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
                "name": "dateArray"
              },
              "value": [
                "2000-01-01-01"
              ]
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "dateArray"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "dateArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateArrayScalar>(input, _options)
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
    [InlineData("booleanArray")]
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
                "2000-01-01"
              ]
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IDateArrayScalar>(input, _options)
        );
    }
}
