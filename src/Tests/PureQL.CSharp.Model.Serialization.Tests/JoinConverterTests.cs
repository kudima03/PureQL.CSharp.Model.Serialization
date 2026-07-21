using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record JoinConverterTests
{
    private readonly JsonSerializerOptions _options;

    public JoinConverterTests()
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

    [Theory]
    [InlineData(JoinType.Inner)]
    [InlineData(JoinType.Full)]
    [InlineData(JoinType.Left)]
    [InlineData(JoinType.Right)]
    public void Read(JoinType type)
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {{JsonSerializer.Serialize(type, _options)}},
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        Join value = JsonSerializer.Deserialize<Join>(input, _options)!;
        Assert.Equal(
            new Join(type, expectedEntity, new BooleanReturning(new BooleanScalar(true))),
            value
        );
    }

    [Theory]
    [InlineData(JoinType.Inner)]
    [InlineData(JoinType.Full)]
    [InlineData(JoinType.Left)]
    [InlineData(JoinType.Right)]
    public void Write(JoinType type)
    {
        const string expectedEntity = "refnhbdjusi";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {{JsonSerializer.Serialize(type, _options)}},
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Join(type, expectedEntity, new BooleanReturning(new BooleanScalar(true))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnWrongJoinType()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "bfheuwdrsj",
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedJoinType()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("string")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongOnType(string type)
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "{{type}}"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOnType()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "rfwsneihjlbinhu"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedOn()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "{{expectedEntity}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyJson()
    {
        const string input = "{}";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyString()
    {
        const string input = "";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedEntity()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEntityWrongType()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": {},
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullEntity()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": null,
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullOn()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "dsijnuf",
              "on": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOnWrongType()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "dsijnuf",
              "on": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    /// <summary>
    /// The realistic join key shape - "ON left.fk = right.id" - built from a
    /// field-to-field eachEqual condition. Every "on" fixture before this only
    /// exercised a trivial boolean literal, never an actual key comparison.
    /// </summary>
    [Fact]
    public void ReadOnFieldToFieldEachEqualityCase()
    {
        const string leftEntity = "people";
        const string leftField = "specialtyId";
        const string rightEntity = "specialties";
        const string rightField = "id";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "inner",
              "entity": "{{rightEntity}}",
              "on": {
                "operator": "eachEqual",
                "left": {
                  "entity": "{{leftEntity}}",
                  "field": "{{leftField}}",
                  "type": {
                    "name": "uuid"
                  }
                },
                "right": {
                  "entity": "{{rightEntity}}",
                  "field": "{{rightField}}",
                  "type": {
                    "name": "uuid"
                  }
                }
              }
            }
            """;

        Join value = JsonSerializer.Deserialize<Join>(input, _options)!;

        Assert.True(value.On.IsT1);
        EachUuidEquality equality = value.On.AsT1.AsT4.AsT6;
        Assert.Equal(new UuidField(leftEntity, leftField), equality.Left.AsT1);
        Assert.Equal(new UuidField(rightEntity, rightField), equality.Right.AsT1.AsT1);
    }

    /// <summary>
    /// A join condition against a literal instead of a key column - only valid
    /// through the row-wise eachEqual family, same as a WHERE filter.
    /// </summary>
    [Fact]
    public void ReadOnFieldToLiteralEachEqualityCase()
    {
        const string joinEntity = "specialties";
        const string joinField = "name";
        const string expectedValue = "Foreman";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "inner",
              "entity": "{{joinEntity}}",
              "on": {
                "operator": "eachEqual",
                "left": {
                  "entity": "{{joinEntity}}",
                  "field": "{{joinField}}",
                  "type": {
                    "name": "string"
                  }
                },
                "right": {
                  "type": {
                    "name": "string"
                  },
                  "value": "{{expectedValue}}"
                }
              }
            }
            """;

        Join value = JsonSerializer.Deserialize<Join>(input, _options)!;

        Assert.True(value.On.IsT1);
        EachStringEquality equality = value.On.AsT1.AsT4.AsT2;
        Assert.Equal(expectedValue, equality.Right.AsT0.AsT1.Value);
    }

    /// <summary>
    /// A compound, multi-column join key: "ON a.x = b.x AND a.y = b.y".
    /// </summary>
    [Fact]
    public void ReadOnCompoundEachAndCase()
    {
        const string leftEntity = "left";
        const string rightEntity = "right";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "inner",
              "entity": "{{rightEntity}}",
              "on": {
                "operator": "eachAnd",
                "conditions": [
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{leftEntity}}",
                      "field": "keyPart1",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "entity": "{{rightEntity}}",
                      "field": "keyPart1",
                      "type": {
                        "name": "string"
                      }
                    }
                  },
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{leftEntity}}",
                      "field": "keyPart2",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "entity": "{{rightEntity}}",
                      "field": "keyPart2",
                      "type": {
                        "name": "string"
                      }
                    }
                  }
                ]
              }
            }
            """;

        Join value = JsonSerializer.Deserialize<Join>(input, _options)!;

        Assert.True(value.On.IsT1);
        EachAndOperator and = value.On.AsT1.AsT5;
        Assert.Equal(2, and.Conditions.Count());
    }

    [Fact]
    public void WriteOnCompoundEachAndCase()
    {
        const string leftEntity = "left";
        const string rightEntity = "right";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "inner",
              "entity": "{{rightEntity}}",
              "on": {
                "operator": "eachAnd",
                "conditions": [
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{leftEntity}}",
                      "field": "keyPart1",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "entity": "{{rightEntity}}",
                      "field": "keyPart1",
                      "type": {
                        "name": "string"
                      }
                    }
                  },
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{leftEntity}}",
                      "field": "keyPart2",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "entity": "{{rightEntity}}",
                      "field": "keyPart2",
                      "type": {
                        "name": "string"
                      }
                    }
                  }
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Join(
                JoinType.Inner,
                rightEntity,
                new BooleanArrayReturning(
                    new EachAndOperator(
                        [
                            new BooleanArrayReturning(
                                new EachEquality(
                                    new EachStringEquality(
                                        new StringArrayReturning(
                                            new StringField(leftEntity, "keyPart1")
                                        ),
                                        new StringArrayReturning(
                                            new StringField(rightEntity, "keyPart1")
                                        )
                                    )
                                )
                            ),
                            new BooleanArrayReturning(
                                new EachEquality(
                                    new EachStringEquality(
                                        new StringArrayReturning(
                                            new StringField(leftEntity, "keyPart2")
                                        ),
                                        new StringArrayReturning(
                                            new StringField(rightEntity, "keyPart2")
                                        )
                                    )
                                )
                            ),
                        ]
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }
}
