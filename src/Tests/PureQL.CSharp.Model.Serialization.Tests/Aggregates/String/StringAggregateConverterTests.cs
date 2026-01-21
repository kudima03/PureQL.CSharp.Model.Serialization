using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.String;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

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
                    "name": "string"
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
                    "name": "string"
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
              "operator": "min_string"
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
              "operator": "min_string",
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
              "operator": "min_string",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMinString()
    {
        const string str = "aeiwnfhsubdrj";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                  "type": {
                    "name": "string"
                  },
                  "value": "{{str}}"
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new StringScalar(str), value.AsT1.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMinString(string type)
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
    public void WriteScalarArgumentOnMinString()
    {
        const string str = "aeiwnfhsubdrj";
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MinString(new StringReturning(new StringScalar(str)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMinString()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_string",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "string"
                  }
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new StringParameter(expectedParamName), value.AsT1.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnMinString(string type)
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
    public void WriteParameterArgumentOnMinString()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_string",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MinString(new StringReturning(new StringParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMinString()
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
                    "name": "string"
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
    public void ThrowsExceptionOnWrongFieldTypeOnMinString(string type)
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
    public void WriteFieldArgumentOnMinString()
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
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MinString(
                    new StringReturning(
                        new StringField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArgumentOnMaxString()
    {
        const string str = "aeiwnfhsubdrj";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                  "type": {
                    "name": "string"
                  },
                  "value": "{{str}}"
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new StringScalar(str), value.AsT0.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMaxString(string type)
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
    public void WriteScalarArgumentOnMaxString()
    {
        const string str = "aeiwnfhsubdrj";
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MaxString(new StringReturning(new StringScalar(str)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMaxString()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_string",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "string"
                  }
                }
            }
            """;

        StringAggregate value = JsonSerializer.Deserialize<StringAggregate>(
            input,
            _options
        )!;
        Assert.Equal(new StringParameter(expectedParamName), value.AsT0.Argument.AsT1);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ehufry")]
    public void ThrowsExceptionOnWrongParameterTypeOnMaxString(string type)
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
    public void WriteParameterArgumentOnMaxString()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_string",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MaxString(new StringReturning(new StringParameter(expectedParamName)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMaxString()
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
                    "name": "string"
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
    public void ThrowsExceptionOnWrongFieldTypeOnMaxString(string type)
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
    public void WriteFieldArgumentOnMaxString()
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
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringAggregate(
                new MaxString(
                    new StringReturning(
                        new StringField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
