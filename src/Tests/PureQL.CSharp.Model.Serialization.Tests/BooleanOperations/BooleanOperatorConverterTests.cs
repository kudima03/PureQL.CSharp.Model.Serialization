using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.BooleanOperations;

public sealed record BooleanOperatorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanOperatorConverterTests()
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
    public void ReadAndOperatorWithBooleanReturningList()
    {
        const string input = /*lang=json,strict*/
            """
            {
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
            """;

        AndOperator andOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT0;

        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ],
            andOperator.Conditions.AsT0
        );
    }

    [Fact]
    public void WriteAndOperatorWithBooleanReturningList()
    {
        const string expected = /*lang=json,strict*/
            """
            {
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
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new AndOperator([
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false)),
                ])
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadAndOperatorWithBooleanArrayReturning()
    {
        const string paramName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": {
                "type": {
                  "name": "booleanArray"
                },
                "name": "{{paramName}}"
              }
            }
            """;

        AndOperator andOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT0;

        Assert.Equal(
            new BooleanArrayParameter(paramName),
            andOperator.Conditions.AsT1.AsT2
        );
    }

    [Fact]
    public void WriteAndOperatorWithBooleanArrayReturning()
    {
        const string paramName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": {
                "name": "{{paramName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new AndOperator(
                    new BooleanArrayReturning(new BooleanArrayParameter(paramName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadOrOperatorWithBooleanReturningList()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "or",
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
            """;

        OrOperator orOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT1;

        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ],
            orOperator.Conditions.AsT0
        );
    }

    [Fact]
    public void WriteOrOperatorWithBooleanReturningList()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "or",
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
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new OrOperator([
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false)),
                ])
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadOrOperatorWithBooleanArrayReturning()
    {
        const string paramName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "or",
              "conditions": {
                "type": {
                  "name": "booleanArray"
                },
                "name": "{{paramName}}"
              }
            }
            """;

        OrOperator orOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT1;

        Assert.Equal(
            new BooleanArrayParameter(paramName),
            orOperator.Conditions.AsT1.AsT2
        );
    }

    [Fact]
    public void WriteOrOperatorWithBooleanArrayReturning()
    {
        const string paramName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "or",
              "conditions": {
                "name": "{{paramName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new OrOperator(
                    new BooleanArrayReturning(new BooleanArrayParameter(paramName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNotOperatorWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        Assert.Equal(
            new NotOperator(new BooleanReturning(new BooleanScalar(true))),
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)!.AsT2
        );
    }

    [Fact]
    public void WriteNotOperatorWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new NotOperator(new BooleanReturning(new BooleanScalar(true)))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNotOperatorWithParameter()
    {
        const string paramName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "name": "{{paramName}}"
              }
            }
            """;

        Assert.Equal(
            new NotOperator(new BooleanReturning(new BooleanParameter(paramName))),
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)!.AsT2
        );
    }

    [Fact]
    public void WriteNotOperatorWithParameter()
    {
        const string paramName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                "name": "{{paramName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new NotOperator(new BooleanReturning(new BooleanParameter(paramName)))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNotOperatorWithEquality()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
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

        Assert.Equal(
            new NotOperator(new BooleanReturning(new Equality(new SingleValueEquality(new BooleanEquality(
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false))
            ))))),
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)!.AsT2
        );
    }

    [Fact]
    public void WriteNotOperatorWithEquality()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
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

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new NotOperator(
                    new BooleanReturning(
                        new Equality(
                            new SingleValueEquality(
                                new BooleanEquality(
                                    new BooleanReturning(new BooleanScalar(true)),
                                    new BooleanReturning(new BooleanScalar(false))
                                )
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
    public void ReadNotOperatorWithBooleanOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "operator": "not",
                "condition": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              }
            }
            """;

        Assert.Equal(
            new NotOperator(new BooleanReturning(new BooleanOperator(
                new NotOperator(new BooleanReturning(new BooleanScalar(true)))
            ))),
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)!.AsT2
        );
    }

    [Fact]
    public void WriteNotOperatorWithBooleanOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "operator": "not",
                "condition": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new NotOperator(
                    new BooleanReturning(
                        new BooleanOperator(
                            new NotOperator(new BooleanReturning(new BooleanScalar(true)))
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNotOperatorWithComparison()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
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
              }
            }
            """;

        Assert.Equal(
            new NotOperator(new BooleanReturning(new Comparison(new NumberComparison(
                ComparisonOperator.GreaterThan,
                new NumberReturning(new NumberScalar(42)),
                new NumberReturning(new NumberScalar(24))
            )))),
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)!.AsT2
        );
    }

    [Fact]
    public void WriteNotOperatorWithComparison()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
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
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new NotOperator(
                    new BooleanReturning(
                        new Comparison(
                            new NumberComparison(
                                ComparisonOperator.GreaterThan,
                                new NumberReturning(new NumberScalar(42)),
                                new NumberReturning(new NumberScalar(24))
                            )
                        )
                    )
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
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)
        );
    }
}
