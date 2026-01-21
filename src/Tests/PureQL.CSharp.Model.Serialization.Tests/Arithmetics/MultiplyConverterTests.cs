using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Arithmetics;

public sealed record MultiplyConverterTests
{
    private readonly JsonSerializerOptions _options;

    public MultiplyConverterTests()
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
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "bduasfehrygcvdubfsahycv",
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArguments()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArguments()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentsWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyArguments()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": []
            }
            """;

        Multiply value = JsonSerializer.Deserialize<Multiply>(input, _options)!;
        Assert.Empty(value.Arguments);
    }

    [Fact]
    public void WriteEmptyArguments()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": []
            }
            """;

        string value = JsonSerializer.Serialize(new Multiply([]), _options);
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArguments()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue1.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue2.ToString(CultureInfo.InvariantCulture)}}
                }
              ]
            }
            """;

        Multiply value = JsonSerializer.Deserialize<Multiply>(input, _options)!;
        Assert.Equal(value.Arguments.First().AsT2, new NumberScalar(expectedValue1));
        Assert.Equal(value.Arguments.Last().AsT2, new NumberScalar(expectedValue2));
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{expectedValue1.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{expectedValue2.ToString(CultureInfo.InvariantCulture)}}
                }
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArguments()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue1.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue2.ToString(CultureInfo.InvariantCulture)}}
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Multiply(
                [
                    new NumberReturning(new NumberScalar(expectedValue1)),
                    new NumberReturning(new NumberScalar(expectedValue2)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArguments()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        Multiply value = JsonSerializer.Deserialize<Multiply>(input, _options)!;
        Assert.Equal(
            value.Arguments.First().AsT1,
            new NumberParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.Arguments.Last().AsT1,
            new NumberParameter(expectedSecondParamName)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
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
              "operator": "multiply",
              "arguments": [
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
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArguments()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Multiply(
                [
                    new NumberReturning(new NumberParameter(expectedFirstParamName)),
                    new NumberReturning(new NumberParameter(expectedSecondParamName)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArguments()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "entity": "{{expectedFirstEntityName}}",
                  "field": "{{expectedFirstFieldName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "entity": "{{expectedSecondEntityName}}",
                  "field": "{{expectedSecondFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        Multiply value = JsonSerializer.Deserialize<Multiply>(input, _options)!;
        Assert.Equal(
            value.Arguments.First().AsT0,
            new NumberField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.Arguments.Last().AsT0,
            new NumberField(expectedSecondEntityName, expectedSecondFieldName)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
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
              "operator": "multiply",
              "arguments": [
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

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Multiply>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArguments()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "entity": "{{expectedFirstEntityName}}",
                  "field": "{{expectedFirstFieldName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "entity": "{{expectedSecondEntityName}}",
                  "field": "{{expectedSecondFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Multiply(
                [
                    new NumberReturning(
                        new NumberField(expectedFirstEntityName, expectedFirstFieldName)
                    ),
                    new NumberReturning(
                        new NumberField(expectedSecondEntityName, expectedSecondFieldName)
                    ),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArguments()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        Multiply value = JsonSerializer.Deserialize<Multiply>(input, _options)!;
        Assert.Equal(new NumberScalar(expectedValue), value.Arguments.First().AsT2);
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.Arguments.Skip(1).First().AsT0
        );
        Assert.Equal(
            value.Arguments.Skip(2).First().AsT1,
            new NumberParameter(expectedParamName)
        );
    }

    [Fact]
    public void WriteMixedConditions()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "multiply",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Multiply(
                [
                    new NumberReturning(new NumberScalar(expectedValue)),
                    new NumberReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    ),
                    new NumberReturning(new NumberParameter(expectedParamName)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
