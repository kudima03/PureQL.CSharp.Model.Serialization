using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Equalities;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Returnings;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record TimeEqualityConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new TimeEqualityConverter(),
            new TimeReturningConverter(),
            new TimeFieldConverter(),
            new TimeParameterConverter(),
            new TimeScalarConverter(),
            new TypeConverter<TimeType>(),
        },
    };

    [Fact]
    public void ThrowsExceptionOnOperatorNameAbsence()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        TimeEquality value = JsonSerializer.Deserialize<TimeEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT2, new TimeScalar(now));
        Assert.Equal(value.Right.AsT2, new TimeScalar(now));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("datetime")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{JsonSerializer.Serialize(now)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeEquality(
                new TimeReturning(new TimeScalar(now)),
                new TimeReturning(new TimeScalar(now))
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
                  "name": "time"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        TimeEquality value = JsonSerializer.Deserialize<TimeEquality>(input, _options)!;
        Assert.Equal(value.Left.AsT1, new TimeParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT1, new TimeParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("datetime")]
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
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
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
                  "name": "time"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeEquality(
                new TimeReturning(new TimeParameter(expectedFirstParamName)),
                new TimeReturning(new TimeParameter(expectedSecondParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgs()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "entity": "{{expectedFirstEntityName}}",
                "field": "{{expectedFirstFieldName}}",
                "type": {
                  "name": "time"
                }
              },
              "left": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        TimeEquality value = JsonSerializer.Deserialize<TimeEquality>(input, _options)!;
        Assert.Equal(
            value.Right.AsT0,
            new TimeField(expectedFirstEntityName, expectedFirstFieldName)
        );
        Assert.Equal(
            value.Left.AsT0,
            new TimeField(expectedSecondEntityName, expectedSecondFieldName)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("datetime")]
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
              "operator": "equal",
              "left": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "{{type}}"
                }
              },
              "right": {
                "entity": "{{expectedFirstEntityName}}",
                "field": "{{expectedFirstFieldName}}",
                "type": {
                  "name": "{{type}}"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgs()
    {
        const string expectedFirstEntityName = "aruhybfe";
        const string expectedFirstFieldName = "erafuhyobdng";

        const string expectedSecondEntityName = "rendgijhsftu";
        const string expectedSecondFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{expectedFirstEntityName}}",
                "field": "{{expectedFirstFieldName}}",
                "type": {
                  "name": "time"
                }
              },
              "right": {
                "entity": "{{expectedSecondEntityName}}",
                "field": "{{expectedSecondFieldName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;
        string value = JsonSerializer.Serialize(
            new TimeEquality(
                new TimeReturning(
                    new TimeField(expectedFirstEntityName, expectedFirstFieldName)
                ),
                new TimeReturning(
                    new TimeField(expectedSecondEntityName, expectedSecondFieldName)
                )
            ),
            _options
        );

        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArgs()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        const string input = $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "time"
                }
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        TimeEquality value = JsonSerializer.Deserialize<TimeEquality>(input, _options)!;
        Assert.Equal(
            value.Left.AsT0,
            new TimeField(expectedEntityName, expectedFieldName)
        );
        Assert.Equal(value.Right.AsT1, new TimeParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("datetime")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongConditionType(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "{{type}}"
                }
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
            JsonSerializer.Deserialize<TimeEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteMixedArgs()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";
        const string expectedParamName = "ashjlbd";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "time"
                }
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new TimeEquality(
                new TimeReturning(new TimeField(expectedEntityName, expectedFieldName)),
                new TimeReturning(new TimeParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
