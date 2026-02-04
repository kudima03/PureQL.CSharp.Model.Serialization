using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record NumberEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberEqualityConverterTests()
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
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "and",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "gfhrjhffifivfhvnuhiu",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedLeft()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedRight()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullLeft()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": null,
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullRight()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnLeftWrongType()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": [],
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnRightWrongType()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyRight()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyLeft()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "left": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgs()
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        NumberEquality value = JsonSerializer.Deserialize<NumberEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new NumberScalar(number));
        Assert.Equal(value.Right.AsT1, new NumberScalar(number));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        double number = Random.Shared.NextDouble();
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {
                "type": {
                  "name": "{{type}}"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgs()
    {
        double number = Random.Shared.NextDouble();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{JsonSerializer.Serialize(number)}}
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberEquality(
                new NumberReturning(new NumberScalar(number)),
                new NumberReturning(new NumberScalar(number))
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
                  "name": "number"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        NumberEquality value = JsonSerializer.Deserialize<NumberEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT0, new NumberParameter(expectedFirstParamName));
        Assert.Equal(value.Right.AsT0, new NumberParameter(expectedSecondParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
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
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
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
                  "name": "number"
                }
              },
              "right": {
                "name": "{{expectedSecondParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberEquality(
                new NumberReturning(new NumberParameter(expectedFirstParamName)),
                new NumberReturning(new NumberParameter(expectedSecondParamName))
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
                  "name": "number"
                },
                "value": 0.5800537796011547
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        NumberEquality value = JsonSerializer.Deserialize<NumberEquality>(
            input,
            _options
        )!;
        Assert.Equal(value.Left.AsT1, new NumberScalar(0.5800537796011547));
        Assert.Equal(value.Right.AsT0, new NumberParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
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
                "value": 0.5800537796011547
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
            JsonSerializer.Deserialize<NumberEquality>(input, _options)
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
                  "name": "number"
                },
                "value": 0.5800537796011547
              },
              "right": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new NumberEquality(
                new NumberReturning(new NumberScalar(0.5800537796011547)),
                new NumberReturning(new NumberParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
