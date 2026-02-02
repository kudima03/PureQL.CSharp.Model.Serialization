using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.BooleanOperations;

public sealed record AndOperatorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public AndOperatorConverterTests()
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
    public void ThrowsExceptionOnOtherOperatorNameAbsence()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "or",
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "bduasfehrygcvdubfsahycv",
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnConditionsWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": []
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Empty(value.Conditions.AsT0);
    }

    [Fact]
    public void WriteEmptyConditions()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": []
            }
            """;

        string value = JsonSerializer.Serialize(new AndOperator([]), _options);
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarConditions()
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

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Equal(value.Conditions.AsT0.First().AsT1, new BooleanScalar(true));
        Assert.Equal(value.Conditions.AsT0.Last().AsT1, new BooleanScalar(false));
    }

    [Theory(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": true
                },
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": false
                }
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarConditions()
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

        string value = JsonSerializer.Serialize(
            new AndOperator(
                [
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterConditions()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Equal(
            value.Conditions.AsT0.First().AsT0,
            new BooleanParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.Conditions.AsT0.Last().AsT0,
            new BooleanParameter(expectedSecondParamName)
        );
    }

    [Theory(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongParameterType(string type)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterConditions()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new AndOperator(
                [
                    new BooleanReturning(new BooleanParameter(expectedFirstParamName)),
                    new BooleanReturning(new BooleanParameter(expectedSecondParamName)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void ReadEqualityConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "equal",
                  "left": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                },
                {
                  "operator": "equal",
                  "left": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Equal(
            value.Conditions.AsT0.First().AsT2,
            new Equality(
                new SingleValueEquality(
                    new BooleanEquality(
                        new BooleanReturning(new BooleanScalar(false)),
                        new BooleanReturning(new BooleanScalar(true))
                    )
                )
            )
        );
        Assert.Equal(
            value.Conditions.AsT0.Last().AsT2,
            new Equality(
                new SingleValueEquality(
                    new BooleanEquality(
                        new BooleanReturning(new BooleanScalar(false)),
                        new BooleanReturning(new BooleanScalar(true))
                    )
                )
            )
        );
    }

    [Theory(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongEqualityType(string type)
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "equal",
                  "left": {
                    "type": {
                      "name": "{{type}}"
                    },
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "{{type}}"
                    },
                    "value": true
                  }
                },
                {
                  "operator": "equal",
                  "left": {
                    "type": {
                      "name": "{{type}}"
                    },
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "{{type}}"
                    },
                    "value": true
                  }
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void WriteEqualityConditions()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "equal",
                  "left": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                },
                {
                  "operator": "equal",
                  "left": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new AndOperator(
                [
                    new BooleanReturning(
                        new Equality(
                            new SingleValueEquality(
                                new BooleanEquality(
                                    new BooleanReturning(new BooleanScalar(false)),
                                    new BooleanReturning(new BooleanScalar(true))
                                )
                            )
                        )
                    ),
                    new BooleanReturning(
                        new Equality(
                            new SingleValueEquality(
                                new BooleanEquality(
                                    new BooleanReturning(new BooleanScalar(false)),
                                    new BooleanReturning(new BooleanScalar(true))
                                )
                            )
                        )
                    ),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void ReadBooleanOperatorConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                },
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Equal(
            value.Conditions.AsT0.First().AsT3,
            new BooleanOperator(
                new AndOperator(
                    [
                        new BooleanReturning(new BooleanScalar(false)),
                        new BooleanReturning(new BooleanScalar(true)),
                    ]
                )
            )
        );
        Assert.Equal(
            value.Conditions.AsT0.Last().AsT3,
            new BooleanOperator(
                new AndOperator(
                    [
                        new BooleanReturning(new BooleanScalar(false)),
                        new BooleanReturning(new BooleanScalar(true)),
                    ]
                )
            )
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
    public void ThrowsExceptionOnWrongBooleanOperatorType(string type)
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": true
                    }
                  ]
                },
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": true
                    }
                  ]
                }
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void WriteBooleanOperatorConditions()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                },
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new AndOperator(
                [
                    new BooleanReturning(
                        new Equality(
                            new SingleValueEquality(
                                new BooleanEquality(
                                    new BooleanReturning(new BooleanScalar(false)),
                                    new BooleanReturning(new BooleanScalar(true))
                                )
                            )
                        )
                    ),
                    new BooleanReturning(
                        new Equality(
                            new SingleValueEquality(
                                new BooleanEquality(
                                    new BooleanReturning(new BooleanScalar(false)),
                                    new BooleanReturning(new BooleanScalar(true))
                                )
                            )
                        )
                    ),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void ReadMixedConditions()
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                },
                {
                  "operator": "equal",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Equal(
            value.Conditions.AsT0.First().AsT3,
            new BooleanOperator(
                new AndOperator(
                    [
                        new BooleanReturning(new BooleanScalar(false)),
                        new BooleanReturning(new BooleanScalar(true)),
                    ]
                )
            )
        );
        Assert.Equal(value.Conditions.AsT0.Skip(1).First().AsT1, new BooleanScalar(true));
        Assert.Equal(
            value.Conditions.AsT0.Skip(3).First().AsT0,
            new BooleanParameter(expectedParamName)
        );
        Assert.Equal(
            value.Conditions.AsT0.Skip(4).First().AsT2,
            new Equality(
                new SingleValueEquality(
                    new BooleanEquality(
                        new BooleanReturning(new BooleanScalar(true)),
                        new BooleanReturning(new BooleanScalar(false))
                    )
                )
            )
        );
    }

    [Theory(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": true
                    }
                  ]
                },
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": true
                },
                {
                  "name": "swdefiujhnr",
                  "type": {
                    "name": "{{type}}"
                  }
                },
                {
                  "operator": "equal",
                  "conditions": [
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "{{type}}"
                      },
                      "value": true
                    }
                  ]
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AndOperator>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedConditions()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "operator": "and",
                  "conditions": [
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": false
                    },
                    {
                      "type": {
                        "name": "boolean"
                      },
                      "value": true
                    }
                  ]
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                },
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
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new AndOperator(
                [
                    new BooleanReturning(
                        new BooleanOperator(
                            new AndOperator(
                                [
                                    new BooleanReturning(new BooleanScalar(false)),
                                    new BooleanReturning(new BooleanScalar(true)),
                                ]
                            )
                        )
                    ),
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanParameter(expectedParamName)),
                    new BooleanReturning(
                        new Equality(
                            new SingleValueEquality(
                                new BooleanEquality(
                                    new BooleanReturning(new BooleanScalar(true)),
                                    new BooleanReturning(new BooleanScalar(false))
                                )
                            )
                        )
                    ),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
