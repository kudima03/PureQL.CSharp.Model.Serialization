using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Time;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

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
                    "name": "time"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "time"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
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
                    "name": "time"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
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
    public void ThrowsExceptionOnNullArgument()
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
    public void ThrowsExceptionOnArgumentWrongType()
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
    public void ReadScalarArgumentOnMinTime()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                  "type": {
                    "name": "time"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeScalar(now), value.AsT1.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMinTime(string type)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMinTime()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(new MinTime(new TimeReturning(new TimeScalar(now)))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMinTime()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "time"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeParameter(expectedParamName), value.AsT1.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnMinTime(string type)
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
    public void WriteParameterArgumentOnMinTime()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_time",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MinTime(new TimeReturning(new TimeParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMinTime()
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
                    "name": "time"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(
            new TimeField(expectedEntityName, expectedFieldName),
            value.AsT1.Argument.AsT0
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
    public void ThrowsExceptionOnWrongFieldTypeOnMinTime(string type)
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
    public void WriteFieldArgumentOnMinTime()
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
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MinTime(
                    new TimeReturning(
                        new TimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnAverageTime()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                  "type": {
                    "name": "time"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeScalar(now), value.AsT2.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnAverageTime(string type)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverageTime()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(new AverageTime(new TimeReturning(new TimeScalar(now)))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverageTime()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "time"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeParameter(expectedParamName), value.AsT2.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnAverageTime(string type)
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
    public void WriteParameterArgumentOnAverageTime()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_time",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new AverageTime(new TimeReturning(new TimeParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnAverageTime()
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
                    "name": "time"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(
            new TimeField(expectedEntityName, expectedFieldName),
            value.AsT2.Argument.AsT0
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
    public void ThrowsExceptionOnWrongFieldTypeOnAverageTime(string type)
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
    public void WriteFieldArgumentOnAverageTime()
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
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new AverageTime(
                    new TimeReturning(
                        new TimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnMaxDate()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                  "type": {
                    "name": "time"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeScalar(now), value.AsT0.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMaxDate(string type)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMaxDate()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(new MaxTime(new TimeReturning(new TimeScalar(now)))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMaxDate()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_time",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "time"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(new TimeParameter(expectedParamName), value.AsT0.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("date")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnMaxDate(string type)
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
    public void WriteParameterArgumentOnMaxDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_time",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MaxTime(new TimeReturning(new TimeParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMaxDate()
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
                    "name": "time"
                  }
                }
            }
            """;

        TimeAggregate value = JsonSerializer.Deserialize<TimeAggregate>(input, _options)!;
        Assert.Equal(
            new TimeField(expectedEntityName, expectedFieldName),
            value.AsT0.Argument.AsT0
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
    public void ThrowsExceptionOnWrongFieldTypeOnMaxDate(string type)
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
    public void WriteFieldArgumentOnMaxDate()
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
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeAggregate(
                new MaxTime(
                    new TimeReturning(
                        new TimeField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
