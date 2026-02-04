using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record ArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ArrayReturningConverterTests()
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
    public void ReadBooleanField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "booleanArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        BooleanField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT0.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new BooleanArrayType(), field.Type);
    }

    [Fact]
    public void WriteBooleanField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new BooleanArrayReturning(new BooleanField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "booleanArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadBooleanArrayScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "booleanArray"
              },
              "value": [
                true,
                false,
                true
              ]
            }
            """;

        BooleanArrayScalar scalar = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT0.AsT0;

        Assert.Equal([true, false, true], scalar.Value);
    }

    [Fact]
    public void WriteBooleanArrayScalar()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "booleanArray"
                  },
                  "value": [
                    true,
                    false,
                    true
                  ]
                }
                """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new BooleanArrayReturning(new BooleanArrayScalar([true, false, true]))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBooleanParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "booleanArray"
              },
              "name": "{{expected}}"
            }
            """;

        BooleanArrayParameter parameter = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT0.AsT2;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteBooleanParameter()
    {
        const string name = "asudu";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "name": "{{name}}",
              "type": {
                "name": "booleanArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new BooleanArrayReturning(new BooleanArrayParameter(name))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateField()
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

        DateField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT1.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateArrayType(), field.Type);
    }

    [Fact]
    public void WriteDateField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new DateArrayReturning(new DateField(expectedEntity, expectedField))
            ),
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
    public void ReadDateArrayScalar()
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
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT1.AsT2;

        Assert.Equal(expectedDates, scalar.Value);
    }

    [Fact]
    public void WriteDateArrayScalar()
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
            new ArrayReturning(
                new DateArrayReturning(new DateArrayScalar(expectedDates))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateParameter()
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
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT1.AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteDateParameter()
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
            new ArrayReturning(new DateArrayReturning(new DateArrayParameter(name))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        DateTimeField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT2.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateTimeArrayType(), field.Type);
    }

    [Fact]
    public void WriteDateTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new DateTimeArrayReturning(
                    new DateTimeField(expectedEntity, expectedField)
                )
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "datetimeArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateTimeArrayScalar()
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
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT2.AsT2;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void WriteDateTimeArrayScalar()
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
            new ArrayReturning(
                new DateTimeArrayReturning(new DateTimeArrayScalar(expectedValues))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDateTimeParameter()
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
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT2.AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteDateTimeParameter()
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
            new ArrayReturning(
                new DateTimeArrayReturning(new DateTimeArrayParameter(name))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        NumberField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT3.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new NumberArrayType(), field.Type);
    }

    [Fact]
    public void WriteNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new NumberArrayReturning(new NumberField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "numberArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadNumberArrayScalar()
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
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT3.AsT2;

        Assert.Equal(values, scalar.Value);
    }

    [Fact]
    public void WriteNumberArrayScalar()
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
            new ArrayReturning(new NumberArrayReturning(new NumberArrayScalar(values))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberParameter()
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
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT3.AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteNumberParameter()
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
            new ArrayReturning(new NumberArrayReturning(new NumberArrayParameter(name))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "stringArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        StringField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT4.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new StringArrayType(), field.Type);
    }

    [Fact]
    public void WriteStringField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new StringArrayReturning(new StringField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "stringArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringArrayScalar()
    {
        string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "stringArray"
              },
              "value": [
                "ianhuedrfiuhaerfd",
                "sdkfnjilhnsjkd"
              ]
            }
            """;

        StringArrayScalar scalar = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT4.AsT2;

        Assert.Equal(["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"], scalar.Value);
    }

    [Fact]
    public void WriteStringArrayScalar()
    {
        string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "stringArray"
              },
              "value": [
                "ianhuedrfiuhaerfd",
                "sdkfnjilhnsjkd"
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new StringArrayReturning(
                    new StringArrayScalar(["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"])
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadStringParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "stringArray"
              },
              "name": "{{expected}}"
            }
            """;

        StringArrayParameter parameter = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT4.AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteStringParameter()
    {
        const string name = "asudu";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "name": "{{name}}",
              "type": {
                "name": "stringArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(new StringArrayReturning(new StringArrayParameter(name))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        TimeField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT5.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new TimeArrayType(), field.Type);
    }

    [Fact]
    public void WriteTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new TimeArrayReturning(new TimeField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "timeArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeArrayScalar()
    {
        IEnumerable<TimeOnly> expected =
        [
            new TimeOnly(14, 30, 15),
            new TimeOnly(15, 30, 15),
            new TimeOnly(14, 40, 16),
        ];

        IEnumerable<string> formattedTimes = expected.Select(x => x.ToString("HH:mm:ss"));

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "value": [
                "{{formattedTimes.First()}}",
                "{{formattedTimes.Skip(1).First()}}",
                "{{formattedTimes.Skip(2).First()}}"
              ]
            }
            """;

        TimeArrayScalar scalar = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT5.AsT2;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void WriteTimeArrayScalar()
    {
        IEnumerable<TimeOnly> expectedValues =
        [
            new TimeOnly(14, 30, 15),
            new TimeOnly(15, 30, 15),
            new TimeOnly(14, 40, 16),
        ];

        IEnumerable<string> formattedTimes = expectedValues.Select(x =>
            x.ToString("HH:mm:ss")
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "value": [
                "{{formattedTimes.First()}}",
                "{{formattedTimes.Skip(1).First()}}",
                "{{formattedTimes.Skip(2).First()}}"
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new TimeArrayReturning(new TimeArrayScalar(expectedValues))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadTimeParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "name": "{{expected}}"
            }
            """;

        TimeArrayParameter parameter = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT5.AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteTimeParameter()
    {
        const string name = "asudu";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "name": "{{name}}",
              "type": {
                "name": "timeArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(new TimeArrayReturning(new TimeArrayParameter(name))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadUuidField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        UuidField field = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT6.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new UuidArrayType(), field.Type);
    }

    [Fact]
    public void WriteUuidField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new ArrayReturning(
                new UuidArrayReturning(new UuidField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "uuidArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidArrayScalar()
    {
        IEnumerable<Guid> expected = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "value": [
                "{{expected.First()}}",
                "{{expected.Skip(1).First()}}",
                "{{expected.Skip(2).First()}}"
              ]
            }
            """;

        UuidArrayScalar scalar = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT6.AsT2;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void WriteUuidArrayScalar()
    {
        IEnumerable<Guid> expected = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "value": [
                "{{expected.First()}}",
                "{{expected.Skip(1).First()}}",
                "{{expected.Skip(2).First()}}"
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(new UuidArrayReturning(new UuidArrayScalar(expected))),
            _options
        );

        Assert.Equal(expectedJson, output);
    }

    [Fact]
    public void ReadUuidParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "name": "{{expected}}"
            }
            """;

        UuidArrayParameter parameter = JsonSerializer
            .Deserialize<ArrayReturning>(input, _options)!
            .AsT6.AsT0;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteUuidParameter()
    {
        const string name = "asudu";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "name": "{{name}}",
              "type": {
                "name": "uuidArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new ArrayReturning(new UuidArrayReturning(new UuidArrayParameter(name))),
            _options
        );

        Assert.Equal(expected, output);
    }
}
