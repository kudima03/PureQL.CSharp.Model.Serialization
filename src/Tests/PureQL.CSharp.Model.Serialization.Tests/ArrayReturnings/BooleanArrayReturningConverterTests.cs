using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record BooleanArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanArrayReturningConverterTests()
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
                "name": "booleanArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        BooleanField field = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new BooleanArrayType(), field.Type);
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new BooleanField(expectedEntity, expectedField)),
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
    public void ReadArrayScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "booleanArray"
              },
              "value": [true, false, true]
            }
            """;

        BooleanArrayScalar scalar = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT0;

        Assert.Equal([true, false, true], scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "booleanArray"
                  },
                  "value": [true, false, true]
                }
                """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new BooleanArrayScalar([true, false, true])),
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
                "name": "booleanArray"
              },
              "name": "{{expected}}"
            }
            """;

        BooleanArrayParameter parameter = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT2;

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
                "name": "boolean"
              }
            }
            """;

        string output = JsonSerializer.Serialize(new BooleanParameter(name), _options);

        Assert.Equal(expected, output);
    }
}
