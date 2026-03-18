using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Equalities;

public sealed record SingleValueEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SingleValueEqualityConverterTests()
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
    public void ReadBooleanEquality()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanScalar(true), equality.Left.AsT1);
        Assert.Equal(new BooleanScalar(false), equality.Right.AsT1);
    }

    [Fact]
    public void WriteBooleanEquality()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new BooleanEquality(
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "date"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        DateEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT1;

        Assert.Equal(new DateParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new DateParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteDateEquality()
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
                  "name": "date"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new DateEquality(
                    new DateReturning(new DateParameter(leftParamName)),
                    new DateReturning(new DateParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        DateTimeEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT2;

        Assert.Equal(new DateTimeParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new DateTimeParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteDateTimeEquality()
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
                  "name": "datetime"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new DateTimeEquality(
                    new DateTimeReturning(new DateTimeParameter(leftParamName)),
                    new DateTimeReturning(new DateTimeParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberEquality()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": 42
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 24
              }
            }
            """;

        NumberEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT3;

        Assert.Equal(new NumberScalar(42), equality.Left.AsT1);
        Assert.Equal(new NumberScalar(24), equality.Right.AsT1);
    }

    [Fact]
    public void WriteNumberEquality()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "number"
                },
                "value": 42
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 24
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new NumberEquality(
                    new NumberReturning(new NumberScalar(42)),
                    new NumberReturning(new NumberScalar(24))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringEquality()
    {
        const string leftValue = "leftValue";
        const string rightValue = "rightValue";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        StringEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT4;

        Assert.Equal(new StringScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new StringScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteStringEquality()
    {
        const string leftValue = "leftValue";
        const string rightValue = "rightValue";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "string"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new StringEquality(
                    new StringReturning(new StringScalar(leftValue)),
                    new StringReturning(new StringScalar(rightValue))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "time"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        TimeEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT5;

        Assert.Equal(new TimeParameter(leftParamName), equality.Left.AsT0);
        Assert.Equal(new TimeParameter(rightParamName), equality.Right.AsT0);
    }

    [Fact]
    public void WriteTimeEquality()
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
                  "name": "time"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new TimeEquality(
                    new TimeReturning(new TimeParameter(leftParamName)),
                    new TimeReturning(new TimeParameter(rightParamName))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidEquality()
    {
        Guid leftValue = new("00000000-0000-0000-0000-000000000001");
        Guid rightValue = new("00000000-0000-0000-0000-000000000002");

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        UuidEquality equality = JsonSerializer
            .Deserialize<SingleValueEquality>(input, _options)!
            .AsT6;

        Assert.Equal(new UuidScalar(leftValue), equality.Left.AsT1);
        Assert.Equal(new UuidScalar(rightValue), equality.Right.AsT1);
    }

    [Fact]
    public void WriteUuidEquality()
    {
        Guid leftValue = new("00000000-0000-0000-0000-000000000001");
        Guid rightValue = new("00000000-0000-0000-0000-000000000002");

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{leftValue}}"
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": "{{rightValue}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueEquality(
                new UuidEquality(
                    new UuidReturning(new UuidScalar(leftValue)),
                    new UuidReturning(new UuidScalar(rightValue))
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
            JsonSerializer.Deserialize<SingleValueEquality>(input, _options)
        );
    }
}
