using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayEqualities;

public sealed record ArrayEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ArrayEqualityConverterTests()
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
    public void ReadBooleanArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "booleanArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "booleanArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        BooleanArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanArrayParameter(leftParamName), equality.Left.AsT2);
        Assert.Equal(new BooleanArrayParameter(rightParamName), equality.Right.AsT2);
    }

    [Fact]
    public void WriteBooleanArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "booleanArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new BooleanArrayEquality(
                    new BooleanArrayReturning(new BooleanArrayParameter(leftParamName)),
                    new BooleanArrayReturning(new BooleanArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "dateArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "dateArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        DateArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT1;

        Assert.Equal(new DateArrayParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new DateArrayParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteDateArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "dateArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new DateArrayEquality(
                    new DateArrayReturning(new DateArrayParameter(leftParamName)),
                    new DateArrayReturning(new DateArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetimeArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "datetimeArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        DateTimeArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT2;

        Assert.Equal(new DateTimeArrayParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new DateTimeArrayParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteDateTimeArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "datetimeArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "datetimeArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new DateTimeArrayEquality(
                    new DateTimeArrayReturning(new DateTimeArrayParameter(leftParamName)),
                    new DateTimeArrayReturning(new DateTimeArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "numberArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "numberArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        NumberArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT3;

        Assert.Equal(new NumberArrayParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new NumberArrayParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteNumberArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "numberArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new NumberArrayEquality(
                    new NumberArrayReturning(new NumberArrayParameter(leftParamName)),
                    new NumberArrayReturning(new NumberArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "stringArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "stringArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        StringArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT4;

        Assert.Equal(new StringArrayParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new StringArrayParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteStringArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "stringArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "stringArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new StringArrayEquality(
                    new StringArrayReturning(new StringArrayParameter(leftParamName)),
                    new StringArrayReturning(new StringArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "timeArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "timeArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        TimeArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT5;

        Assert.Equal(new TimeArrayParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new TimeArrayParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteTimeArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "timeArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new TimeArrayEquality(
                    new TimeArrayReturning(new TimeArrayParameter(leftParamName)),
                    new TimeArrayReturning(new TimeArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuidArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "uuidArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        UuidArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT6;

        Assert.Equal(new UuidArrayParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new UuidArrayParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteUuidArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "uuidArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "uuidArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new UuidArrayEquality(
                    new UuidArrayReturning(new UuidArrayParameter(leftParamName)),
                    new UuidArrayReturning(new UuidArrayParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }
}
