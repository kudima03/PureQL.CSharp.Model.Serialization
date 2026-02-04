using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Comparisons;

public sealed record NumberComparisonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberComparisonConverterTests()
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeft(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRight(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeft(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRight(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongType(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongType(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRight(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeft(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgs(ComparisonOperator @operator)
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

        NumberComparison comparison = JsonSerializer.Deserialize<NumberComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, comparison.Operator);
        Assert.Equal(new NumberScalar(value), comparison.Left.AsT1);
        Assert.Equal(new NumberScalar(value), comparison.Right.AsT1);
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
    public void ThrowsExceptionOnWrongScalarType(
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgs(ComparisonOperator @operator)
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
            new NumberComparison(
                @operator,
                new NumberReturning(new NumberScalar(value)),
                new NumberReturning(new NumberScalar(value))
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

        NumberComparison value = JsonSerializer.Deserialize<NumberComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(value.Left.AsT0, new NumberParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new NumberParameter(expectedSecondParamName));
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
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
            new NumberComparison(
                @operator,
                new NumberReturning(new NumberParameter(expectedFirstParamName)),
                new NumberReturning(new NumberParameter(expectedSecondParamName))
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

        NumberComparison value = JsonSerializer.Deserialize<NumberComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(value.Left.AsT1, new NumberScalar(0.5800537796011547));
        Assert.Equal(value.Right.AsT0, new NumberParameter(expectedParamName));
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
    public void ThrowsExceptionOnWrongConditionType(
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
            JsonSerializer.Deserialize<NumberComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteMixedArgs(ComparisonOperator @operator)
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
            new NumberComparison(
                @operator,
                new NumberReturning(new NumberScalar(0.5800537796011547)),
                new NumberReturning(new NumberParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
