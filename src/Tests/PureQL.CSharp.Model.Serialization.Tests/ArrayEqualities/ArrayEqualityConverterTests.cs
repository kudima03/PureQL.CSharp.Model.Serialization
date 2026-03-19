using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

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
    public void ReadBooleanArrayEqualityWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  true,
                  false
                ]
              },
              "right": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  false,
                  true
                ]
              }
            }
            """;

        BooleanArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT0;

        Assert.Equal(
            new bool[] { true, false }.Concat([false, true]),
            equality.Left.AsT0.Value.Concat(equality.Right.AsT0.Value)
        );
    }

    [Fact]
    public void WriteBooleanArrayEqualityWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  true,
                  false
                ]
              },
              "right": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  false,
                  true
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new BooleanArrayEquality(
                    new BooleanArrayReturning(new BooleanArrayScalar([true, false])),
                    new BooleanArrayReturning(new BooleanArrayScalar([false, true]))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "booleanArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "booleanArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new BooleanArrayEquality(
                new BooleanArrayReturning(new BooleanField(leftEntity, leftField)),
                new BooleanArrayReturning(new BooleanField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteBooleanArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "booleanArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new BooleanArrayEquality(
                    new BooleanArrayReturning(new BooleanField(leftEntity, leftField)),
                    new BooleanArrayReturning(new BooleanField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new BooleanArrayEquality(
                    new BooleanArrayReturning(new BooleanArrayParameter(leftParamName)),
                    new BooleanArrayReturning(new BooleanArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteBooleanArrayEqualityWithParameter()
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
    public void ReadDateArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new DateArrayEquality(
                    new DateArrayReturning(new DateArrayParameter(leftParamName)),
                    new DateArrayReturning(new DateArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteDateArrayEqualityWithParameter()
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
    public void ReadDateArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "dateArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "dateArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new DateArrayEquality(
                new DateArrayReturning(new DateField(leftEntity, leftField)),
                new DateArrayReturning(new DateField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteDateArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "dateArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new DateArrayEquality(
                    new DateArrayReturning(new DateField(leftEntity, leftField)),
                    new DateArrayReturning(new DateField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateArrayEqualityWithScalar()
    {
        IEnumerable<DateOnly> leftDates = [new(2024, 1, 15), new(2024, 6, 20)];
        IEnumerable<DateOnly> rightDates = [new(2024, 3, 10), new(2024, 9, 25)];

        IEnumerable<string> leftFormatted = leftDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );
        IEnumerable<string> rightFormatted = rightDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "dateArray"
                },
                "value": [
                  "{{leftFormatted.First()}}",
                  "{{leftFormatted.Skip(1).First()}}"
                ]
              },
              "right": {
                "type": {
                  "name": "dateArray"
                },
                "value": [
                  "{{rightFormatted.First()}}",
                  "{{rightFormatted.Skip(1).First()}}"
                ]
              }
            }
            """;

        DateArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT1;

        Assert.Equal(
            leftDates.Concat(rightDates),
            equality.Left.AsT2.Value.Concat(equality.Right.AsT2.Value)
        );
    }

    [Fact]
    public void WriteDateArrayEqualityWithScalar()
    {
        IEnumerable<DateOnly> leftDates = [new(2024, 1, 15), new(2024, 6, 20)];
        IEnumerable<DateOnly> rightDates = [new(2024, 3, 10), new(2024, 9, 25)];

        IEnumerable<string> leftFormatted = leftDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );
        IEnumerable<string> rightFormatted = rightDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "dateArray"
                },
                "value": [
                  "{{leftFormatted.First()}}",
                  "{{leftFormatted.Skip(1).First()}}"
                ]
              },
              "right": {
                "type": {
                  "name": "dateArray"
                },
                "value": [
                  "{{rightFormatted.First()}}",
                  "{{rightFormatted.Skip(1).First()}}"
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new DateArrayEquality(
                    new DateArrayReturning(new DateArrayScalar(leftDates)),
                    new DateArrayReturning(new DateArrayScalar(rightDates))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new DateTimeArrayEquality(
                    new DateTimeArrayReturning(new DateTimeArrayParameter(leftParamName)),
                    new DateTimeArrayReturning(new DateTimeArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteDateTimeArrayEqualityWithParameter()
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
    public void ReadDateTimeArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetimeArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "datetimeArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new DateTimeArrayEquality(
                new DateTimeArrayReturning(new DateTimeField(leftEntity, leftField)),
                new DateTimeArrayReturning(new DateTimeField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteDateTimeArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "datetimeArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "datetimeArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new DateTimeArrayEquality(
                    new DateTimeArrayReturning(new DateTimeField(leftEntity, leftField)),
                    new DateTimeArrayReturning(new DateTimeField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeArrayEqualityWithScalar()
    {
        IEnumerable<DateTime> leftValues =
        [
            new(2024, 1, 15, 10, 30, 0),
            new(2024, 6, 20, 15, 45, 0),
        ];
        IEnumerable<DateTime> rightValues =
        [
            new(2024, 3, 10, 8, 0, 0),
            new(2024, 9, 25, 12, 0, 0),
        ];

        IEnumerable<string> leftFormatted = leftValues.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );
        IEnumerable<string> rightFormatted = rightValues.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  {{leftFormatted.First()}},
                  {{leftFormatted.Skip(1).First()}}
                ]
              },
              "right": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  {{rightFormatted.First()}},
                  {{rightFormatted.Skip(1).First()}}
                ]
              }
            }
            """;

        DateTimeArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT2;

        Assert.Equal(
            leftValues.Concat(rightValues),
            equality.Left.AsT2.Value.Concat(equality.Right.AsT2.Value)
        );
    }

    [Fact]
    public void WriteDateTimeArrayEqualityWithScalar()
    {
        IEnumerable<DateTime> leftValues =
        [
            new(2024, 1, 15, 10, 30, 0),
            new(2024, 6, 20, 15, 45, 0),
        ];
        IEnumerable<DateTime> rightValues =
        [
            new(2024, 3, 10, 8, 0, 0),
            new(2024, 9, 25, 12, 0, 0),
        ];

        IEnumerable<string> leftFormatted = leftValues.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );
        IEnumerable<string> rightFormatted = rightValues.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  {{leftFormatted.First()}},
                  {{leftFormatted.Skip(1).First()}}
                ]
              },
              "right": {
                "type": {
                  "name": "datetimeArray"
                },
                "value": [
                  {{rightFormatted.First()}},
                  {{rightFormatted.Skip(1).First()}}
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new DateTimeArrayEquality(
                    new DateTimeArrayReturning(new DateTimeArrayScalar(leftValues)),
                    new DateTimeArrayReturning(new DateTimeArrayScalar(rightValues))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new NumberArrayEquality(
                    new NumberArrayReturning(new NumberArrayParameter(leftParamName)),
                    new NumberArrayReturning(new NumberArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteNumberArrayEqualityWithParameter()
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
    public void ReadNumberArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "numberArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "numberArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new NumberArrayEquality(
                new NumberArrayReturning(new NumberField(leftEntity, leftField)),
                new NumberArrayReturning(new NumberField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteNumberArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "numberArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new NumberArrayEquality(
                    new NumberArrayReturning(new NumberField(leftEntity, leftField)),
                    new NumberArrayReturning(new NumberField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberArrayEqualityWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  42,
                  24
                ]
              },
              "right": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  10,
                  20
                ]
              }
            }
            """;

        NumberArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT3;

        Assert.Equal(
            new double[] { 42, 24 }.Concat([10, 20]),
            equality.Left.AsT2.Value.Concat(equality.Right.AsT2.Value)
        );
    }

    [Fact]
    public void WriteNumberArrayEqualityWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  42,
                  24
                ]
              },
              "right": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  10,
                  20
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new NumberArrayEquality(
                    new NumberArrayReturning(new NumberArrayScalar([42, 24])),
                    new NumberArrayReturning(new NumberArrayScalar([10, 20]))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new StringArrayEquality(
                    new StringArrayReturning(new StringArrayParameter(leftParamName)),
                    new StringArrayReturning(new StringArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteStringArrayEqualityWithParameter()
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
    public void ReadStringArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "stringArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "stringArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new StringArrayEquality(
                new StringArrayReturning(new StringField(leftEntity, leftField)),
                new StringArrayReturning(new StringField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteStringArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "stringArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "stringArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new StringArrayEquality(
                    new StringArrayReturning(new StringField(leftEntity, leftField)),
                    new StringArrayReturning(new StringField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringArrayEqualityWithScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "stringArray"
                },
                "value": [
                  "leftValue1",
                  "leftValue2"
                ]
              },
              "right": {
                "type": {
                  "name": "stringArray"
                },
                "value": [
                  "rightValue1",
                  "rightValue2"
                ]
              }
            }
            """;

        StringArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT4;

        Assert.Equal(
            new string[] { "leftValue1", "leftValue2" }.Concat(["rightValue1", "rightValue2"]),
            equality.Left.AsT2.Value.Concat(equality.Right.AsT2.Value)
        );
    }

    [Fact]
    public void WriteStringArrayEqualityWithScalar()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "stringArray"
                },
                "value": [
                  "leftValue1",
                  "leftValue2"
                ]
              },
              "right": {
                "type": {
                  "name": "stringArray"
                },
                "value": [
                  "rightValue1",
                  "rightValue2"
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new StringArrayEquality(
                    new StringArrayReturning(
                        new StringArrayScalar(["leftValue1", "leftValue2"])
                    ),
                    new StringArrayReturning(
                        new StringArrayScalar(["rightValue1", "rightValue2"])
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new TimeArrayEquality(
                    new TimeArrayReturning(new TimeArrayParameter(leftParamName)),
                    new TimeArrayReturning(new TimeArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteTimeArrayEqualityWithParameter()
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
    public void ReadTimeArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "timeArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "timeArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new TimeArrayEquality(
                new TimeArrayReturning(new TimeField(leftEntity, leftField)),
                new TimeArrayReturning(new TimeField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteTimeArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
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
            new ArrayEquality(
                new TimeArrayEquality(
                    new TimeArrayReturning(new TimeField(leftEntity, leftField)),
                    new TimeArrayReturning(new TimeField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeArrayEqualityWithScalar()
    {
        IEnumerable<TimeOnly> leftValues = [new(10, 30, 0), new(14, 45, 0)];
        IEnumerable<TimeOnly> rightValues = [new(8, 0, 0), new(16, 30, 0)];

        IEnumerable<string> leftFormatted = leftValues.Select(x =>
            x.ToString("HH:mm:ss")
        );
        IEnumerable<string> rightFormatted = rightValues.Select(x =>
            x.ToString("HH:mm:ss")
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{leftFormatted.First()}}",
                  "{{leftFormatted.Skip(1).First()}}"
                ]
              },
              "right": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{rightFormatted.First()}}",
                  "{{rightFormatted.Skip(1).First()}}"
                ]
              }
            }
            """;

        TimeArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT5;

        Assert.Equal(
            leftValues.Concat(rightValues),
            equality.Left.AsT2.Value.Concat(equality.Right.AsT2.Value)
        );
    }

    [Fact]
    public void WriteTimeArrayEqualityWithScalar()
    {
        IEnumerable<TimeOnly> leftValues = [new(10, 30, 0), new(14, 45, 0)];
        IEnumerable<TimeOnly> rightValues = [new(8, 0, 0), new(16, 30, 0)];

        IEnumerable<string> leftFormatted = leftValues.Select(x =>
            x.ToString("HH:mm:ss")
        );
        IEnumerable<string> rightFormatted = rightValues.Select(x =>
            x.ToString("HH:mm:ss")
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{leftFormatted.First()}}",
                  "{{leftFormatted.Skip(1).First()}}"
                ]
              },
              "right": {
                "type": {
                  "name": "timeArray"
                },
                "value": [
                  "{{rightFormatted.First()}}",
                  "{{rightFormatted.Skip(1).First()}}"
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new TimeArrayEquality(
                    new TimeArrayReturning(new TimeArrayScalar(leftValues)),
                    new TimeArrayReturning(new TimeArrayScalar(rightValues))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidArrayEqualityWithParameter()
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

        Assert.Equal(
            new ArrayEquality(
                new UuidArrayEquality(
                    new UuidArrayReturning(new UuidArrayParameter(leftParamName)),
                    new UuidArrayReturning(new UuidArrayParameter(rightParamName))
                )
            ),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteUuidArrayEqualityWithParameter()
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

    [Fact]
    public void ReadUuidArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuidArray"
                },
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}"
              },
              "right": {
                "type": {
                  "name": "uuidArray"
                },
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}"
              }
            }
            """;

        Assert.Equal(
            new ArrayEquality(new UuidArrayEquality(
                new UuidArrayReturning(new UuidField(leftEntity, leftField)),
                new UuidArrayReturning(new UuidField(rightEntity, rightField))
            )),
            JsonSerializer.Deserialize<ArrayEquality>(input, _options)
        );
    }

    [Fact]
    public void WriteUuidArrayEqualityWithField()
    {
        const string leftEntity = "leftEntity";
        const string leftField = "leftField";
        const string rightEntity = "rightEntity";
        const string rightField = "rightField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "entity": "{{leftEntity}}",
                "field": "{{leftField}}",
                "type": {
                  "name": "uuidArray"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "uuidArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new UuidArrayEquality(
                    new UuidArrayReturning(new UuidField(leftEntity, leftField)),
                    new UuidArrayReturning(new UuidField(rightEntity, rightField))
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidArrayEqualityWithScalar()
    {
        IEnumerable<Guid> leftValues =
        [
            new("00000000-0000-0000-0000-000000000001"),
            new("00000000-0000-0000-0000-000000000002"),
        ];
        IEnumerable<Guid> rightValues =
        [
            new("00000000-0000-0000-0000-000000000003"),
            new("00000000-0000-0000-0000-000000000004"),
        ];

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuidArray"
                },
                "value": [
                  "{{leftValues.First()}}",
                  "{{leftValues.Skip(1).First()}}"
                ]
              },
              "right": {
                "type": {
                  "name": "uuidArray"
                },
                "value": [
                  "{{rightValues.First()}}",
                  "{{rightValues.Skip(1).First()}}"
                ]
              }
            }
            """;

        UuidArrayEquality equality = JsonSerializer
            .Deserialize<ArrayEquality>(input, _options)!
            .AsT6;

        Assert.Equal(
            leftValues.Concat(rightValues),
            equality.Left.AsT2.Value.Concat(equality.Right.AsT2.Value)
        );
    }

    [Fact]
    public void WriteUuidArrayEqualityWithScalar()
    {
        IEnumerable<Guid> leftValues =
        [
            new("00000000-0000-0000-0000-000000000001"),
            new("00000000-0000-0000-0000-000000000002"),
        ];
        IEnumerable<Guid> rightValues =
        [
            new("00000000-0000-0000-0000-000000000003"),
            new("00000000-0000-0000-0000-000000000004"),
        ];

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "uuidArray"
                },
                "value": [
                  "{{leftValues.First()}}",
                  "{{leftValues.Skip(1).First()}}"
                ]
              },
              "right": {
                "type": {
                  "name": "uuidArray"
                },
                "value": [
                  "{{rightValues.First()}}",
                  "{{rightValues.Skip(1).First()}}"
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayEquality(
                new UuidArrayEquality(
                    new UuidArrayReturning(new UuidArrayScalar(leftValues)),
                    new UuidArrayReturning(new UuidArrayScalar(rightValues))
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
