using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record SingleValueEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SingleValueEqualityConverterTests()
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
    public void ReadBooleanEqualityWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanScalar(true), equality.Left.AsT1);
        Assert.Equal(new BooleanScalar(false), equality.Right.AsT1);
    }

    [Fact]
    public void WriteBooleanEqualityWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new BooleanEquality(
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new BooleanParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteBooleanEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "boolean"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new BooleanEquality(
                    new BooleanReturning(new BooleanParameter(leftParamName)),
                    new BooleanReturning(new BooleanParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanEqualityWithEquality()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "operator": "equal",
                "left": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              },
              "right": {
                "operator": "equal",
                "left": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanScalar(true), equality.Left.AsT2.AsT0.AsT0.Left.AsT1);
        Assert.Equal(new BooleanScalar(false), equality.Left.AsT2.AsT0.AsT0.Right.AsT1);
    }

    [Fact]
    public void WriteBooleanEqualityWithEquality()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "operator": "equal",
                "left": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              },
              "right": {
                "operator": "equal",
                "left": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              }
            }
            """;

        BooleanEquality innerEquality = new(
            new BooleanReturning(new BooleanScalar(true)),
            new BooleanReturning(new BooleanScalar(false))
        );

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new BooleanEquality(
                    new BooleanReturning(new Equality(new SingleValueEquality(innerEquality))),
                    new BooleanReturning(new Equality(new SingleValueEquality(innerEquality)))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanEqualityWithBooleanOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "operator": "not",
                "condition": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              },
              "right": {
                "operator": "and",
                "conditions": [
                  {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  },
                  {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  }
                ]
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanScalar(true), equality.Left.AsT3.AsT2.Condition.AsT1);
        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ],
            equality.Right.AsT3.AsT0.Conditions.AsT0
        );
    }

    [Fact]
    public void WriteBooleanEqualityWithBooleanOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "operator": "not",
                "condition": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              },
              "right": {
                "operator": "and",
                "conditions": [
                  {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  },
                  {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  }
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new BooleanEquality(
                    new BooleanReturning(
                        new BooleanOperator(
                            new NotOperator(new BooleanReturning(new BooleanScalar(true)))
                        )
                    ),
                    new BooleanReturning(
                        new BooleanOperator(
                            new AndOperator([
                                new BooleanReturning(new BooleanScalar(true)),
                                new BooleanReturning(new BooleanScalar(false)),
                            ])
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanEqualityWithComparison()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "operator": "greaterThan",
                "left": {
                  "type": {
                    "name": "number"
                  },
                  "value": 42
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 24
                }
              },
              "right": {
                "operator": "lessThan",
                "left": {
                  "type": {
                    "name": "number"
                  },
                  "value": 10
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 20
                }
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT0;

        Assert.Equal(ComparisonOperator.GreaterThan, equality.Left.AsT4.AsT2.Operator);
        Assert.Equal(new NumberScalar(42), equality.Left.AsT4.AsT2.Left.AsT1);
        Assert.Equal(ComparisonOperator.LessThan, equality.Right.AsT4.AsT2.Operator);
        Assert.Equal(new NumberScalar(10), equality.Right.AsT4.AsT2.Left.AsT1);
    }

    [Fact]
    public void WriteBooleanEqualityWithComparison()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "operator": "greaterThan",
                "left": {
                  "type": {
                    "name": "number"
                  },
                  "value": 42
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 24
                }
              },
              "right": {
                "operator": "lessThan",
                "left": {
                  "type": {
                    "name": "number"
                  },
                  "value": 10
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 20
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new BooleanEquality(
                    new BooleanReturning(
                        new Comparison(
                            new NumberComparison(
                                ComparisonOperator.GreaterThan,
                                new NumberReturning(new NumberScalar(42)),
                                new NumberReturning(new NumberScalar(24))
                            )
                        )
                    ),
                    new BooleanReturning(
                        new Comparison(
                            new NumberComparison(
                                ComparisonOperator.LessThan,
                                new NumberReturning(new NumberScalar(10)),
                                new NumberReturning(new NumberScalar(20))
                            )
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        DateEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT1;

        Assert.Equal(new DateParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new DateParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteDateEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new DateEquality(
                    new DateReturning(new DateParameter(leftParamName)),
                    new DateReturning(new DateParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateEqualityWithScalar()
    {
        DateOnly leftValue = new(2024, 1, 15);
        DateOnly rightValue = new(2024, 6, 20);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "{{leftValue:yyyy-MM-dd}}"
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": "{{rightValue:yyyy-MM-dd}}"
              }
            }
            """;

        DateEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT1;

        Assert.Equal(new DateScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new DateScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteDateEqualityWithScalar()
    {
        DateOnly leftValue = new(2024, 1, 15);
        DateOnly rightValue = new(2024, 6, 20);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "{{leftValue:yyyy-MM-dd}}"
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": "{{rightValue:yyyy-MM-dd}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new DateEquality(
                    new DateReturning(new DateScalar(leftValue)),
                    new DateReturning(new DateScalar(rightValue))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        DateTimeEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT2;

        Assert.Equal(new DateTimeParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new DateTimeParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteDateTimeEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new DateTimeEquality(
                    new DateTimeReturning(new DateTimeParameter(leftParamName)),
                    new DateTimeReturning(new DateTimeParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeEqualityWithScalar()
    {
        DateTime leftValue = new(2024, 1, 15, 10, 30, 0);
        DateTime rightValue = new(2024, 6, 20, 15, 45, 0);

        string leftJson = JsonSerializer.Serialize(leftValue, _options);
        string rightJson = JsonSerializer.Serialize(rightValue, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{leftJson}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{rightJson}}
              }
            }
            """;

        DateTimeEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT2;

        Assert.Equal(new DateTimeScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new DateTimeScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteDateTimeEqualityWithScalar()
    {
        DateTime leftValue = new(2024, 1, 15, 10, 30, 0);
        DateTime rightValue = new(2024, 6, 20, 15, 45, 0);

        string leftJson = JsonSerializer.Serialize(leftValue, _options);
        string rightJson = JsonSerializer.Serialize(rightValue, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{leftJson}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{rightJson}}
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new DateTimeEquality(
                    new DateTimeReturning(new DateTimeScalar(leftValue)),
                    new DateTimeReturning(new DateTimeScalar(rightValue))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberEqualityWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": 42
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 24
              }
            }
            """;

        NumberEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT3;

        Assert.Equal(new NumberScalar(42), equality.Left.AsT1);
        Assert.Equal(new NumberScalar(24), equality.Right.AsT1);
    }

    [Fact]
    public void WriteNumberEqualityWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": 42
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 24
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new NumberEquality(
                    new NumberReturning(new NumberScalar(42)),
                    new NumberReturning(new NumberScalar(24))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        NumberEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT3;

        Assert.Equal(new NumberParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new NumberParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteNumberEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new NumberEquality(
                    new NumberReturning(new NumberParameter(leftParamName)),
                    new NumberReturning(new NumberParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringEqualityWithScalar()
    {
        const string leftValue = "leftValue";
        const string rightValue = "rightValue";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        StringEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT4;

        Assert.Equal(new StringScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new StringScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteStringEqualityWithScalar()
    {
        const string leftValue = "leftValue";
        const string rightValue = "rightValue";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new StringEquality(
                    new StringReturning(new StringScalar(leftValue)),
                    new StringReturning(new StringScalar(rightValue))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        StringEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT4;

        Assert.Equal(new StringParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new StringParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteStringEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "string"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new StringEquality(
                    new StringReturning(new StringParameter(leftParamName)),
                    new StringReturning(new StringParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        TimeEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT5;

        Assert.Equal(new TimeParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new TimeParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteTimeEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "time"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new TimeEquality(
                    new TimeReturning(new TimeParameter(leftParamName)),
                    new TimeReturning(new TimeParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeEqualityWithScalar()
    {
        TimeOnly leftValue = new(10, 30, 0);
        TimeOnly rightValue = new(14, 45, 0);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": "{{leftValue:HH:mm:ss}}"
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": "{{rightValue:HH:mm:ss}}"
              }
            }
            """;

        TimeEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT5;

        Assert.Equal(new TimeScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new TimeScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteTimeEqualityWithScalar()
    {
        TimeOnly leftValue = new(10, 30, 0);
        TimeOnly rightValue = new(14, 45, 0);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": "{{leftValue:HH:mm:ss}}"
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": "{{rightValue:HH:mm:ss}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new TimeEquality(
                    new TimeReturning(new TimeScalar(leftValue)),
                    new TimeReturning(new TimeScalar(rightValue))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidEqualityWithScalar()
    {
        Guid leftValue = new("00000000-0000-0000-0000-000000000001");
        Guid rightValue = new("00000000-0000-0000-0000-000000000002");

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        UuidEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT6;

        Assert.Equal(new UuidScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new UuidScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteUuidEqualityWithScalar()
    {
        Guid leftValue = new("00000000-0000-0000-0000-000000000001");
        Guid rightValue = new("00000000-0000-0000-0000-000000000002");

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new UuidEquality(
                    new UuidReturning(new UuidScalar(leftValue)),
                    new UuidReturning(new UuidScalar(rightValue))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        UuidEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT6;

        Assert.Equal(new UuidParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new UuidParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteUuidEqualityWithParameter()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "uuid"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "uuid"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new UuidEquality(
                    new UuidReturning(new UuidParameter(leftParamName)),
                    new UuidReturning(new UuidParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SingleValueEquality>(input, _options)
        );
    }
}
