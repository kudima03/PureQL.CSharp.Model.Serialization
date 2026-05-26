using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachDateArithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record DateArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateArrayReturningConverterTests()
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
                "name": "dateArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new DateField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<DateArrayReturning>(input, _options)!.AsT1
        );
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new DateArrayReturning(new DateField(expectedEntity, expectedField)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "dateArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadArrayScalar()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": [
                "{{formattedDates.First()}}",
                "{{formattedDates.Skip(1).First()}}",
                "{{formattedDates.Skip(2).First()}}"
              ]
            }
            """;

        DateArrayScalar scalar = JsonSerializer
            .Deserialize<DateArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedDates, scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": [
                "{{formattedDates.First()}}",
                "{{formattedDates.Skip(1).First()}}",
                "{{formattedDates.Skip(2).First()}}"
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateArrayReturning(new DateArrayScalar(expectedDates)),
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
                "name": "dateArray"
              },
              "name": "{{expected}}"
            }
            """;

        DateArrayParameter parameter = JsonSerializer
            .Deserialize<DateArrayReturning>(input, _options)!
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
                "name": "dateArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateArrayReturning(new DateArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachDateAddDays()
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
                  "name": "dateArray"
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

        EachDateAddDays addDays = JsonSerializer
            .Deserialize<DateArrayReturning>(input, _options)!
            .AsT3;
        Assert.Equal(new DateField(dateEntity, dateField), addDays.Left.AsT1.AsT1);
    }

    [Fact]
    public void WriteEachDateAddDays()
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
                  "name": "dateArray"
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
            new DateArrayReturning(
                new EachDateAddDays(
                    new DateArrayReturning(new DateField(dateEntity, dateField)),
                    new NumberArrayReturning(new NumberField(numEntity, numField))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachDateAddDaysWithScalarRight()
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
                  "name": "dateArray"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 7
              }
            }
            """;

        EachDateAddDays addDays = JsonSerializer
            .Deserialize<DateArrayReturning>(input, _options)!
            .AsT3;
        Assert.Equal(7, addDays.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteEachDateAddDaysWithScalarRight()
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
                  "name": "dateArray"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 7
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateArrayReturning(
                new EachDateAddDays(
                    new DateArrayReturning(new DateField(dateEntity, dateField)),
                    new NumberReturning(new NumberScalar(7))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
