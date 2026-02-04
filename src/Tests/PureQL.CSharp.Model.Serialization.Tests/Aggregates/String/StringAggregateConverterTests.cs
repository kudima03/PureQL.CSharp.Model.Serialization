using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.String;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.String;

public sealed record StringAggregateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringAggregateConverterTests()
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
                    "name": "stringArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
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
                    "name": "stringArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_string"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_string",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_string",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMax()
    {
        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                  "type": {
                    "name": "stringArray"
                  },
                  "value": ["afirndhujvr", "sahbjndfashbndfj", "dnfjkanjkf"]
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            ["afirndhujvr", "sahbjndfashbndfj", "dnfjkanjkf"],
            value.AsT0.Argument.AsT2.Value
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMax(string type)
    {
        const string str = "aeiwnfhsubdrj";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": "{{str}}"
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMax()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                "type": {
                  "name": "stringArray"
                },
                "value": [
                  "afirndhujvr",
                  "sahbjndfashbndfj",
                  "dnfjkanjkf"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MaxString(
                    new StringArrayReturning(
                        new StringArrayScalar(
                            ["afirndhujvr", "sahbjndfashbndfj", "dnfjkanjkf"]
                        )
                    )
                )
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
              "operator": "max_string",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "stringArray"
                  }
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new StringArrayParameter(expectedParamName),
            value.AsT0.Argument.AsT0
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMax(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMax()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_string",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "stringArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MaxString(
                    new StringArrayReturning(new StringArrayParameter(expectedParamName))
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
              "operator": "max_string",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "stringArray"
                  }
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new StringField(expectedEntityName, expectedFieldName),
            value.AsT0.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
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
              "operator": "max_string",
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
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
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
              "operator": "max_string",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "stringArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MaxString(
                    new StringArrayReturning(
                        new StringField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsenceOnMin()
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
                    "name": "stringArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMin()
    {
        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                  "type": {
                    "name": "stringArray"
                  },
                  "value": ["afirndhujvr", "sahbjndfashbndfj", "dnfjkanjkf"]
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            ["afirndhujvr", "sahbjndfashbndfj", "dnfjkanjkf"],
            value.AsT1.Argument.AsT2.Value
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMin(string type)
    {
        const string str = "aeiwnfhsubdrj";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": "{{str}}"
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMin()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                "type": {
                  "name": "stringArray"
                },
                "value": [
                  "afirndhujvr",
                  "sahbjndfashbndfj",
                  "dnfjkanjkf"
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MinString(
                    new StringArrayReturning(
                        new StringArrayScalar(
                            ["afirndhujvr", "sahbjndfashbndfj", "dnfjkanjkf"]
                        )
                    )
                )
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
              "operator": "min_string",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "stringArray"
                  }
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new StringArrayParameter(expectedParamName),
            value.AsT1.Argument.AsT0
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMin(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMin()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_string",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "stringArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MinString(
                    new StringArrayReturning(new StringArrayParameter(expectedParamName))
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
              "operator": "min_string",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "stringArray"
                  }
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(
            new StringField(expectedEntityName, expectedFieldName),
            value.AsT1.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
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
              "operator": "min_string",
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
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
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
              "operator": "min_string",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "stringArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MinString(
                    new StringArrayReturning(
                        new StringField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
