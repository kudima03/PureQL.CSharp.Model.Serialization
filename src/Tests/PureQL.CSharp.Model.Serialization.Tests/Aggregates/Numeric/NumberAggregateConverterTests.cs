using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

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
                    "name": "numberArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnAverage()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(values, value.AsT0.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnAverage(string type)
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  {{values.First()}},
                  {{values.Skip(1).First()}},
                  {{values.Skip(2).First()}}
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverage()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new AverageNumber(new NumberArrayReturning(new NumberArrayScalar(values)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverage()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberArrayParameter(expectedParamName),
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnAverage(string type)
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
    public void WriteParameterArgumentOnAverage()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new AverageNumber(
                    new NumberArrayReturning(new NumberArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnAverage()
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
                    "name": "numberArray"
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
            value.AsT0.Argument.AsT1
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnAverage(string type)
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
    public void WriteFieldArgumentOnAverage()
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
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new AverageNumber(
                    new NumberArrayReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
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
                    "name": "numberArray"
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
              "operator": "max_number"
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
              "operator": "max_number",
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
              "operator": "max_number",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMax()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(values, value.AsT1.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMax(string type)
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  {{values.First()}},
                  {{values.Skip(1).First()}},
                  {{values.Skip(2).First()}}
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMax()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MaxNumber(new NumberArrayReturning(new NumberArrayScalar(values)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMax()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberArrayParameter(expectedParamName),
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMax(string type)
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
    public void WriteParameterArgument()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MaxNumber(
                    new NumberArrayReturning(new NumberArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMax()
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
                    "name": "numberArray"
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
            value.AsT1.Argument.AsT1
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnMax(string type)
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
    public void WriteFieldArgumentOnMax()
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
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MaxNumber(
                    new NumberArrayReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnMin()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(values, value.AsT2.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMin(string type)
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
    public void WriteScalarArgumentOnMin()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MinNumber(new NumberArrayReturning(new NumberArrayScalar(values)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        NumberAggregate value = JsonSerializer.Deserialize<NumberAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new NumberArrayParameter(expectedParamName),
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMin(string type)
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
    public void WriteParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MinNumber(
                    new NumberArrayReturning(new NumberArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMin()
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
                    "name": "numberArray"
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
            value.AsT2.Argument.AsT1
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnMin(string type)
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
    public void WriteFieldArgumentOnMin()
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
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberAggregate(
                new MinNumber(
                    new NumberArrayReturning(
                        new NumberField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgument()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        SumNumber value = JsonSerializer.Deserialize<SumNumber>(input, _options)!;
        Assert.Equal(values, value.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnSum(string type)
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SumNumber>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnSum()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new SumNumber(new NumberArrayReturning(new NumberArrayScalar(values))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnSum()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "sum",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        SumNumber value = JsonSerializer.Deserialize<SumNumber>(input, _options)!;
        Assert.Equal(new NumberArrayParameter(expectedParamName), value.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnSum(string type)
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
            JsonSerializer.Deserialize<SumNumber>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnSum()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "sum",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new SumNumber(
                new NumberArrayReturning(new NumberArrayParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnSum()
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
                    "name": "numberArray"
                  }
                }
            }
            """;

        SumNumber value = JsonSerializer.Deserialize<SumNumber>(input, _options)!;
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.Argument.AsT1
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnSum(string type)
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
            JsonSerializer.Deserialize<SumNumber>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnSum()
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
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new SumNumber(
                new NumberArrayReturning(
                    new NumberField(expectedEntityName, expectedFieldName)
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
