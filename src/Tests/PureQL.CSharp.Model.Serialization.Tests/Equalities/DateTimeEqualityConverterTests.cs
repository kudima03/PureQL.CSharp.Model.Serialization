using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record DateTimeEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeEqualityConverterTests()
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
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        DateTimeEquality value = JsonSerializer.Deserialize<DateTimeEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new DateTimeScalar(now));
        Assert.Equal(value.Right.AsT1, new DateTimeScalar(now));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        DateTime now = DateTime.Now;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
    {
        DateTime now = DateTime.Now;
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeEquality(
                new DateTimeReturning(new DateTimeScalar(now)),
                new DateTimeReturning(new DateTimeScalar(now))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgs()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        DateTimeEquality value = JsonSerializer.Deserialize<DateTimeEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT0, new DateTimeParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new DateTimeParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
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
              "operator": "equal",
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "{{type}}"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "{{type}}"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgs()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{expectedFirstParamName}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeEquality(
                new DateTimeReturning(new DateTimeParameter(expectedFirstParamName)),
                new DateTimeReturning(new DateTimeParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgs()
    {
        DateTime expected = DateTime.Now;
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": "{{expected:O}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        DateTimeEquality value = JsonSerializer.Deserialize<DateTimeEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new DateTimeScalar(expected));
        Assert.Equal(value.Right.AsT0, new DateTimeParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        DateTime expected = DateTime.Now;
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{expected:O}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "{{type}}"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedArgs()
    {
        DateTime expectedValue = DateTime.Now;
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{JsonSerializer.Serialize(expectedValue, _options)}}
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateTimeEquality(
                new DateTimeReturning(new DateTimeScalar(expectedValue)),
                new DateTimeReturning(new DateTimeParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
