using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachEqualities;

public sealed record EachEqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachEqualityConverterTests()
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
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "boolean"
                }
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachBooleanEquality eq = value.AsT0;
        Assert.Equal(new BooleanField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.True(eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteBooleanEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "boolean"
                }
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachEquality(
                new EachBooleanEquality(
                    new BooleanArrayReturning(
                        new BooleanField(expectedEntity, expectedField)
                    ),
                    new BooleanReturning(new BooleanScalar(true))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
                "value": 42
              }
            }
            """;

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachNumberEquality eq = value.AsT1;
        Assert.Equal(new NumberField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.Equal(42, eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteNumberEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
                "value": 42
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachEquality(
                new EachNumberEquality(
                    new NumberArrayReturning(
                        new NumberField(expectedEntity, expectedField)
                    ),
                    new NumberReturning(new NumberScalar(42))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
                "value": "hello"
              }
            }
            """;

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachStringEquality eq = value.AsT2;
        Assert.Equal(new StringField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.Equal("hello", eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteStringEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
                "value": "hello"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachEquality(
                new EachStringEquality(
                    new StringArrayReturning(
                        new StringField(expectedEntity, expectedField)
                    ),
                    new StringReturning(new StringScalar("hello"))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateOnly expectedDate = new DateOnly(2024, 1, 15);
        string expectedDateStr = JsonSerializer.Serialize(expectedDate, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachDateEquality eq = value.AsT3;
        Assert.Equal(new DateField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.Equal(expectedDate, eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteDateEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateOnly expectedDate = new DateOnly(2024, 1, 15);
        string expectedDateStr = JsonSerializer.Serialize(expectedDate, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
            new EachEquality(
                new EachDateEquality(
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
    public void ReadTimeEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        TimeOnly expectedTime = new TimeOnly(10, 30, 0);
        string expectedTimeStr = JsonSerializer.Serialize(expectedTime, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachTimeEquality eq = value.AsT4;
        Assert.Equal(new TimeField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.Equal(expectedTime, eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteTimeEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        TimeOnly expectedTime = new TimeOnly(10, 30, 0);
        string expectedTimeStr = JsonSerializer.Serialize(expectedTime, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
            new EachEquality(
                new EachTimeEquality(
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
    public void ReadDateTimeEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateTime expectedDateTime = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        string expectedDateTimeStr = JsonSerializer.Serialize(expectedDateTime, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachDateTimeEquality eq = value.AsT5;
        Assert.Equal(new DateTimeField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.Equal(expectedDateTime, eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteDateTimeEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        DateTime expectedDateTime = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        string expectedDateTimeStr = JsonSerializer.Serialize(expectedDateTime, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
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
            new EachEquality(
                new EachDateTimeEquality(
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
    public void ReadUuidEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        Guid expectedUuid = new Guid("550e8400-e29b-41d4-a716-446655440000");
        string expectedUuidStr = JsonSerializer.Serialize(expectedUuid, _options);

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "uuid"
                }
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{expectedUuidStr}}
              }
            }
            """;

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachUuidEquality eq = value.AsT6;
        Assert.Equal(new UuidField(expectedEntity, expectedField), eq.Left.AsT1);
        Assert.Equal(expectedUuid, eq.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteUuidEquality()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";
        Guid expectedUuid = new Guid("550e8400-e29b-41d4-a716-446655440000");
        string expectedUuidStr = JsonSerializer.Serialize(expectedUuid, _options);

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "uuid"
                }
              },
              "right": {
                "type": {
                  "name": "uuid"
                },
                "value": {{expectedUuidStr}}
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachEquality(
                new EachUuidEquality(
                    new UuidArrayReturning(
                        new UuidField(expectedEntity, expectedField)
                    ),
                    new UuidReturning(new UuidScalar(expectedUuid))
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
            JsonSerializer.Deserialize<EachEquality>(input, _options)
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
              "operator": "eachEqual",
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

        EachEquality value = JsonSerializer.Deserialize<EachEquality>(input, _options)!;
        EachNumberEquality eq = value.AsT1;
        Assert.Equal(new NumberField(leftEntity, leftField), eq.Left.AsT1);
        Assert.Equal(new NumberField(rightEntity, rightField), eq.Right.AsT1.AsT1);
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
              "operator": "eachEqual",
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
            new EachEquality(
                new EachNumberEquality(
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
