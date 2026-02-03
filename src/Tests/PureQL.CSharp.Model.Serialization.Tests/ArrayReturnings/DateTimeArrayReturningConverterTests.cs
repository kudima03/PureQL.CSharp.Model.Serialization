using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

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
                "name": "datetimeArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        DateTimeField field = JsonSerializer
            .Deserialize<DateTimeArrayReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateTimeArrayType(), field.Type);
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
                "name": "datetimeArray"
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

        IEnumerable<string> formattedDates = expected.Select(x => x.ToString("O"));

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "value": [
                "{{formattedDates.First()}}",
                "{{formattedDates.Skip(1).First()}}",
                "{{formattedDates.Skip(2).First()}}"
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
}
