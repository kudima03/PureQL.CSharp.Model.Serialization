using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachDateTimeArithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record DateTimeArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeArrayReturningConverterTests()
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
                "name": "datetime"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new DateTimeField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<DateTimeArrayReturning>(input, _options)!.AsT1
        );
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new DateTimeArrayReturning(new DateTimeField(expectedEntity, expectedField)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "datetime"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadArrayScalar()
    {
        IEnumerable<DateTime> expected =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expected.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "value": [
                {{formattedDates.First()}},
                {{formattedDates.Skip(1).First()}},
                {{formattedDates.Skip(2).First()}}
              ]
            }
            """;

        DateTimeArrayScalar scalar = JsonSerializer
            .Deserialize<DateTimeArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        IEnumerable<DateTime> expectedValues =
        [
            DateTime.Now,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddYears(1),
        ];

        IEnumerable<string> formattedDates = expectedValues.Select(x =>
            JsonSerializer.Serialize(x, _options)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "value": [
                {{formattedDates.First()}},
                {{formattedDates.Skip(1).First()}},
                {{formattedDates.Skip(2).First()}}
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateTimeArrayReturning(new DateTimeArrayScalar(expectedValues)),
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
                "name": "datetimeArray"
              },
              "name": "{{expected}}"
            }
            """;

        DateTimeArrayParameter parameter = JsonSerializer
            .Deserialize<DateTimeArrayReturning>(input, _options)!
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
                "name": "datetimeArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateTimeArrayReturning(new DateTimeArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachDateTimeAddSeconds()
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

        EachDateTimeAddSeconds addSeconds = JsonSerializer
            .Deserialize<DateTimeArrayReturning>(input, _options)!
            .AsT3;
        Assert.Equal(
            new DateTimeField(dtEntity, dtField),
            addSeconds.Left.AsT1.AsT1
        );
    }

    [Fact]
    public void WriteEachDateTimeAddSeconds()
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
            new DateTimeArrayReturning(
                new EachDateTimeAddSeconds(
                    new DateTimeArrayReturning(new DateTimeField(dtEntity, dtField)),
                    new NumberArrayReturning(new NumberField(numEntity, numField))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachDateTimeAddSecondsWithScalarRight()
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

        EachDateTimeAddSeconds addSeconds = JsonSerializer
            .Deserialize<DateTimeArrayReturning>(input, _options)!
            .AsT3;
        Assert.Equal(3600, addSeconds.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteEachDateTimeAddSecondsWithScalarRight()
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
            new DateTimeArrayReturning(
                new EachDateTimeAddSeconds(
                    new DateTimeArrayReturning(new DateTimeField(dtEntity, dtField)),
                    new NumberReturning(new NumberScalar(3600))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
