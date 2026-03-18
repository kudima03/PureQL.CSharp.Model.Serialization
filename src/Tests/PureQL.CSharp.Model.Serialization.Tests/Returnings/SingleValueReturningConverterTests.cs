using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record SingleValueReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SingleValueReturningConverterTests()
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
    public void ReadBooleanReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "boolean"
              },
              "name": "{{paramName}}"
            }
            """;

        BooleanParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT0.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new BooleanType(), parameter.Type);
    }

    [Fact]
    public void WriteBooleanReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new BooleanReturning(new BooleanParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "boolean"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadBooleanReturningWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        BooleanScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT0.AsT1;

        Assert.Equal(new BooleanScalar(true), scalar);
    }

    [Fact]
    public void WriteBooleanReturningWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new BooleanReturning(new BooleanScalar(true))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanReturningWithEquality()
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

        Equality equality = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT0.AsT2;

        Assert.Equal(new BooleanScalar(true), equality.AsT0.AsT0.Left.AsT1);
        Assert.Equal(new BooleanScalar(false), equality.AsT0.AsT0.Right.AsT1);
    }

    [Fact]
    public void WriteBooleanReturningWithEquality()
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
            new SingleValueReturning(
                new BooleanReturning(
                    new Equality(
                        new SingleValueEquality(
                            new BooleanEquality(
                                new BooleanReturning(new BooleanScalar(true)),
                                new BooleanReturning(new BooleanScalar(false))
                            )
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanReturningWithBooleanOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        BooleanOperator booleanOperator = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT0.AsT3;

        Assert.Equal(new BooleanScalar(true), booleanOperator.AsT2.Condition.AsT1);
    }

    [Fact]
    public void WriteBooleanReturningWithBooleanOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new BooleanReturning(
                    new BooleanOperator(
                        new NotOperator(new BooleanReturning(new BooleanScalar(true)))
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanReturningWithComparison()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "greaterThan",
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

        Comparison comparison = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT0.AsT4;

        Assert.Equal(ComparisonOperator.GreaterThan, comparison.AsT2.Operator);
        Assert.Equal(new NumberScalar(42), comparison.AsT2.Left.AsT1);
        Assert.Equal(new NumberScalar(24), comparison.AsT2.Right.AsT1);
    }

    [Fact]
    public void WriteBooleanReturningWithComparison()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "greaterThan",
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
            new SingleValueReturning(
                new BooleanReturning(
                    new Comparison(
                        new NumberComparison(
                            ComparisonOperator.GreaterThan,
                            new NumberReturning(new NumberScalar(42)),
                            new NumberReturning(new NumberScalar(24))
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "name": "{{paramName}}"
            }
            """;

        DateParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT1.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateType(), parameter.Type);
    }

    [Fact]
    public void WriteDateReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new DateReturning(new DateParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "date"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateReturningWithScalar()
    {
        DateOnly value = new(2024, 1, 15);

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{value:yyyy-MM-dd}}"
            }
            """;

        DateScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT1.AsT1;

        Assert.Equal(new DateScalar(value), scalar);
    }

    [Fact]
    public void WriteDateReturningWithScalar()
    {
        DateOnly value = new(2024, 1, 15);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{value:yyyy-MM-dd}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new DateReturning(new DateScalar(value))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "name": "{{paramName}}"
            }
            """;

        DateTimeParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT2.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateTimeType(), parameter.Type);
    }

    [Fact]
    public void WriteDateTimeReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new DateTimeReturning(new DateTimeParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "datetime"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateTimeReturningWithScalar()
    {
        DateTime value = new(2024, 1, 15, 10, 30, 0);
        string valueJson = JsonSerializer.Serialize(value, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{valueJson}}
            }
            """;

        DateTimeScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT2.AsT1;

        Assert.Equal(new DateTimeScalar(value), scalar);
    }

    [Fact]
    public void WriteDateTimeReturningWithScalar()
    {
        DateTime value = new(2024, 1, 15, 10, 30, 0);
        string valueJson = JsonSerializer.Serialize(value, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{valueJson}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new DateTimeReturning(new DateTimeScalar(value))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "name": "{{paramName}}"
            }
            """;

        NumberParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT3.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new NumberType(), parameter.Type);
    }

    [Fact]
    public void WriteNumberReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new NumberReturning(new NumberParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "number"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadNumberReturningWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "number"
              },
              "value": 42
            }
            """;

        NumberScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT3.AsT1;

        Assert.Equal(new NumberScalar(42), scalar);
    }

    [Fact]
    public void WriteNumberReturningWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "number"
              },
              "value": 42
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new NumberReturning(new NumberScalar(42))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "string"
              },
              "name": "{{paramName}}"
            }
            """;

        StringParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT4.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new StringType(), parameter.Type);
    }

    [Fact]
    public void WriteStringReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new StringReturning(new StringParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "string"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringReturningWithScalar()
    {
        const string value = "someStringValue";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "string"
              },
              "value": "{{value}}"
            }
            """;

        StringScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT4.AsT1;

        Assert.Equal(new StringScalar(value), scalar);
    }

    [Fact]
    public void WriteStringReturningWithScalar()
    {
        const string value = "someStringValue";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "string"
              },
              "value": "{{value}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new StringReturning(new StringScalar(value))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "name": "{{paramName}}"
            }
            """;

        TimeParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT5.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new TimeType(), parameter.Type);
    }

    [Fact]
    public void WriteTimeReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new TimeReturning(new TimeParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "time"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeReturningWithScalar()
    {
        TimeOnly value = new(10, 30, 0);

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "value": "{{value:HH:mm:ss}}"
            }
            """;

        TimeScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT5.AsT1;

        Assert.Equal(new TimeScalar(value), scalar);
    }

    [Fact]
    public void WriteTimeReturningWithScalar()
    {
        TimeOnly value = new(10, 30, 0);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "value": "{{value:HH:mm:ss}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new TimeReturning(new TimeScalar(value))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidReturningWithParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "name": "{{paramName}}"
            }
            """;

        UuidParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT6.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new UuidType(), parameter.Type);
    }

    [Fact]
    public void WriteUuidReturningWithParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new UuidReturning(new UuidParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "uuid"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidReturningWithScalar()
    {
        Guid value = new("00000000-0000-0000-0000-000000000001");

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{value}}"
            }
            """;

        UuidScalar scalar = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT6.AsT1;

        Assert.Equal(new UuidScalar(value), scalar);
    }

    [Fact]
    public void WriteUuidReturningWithScalar()
    {
        Guid value = new("00000000-0000-0000-0000-000000000001");

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{value}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(new UuidReturning(new UuidScalar(value))),
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
            JsonSerializer.Deserialize<SingleValueReturning>(input, _options)
        );
    }
}
