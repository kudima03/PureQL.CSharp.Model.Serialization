using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachDateTimeArithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachDateTimeArithmetics;

public sealed record EachDateTimeArithmeticConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachDateTimeArithmeticConverterTests()
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
    public void ThrowsExceptionOnAddSecondsOperatorAbsence()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 60
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateTimeAddSeconds>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnAddSecondsWrongOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachDatetimeDiffSeconds",
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 60
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateTimeAddSeconds>(input, _options)
        );
    }

    [Fact]
    public void ReadAddSeconds()
    {
        const string dtEntity = "dtEntity";
        const string dtField = "dtField";
        const string numEntity = "numEntity";
        const string numField = "numField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeAddSeconds",
              "left": {
                "entity": "{{dtEntity}}",
                "field": "{{dtField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "entity": "{{numEntity}}",
                "field": "{{numField}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        EachDateTimeAddSeconds value = JsonSerializer.Deserialize<EachDateTimeAddSeconds>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeField(dtEntity, dtField), value.Left.AsT1.AsT1);
        Assert.Equal(new NumberField(numEntity, numField), value.Right.AsT1.AsT1);
    }

    [Fact]
    public void WriteAddSeconds()
    {
        const string dtEntity = "dtEntity";
        const string dtField = "dtField";
        const string numEntity = "numEntity";
        const string numField = "numField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeAddSeconds",
              "left": {
                "entity": "{{dtEntity}}",
                "field": "{{dtField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "entity": "{{numEntity}}",
                "field": "{{numField}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachDateTimeAddSeconds(
                new DateTimeArrayReturning(new DateTimeField(dtEntity, dtField)),
                new NumberArrayReturning(new NumberField(numEntity, numField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadAddSecondsWithScalarRight()
    {
        const string dtEntity = "dtEntity";
        const string dtField = "dtField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeAddSeconds",
              "left": {
                "entity": "{{dtEntity}}",
                "field": "{{dtField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 3600
              }
            }
            """;

        EachDateTimeAddSeconds value = JsonSerializer.Deserialize<EachDateTimeAddSeconds>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeField(dtEntity, dtField), value.Left.AsT1.AsT1);
        Assert.Equal(3600, value.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteAddSecondsWithScalarRight()
    {
        const string dtEntity = "dtEntity";
        const string dtField = "dtField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeAddSeconds",
              "left": {
                "entity": "{{dtEntity}}",
                "field": "{{dtField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 3600
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachDateTimeAddSeconds(
                new DateTimeArrayReturning(new DateTimeField(dtEntity, dtField)),
                new NumberReturning(new NumberScalar(3600))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnDiffSecondsOperatorAbsence()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "entity": "e2",
                "field": "f2",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateTimeDiffSeconds>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnDiffSecondsWrongOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachDatetimeAddSeconds",
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "entity": "e2",
                "field": "f2",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateTimeDiffSeconds>(input, _options)
        );
    }

    [Fact]
    public void ReadDiffSeconds()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeDiffSeconds",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        EachDateTimeDiffSeconds value = JsonSerializer.Deserialize<EachDateTimeDiffSeconds>(
            input,
            _options
        )!;
        Assert.Equal(new DateTimeField(leftEntity, leftField), value.Left.AsT1.AsT1);
        Assert.Equal(new DateTimeField(rightEntity, rightField), value.Right.AsT1.AsT1);
    }

    [Fact]
    public void WriteDiffSeconds()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeDiffSeconds",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachDateTimeDiffSeconds(
                new DateTimeArrayReturning(new DateTimeField(leftEntity, leftField)),
                new DateTimeArrayReturning(new DateTimeField(rightEntity, rightField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDiffSecondsWithScalarLeft()
    {
        DateTime expectedDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        string expectedDateTimeStr = JsonSerializer.Serialize(expectedDateTime, _options);
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDatetimeDiffSeconds",
              "left": {
                "type": {
                  "name": "datetime"
                },
                "value": {{expectedDateTimeStr}}
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "datetime"
                }
              }
            }
            """;

        EachDateTimeDiffSeconds value = JsonSerializer.Deserialize<EachDateTimeDiffSeconds>(
            input,
            _options
        )!;
        Assert.Equal(expectedDateTime, value.Left.AsT0.AsT1.Value);
        Assert.Equal(new DateTimeField(rightEntity, rightField), value.Right.AsT1.AsT1);
    }
}
