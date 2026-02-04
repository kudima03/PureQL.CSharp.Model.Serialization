using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record BooleanEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanEqualityConverterTests()
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
        const string input = /*lang=json,strict*/
            """
            {
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
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
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
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "gfhrjhffifivfhvnuhiu",
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
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
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
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
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
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
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
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
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
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
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

        BooleanEquality value = JsonSerializer.Deserialize<BooleanEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new BooleanScalar(true));
        Assert.Equal(value.Right.AsT1, new BooleanScalar(false));
    }

    [Theory]
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
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": false
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
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

        string value = JsonSerializer.Serialize(
            new BooleanEquality(
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgs()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "boolean"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        BooleanEquality value = JsonSerializer.Deserialize<BooleanEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT0, new BooleanParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new BooleanParameter(expectedSecondParamName));
    }

    [Theory]
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
              "operator": "equal",
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
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgs()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "boolean"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new BooleanEquality(
                new BooleanReturning(new BooleanParameter(expectedFirstParamName)),
                new BooleanReturning(new BooleanParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadEqualityArgs()
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
                  "value": false
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              },
              "right": {
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

        BooleanEquality value = JsonSerializer.Deserialize<BooleanEquality>(
            input,
            _options
        )!;
        Assert.Equal(
            value.Left.AsT2,
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
            value.Right.AsT2,
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

    [Theory]
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
              "operator": "equal",
              "left": {
                "operator": "equal",
                "left": {
                  "type": {
                    "name": {{type}}
                  },
                  "value": false
                },
                "right": {
                  "type": {
                    "name": {{type}}
                  },
                  "value": true
                }
              },
              "right": {
                "operator": "equal",
                "left": {
                  "type": {
                    "name": {{type}}
                  },
                  "value": false
                },
                "right": {
                  "type": {
                    "name": {{type}}
                  },
                  "value": true
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteEqualityArgs()
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
                  "value": false
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              },
              "right": {
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
            new BooleanEquality(
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
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadBooleanOperatorArgs()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
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

        BooleanEquality value = JsonSerializer.Deserialize<BooleanEquality>(
            input,
            _options
        )!;
        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(false)),
                new BooleanReturning(new BooleanScalar(true)),
            ],
            value.Left.AsT3.AsT0.Conditions.AsT0
        );

        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ],
            value.Right.AsT3.AsT0.Conditions.AsT0
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
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteBooleanOperatorArgs()
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
                  "value": false
                },
                "right": {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              },
              "right": {
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
            new BooleanEquality(
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
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgs()
    {
        const string expectedParamName = "ashjlbd";

        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        BooleanEquality value = JsonSerializer.Deserialize<BooleanEquality>(
            input,
            _options
        )!;
        Assert.Equal(
            value,
            new BooleanEquality(
                new BooleanReturning(new BooleanScalar(false)),
                new BooleanReturning(new BooleanParameter(expectedParamName))
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
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
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
            JsonSerializer.Deserialize<BooleanEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedArgs()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new BooleanEquality(
                new BooleanReturning(new BooleanScalar(false)),
                new BooleanReturning(new BooleanParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
