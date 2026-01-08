using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Fields;
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
                "value": {{JsonSerializer.Serialize(value)}}
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
        Assert.Equal(new NumberScalar(value), comparison.Left.AsT2);
        Assert.Equal(new NumberScalar(value), comparison.Right.AsT2);
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
        Assert.Equal(value.Left.AsT1, new NumberParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT1, new NumberParameter(expectedSecondParamName));
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
    public void ReadFieldArgs(ComparisonOperator @operator)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "right": {
                "entity": "{{expectedFirstEntityName}}",
                "field": "{{expectedFirstFieldName}}",
                "type": {
                  "name": "number"
                }
              },
              "left": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
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
        Assert.Equal(
            value.Right.AsT0,
            new NumberField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.Left.AsT0,
            new NumberField(expectedSecondEntityName, expectedSecondFieldName)
        );
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
    public void ThrowsExceptionOnWrongFieldType(string type, ComparisonOperator @operator)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "{{type}}"
                }
              },
              "right": {
                "entity": "{{expectedFirstEntityName}}",
                "field": "{{expectedFirstFieldName}}",
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
    public void WriteFieldArgs(ComparisonOperator @operator)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "entity": "{{expectedFirstEntityName}}",
                "field": "{{expectedFirstFieldName}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;
        string value = JsonSerializer.Serialize(
            new NumberComparison(
                @operator,
                new NumberReturning(
                    new NumberField(expectedFirstEntityName, expectedFirstFieldName)
                ),
                new NumberReturning(
                    new NumberField(expectedSecondEntityName, expectedSecondFieldName)
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
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
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
        Assert.Equal(
            value.Left.AsT0,
            new NumberField(expectedEntityName, expectedFieldName)
        );
        Assert.Equal(value.Right.AsT1, new NumberParameter(expectedParamName));
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
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "{{type}}"
                }
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
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": {{JsonSerializer.Serialize(@operator, _options)}},
              "left": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
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
                new NumberReturning(
                    new NumberField(expectedEntityName, expectedFieldName)
                ),
                new NumberReturning(new NumberParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
