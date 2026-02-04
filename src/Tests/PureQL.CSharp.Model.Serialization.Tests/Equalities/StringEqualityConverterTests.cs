using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record StringEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringEqualityConverterTests()
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
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
    {
        const string str = "bjhfldbhjfd";
        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        StringEquality value = JsonSerializer.Deserialize<StringEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new StringScalar(str));
        Assert.Equal(value.Right.AsT1, new StringScalar(str));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        const string str = "bjhfldbhjfd";
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{str}}"
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{str}}"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
    {
        const string str = "bjhfldbhjfd";
        const string expected = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{str}}"
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringEquality(
                new StringReturning(new StringScalar(str)),
                new StringReturning(new StringScalar(str))
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
                  "name": "string"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        StringEquality value = JsonSerializer.Deserialize<StringEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT0, new StringParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new StringParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
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
            JsonSerializer.Deserialize<StringEquality>(input, _options)
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
                  "name": "string"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringEquality(
                new StringReturning(new StringParameter(expectedFirstParamName)),
                new StringReturning(new StringParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgs()
    {
        const string expectedParamName = "ashjlbd";

        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "ianhuedrfiuhaerfd"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        StringEquality value = JsonSerializer.Deserialize<StringEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new StringScalar("ianhuedrfiuhaerfd"));
        Assert.Equal(value.Right.AsT0, new StringParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "ianhuedrfiuhaerfd"
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
            JsonSerializer.Deserialize<StringEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedArgs()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "ianhuedrfiuhaerfd"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new StringEquality(
                new StringReturning(new StringScalar("ianhuedrfiuhaerfd")),
                new StringReturning(new StringParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
