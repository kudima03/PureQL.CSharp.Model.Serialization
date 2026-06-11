using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachComparisons;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachComparisons;

public sealed record EachComparisonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachComparisonConverterTests()
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
    public void ReadNumberComparisonGreaterThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 10
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachNumberComparison comp = value.AsT0;
        Assert.Equal(EachComparisonOperator.EachGreaterThan, comp.Operator);
        Assert.Equal(new NumberField(expectedEntity, expectedField), comp.Left.AsT1);
        Assert.Equal(10, comp.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteNumberComparisonGreaterThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 10
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachNumberComparison(
                    EachComparisonOperator.EachGreaterThan,
                    new NumberArrayReturning(
                        new NumberField(expectedEntity, expectedField)
                    ),
                    new NumberReturning(new NumberScalar(10))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberComparisonLessThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachLessThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 10
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachNumberComparison comp = value.AsT0;
        Assert.Equal(EachComparisonOperator.EachLessThan, comp.Operator);
    }

    [Fact]
    public void WriteNumberComparisonLessThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachLessThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 10
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachNumberComparison(
                    EachComparisonOperator.EachLessThan,
                    new NumberArrayReturning(
                        new NumberField(expectedEntity, expectedField)
                    ),
                    new NumberReturning(new NumberScalar(10))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringComparisonGreaterThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "string"
                }
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "alpha"
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachStringComparison comp = value.AsT1;
        Assert.Equal(EachComparisonOperator.EachGreaterThan, comp.Operator);
        Assert.Equal(new StringField(expectedEntity, expectedField), comp.Left.AsT1);
        Assert.Equal("alpha", comp.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteStringComparisonGreaterThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "string"
                }
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "alpha"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachStringComparison(
                    EachComparisonOperator.EachGreaterThan,
                    new StringArrayReturning(
                        new StringField(expectedEntity, expectedField)
                    ),
                    new StringReturning(new StringScalar("alpha"))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateComparisonLessThanOrEqual()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateOnly expectedDate = new DateOnly(2024, 6, 1);
        string expectedDateStr = JsonSerializer.Serialize(expectedDate, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachLessThanOrEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{expectedDateStr}}
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachDateComparison comp = value.AsT2;
        Assert.Equal(EachComparisonOperator.EachLessThanOrEqual, comp.Operator);
        Assert.Equal(new DateField(expectedEntity, expectedField), comp.Left.AsT1);
        Assert.Equal(expectedDate, comp.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteDateComparisonLessThanOrEqual()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateOnly expectedDate = new DateOnly(2024, 6, 1);
        string expectedDateStr = JsonSerializer.Serialize(expectedDate, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachLessThanOrEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "date"
                }
              },
              "right": {
                "type": {
                  "name": "date"
                },
                "value": {{expectedDateStr}}
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachDateComparison(
                    EachComparisonOperator.EachLessThanOrEqual,
                    new DateArrayReturning(
                        new DateField(expectedEntity, expectedField)
                    ),
                    new DateReturning(new DateScalar(expectedDate))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeComparisonGreaterThanOrEqual()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateTime expectedDateTime = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        string expectedDateTimeStr = JsonSerializer.Serialize(expectedDateTime, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThanOrEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{expectedDateTimeStr}}
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachDateTimeComparison comp = value.AsT3;
        Assert.Equal(EachComparisonOperator.EachGreaterThanOrEqual, comp.Operator);
        Assert.Equal(new DateTimeField(expectedEntity, expectedField), comp.Left.AsT1);
        Assert.Equal(expectedDateTime, comp.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteDateTimeComparisonGreaterThanOrEqual()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateTime expectedDateTime = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        string expectedDateTimeStr = JsonSerializer.Serialize(expectedDateTime, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThanOrEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "datetime"
                }
              },
              "right": {
                "type": {
                  "name": "datetime"
                },
                "value": {{expectedDateTimeStr}}
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachDateTimeComparison(
                    EachComparisonOperator.EachGreaterThanOrEqual,
                    new DateTimeArrayReturning(
                        new DateTimeField(expectedEntity, expectedField)
                    ),
                    new DateTimeReturning(new DateTimeScalar(expectedDateTime))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeComparisonGreaterThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        TimeOnly expectedTime = new TimeOnly(12, 0, 0);
        string expectedTimeStr = JsonSerializer.Serialize(expectedTime, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "time"
                }
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{expectedTimeStr}}
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachTimeComparison comp = value.AsT4;
        Assert.Equal(EachComparisonOperator.EachGreaterThan, comp.Operator);
        Assert.Equal(new TimeField(expectedEntity, expectedField), comp.Left.AsT1);
        Assert.Equal(expectedTime, comp.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteTimeComparisonGreaterThan()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        TimeOnly expectedTime = new TimeOnly(12, 0, 0);
        string expectedTimeStr = JsonSerializer.Serialize(expectedTime, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "time"
                }
              },
              "right": {
                "type": {
                  "name": "time"
                },
                "value": {{expectedTimeStr}}
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachTimeComparison(
                    EachComparisonOperator.EachGreaterThan,
                    new TimeArrayReturning(
                        new TimeField(expectedEntity, expectedField)
                    ),
                    new TimeReturning(new TimeScalar(expectedTime))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnBadJson()
    {
        const string input = "{}";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachComparison>(input, _options)
        );
    }

    [Fact]
    public void ReadRightAsArrayReturning()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        EachComparison value = JsonSerializer.Deserialize<EachComparison>(input, _options)!;
        EachNumberComparison comp = value.AsT0;
        Assert.Equal(new NumberField(leftEntity, leftField), comp.Left.AsT1);
        Assert.Equal(new NumberField(rightEntity, rightField), comp.Right.AsT1.AsT1);
    }

    [Fact]
    public void WriteRightAsArrayReturning()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachComparison(
                new EachNumberComparison(
                    EachComparisonOperator.EachGreaterThan,
                    new NumberArrayReturning(
                        new NumberField(leftEntity, leftField)
                    ),
                    new NumberArrayReturning(
                        new NumberField(rightEntity, rightField)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
