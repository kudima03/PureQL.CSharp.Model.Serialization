using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using StringComparison = PureQL.CSharp.Model.Comparisons.StringComparison;

namespace PureQL.CSharp.Model.Serialization.Tests.Comparisons;

public sealed record StringComparisonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringComparisonConverterTests()
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedLeft(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnUndefinedRight(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullLeft(ComparisonOperator @operator)
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
                "value": {{JsonSerializer.Serialize(value)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnNullRight(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnLeftWrongType(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnRightWrongType(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyRight(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ThrowsExceptionOnEmptyLeft(ComparisonOperator @operator)
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void ReadScalarArgs(ComparisonOperator @operator)
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

        StringComparison comparison = JsonSerializer.Deserialize<StringComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, comparison.Operator);
        Assert.Equal(new StringScalar(value), comparison.Left.AsT2);
        Assert.Equal(new StringScalar(value), comparison.Right.AsT2);
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
    public void ThrowsExceptionOnWrongScalarType(
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
        );
    }

    [Theory]
    [InlineData(ComparisonOperator.GreaterThan)]
    [InlineData(ComparisonOperator.GreaterThanOrEqual)]
    [InlineData(ComparisonOperator.LessThan)]
    [InlineData(ComparisonOperator.LessThanOrEqual)]
    public void WriteScalarArgs(ComparisonOperator @operator)
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
            new StringComparison(
                @operator,
                new StringReturning(new StringScalar(value)),
                new StringReturning(new StringScalar(value))
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

        StringComparison value = JsonSerializer.Deserialize<StringComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(value.Left.AsT1, new StringParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT1, new StringParameter(expectedSecondParamName));
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
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
            new StringComparison(
                @operator,
                new StringReturning(new StringParameter(expectedFirstParamName)),
                new StringReturning(new StringParameter(expectedSecondParamName))
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
                  "name": "string"
                }
              },
              "left": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        StringComparison value = JsonSerializer.Deserialize<StringComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(
            value.Right.AsT0,
            new StringField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.Left.AsT0,
            new StringField(expectedSecondEntityName, expectedSecondFieldName)
        );
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
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
                  "name": "string"
                }
              },
              "right": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;
        string value = JsonSerializer.Serialize(
            new StringComparison(
                @operator,
                new StringReturning(
                    new StringField(expectedFirstEntityName, expectedFirstFieldName)
                ),
                new StringReturning(
                    new StringField(expectedSecondEntityName, expectedSecondFieldName)
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
                  "name": "string"
                }
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        StringComparison value = JsonSerializer.Deserialize<StringComparison>(
            input,
            _options
        )!;
        Assert.Equal(@operator, value.Operator);
        Assert.Equal(
            value.Left.AsT0,
            new StringField(expectedEntityName, expectedFieldName)
        );
        Assert.Equal(value.Right.AsT1, new StringParameter(expectedParamName));
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
            JsonSerializer.Deserialize<StringComparison>(input, _options)
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
                  "name": "string"
                }
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
            new StringComparison(
                @operator,
                new StringReturning(
                    new StringField(expectedEntityName, expectedFieldName)
                ),
                new StringReturning(new StringParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
