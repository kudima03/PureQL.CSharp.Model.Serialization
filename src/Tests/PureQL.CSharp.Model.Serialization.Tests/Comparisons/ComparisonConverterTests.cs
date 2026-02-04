using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using Comparison = PureQL.CSharp.Model.Comparisons.Comparison;
using StringComparison = PureQL.CSharp.Model.Comparisons.StringComparison;

namespace PureQL.CSharp.Model.Serialization.Tests.Comparisons;

public sealed record ComparisonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ComparisonConverterTests()
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
    public void ThrowsExceptionOnOperatorNameAbsenceOnDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnDate()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeftOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRightOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeftOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": null,
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRightOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongTypeOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": [],
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongTypeOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRightOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeftOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgsOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT0.Operator);
        Assert.Equal(value.AsT0.Left.AsT1, new DateScalar(now));
        Assert.Equal(value.AsT0.Right.AsT1, new DateScalar(now));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongScalarTypeOnDate(
        string type,
        ComparisonOperator @operator
    )
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgsOnDate(ComparisonOperator @operator)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new DateComparison(
                    @operator,
                    new DateReturning(new DateScalar(now)),
                    new DateReturning(new DateScalar(now))
                )
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
    public void ReadParameterArgsOnDate(ComparisonOperator @operator)
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
                  "name": "date"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT0.Operator);
        Assert.Equal(value.AsT0.Left.AsT0, new DateParameter(expectedFirstParamName));
        Assert.Equal(value.AsT0.Right.AsT0, new DateParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongParameterTypeOnDate(
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteParameterArgsOnDate(ComparisonOperator @operator)
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
                  "name": "date"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new DateComparison(
                    @operator,
                    new DateReturning(new DateParameter(expectedFirstParamName)),
                    new DateReturning(new DateParameter(expectedSecondParamName))
                )
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
    public void ReadMixedArgsOnDate(ComparisonOperator @operator)
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);

        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "{{expectedDate:yyyy-MM-dd}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT0.Operator);
        Assert.Equal(value.AsT0.Left.AsT1, new DateScalar(expectedDate));
        Assert.Equal(value.AsT0.Right.AsT0, new DateParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongConditionTypeOnDate(
        string type,
        ComparisonOperator @operator
    )
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{expectedDate:yyyy-MM-dd}}"
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgsOnDate(ComparisonOperator @operator)
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "{{expectedDate:yyyy-MM-dd}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new DateComparison(
                    @operator,
                    new DateReturning(new DateScalar(expectedDate)),
                    new DateReturning(new DateParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnDateTime()
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnDateTime()
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnDateTime()
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeftOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRightOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeftOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRightOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongTypeOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongTypeOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRightOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeftOnDateTime(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgsOnDateTime(ComparisonOperator @operator)
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

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT1.Operator);
        Assert.Equal(value.AsT1.Left.AsT1, new DateTimeScalar(now));
        Assert.Equal(value.AsT1.Right.AsT1, new DateTimeScalar(now));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongScalarTypeOnDateTime(
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgsOnDateTime(ComparisonOperator @operator)
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
            new Comparison(
                new DateTimeComparison(
                    @operator,
                    new DateTimeReturning(new DateTimeScalar(now)),
                    new DateTimeReturning(new DateTimeScalar(now))
                )
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
    public void ReadParameterArgsOnDateTime(ComparisonOperator @operator)
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

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT1.Operator);
        Assert.Equal(value.AsT1.Left.AsT0, new DateTimeParameter(expectedFirstParamName));
        Assert.Equal(
            value.AsT1.Right.AsT0,
            new DateTimeParameter(expectedSecondParamName)
        );
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongParameterTypeOnDateTime(
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteParameterArgsOnDateTime(ComparisonOperator @operator)
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
            new Comparison(
                new DateTimeComparison(
                    @operator,
                    new DateTimeReturning(new DateTimeParameter(expectedFirstParamName)),
                    new DateTimeReturning(new DateTimeParameter(expectedSecondParamName))
                )
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
    public void ReadMixedArgsOnDateTime(ComparisonOperator @operator)
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

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT1.Operator);
        Assert.Equal(value.AsT1.Left.AsT1, new DateTimeScalar(expected));
        Assert.Equal(value.AsT1.Right.AsT0, new DateTimeParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongConditionTypeOnDateTime(
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgsOnDateTime(ComparisonOperator @operator)
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
            new Comparison(
                new DateTimeComparison(
                    @operator,
                    new DateTimeReturning(new DateTimeScalar(expectedValue)),
                    new DateTimeReturning(new DateTimeParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnNumber()
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnNumber()
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnNumber()
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeftOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRightOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeftOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": null,
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRightOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongTypeOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": [],
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongTypeOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRightOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeftOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgsOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        Comparison comparison = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, comparison.AsT2.Operator);
        Assert.Equal(new NumberScalar(value), comparison.AsT2.Left.AsT1);
        Assert.Equal(new NumberScalar(value), comparison.AsT2.Right.AsT1);
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("datetime", ComparisonOperator.GreaterThan)]
    [InlineData("string", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("string", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("datetime", ComparisonOperator.LessThan)]
    [InlineData("string", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.LessThanOrEqual)]
    [InlineData("string", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongScalarTypeOnNumber(
        string type,
        ComparisonOperator @operator
    )
    {
        double value = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgsOnNumber(ComparisonOperator @operator)
    {
        double value = Random.Shared.NextDouble();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        string serialized = JsonSerializer.Serialize(
            new Comparison(
                new NumberComparison(
                    @operator,
                    new NumberReturning(new NumberScalar(value)),
                    new NumberReturning(new NumberScalar(value))
                )
            ),
            _options
        );
        Assert.Equal(expected, serialized);
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadParameterArgsOnNumber(ComparisonOperator @operator)
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
                  "name": "number"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT2.Operator);
        Assert.Equal(value.AsT2.Left.AsT0, new NumberParameter(expectedFirstParamName));
        Assert.Equal(value.AsT2.Right.AsT0, new NumberParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongParameterTypeOnNumber(
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteParameterArgsOnNumber(ComparisonOperator @operator)
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
                  "name": "number"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new NumberComparison(
                    @operator,
                    new NumberReturning(new NumberParameter(expectedFirstParamName)),
                    new NumberReturning(new NumberParameter(expectedSecondParamName))
                )
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
    public void ReadMixedArgsOnNumber(ComparisonOperator @operator)
    {
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": 0.5800537796011547
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT2.Operator);
        Assert.Equal(value.AsT2.Left.AsT1, new NumberScalar(0.5800537796011547));
        Assert.Equal(value.AsT2.Right.AsT0, new NumberParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("datetime", ComparisonOperator.GreaterThan)]
    [InlineData("string", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("string", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("datetime", ComparisonOperator.LessThan)]
    [InlineData("string", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.LessThanOrEqual)]
    [InlineData("string", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongConditionTypeOnNumber(
        string type,
        ComparisonOperator @operator
    )
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": 0.5800537796011547
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgsOnNumber(ComparisonOperator @operator)
    {
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "number"
                },
                "value": 0.5800537796011547
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new NumberComparison(
                    @operator,
                    new NumberReturning(new NumberScalar(0.5800537796011547)),
                    new NumberReturning(new NumberParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnString()
    {
        const string value = "reuafbhyafugeyb";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorNameOnString()
    {
        const string value = "reuafbhyafugeyb";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorNameOnString()
    {
        const string value = "reuafbhyafugeyb";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeftOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRightOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeftOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": null,
              "right": {
                "type": {
                  "name": "string"
                },
                "value": {{JsonSerializer.Serialize(value, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRightOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongTypeOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": [],
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongTypeOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRightOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeftOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgsOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        Comparison comparison = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, comparison.AsT3.Operator);
        Assert.Equal(new StringScalar(value), comparison.AsT3.Left.AsT1);
        Assert.Equal(new StringScalar(value), comparison.AsT3.Right.AsT1);
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("datetime", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("datetime", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongScalarTypeOnString(
        string type,
        ComparisonOperator @operator
    )
    {
        const string value = "reuafbhyafugeyb";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{value}}"
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{value}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgsOnString(ComparisonOperator @operator)
    {
        const string value = "reuafbhyafugeyb";
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{value}}"
              }
            }
            """;

        string serialized = JsonSerializer.Serialize(
            new Comparison(
                new StringComparison(
                    @operator,
                    new StringReturning(new StringScalar(value)),
                    new StringReturning(new StringScalar(value))
                )
            ),
            _options
        );
        Assert.Equal(expected, serialized);
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadParameterArgsOnString(ComparisonOperator @operator)
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
                  "name": "string"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT3.Operator);
        Assert.Equal(value.AsT3.Left.AsT0, new StringParameter(expectedFirstParamName));
        Assert.Equal(value.AsT3.Right.AsT0, new StringParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongParameterTypeOnString(
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteParameterArgsOnString(ComparisonOperator @operator)
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
                  "name": "string"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new StringComparison(
                    @operator,
                    new StringReturning(new StringParameter(expectedFirstParamName)),
                    new StringReturning(new StringParameter(expectedSecondParamName))
                )
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
    public void ReadMixedArgsOnString(ComparisonOperator @operator)
    {
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "adsihuowbfohuasdfipsduF"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT3.Operator);
        Assert.Equal(value.AsT3.Left.AsT1, new StringScalar("adsihuowbfohuasdfipsduF"));
        Assert.Equal(value.AsT3.Right.AsT0, new StringParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("datetime", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("time", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("time", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("datetime", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("time", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("time", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongConditionTypeOnString(
        string type,
        ComparisonOperator @operator
    )
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "adsihuowbfohuasdfipsduF"
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgsOnString(ComparisonOperator @operator)
    {
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "adsihuowbfohuasdfipsduF"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new StringComparison(
                    @operator,
                    new StringReturning(new StringScalar("adsihuowbfohuasdfipsduF")),
                    new StringReturning(new StringParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsence()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeft(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRight(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeft(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": null,
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRight(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongType(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": [],
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongType(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRight(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeft(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgs(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        Comparison comparison = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, comparison.AsT4.Operator);
        Assert.Equal(new TimeScalar(now), comparison.AsT4.Left.AsT1);
        Assert.Equal(new TimeScalar(now), comparison.AsT4.Right.AsT1);
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongScalarType(
        string type,
        ComparisonOperator @operator
    )
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgs(ComparisonOperator @operator)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string serialized = JsonSerializer.Serialize(
            new Comparison(
                new TimeComparison(
                    @operator,
                    new TimeReturning(new TimeScalar(now)),
                    new TimeReturning(new TimeScalar(now))
                )
            ),
            _options
        );
        Assert.Equal(expected, serialized);
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
                  "name": "time"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT4.Operator);
        Assert.Equal(value.AsT4.Left.AsT0, new TimeParameter(expectedFirstParamName));
        Assert.Equal(value.AsT4.Right.AsT0, new TimeParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThan)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThan)]
    [InlineData("nullArray", ComparisonOperator.GreaterThan)]
    [InlineData("numberArray", ComparisonOperator.GreaterThan)]
    [InlineData("stringArray", ComparisonOperator.GreaterThan)]
    [InlineData("timeArray", ComparisonOperator.GreaterThan)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThan)]
    [InlineData("booleanArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("booleanArray", ComparisonOperator.LessThan)]
    [InlineData("datetimeArray", ComparisonOperator.LessThan)]
    [InlineData("nullArray", ComparisonOperator.LessThan)]
    [InlineData("numberArray", ComparisonOperator.LessThan)]
    [InlineData("stringArray", ComparisonOperator.LessThan)]
    [InlineData("timeArray", ComparisonOperator.LessThan)]
    [InlineData("uuidArray", ComparisonOperator.LessThan)]
    [InlineData("booleanArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetimeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("nullArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("numberArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("stringArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("timeArray", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuidArray", ComparisonOperator.LessThanOrEqual)]
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
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
                  "name": "time"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new TimeComparison(
                    @operator,
                    new TimeReturning(new TimeParameter(expectedFirstParamName)),
                    new TimeReturning(new TimeParameter(expectedSecondParamName))
                )
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
        TimeOnly expected = new TimeOnly(14, 30, 15);
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": "{{expected:HH:mm:ss}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        Comparison value = JsonSerializer.Deserialize<Comparison>(input, _options)!;
        Assert.Equal(@operator, value.AsT4.Operator);
        Assert.Equal(value.AsT4.Left.AsT1, new TimeScalar(expected));
        Assert.Equal(value.AsT4.Right.AsT0, new TimeParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean", ComparisonOperator.GreaterThan)]
    [InlineData("date", ComparisonOperator.GreaterThan)]
    [InlineData("null", ComparisonOperator.GreaterThan)]
    [InlineData("datetime", ComparisonOperator.GreaterThan)]
    [InlineData("number", ComparisonOperator.GreaterThan)]
    [InlineData("uuid", ComparisonOperator.GreaterThan)]
    [InlineData("boolean", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("date", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("null", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("number", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.GreaterThanOrEqual)]
    [InlineData("boolean", ComparisonOperator.LessThan)]
    [InlineData("date", ComparisonOperator.LessThan)]
    [InlineData("null", ComparisonOperator.LessThan)]
    [InlineData("datetime", ComparisonOperator.LessThan)]
    [InlineData("number", ComparisonOperator.LessThan)]
    [InlineData("uuid", ComparisonOperator.LessThan)]
    [InlineData("boolean", ComparisonOperator.LessThanOrEqual)]
    [InlineData("date", ComparisonOperator.LessThanOrEqual)]
    [InlineData("null", ComparisonOperator.LessThanOrEqual)]
    [InlineData("datetime", ComparisonOperator.LessThanOrEqual)]
    [InlineData("number", ComparisonOperator.LessThanOrEqual)]
    [InlineData("uuid", ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnWrongConditionType(
        string type,
        ComparisonOperator @operator
    )
    {
        TimeOnly expected = new TimeOnly(14, 30, 15);
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{expected:HH:mm:ss}}"
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
            JsonSerializer.Deserialize<Comparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgs(ComparisonOperator @operator)
    {
        TimeOnly expectedValue = new TimeOnly(14, 30, 15);
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "type": {
                  "name": "time"
                },
                "value": "{{expectedValue:HH:mm:ss}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new Comparison(
                new TimeComparison(
                    @operator,
                    new TimeReturning(new TimeScalar(expectedValue)),
                    new TimeReturning(new TimeParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
