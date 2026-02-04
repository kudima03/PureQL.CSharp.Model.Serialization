using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Comparisons;

public sealed record DateTimeComparisonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeComparisonConverterTests()
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
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeft(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRight(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeft(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": null,
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRight(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongType(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": [],
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongType(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRight(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeft(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgs(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        DateTimeComparison value = JsonSerializer.Deserialize<DateTimeComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(value.Left.AsT1, new DateTimeScalar(now));
        Assert.Equal(value.Right.AsT1, new DateTimeScalar(now));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("string", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("string", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("string", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("string", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongScalarType(
        string type,
        ComparisonOperator @operator
    )
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgs(ComparisonOperator @operator)
    {
        DateTime now = DateTime.Now;
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeComparison(
                @operator,
                new DateTimeReturning(new DateTimeScalar(now)),
                new DateTimeReturning(new DateTimeScalar(now))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadParameterArgs(ComparisonOperator @operator)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        DateTimeComparison value = JsonSerializer.Deserialize<DateTimeComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(value.Left.AsT0, new DateTimeParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new DateTimeParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("string", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("string", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("string", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("string", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongParameterType(
        string type,
        ComparisonOperator @operator
    )
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "{{type}}"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "{{type}}"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteParameterArgs(ComparisonOperator @operator)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeComparison(
                @operator,
                new DateTimeReturning(new DateTimeParameter(expectedFirstParamName)),
                new DateTimeReturning(new DateTimeParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadMixedArgs(ComparisonOperator @operator)
    {
        DateTime expected = DateTime.Now;
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": "{{expected:O}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        DateTimeComparison value = JsonSerializer.Deserialize<DateTimeComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(value.Left.AsT1, new DateTimeScalar(expected));
        Assert.Equal(value.Right.AsT0, new DateTimeParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("string", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("string", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("string", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("string", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongConditionType(
        string type,
        ComparisonOperator @operator
    )
    {
        DateTime expected = DateTime.Now;
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{expected:O}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "{{type}}"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgs(ComparisonOperator @operator)
    {
        DateTime expectedValue = DateTime.Now;
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(expectedValue, _options)}}
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeComparison(
                @operator,
                new DateTimeReturning(new DateTimeScalar(expectedValue)),
                new DateTimeReturning(new DateTimeParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
