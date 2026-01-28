using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record StringArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringArrayReturningConverterTests()
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
                "name": "stringArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        StringField field = JsonSerializer
            .Deserialize<StringArrayReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new StringArrayType(), field.Type);
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new StringArrayReturning(new StringField(expectedEntity, expectedField)),
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
    public void ReadArrayScalar()
    {
        string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "stringArray"
              },
              "value": ["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"]
            }
            """;

        StringArrayScalar scalar = JsonSerializer
            .Deserialize<StringArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"], scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "stringArray"
              },
              "value": ["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"]
            }
            """;

        string output = JsonSerializer.Serialize(
            new StringArrayReturning(
                new StringArrayScalar(["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"])
            ),
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
                "name": "stringArray"
              },
              "name": "{{expected}}"
            }
            """;

        StringArrayParameter parameter = JsonSerializer
            .Deserialize<StringArrayReturning>(input, _options)!
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
                "name": "stringArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new StringArrayReturning(new StringArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }
}
