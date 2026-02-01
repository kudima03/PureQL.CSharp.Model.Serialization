using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.DateTime;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Datetime;

public sealed record DateTimeAggregateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeAggregateConverterTests()
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
    public void ThrowsExceptionOnOperatorNameAbsenceOnAverage()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnAverage()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnAverage()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnAverage()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_datetime"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnAverage()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_datetime",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnAverage()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_datetime",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnAverage()
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
              "operator": "average_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeArrayScalar(expected), value.AsT0.Argument.AsT2);
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
    public void ThrowsExceptionOnWrongScalarTypeOnAverage(string type)
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverage()
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x => x.ToString("O"));

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeAggregate(
                new AverageDateTime(
                    new DateTimeArrayReturning(new DateTimeArrayScalar(expected))
                )
            ),
            _options
        );
        Assert.Equal(expectedJson, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverage()
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

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeArrayParameter(expectedParamName),
            value.AsT0.Argument.AsT0
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
    public void ThrowsExceptionOnWrongParameterTypeOnAverage(string type)
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnAverage()
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
            new DateTimeAggregate(
                new AverageDateTime(
                    new DateTimeArrayReturning(
                        new DateTimeArrayParameter(expectedParamName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnAverage()
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

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeField(expectedEntityName, expectedFieldName),
            value.AsT0.Argument.AsT1
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
    public void ThrowsExceptionOnWrongFieldTypeOnAverage(string type)
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnAverage()
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
            new DateTimeAggregate(
                new AverageDateTime(
                    new DateTimeArrayReturning(
                        new DateTimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnMax()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnMax()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnMax()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMax()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_datetime"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMax()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_datetime",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMax()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_datetime",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMax()
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
              "operator": "max_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeArrayScalar(expected), value.AsT1.Argument.AsT2);
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
    public void ThrowsExceptionOnWrongScalarTypeOnMax(string type)
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
              "operator": "max_datetime",
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMax()
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x => x.ToString("O"));

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeAggregate(
                new MaxDateTime(
                    new DateTimeArrayReturning(new DateTimeArrayScalar(expected))
                )
            ),
            _options
        );
        Assert.Equal(expectedJson, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMax()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_datetime",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeArrayParameter(expectedParamName),
            value.AsT1.Argument.AsT0
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
    public void ThrowsExceptionOnWrongParameterTypeOnMax(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_datetime",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMax()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_datetime",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetimeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeAggregate(
                new MaxDateTime(
                    new DateTimeArrayReturning(
                        new DateTimeArrayParameter(expectedParamName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMax()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_datetime",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeField(expectedEntityName, expectedFieldName),
            value.AsT1.Argument.AsT1
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
    public void ThrowsExceptionOnWrongFieldTypeOnMax(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_datetime",
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMax()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_datetime",
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
            new DateTimeAggregate(
                new MaxDateTime(
                    new DateTimeArrayReturning(
                        new DateTimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnMin()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnMin()
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

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnMin()
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMin()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_datetime"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMin()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_datetime",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMin()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_datetime",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMin()
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
              "operator": "min_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeArrayScalar(expected), value.AsT2.Argument.AsT2);
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
    public void ThrowsExceptionOnWrongScalarTypeOnMin(string type)
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
              "operator": "min_datetime",
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMin()
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x => x.ToString("O"));

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
              "arg": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  "{{formattedDates.First()}}",
                  "{{formattedDates.Skip(1).First()}}",
                  "{{formattedDates.Skip(2).First()}}"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeAggregate(
                new MinDateTime(
                    new DateTimeArrayReturning(new DateTimeArrayScalar(expected))
                )
            ),
            _options
        );
        Assert.Equal(expectedJson, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeArrayParameter(expectedParamName),
            value.AsT2.Argument.AsT0
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
    public void ThrowsExceptionOnWrongParameterTypeOnMin(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_datetime",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetimeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeAggregate(
                new MinDateTime(
                    new DateTimeArrayReturning(
                        new DateTimeArrayParameter(expectedParamName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMin()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "datetimeArray"
                  }
                }
            }
            """;

        DateTimeAggregate value = JsonSerializer.Deserialize<DateTimeAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new DateTimeField(expectedEntityName, expectedFieldName),
            value.AsT2.Argument.AsT1
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
    public void ThrowsExceptionOnWrongFieldTypeOnMin(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
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
            JsonSerializer.Deserialize<DateTimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMin()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_datetime",
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
            new DateTimeAggregate(
                new MinDateTime(
                    new DateTimeArrayReturning(
                        new DateTimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
