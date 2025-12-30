using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.BooleanOperations;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Returnings;
using PureQL.CSharp.Model.Serialization.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.BooleanOperations;

public sealed record AndOperatorConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new AndOperatorConverter(),
            new BooleanReturningConverter(),
            new BooleanFieldConverter(),
            new BooleanParameterConverter(),
            new BooleanScalarConverter(),
        },
    };

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
        Assert.Empty(value.Conditions);
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
        Assert.Equal(value.Conditions.First().AsT2, new BooleanScalar(true));
        Assert.Equal(value.Conditions.Last().AsT2, new BooleanScalar(false));
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
            new AndOperator([
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ]),
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
            value.Conditions.First().AsT1,
            new BooleanParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.Conditions.Last().AsT1,
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
            new AndOperator([
                new BooleanReturning(new BooleanParameter(expectedFirstParamName)),
                new BooleanReturning(new BooleanParameter(expectedSecondParamName)),
            ]),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldConditions()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "entity": "{{expectedFirstEntityName}}",
                  "field": "{{expectedFirstFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
                },
                {
                  "entity": "{{expectedSecondEntityName}}",
                  "field": "{{expectedSecondFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
                }
              ]
            }
            """;

        AndOperator value = JsonSerializer.Deserialize<AndOperator>(input, _options)!;
        Assert.Equal(
            value.Conditions.First().AsT0,
            new BooleanField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.Conditions.Last().AsT0,
            new BooleanField(expectedSecondEntityName, expectedSecondFieldName)
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
    public void ThrowsExceptionOnWrongFieldType(string type)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "entity": "{{expectedFirstEntityName}}"
                  "field": "{{expectedFirstFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                },
                {
                  "entity": "{{expectedSecondEntityName}}"
                  "name": "{{expectedSecondFieldName}}",
                  "type": {
                    "name": "{{type}}"
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

    [Fact]
    public void WriteFieldConditions()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "conditions": [
                {
                  "entity": "{{expectedFirstEntityName}}",
                  "field": "{{expectedFirstFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
                },
                {
                  "entity": "{{expectedSecondEntityName}}",
                  "field": "{{expectedSecondFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new AndOperator([
                new BooleanReturning(
                    new BooleanField(expectedFirstEntityName, expectedFirstFieldName)
                ),
                new BooleanReturning(
                    new BooleanField(expectedSecondEntityName, expectedSecondFieldName)
                ),
            ]),
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
            value.Conditions.First().AsT3,
            new Equality(
                new BooleanEquality(
                    new BooleanReturning(new BooleanScalar(false)),
                    new BooleanReturning(new BooleanScalar(true))
                )
            )
        );
        Assert.Equal(
            value.Conditions.Last().AsT3,
            new Equality(
                new BooleanEquality(
                    new BooleanReturning(new BooleanScalar(false)),
                    new BooleanReturning(new BooleanScalar(true))
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
            new AndOperator([
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true))
                        )
                    )
                ),
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true))
                        )
                    )
                ),
            ]),
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
            value.Conditions.First().AsT4,
            new BooleanOperator(
                new AndOperator([
                    new BooleanReturning(new BooleanScalar(false)),
                    new BooleanReturning(new BooleanScalar(true)),
                ])
            )
        );
        Assert.Equal(
            value.Conditions.Last().AsT4,
            new BooleanOperator(
                new AndOperator([
                    new BooleanReturning(new BooleanScalar(false)),
                    new BooleanReturning(new BooleanScalar(true)),
                ])
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
            new AndOperator([
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true))
                        )
                    )
                ),
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true))
                        )
                    )
                ),
            ]),
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
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
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
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
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
            value.Conditions.First().AsT4,
            new BooleanOperator(
                new AndOperator([
                    new BooleanReturning(new BooleanScalar(false)),
                    new BooleanReturning(new BooleanScalar(true)),
                ])
            )
        );
        Assert.Equal(value.Conditions.Skip(1).First().AsT2, new BooleanScalar(true));
        Assert.Equal(
            value.Conditions.Skip(2).First().AsT0,
            new BooleanField(expectedEntityName, expectedFieldName)
        );
        Assert.Equal(
            value.Conditions.Skip(3).First().AsT1,
            new BooleanParameter(expectedParamName)
        );
        Assert.Equal(
            value.Conditions.Skip(4).First().AsT3,
            new Equality(
                new BooleanEquality(
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false))
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
                  "entity": "arefdjhubn",
                  "field": "fdevihjn",
                  "type": {
                    "name": "{{type}}"
                  }
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

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void WriteMixedConditions()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
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
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
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

        string value = JsonSerializer.Serialize(
            new AndOperator([
                new BooleanReturning(
                    new BooleanOperator(
                        new AndOperator([
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true)),
                        ])
                    )
                ),
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(
                    new BooleanField(expectedEntityName, expectedFieldName)
                ),
                new BooleanReturning(new BooleanParameter(expectedParamName)),
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(true)),
                            new BooleanReturning(new BooleanScalar(false))
                        )
                    )
                ),
            ]),
            _options
        );
        Assert.Equal(expected, value);
    }
}
