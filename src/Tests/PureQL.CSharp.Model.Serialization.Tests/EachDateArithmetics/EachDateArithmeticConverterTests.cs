using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachDateArithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachDateArithmetics;

public sealed record EachDateArithmeticConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachDateArithmeticConverterTests()
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
    public void ThrowsExceptionOnAddDaysOperatorAbsence()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "left": {
                "type": {
                  "name": "date"
                },
                "value": "2024-01-01"
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 7
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateAddDays>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnAddDaysWrongOperator()
    {
        DateOnly date = new DateOnly(2024, 1, 1);
        string dateStr = JsonSerializer.Serialize(date, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateDiffDays",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{dateStr}}
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 7
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateAddDays>(input, _options)
        );
    }

    [Fact]
    public void ReadAddDays()
    {
        const string dateEntity = "dateEntity";
        const string dateField = "dateField";
        const string numEntity = "numEntity";
        const string numField = "numField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateAddDays",
              "left": {
                "entity": "{{dateEntity}}",
                "field": "{{dateField}}",
                "type": {
                  "name": "date"
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

        EachDateAddDays value = JsonSerializer.Deserialize<EachDateAddDays>(
            input,
            _options
        )!;
        Assert.Equal(new DateField(dateEntity, dateField), value.Left.AsT1.AsT1);
        Assert.Equal(new NumberField(numEntity, numField), value.Right.AsT1.AsT1);
    }

    [Fact]
    public void WriteAddDays()
    {
        const string dateEntity = "dateEntity";
        const string dateField = "dateField";
        const string numEntity = "numEntity";
        const string numField = "numField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateAddDays",
              "left": {
                "entity": "{{dateEntity}}",
                "field": "{{dateField}}",
                "type": {
                  "name": "date"
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
            new EachDateAddDays(
                new DateArrayReturning(new DateField(dateEntity, dateField)),
                new NumberArrayReturning(new NumberField(numEntity, numField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadAddDaysWithScalarRight()
    {
        const string dateEntity = "dateEntity";
        const string dateField = "dateField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateAddDays",
              "left": {
                "entity": "{{dateEntity}}",
                "field": "{{dateField}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 30
              }
            }
            """;

        EachDateAddDays value = JsonSerializer.Deserialize<EachDateAddDays>(
            input,
            _options
        )!;
        Assert.Equal(new DateField(dateEntity, dateField), value.Left.AsT1.AsT1);
        Assert.Equal(30, value.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteAddDaysWithScalarRight()
    {
        const string dateEntity = "dateEntity";
        const string dateField = "dateField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateAddDays",
              "left": {
                "entity": "{{dateEntity}}",
                "field": "{{dateField}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 30
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachDateAddDays(
                new DateArrayReturning(new DateField(dateEntity, dateField)),
                new NumberReturning(new NumberScalar(30))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnDiffDaysOperatorAbsence()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "entity": "e2",
                "field": "f2",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateDiffDays>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnDiffDaysWrongOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachDateAddDays",
              "left": {
                "entity": "e1",
                "field": "f1",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "entity": "e2",
                "field": "f2",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDateDiffDays>(input, _options)
        );
    }

    [Fact]
    public void ReadDiffDays()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateDiffDays",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        EachDateDiffDays value = JsonSerializer.Deserialize<EachDateDiffDays>(
            input,
            _options
        )!;
        Assert.Equal(new DateField(leftEntity, leftField), value.Left.AsT1.AsT1);
        Assert.Equal(new DateField(rightEntity, rightField), value.Right.AsT1.AsT1);
    }

    [Fact]
    public void WriteDiffDays()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateDiffDays",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachDateDiffDays(
                new DateArrayReturning(new DateField(leftEntity, leftField)),
                new DateArrayReturning(new DateField(rightEntity, rightField))
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDiffDaysWithScalarLeft()
    {
        DateOnly expectedDate = new DateOnly(2024, 1, 1);
        string expectedDateStr = JsonSerializer.Serialize(expectedDate, _options);
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachDateDiffDays",
              "left": {
                "type": {
                  "name": "date"
                },
                "value": {{expectedDateStr}}
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "date"
                }
              }
            }
            """;

        EachDateDiffDays value = JsonSerializer.Deserialize<EachDateDiffDays>(
            input,
            _options
        )!;
        Assert.Equal(expectedDate, value.Left.AsT0.AsT1.Value);
        Assert.Equal(new DateField(rightEntity, rightField), value.Right.AsT1.AsT1);
    }
}
