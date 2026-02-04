using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record DateEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateEqualityConverterTests()
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
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        DateEquality value = JsonSerializer.Deserialize<DateEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT1, new DateScalar(now));
        Assert.Equal(value.Right.AsT1, new DateScalar(now));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
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
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{JsonSerializer.Serialize(now, _options)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateEquality(
                new DateReturning(new DateScalar(now)),
                new DateReturning(new DateScalar(now))
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
                  "name": "date"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        DateEquality value = JsonSerializer.Deserialize<DateEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT0, new DateParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new DateParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
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
            JsonSerializer.Deserialize<DateEquality>(input, _options)
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
                  "name": "date"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateEquality(
                new DateReturning(new DateParameter(expectedFirstParamName)),
                new DateReturning(new DateParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgs()
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "{{expectedDate:yyyy-MM-dd}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        DateEquality value = JsonSerializer.Deserialize<DateEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT1, new DateScalar(expectedDate));
        Assert.Equal(value.Right.AsT0, new DateParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{expectedDate:yyyy-MM-dd}}"
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
            JsonSerializer.Deserialize<DateEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedArgs()
    {
        DateOnly expectedDate = DateOnly.FromDateTime(DateTime.Now);

        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "{{expectedDate:yyyy-MM-dd}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateEquality(
                new DateReturning(new DateScalar(expectedDate)),
                new DateReturning(new DateParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
