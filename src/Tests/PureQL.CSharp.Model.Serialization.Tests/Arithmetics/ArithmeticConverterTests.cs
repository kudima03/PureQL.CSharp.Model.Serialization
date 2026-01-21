using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Arithmetics;

public sealed record ArithmeticConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ArithmeticConverterTests()
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "or",
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentsOnAdd()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentsOnAdd()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentsWrongTypeOnAdd()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyArgumentsOnAdd()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Empty(value.AsT0.Arguments);
    }

    [Fact]
    public void WriteEmptyArgumentsOnAdd()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        string value = JsonSerializer.Serialize(new Arithmetic(new Add([])), _options);
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentsOnAdd()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(value.AsT0.Arguments.First().AsT2, new NumberScalar(expectedValue1));
        Assert.Equal(value.AsT0.Arguments.Last().AsT2, new NumberScalar(expectedValue2));
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarTypeOnAdd(string type)
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentsOnAdd()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            new Arithmetic(
                new Add(
                    [
                        new NumberReturning(new NumberScalar(expectedValue1)),
                        new NumberReturning(new NumberScalar(expectedValue2)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentsOnAdd()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT0.Arguments.First().AsT1,
            new NumberParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.AsT0.Arguments.Last().AsT1,
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
    public void ThrowsExceptionOnWrongParameterTypeOnAdd(string type)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentsOnAdd()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            new Arithmetic(
                new Add(
                    [
                        new NumberReturning(new NumberParameter(expectedFirstParamName)),
                        new NumberReturning(new NumberParameter(expectedSecondParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentsOnAdd()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT0.Arguments.First().AsT0,
            new NumberField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.AsT0.Arguments.Last().AsT0,
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
    public void ThrowsExceptionOnWrongFieldTypeOnAdd(string type)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentsOnAdd()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            new Arithmetic(
                new Add(
                    [
                        new NumberReturning(
                            new NumberField(
                                expectedFirstEntityName,
                                expectedFirstFieldName
                            )
                        ),
                        new NumberReturning(
                            new NumberField(
                                expectedSecondEntityName,
                                expectedSecondFieldName
                            )
                        ),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgumentsOnAdd()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(new NumberScalar(expectedValue), value.AsT0.Arguments.First().AsT2);
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT0.Arguments.Skip(1).First().AsT0
        );
        Assert.Equal(
            value.AsT0.Arguments.Skip(2).First().AsT1,
            new NumberParameter(expectedParamName)
        );
    }

    [Fact]
    public void WriteMixedConditionsOnAdd()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
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
            new Arithmetic(
                new Add(
                    [
                        new NumberReturning(new NumberScalar(expectedValue)),
                        new NumberReturning(
                            new NumberField(expectedEntityName, expectedFieldName)
                        ),
                        new NumberReturning(new NumberParameter(expectedParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentsOnDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "divide"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentsOnDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "divide",
              "arguments": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentsWrongTypeOnDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "divide",
              "arguments": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyArgumentsOnDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "divide",
              "arguments": []
            }
            """;

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Empty(value.AsT1.Arguments);
    }

    [Fact]
    public void WriteEmptyArgumentsOnDivide()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "divide",
              "arguments": []
            }
            """;

        string value = JsonSerializer.Serialize(new Arithmetic(new Divide([])), _options);
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentsOnDivide()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(value.AsT1.Arguments.First().AsT2, new NumberScalar(expectedValue1));
        Assert.Equal(value.AsT1.Arguments.Last().AsT2, new NumberScalar(expectedValue2));
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarTypeOnDivide(string type)
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentsOnDivide()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            new Arithmetic(
                new Divide(
                    [
                        new NumberReturning(new NumberScalar(expectedValue1)),
                        new NumberReturning(new NumberScalar(expectedValue2)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentsOnDivide()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT1.Arguments.First().AsT1,
            new NumberParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.AsT1.Arguments.Last().AsT1,
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
    public void ThrowsExceptionOnWrongParameterTypeOnDivide(string type)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentsOnDivide()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            new Arithmetic(
                new Divide(
                    [
                        new NumberReturning(new NumberParameter(expectedFirstParamName)),
                        new NumberReturning(new NumberParameter(expectedSecondParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentsOnDivide()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT1.Arguments.First().AsT0,
            new NumberField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.AsT1.Arguments.Last().AsT0,
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
    public void ThrowsExceptionOnWrongFieldTypeOnDivide(string type)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentsOnDivide()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            new Arithmetic(
                new Divide(
                    [
                        new NumberReturning(
                            new NumberField(
                                expectedFirstEntityName,
                                expectedFirstFieldName
                            )
                        ),
                        new NumberReturning(
                            new NumberField(
                                expectedSecondEntityName,
                                expectedSecondFieldName
                            )
                        ),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgumentsOnDivide()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(new NumberScalar(expectedValue), value.AsT1.Arguments.First().AsT2);
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT1.Arguments.Skip(1).First().AsT0
        );
        Assert.Equal(
            value.AsT1.Arguments.Skip(2).First().AsT1,
            new NumberParameter(expectedParamName)
        );
    }

    [Fact]
    public void WriteMixedConditionsOnDivide()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "divide",
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
            new Arithmetic(
                new Divide(
                    [
                        new NumberReturning(new NumberScalar(expectedValue)),
                        new NumberReturning(
                            new NumberField(expectedEntityName, expectedFieldName)
                        ),
                        new NumberReturning(new NumberParameter(expectedParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentsOnMultiply()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentsOnMultiply()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentsWrongTypeOnMultiply()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyArgumentsOnMultiplyOnMultiply()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": []
            }
            """;

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Empty(value.AsT2.Arguments);
    }

    [Fact]
    public void WriteEmptyArgumentsOnMultiply()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "multiply",
              "arguments": []
            }
            """;

        string value = JsonSerializer.Serialize(
            new Arithmetic(new Multiply([])),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentsOnMultiply()
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(value.AsT2.Arguments.First().AsT2, new NumberScalar(expectedValue1));
        Assert.Equal(value.AsT2.Arguments.Last().AsT2, new NumberScalar(expectedValue2));
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarTypeOnMultiply(string type)
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentsOnMultiply()
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
            new Arithmetic(
                new Multiply(
                    [
                        new NumberReturning(new NumberScalar(expectedValue1)),
                        new NumberReturning(new NumberScalar(expectedValue2)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentsOnMultiply()
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT2.Arguments.First().AsT1,
            new NumberParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.AsT2.Arguments.Last().AsT1,
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
    public void ThrowsExceptionOnWrongParameterTypeOnMultiply(string type)
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentsOnMultiply()
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
            new Arithmetic(
                new Multiply(
                    [
                        new NumberReturning(new NumberParameter(expectedFirstParamName)),
                        new NumberReturning(new NumberParameter(expectedSecondParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentsOnMultiply()
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT2.Arguments.First().AsT0,
            new NumberField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.AsT2.Arguments.Last().AsT0,
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
    public void ThrowsExceptionOnWrongFieldTypeOnMultiply(string type)
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentsOnMultiply()
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
            new Arithmetic(
                new Multiply(
                    [
                        new NumberReturning(
                            new NumberField(
                                expectedFirstEntityName,
                                expectedFirstFieldName
                            )
                        ),
                        new NumberReturning(
                            new NumberField(
                                expectedSecondEntityName,
                                expectedSecondFieldName
                            )
                        ),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgumentsOnMultiply()
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(new NumberScalar(expectedValue), value.AsT2.Arguments.First().AsT2);
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT2.Arguments.Skip(1).First().AsT0
        );
        Assert.Equal(
            value.AsT2.Arguments.Skip(2).First().AsT1,
            new NumberParameter(expectedParamName)
        );
    }

    [Fact]
    public void WriteMixedConditionsOnMultiply()
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
            new Arithmetic(
                new Multiply(
                    [
                        new NumberReturning(new NumberScalar(expectedValue)),
                        new NumberReturning(
                            new NumberField(expectedEntityName, expectedFieldName)
                        ),
                        new NumberReturning(new NumberParameter(expectedParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentsOnSubtract()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentsOnSubtract()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract",
              "arguments": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentsWrongTypeOnSubtract()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract",
              "arguments": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyArgumentsOnSubtract()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract",
              "arguments": []
            }
            """;

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Empty(value.AsT3.Arguments);
    }

    [Fact]
    public void WriteEmptyArgumentsOnSubtract()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "subtract",
              "arguments": []
            }
            """;

        string value = JsonSerializer.Serialize(
            new Arithmetic(new Subtract([])),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentsOnSubtract()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(value.AsT3.Arguments.First().AsT2, new NumberScalar(expectedValue1));
        Assert.Equal(value.AsT3.Arguments.Last().AsT2, new NumberScalar(expectedValue2));
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarTypeOnSubtract(string type)
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentsOnSubtract()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            new Arithmetic(
                new Subtract(
                    [
                        new NumberReturning(new NumberScalar(expectedValue1)),
                        new NumberReturning(new NumberScalar(expectedValue2)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentsOnSubtract()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT3.Arguments.First().AsT1,
            new NumberParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.AsT3.Arguments.Last().AsT1,
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
    public void ThrowsExceptionOnWrongParameterTypeOnSubtract(string type)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentsOnSubtract()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            new Arithmetic(
                new Subtract(
                    [
                        new NumberReturning(new NumberParameter(expectedFirstParamName)),
                        new NumberReturning(new NumberParameter(expectedSecondParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentsOnSubtract()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(
            value.AsT3.Arguments.First().AsT0,
            new NumberField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.AsT3.Arguments.Last().AsT0,
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
    public void ThrowsExceptionOnWrongFieldTypeOnSubtract(string type)
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            JsonSerializer.Deserialize<Arithmetic>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentsOnSubtract()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            new Arithmetic(
                new Subtract(
                    [
                        new NumberReturning(
                            new NumberField(
                                expectedFirstEntityName,
                                expectedFirstFieldName
                            )
                        ),
                        new NumberReturning(
                            new NumberField(
                                expectedSecondEntityName,
                                expectedSecondFieldName
                            )
                        ),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgumentsOnSubtract()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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

        Arithmetic value = JsonSerializer.Deserialize<Arithmetic>(input, _options)!;
        Assert.Equal(new NumberScalar(expectedValue), value.AsT3.Arguments.First().AsT2);
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT3.Arguments.Skip(1).First().AsT0
        );
        Assert.Equal(
            value.AsT3.Arguments.Skip(2).First().AsT1,
            new NumberParameter(expectedParamName)
        );
    }

    [Fact]
    public void WriteMixedConditionsOnSubtract()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "subtract",
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
            new Arithmetic(
                new Subtract(
                    [
                        new NumberReturning(new NumberScalar(expectedValue)),
                        new NumberReturning(
                            new NumberField(expectedEntityName, expectedFieldName)
                        ),
                        new NumberReturning(new NumberParameter(expectedParamName)),
                    ]
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
