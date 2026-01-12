using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Date;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Date;

public sealed record DateAggregateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateAggregateConverterTests()
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
                    "name": "date"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnAverageDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnAverageDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnAverageDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnAverageDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "type": {
                    "name": "date"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateScalar(now), value.AsT2.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarTypeOnAverageDate(string type)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverageDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(new AverageDate(new DateReturning(new DateScalar(now)))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverageDate()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "date"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(value.AsT2.Argument.AsT1, new DateParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongParameterTypeOnAverageDate(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnAverageDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_date",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new AverageDate(new DateReturning(new DateParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnAverageDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "date"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(
            value.AsT2.Argument.AsT0,
            new DateField(expectedEntityName, expectedFieldName)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnAverageDate(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "entity": "{{expectedEntityName}}"
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnAverageDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new AverageDate(
                    new DateReturning(
                        new DateField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMaxDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMaxDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMaxDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMaxDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "type": {
                    "name": "date"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateScalar(now), value.AsT0.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("hiujerfndsa")]
    public void ThrowsExceptionOnWrongScalarTypeOnMaxDate(string type)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMaxDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(new MaxDate(new DateReturning(new DateScalar(now)))),
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
              "operator": "max_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "date"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateParameter(expectedParamName), value.AsT0.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("eharuinjfg")]
    public void ThrowsExceptionOnWrongParameterTypeOnMaxDate(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMaxDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_date",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MaxDate(new DateReturning(new DateParameter(expectedParamName)))
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
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "date"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(
            value.AsT0.Argument.AsT0,
            new DateField(expectedEntityName, expectedFieldName)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnMaxDate(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}"
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
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
              "operator": "max_date",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MaxDate(
                    new DateReturning(
                        new DateField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMinDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMinDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMinDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMinDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "type": {
                    "name": "date"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateScalar(now), value.AsT1.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("dfagijhnu")]
    public void ThrowsExceptionOnWrongScalarTypeOnMinDate(string type)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{JsonSerializer.Serialize(now, _options)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMinDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(new MinDate(new DateReturning(new DateScalar(now)))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMinDate()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "date"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateParameter(expectedParamName), value.AsT1.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("rfeagjmoi")]
    public void ThrowsExceptionOnWrongParameterTypeOnMinDate(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMinDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_date",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MinDate(new DateReturning(new DateParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMinDate()
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
                    "name": "date"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(
            value.AsT1.Argument.AsT0,
            new DateField(expectedEntityName, expectedFieldName)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnMinDate(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "entity": "{{expectedEntityName}}"
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMinDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MinDate(
                    new DateReturning(
                        new DateField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
