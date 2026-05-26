using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachTimeArithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachTimeArithmetics;

public sealed record EachTimeArithmeticConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachTimeArithmeticConverterTests()
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
                  "name": "timeArray"
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
            JsonSerializer.Deserialize<EachTimeAddSeconds>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnAddSecondsWrongOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachTimeDiffSeconds",
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "timeArray"
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
            JsonSerializer.Deserialize<EachTimeAddSeconds>(input, _options)
        );
    }

    [Fact]
    public void ReadAddSeconds()
    {
        const string timeEntity = "timeEntity";
        const string timeField = "timeField";
        const string numEntity = "numEntity";
        const string numField = "numField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachTimeAddSeconds",
              "left": {
                "entity": "{{timeEntity}}",
                "field": "{{timeField}}",
                "type": {
                  "name": "timeArray"
                }
              },
              "right": {
                "entity": "{{numEntity}}",
                "field": "{{numField}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        EachTimeAddSeconds value = JsonSerializer.Deserialize<EachTimeAddSeconds>(
            input,
            _options
        )!;
        Assert.Equal(new TimeField(timeEntity, timeField), value.Left.AsT1.AsT1);
        Assert.Equal(new NumberField(numEntity, numField), value.Right.AsT1.AsT1);
    }

    [Fact]
    public void WriteAddSeconds()
    {
        const string timeEntity = "timeEntity";
        const string timeField = "timeField";
        const string numEntity = "numEntity";
        const string numField = "numField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachTimeAddSeconds",
              "left": {
                "entity": "{{timeEntity}}",
                "field": "{{timeField}}",
                "type": {
                  "name": "timeArray"
                }
              },
              "right": {
                "entity": "{{numEntity}}",
                "field": "{{numField}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachTimeAddSeconds(
                new TimeArrayReturning(new TimeField(timeEntity, timeField)),
                new NumberArrayReturning(new NumberField(numEntity, numField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadAddSecondsWithScalarRight()
    {
        const string timeEntity = "timeEntity";
        const string timeField = "timeField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachTimeAddSeconds",
              "left": {
                "entity": "{{timeEntity}}",
                "field": "{{timeField}}",
                "type": {
                  "name": "timeArray"
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

        EachTimeAddSeconds value = JsonSerializer.Deserialize<EachTimeAddSeconds>(
            input,
            _options
        )!;
        Assert.Equal(new TimeField(timeEntity, timeField), value.Left.AsT1.AsT1);
        Assert.Equal(3600, value.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteAddSecondsWithScalarRight()
    {
        const string timeEntity = "timeEntity";
        const string timeField = "timeField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachTimeAddSeconds",
              "left": {
                "entity": "{{timeEntity}}",
                "field": "{{timeField}}",
                "type": {
                  "name": "timeArray"
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
            new EachTimeAddSeconds(
                new TimeArrayReturning(new TimeField(timeEntity, timeField)),
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
                  "name": "timeArray"
                }
              },
              "right": {
                "entity": "e2",
                "field": "f2",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachTimeDiffSeconds>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnDiffSecondsWrongOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachTimeAddSeconds",
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "timeArray"
                }
              },
              "right": {
                "entity": "e2",
                "field": "f2",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachTimeDiffSeconds>(input, _options)
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
              "operator": "eachTimeDiffSeconds",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "timeArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        EachTimeDiffSeconds value = JsonSerializer.Deserialize<EachTimeDiffSeconds>(
            input,
            _options
        )!;
        Assert.Equal(new TimeField(leftEntity, leftField), value.Left.AsT1.AsT1);
        Assert.Equal(new TimeField(rightEntity, rightField), value.Right.AsT1.AsT1);
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
              "operator": "eachTimeDiffSeconds",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "timeArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachTimeDiffSeconds(
                new TimeArrayReturning(new TimeField(leftEntity, leftField)),
                new TimeArrayReturning(new TimeField(rightEntity, rightField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDiffSecondsWithScalarLeft()
    {
        TimeOnly expectedTime = new TimeOnly(8, 0, 0);
        string expectedTimeStr = JsonSerializer.Serialize(expectedTime, _options);
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachTimeDiffSeconds",
              "left": {
                "type": {
                  "name": "time"
                },
                "value": {{expectedTimeStr}}
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "timeArray"
                }
              }
            }
            """;

        EachTimeDiffSeconds value = JsonSerializer.Deserialize<EachTimeDiffSeconds>(
            input,
            _options
        )!;
        Assert.Equal(expectedTime, value.Left.AsT0.AsT1.Value);
        Assert.Equal(new TimeField(rightEntity, rightField), value.Right.AsT1.AsT1);
    }
}
