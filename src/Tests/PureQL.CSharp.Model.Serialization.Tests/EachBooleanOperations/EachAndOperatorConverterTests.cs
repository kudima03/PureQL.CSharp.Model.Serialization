using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.EachBooleanOperations;

public sealed record EachAndOperatorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachAndOperatorConverterTests()
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
            JsonSerializer.Deserialize<EachAndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "invalidOperatorName",
              "conditions": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAndOperator>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd",
              "conditions": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAndOperator>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyConditions()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd",
              "conditions": []
            }
            """;

        EachAndOperator value = JsonSerializer.Deserialize<EachAndOperator>(
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
              "operator": "eachAnd",
              "conditions": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachAndOperator([]),
            _options
        );
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
              "operator": "eachAnd",
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

        EachAndOperator value = JsonSerializer.Deserialize<EachAndOperator>(
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
              "operator": "eachAnd",
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
            new EachAndOperator([
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
              "operator": "eachAnd",
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

        EachAndOperator value = JsonSerializer.Deserialize<EachAndOperator>(
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
              "operator": "eachAnd",
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
            new EachAndOperator([
                new BooleanArrayReturning(
                    new BooleanArrayParameter(expectedParamName)
                ),
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
              "operator": "eachAnd",
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

        EachAndOperator value = JsonSerializer.Deserialize<EachAndOperator>(
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
              "operator": "eachAnd",
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
            new EachAndOperator([
                new BooleanArrayReturning(
                    new BooleanArrayScalar([true, false])
                ),
            ]),
            _options
        );
        Assert.Equal(expected, output);
    }
}
