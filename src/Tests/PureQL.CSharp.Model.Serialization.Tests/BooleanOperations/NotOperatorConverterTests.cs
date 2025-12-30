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

public sealed record NotOperatorConverterTests
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
            new NotOperatorConverter(),
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
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "bduasfehrygcvdubfsahycv",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedcondition()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullcondition()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnconditionWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarCondition()
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

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;
        Assert.Equal(value.Condition.AsT2, new BooleanScalar(true));
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
              "operator": "not",
              "condition": 
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": true
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarCondition()
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

        string value = JsonSerializer.Serialize(
            new NotOperator(new BooleanReturning(new BooleanScalar(true))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterCondition()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "boolean"
                  }
                }
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;
        Assert.Equal(value.Condition.AsT1, new BooleanParameter(expectedParamName));
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
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void WriteParametercondition()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NotOperator(
                new BooleanReturning(new BooleanParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldcondition()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "boolean"
                  }
                }
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;
        Assert.Equal(
            value.Condition.AsT0,
            new BooleanField(expectedEntityName, expectedFieldName)
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
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                  "entity": "{{expectedEntityName}}"
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldcondition()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "not",
              "condition": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NotOperator(
                new BooleanReturning(
                    new BooleanField(expectedEntityName, expectedFieldName)
                )
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
    public void ReadEqualitycondition()
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
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;
        Assert.Equal(
            value.Condition.AsT3,
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
              "operator": "not",
              "condition": {
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
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void WriteEqualitycondition()
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
                    "value": false
                  },
                  "right": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NotOperator(
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true))
                        )
                    )
                )
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
    public void ReadBooleanOperatorcondition()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                  "operator": "or",
                  "condition": [
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
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;
        Assert.Equal(
            value.Condition.AsT4,
            new BooleanOperator(
                new OrOperator([
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
              "operator": "not",
              "condition": {
                  "operator": "or",
                  "condition": [
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
            }
            """;

        NotOperator value = JsonSerializer.Deserialize<NotOperator>(input, _options)!;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NotOperator>(input, _options)
        );
    }

    [Fact(Skip = "NotImplemented")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "xUnit1004:Test methods should not be skipped",
        Justification = "<Pending>"
    )]
    public void WriteBooleanOperatorcondition()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                  "operator": "or",
                  "condition": [
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
            }
            """;

        string value = JsonSerializer.Serialize(
            new NotOperator(
                new BooleanReturning(
                    new Equality(
                        new BooleanEquality(
                            new BooleanReturning(new BooleanScalar(false)),
                            new BooleanReturning(new BooleanScalar(true))
                        )
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
