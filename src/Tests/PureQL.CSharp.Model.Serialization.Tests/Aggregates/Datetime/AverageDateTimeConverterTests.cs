using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.DateTime;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Datetime;

public sealed record AverageDateTimeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public AverageDateTimeConverterTests()
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
    public void ThrowsExceptionOnOperatorNameAbsence()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "euhwyrfdbuyeghrfdb",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_datetime"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_datetime",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_datetime",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgument()
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  {{formattedDates.First()}},
                  {{formattedDates.Skip(1).First()}},
                  {{formattedDates.Skip(2).First()}}
                ]
              }
            }
            """;

        AverageDateTime value = JsonSerializer.Deserialize<AverageDateTime>(
            input,
            _options
        )!;
        Assert.Equal(expected, value.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("datetime")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgument()
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  {{formattedDates.First()}},
                  {{formattedDates.Skip(1).First()}},
                  {{formattedDates.Skip(2).First()}}
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new AverageDateTime(
                new DateTimeArrayReturning(new DateTimeArrayScalar(expected))
            ),
            _options
        );
        Assert.Equal(expectedJson, value);
    }

    [Fact]
    public void ReadParameterArgument()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        AverageDateTime value = JsonSerializer.Deserialize<AverageDateTime>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeArrayParameter(expectedParamName), value.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("datetime")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterType(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgument()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_datetime",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetimeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new AverageDateTime(
                new DateTimeArrayReturning(new DateTimeArrayParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgument()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        AverageDateTime value = JsonSerializer.Deserialize<AverageDateTime>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeField(expectedEntityName, expectedFieldName),
            value.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("datetime")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldType(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDateTime>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgument()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "datetimeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new AverageDateTime(
                new DateTimeArrayReturning(
                    new DateTimeField(expectedEntityName, expectedFieldName)
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
