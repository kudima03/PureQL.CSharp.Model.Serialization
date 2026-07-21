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

public sealed record EachOrOperatorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachOrOperatorConverterTests()
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
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachOrOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd",
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachOrOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "badOperator",
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachOrOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachOr"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachOrOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachOrOperator>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": []
            }
            """;

        EachOrOperator value = JsonSerializer.Deserialize<EachOrOperator>(
            input,
            _options
        )!;
        Assert.Empty(value.Conditions);
    }

    [Fact]
    public void WriteEmptyConditions()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": []
            }
            """;

        string output = JsonSerializer.Serialize(new EachOrOperator([]), _options);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadFieldConditions()
    {
        const string expectedEntity = "someEntity";
        const string expectedField = "someField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachOr",
              "conditions": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "boolean"
                  }
                }
              ]
            }
            """;

        EachOrOperator value = JsonSerializer.Deserialize<EachOrOperator>(
            input,
            _options
        )!;
        Assert.Equal(
            new BooleanField(expectedEntity, expectedField),
            value.Conditions.Single().AsT1
        );
    }

    [Fact]
    public void WriteFieldConditions()
    {
        const string expectedEntity = "someEntity";
        const string expectedField = "someField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachOr",
              "conditions": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "boolean"
                  }
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachOrOperator([
                new BooleanArrayReturning(
                    new BooleanField(expectedEntity, expectedField)
                ),
            ]),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadParameterConditions()
    {
        const string expectedParamName = "myParam";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachOr",
              "conditions": [
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "booleanArray"
                  }
                }
              ]
            }
            """;

        EachOrOperator value = JsonSerializer.Deserialize<EachOrOperator>(
            input,
            _options
        )!;
        Assert.Equal(
            new BooleanArrayParameter(expectedParamName),
            value.Conditions.Single().AsT2
        );
    }

    [Fact]
    public void WriteParameterConditions()
    {
        const string expectedParamName = "myParam";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachOr",
              "conditions": [
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "booleanArray"
                  }
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachOrOperator([
                new BooleanArrayReturning(new BooleanArrayParameter(expectedParamName)),
            ]),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadScalarConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": [
                {
                  "type": {
                    "name": "booleanArray"
                  },
                  "value": [
                    true,
                    false
                  ]
                }
              ]
            }
            """;

        EachOrOperator value = JsonSerializer.Deserialize<EachOrOperator>(
            input,
            _options
        )!;
        Assert.Equal([true, false], value.Conditions.Single().AsT0.Value);
    }

    [Fact]
    public void WriteScalarConditions()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": [
                {
                  "type": {
                    "name": "booleanArray"
                  },
                  "value": [
                    true,
                    false
                  ]
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachOrOperator([
                new BooleanArrayReturning(new BooleanArrayScalar([true, false])),
            ]),
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
              "operator": "eachOr",
              "conditions": [
                {
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
              ]
            }
            """;

        EachOrOperator value = JsonSerializer.Deserialize<EachOrOperator>(
            input,
            _options
        )!;
        EachNumberComparison comp = value.Conditions.Single().AsT3.AsT0;
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
              "operator": "eachOr",
              "conditions": [
                {
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
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachOrOperator([
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
                ),
            ]),
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
              "operator": "eachOr",
              "conditions": [
                {
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
              ]
            }
            """;

        EachOrOperator value = JsonSerializer.Deserialize<EachOrOperator>(
            input,
            _options
        )!;
        EachStringEquality equality = value.Conditions.Single().AsT4.AsT2;
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
              "operator": "eachOr",
              "conditions": [
                {
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
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachOrOperator([
                new BooleanArrayReturning(
                    new EachEquality(
                        new EachStringEquality(
                            new StringArrayReturning(
                                new StringField(expectedEntity, expectedField)
                            ),
                            new StringReturning(new StringScalar(expectedValue))
                        )
                    )
                ),
            ]),
            _options
        );
        Assert.Equal(expected, output);
    }
}
