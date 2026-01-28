using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record TimeArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public TimeArrayReturningConverterTests()
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
                "name": "timeArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        TimeField field = JsonSerializer
            .Deserialize<TimeArrayReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new TimeArrayType(), field.Type);
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new TimeArrayReturning(new TimeField(expectedEntity, expectedField)),
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
    public void ReadArrayScalar()
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
              "value": [{{formattedTimes.First()}}, {{formattedTimes.Skip(
                1
            ).First()}}, {{formattedTimes.Skip(2).First()}}]
            }
            """;

        TimeArrayScalar scalar = JsonSerializer
            .Deserialize<TimeArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
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
              "value": [{{formattedTimes.First()}}, {{formattedTimes.Skip(
                1
            ).First()}}, {{formattedTimes.Skip(2).First()}}]
            }
            """;

        string output = JsonSerializer.Serialize(
            new TimeArrayReturning(new TimeArrayScalar(expectedValues)),
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
                "name": "timeArray"
              },
              "name": "{{expected}}"
            }
            """;

        TimeArrayParameter parameter = JsonSerializer
            .Deserialize<TimeArrayReturning>(input, _options)!
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
                "name": "timeArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new TimeArrayReturning(new TimeArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }
}
