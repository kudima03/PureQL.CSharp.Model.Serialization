using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Numeric;

public sealed record NumberAggregateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberAggregateConverterTests()
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
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "euhwyrfdbuyeghrfdb",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_number"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_number",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_number",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnAverageNumber()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "type": {
                    "name": "number"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberScalar(number), value.AsT0.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnAverageNumber(string type)
    {
        DateOnly number = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverageNumber()
    {
        double number = Random.Shared.NextDouble();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                "type": {
                  "name": "number"
                },
                "value": {{number.ToString(CultureInfo.InvariantCulture)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new AverageNumber(new NumberReturning(new NumberScalar(number)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverageNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberParameter(expectedParamName), value.AsT0.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnAverageNumber(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnAverageNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new AverageNumber(
                    new NumberReturning(new NumberParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnAverageNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT0.Argument.AsT0
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnAverageNumber(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnAverageNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new AverageNumber(
                    new NumberReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnMaxNumber()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "type": {
                    "name": "number"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberScalar(number), value.AsT1.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMaxNumber(string type)
    {
        DateOnly number = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMaxNumber()
    {
        double number = Random.Shared.NextDouble();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                "type": {
                  "name": "number"
                },
                "value": {{number.ToString(CultureInfo.InvariantCulture)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MaxNumber(new NumberReturning(new NumberScalar(number)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMaxNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberParameter(expectedParamName), value.AsT1.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnMaxNumber(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMaxNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MaxNumber(new NumberReturning(new NumberParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMaxNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT1.Argument.AsT0
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnMaxNumber(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMaxNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MaxNumber(
                    new NumberReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnMinNumber()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "type": {
                    "name": "number"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberScalar(number), value.AsT2.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMinNumber(string type)
    {
        DateOnly number = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMinNumber()
    {
        double number = Random.Shared.NextDouble();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "type": {
                  "name": "number"
                },
                "value": {{number.ToString(CultureInfo.InvariantCulture)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MinNumber(new NumberReturning(new NumberScalar(number)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMinNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberParameter(expectedParamName), value.AsT2.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnMinNumber(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMinNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MinNumber(new NumberReturning(new NumberParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMinNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT2.Argument.AsT0
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnMinNumber(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMinNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MinNumber(
                    new NumberReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnSumNumber()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "type": {
                    "name": "number"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberScalar(number), value.AsT3.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnSumNumber(string type)
    {
        DateOnly number = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{number.ToString(CultureInfo.InvariantCulture)}}
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnSumNumber()
    {
        double number = Random.Shared.NextDouble();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                "type": {
                  "name": "number"
                },
                "value": {{number.ToString(CultureInfo.InvariantCulture)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new SumNumber(new NumberReturning(new NumberScalar(number)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnSumNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new NumberParameter(expectedParamName), value.AsT3.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnSumNumber(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnSumNumber()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "sum",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new SumNumber(new NumberReturning(new NumberParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnSumNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "number"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.AsT3.Argument.AsT0
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongFieldTypeOnSumNumber(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnSumNumber()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new SumNumber(
                    new NumberReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
