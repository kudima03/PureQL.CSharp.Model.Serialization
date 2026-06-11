using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachArithmetics;
using PureQL.CSharp.Model.EachDateArithmetics;
using PureQL.CSharp.Model.EachDateTimeArithmetics;
using PureQL.CSharp.Model.EachTimeArithmetics;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record NumberArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberArrayReturningConverterTests()
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
    public void ReadField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new NumberField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<NumberArrayReturning>(input, _options)!.AsT1
        );
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new NumberArrayReturning(new NumberField(expectedEntity, expectedField)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "number"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadArrayScalar()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "value": [
                {{formattedValues.First()}},
                {{formattedValues.Skip(1).First()}},
                {{formattedValues.Skip(2).First()}}
              ]
            }
            """;

        NumberArrayScalar scalar = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(values, scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "value": [
                {{formattedValues.First()}},
                {{formattedValues.Skip(1).First()}},
                {{formattedValues.Skip(2).First()}}
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberArrayReturning(new NumberArrayScalar(values)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "name": "{{expected}}"
            }
            """;

        NumberArrayParameter parameter = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteParameter()
    {
        const string name = "asudu";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "name": "{{name}}",
              "type": {
                "name": "numberArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberArrayReturning(new NumberArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachArithmetic()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        EachArithmetic arithmetic = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT3;
        Assert.True(arithmetic.IsT0);
    }

    [Fact]
    public void WriteEachArithmetic()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberArrayReturning(new EachArithmetic(new EachAdd([]))),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachDateDiffDays()
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

        EachDateDiffDays diff = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT4;
        Assert.Equal(new DateField(leftEntity, leftField), diff.Left.AsT1.AsT1);
    }

    [Fact]
    public void WriteEachDateDiffDays()
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
            new NumberArrayReturning(
                new EachDateDiffDays(
                    new DateArrayReturning(new DateField(leftEntity, leftField)),
                    new DateArrayReturning(new DateField(rightEntity, rightField))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachDateTimeDiffSeconds()
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

        EachDateTimeDiffSeconds diff = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT5;
        Assert.Equal(
            new DateTimeField(leftEntity, leftField),
            diff.Left.AsT1.AsT1
        );
    }

    [Fact]
    public void WriteEachDateTimeDiffSeconds()
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
            new NumberArrayReturning(
                new EachDateTimeDiffSeconds(
                    new DateTimeArrayReturning(
                        new DateTimeField(leftEntity, leftField)
                    ),
                    new DateTimeArrayReturning(
                        new DateTimeField(rightEntity, rightField)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachTimeDiffSeconds()
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
                  "name": "time"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        EachTimeDiffSeconds diff = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT6;
        Assert.Equal(
            new TimeField(leftEntity, leftField),
            diff.Left.AsT1.AsT1
        );
    }

    [Fact]
    public void WriteEachTimeDiffSeconds()
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
                  "name": "time"
                }
              },
              "right": {
                "entity": "{{rightEntity}}",
                "field": "{{rightField}}",
                "type": {
                  "name": "time"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberArrayReturning(
                new EachTimeDiffSeconds(
                    new TimeArrayReturning(new TimeField(leftEntity, leftField)),
                    new TimeArrayReturning(new TimeField(rightEntity, rightField))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
