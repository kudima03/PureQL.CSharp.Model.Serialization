using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.Fields;

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
                  "name": "booleanArray"
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
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachNotOperator(
                new BooleanArrayReturning(
                    new BooleanField(expectedEntity, expectedField)
                )
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
        Assert.Equal(
            new BooleanArrayParameter(expectedParamName),
            value.Condition.AsT2
        );
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
                new BooleanArrayReturning(
                    new BooleanArrayParameter(expectedParamName)
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
