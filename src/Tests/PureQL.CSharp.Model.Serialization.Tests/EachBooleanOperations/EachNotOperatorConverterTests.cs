using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.EachComparisons;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachBooleanOperations;

public sealed record EachNotOperatorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachNotOperatorConverterTests()
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
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [true]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachNotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd",
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [true]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachNotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "bad_operator",
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [true]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachNotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedCondition()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachNot"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachNotOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullCondition()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachNot",
              "condition": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachNotOperator>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarCondition()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachNot",
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  true,
                  false
                ]
              }
            }
            """;

        EachNotOperator value = JsonSerializer.Deserialize<EachNotOperator>(
            input,
            _options
        )!;
        Assert.Equal([true, false], value.Condition.AsT0.Value);
    }

    [Fact]
    public void WriteScalarCondition()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachNot",
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  true,
                  false
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachNotOperator(
                new BooleanArrayReturning(new BooleanArrayScalar([true, false]))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadFieldCondition()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        EachNotOperator value = JsonSerializer.Deserialize<EachNotOperator>(
            input,
            _options
        )!;
        Assert.Equal(
            new BooleanField(expectedEntity, expectedField),
            value.Condition.AsT1
        );
    }

    [Fact]
    public void WriteFieldCondition()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "boolean"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachNotOperator(
                new BooleanArrayReturning(new BooleanField(expectedEntity, expectedField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadParameterCondition()
    {
        const string expectedParamName = "myParam";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        EachNotOperator value = JsonSerializer.Deserialize<EachNotOperator>(
            input,
            _options
        )!;
        Assert.Equal(new BooleanArrayParameter(expectedParamName), value.Condition.AsT2);
    }

    [Fact]
    public void WriteParameterCondition()
    {
        const string expectedParamName = "myParam";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachNotOperator(
                new BooleanArrayReturning(new BooleanArrayParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachComparisonCondition()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "operator": "eachGreaterThan",
                "left": {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "number"
                  }
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 10
                }
              }
            }
            """;

        EachNotOperator value = JsonSerializer.Deserialize<EachNotOperator>(
            input,
            _options
        )!;
        EachNumberComparison comp = value.Condition.AsT3.AsT0;
        Assert.Equal(EachComparisonOperator.EachGreaterThan, comp.Operator);
        Assert.Equal(new NumberField(expectedEntity, expectedField), comp.Left.AsT1);
    }

    [Fact]
    public void WriteEachComparisonCondition()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "operator": "eachGreaterThan",
                "left": {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "number"
                  }
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 10
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachNotOperator(
                new BooleanArrayReturning(
                    new EachComparison(
                        new EachNumberComparison(
                            EachComparisonOperator.EachGreaterThan,
                            new NumberArrayReturning(
                                new NumberField(expectedEntity, expectedField)
                            ),
                            new NumberReturning(new NumberScalar(10))
                        )
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachEqualityCondition()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        const string expectedValue = "sand";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "operator": "eachEqual",
                "left": {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
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

        EachNotOperator value = JsonSerializer.Deserialize<EachNotOperator>(
            input,
            _options
        )!;
        EachStringEquality equality = value.Condition.AsT4.AsT2;
        Assert.Equal(expectedEntity, equality.Left.AsT1.Entity);
        Assert.Equal(expectedValue, equality.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteEachEqualityCondition()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        const string expectedValue = "sand";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachNot",
              "condition": {
                "operator": "eachEqual",
                "left": {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
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

        string output = JsonSerializer.Serialize(
            new EachNotOperator(
                new BooleanArrayReturning(
                    new EachEquality(
                        new EachStringEquality(
                            new StringArrayReturning(
                                new StringField(expectedEntity, expectedField)
                            ),
                            new StringReturning(new StringScalar(expectedValue))
                        )
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
