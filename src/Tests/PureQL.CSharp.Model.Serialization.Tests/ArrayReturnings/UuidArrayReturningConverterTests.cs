using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record UuidArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidArrayReturningConverterTests()
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
                "name": "guidArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        UuidField field = JsonSerializer
            .Deserialize<UuidArrayReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new UuidArrayType(), field.Type);
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new UuidArrayReturning(new UuidField(expectedEntity, expectedField)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "guidArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadArrayScalar()
    {
        IEnumerable<Guid> expected = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        string input = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "value": ["{{expected.First()}}", "{{expected.Skip(
                1
            ).First()}}", "{{expected.Skip(2).First()}}"]
            }
            """;

        UuidArrayScalar scalar = JsonSerializer
            .Deserialize<UuidArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        IEnumerable<Guid> expected = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        string expectedJson = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "value": ["{{expected.First()}}", "{{expected.Skip(
                1
            ).First()}}", "{{expected.Skip(2).First()}}"]
            }
            """;

        string output = JsonSerializer.Serialize(
            new UuidArrayReturning(new UuidArrayScalar(expected)),
            _options
        );

        Assert.Equal(expectedJson, output);
    }

    [Fact]
    public void ReadParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "guidArray"
              },
              "name": "{{expected}}"
            }
            """;

        UuidArrayParameter parameter = JsonSerializer
            .Deserialize<UuidArrayReturning>(input, _options)!
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
                "name": "guidArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new UuidArrayReturning(new UuidArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }
}
