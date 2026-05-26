using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Numeric;

public sealed record CountConverterTests
{
    private readonly JsonSerializerOptions _options;

    public CountConverterTests()
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
              "arg": {
                "entity": "entityName",
                "field": "fieldName",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Count>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "sum",
              "arg": {
                "entity": "entityName",
                "field": "fieldName",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Count>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "euhwyrfdbuyeghrfdb",
              "arg": {
                "entity": "entityName",
                "field": "fieldName",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Count>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "count"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Count>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "count",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Count>(input, _options)
        );
    }

    [Fact]
    public void ReadFieldArgument()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        Count value = JsonSerializer.Deserialize<Count>(input, _options)!;
        Assert.Equal(
            new NumberField(expectedEntity, expectedFieldName),
            value.Argument.AsT3.AsT1
        );
    }

    [Fact]
    public void WriteFieldArgument()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Count(
                new ArrayReturning(
                    new NumberArrayReturning(
                        new NumberField(expectedEntity, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadParameterArgument()
    {
        const string expectedParamName = "myParam";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        Count value = JsonSerializer.Deserialize<Count>(input, _options)!;
        Assert.Equal(new NumberArrayParameter(expectedParamName), value.Argument.AsT3.AsT0);
    }

    [Fact]
    public void WriteParameterArgument()
    {
        const string expectedParamName = "myParam";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Count(
                new ArrayReturning(
                    new NumberArrayReturning(new NumberArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadScalarArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "count",
              "arg": {
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

        Count value = JsonSerializer.Deserialize<Count>(input, _options)!;
        Assert.Equal([true, false], value.Argument.AsT0.AsT0.Value);
    }

    [Fact]
    public void WriteScalarArgument()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "count",
              "arg": {
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
            new Count(
                new ArrayReturning(
                    new BooleanArrayReturning(new BooleanArrayScalar([true, false]))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanFieldArgument()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        Count value = JsonSerializer.Deserialize<Count>(input, _options)!;
        Assert.Equal(
            new BooleanField(expectedEntity, expectedFieldName),
            value.Argument.AsT0.AsT1
        );
    }
}
