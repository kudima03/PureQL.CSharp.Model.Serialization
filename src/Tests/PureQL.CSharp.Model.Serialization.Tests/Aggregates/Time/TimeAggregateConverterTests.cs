using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Time;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Time;

public sealed record TimeAggregateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public TimeAggregateConverterTests()
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnAverage()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnAverage()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_time",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnAverage()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_time",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnAverage()
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

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(expectedValues, value.AsT2.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnAverage(string type)
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

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverage()
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
              "operator": "average_time",
              "arg": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new AverageTime(
                    new TimeArrayReturning(new TimeArrayScalar(expectedValues))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverage()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeArrayParameter(expectedParamName), value.AsT2.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    public void ThrowsExceptionOnWrongParameterTypeOnAverage(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnAverage()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_time",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new AverageTime(
                    new TimeArrayReturning(new TimeArrayParameter(expectedParamName))
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
              "operator": "average_time",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(
            new TimeField(expectedEntityName, expectedFieldName),
            value.AsT2.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    public void ThrowsExceptionOnWrongFieldTypeOnAverage(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
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
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
              "operator": "average_time",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new AverageTime(
                    new TimeArrayReturning(
                        new TimeField(expectedEntityName, expectedFieldName)
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMin()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMin()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_time",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMin()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_time",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMin()
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

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(expectedValues, value.AsT1.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMin(string type)
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

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMin()
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
              "operator": "min_time",
              "arg": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MinTime(new TimeArrayReturning(new TimeArrayScalar(expectedValues)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeArrayParameter(expectedParamName), value.AsT1.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMin(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_time",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MinTime(
                    new TimeArrayReturning(new TimeArrayParameter(expectedParamName))
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
              "operator": "min_time",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(
            new TimeField(expectedEntityName, expectedFieldName),
            value.AsT1.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnMin(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
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
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
              "operator": "min_time",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MinTime(
                    new TimeArrayReturning(
                        new TimeField(expectedEntityName, expectedFieldName)
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
                    "name": "timeArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMax()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMax()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_time",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMax()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_time",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMax()
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

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(expectedValues, value.AsT0.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMax(string type)
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

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMax()
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
              "operator": "max_time",
              "arg": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{formattedTimes.First()}}",
                  "{{formattedTimes.Skip(1).First()}}",
                  "{{formattedTimes.Skip(2).First()}}"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MaxTime(new TimeArrayReturning(new TimeArrayScalar(expectedValues)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMax()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeArrayParameter(expectedParamName), value.AsT0.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMax(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMax()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_time",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MaxTime(
                    new TimeArrayReturning(new TimeArrayParameter(expectedParamName))
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
              "operator": "max_time",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "timeArray"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(
            new TimeField(expectedEntityName, expectedFieldName),
            value.AsT0.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("dateArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnMax(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
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
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
              "operator": "max_time",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MaxTime(
                    new TimeArrayReturning(
                        new TimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
