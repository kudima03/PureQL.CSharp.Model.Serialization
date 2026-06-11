using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.Fields;

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

        string output = JsonSerializer.Serialize(
            new EachOrOperator([]),
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
                new BooleanArrayReturning(
                    new BooleanArrayParameter(expectedParamName)
                ),
            ]),
            _options
        );
        Assert.Equal(expected, output);
    }
}
