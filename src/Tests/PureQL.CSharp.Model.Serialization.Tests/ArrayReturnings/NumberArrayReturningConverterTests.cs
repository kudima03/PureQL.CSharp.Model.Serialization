using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
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
                "name": "numberArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        NumberField field = JsonSerializer
            .Deserialize<NumberArrayReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new NumberArrayType(), field.Type);
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
                "name": "numberArray"
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
}
