using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record UuidEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidEqualityConverterTests()
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
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        UuidEquality value = JsonSerializer.Deserialize<UuidEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT1, new UuidScalar(guid));
        Assert.Equal(value.Right.AsT1, new UuidScalar(guid));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("number")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        Guid guid = Guid.NewGuid();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
    {
        Guid guid = Guid.NewGuid();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{JsonSerializer.Serialize(guid)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new UuidEquality(
                new UuidReturning(new UuidScalar(guid)),
                new UuidReturning(new UuidScalar(guid))
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
                  "name": "uuid"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "uuid"
                }
              }
            }
            """;

        UuidEquality value = JsonSerializer.Deserialize<UuidEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT0, new UuidParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new UuidParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("number")]
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
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
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
                  "name": "uuid"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "uuid"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new UuidEquality(
                new UuidReturning(new UuidParameter(expectedFirstParamName)),
                new UuidReturning(new UuidParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgs()
    {
        Guid expected = Guid.NewGuid();
        const string expectedParamName = "ashjlbd";

        string input = $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{expected}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "uuid"
                }
              }
            }
            """;

        UuidEquality value = JsonSerializer.Deserialize<UuidEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT1, new UuidScalar(expected));
        Assert.Equal(value.Right.AsT0, new UuidParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("number")]
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        Guid expected = Guid.NewGuid();
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": "{{expected}}"
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
            JsonSerializer.Deserialize<UuidEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedArgs()
    {
        Guid expectedValue = Guid.NewGuid();
        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{expectedValue}}"
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "uuid"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new UuidEquality(
                new UuidReturning(new UuidScalar(expectedValue)),
                new UuidReturning(new UuidParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
