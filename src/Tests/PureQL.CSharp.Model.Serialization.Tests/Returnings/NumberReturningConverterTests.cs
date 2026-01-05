using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record NumberReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberReturningConverterTests()
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
    public void ReadNumberField()
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

        NumberField field = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new NumberType(), field.Type);
    }

    [Fact]
    public void WriteNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new NumberReturning(new NumberField(expectedEntity, expectedField)),
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
    public void ReadNumberParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "name": "{{paramName}}"
            }
            """;

        NumberParameter parameter = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new NumberType(), parameter.Type);
    }

    [Fact]
    public void WriteNumberParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new NumberReturning(new NumberParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "number"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadNumberScalar()
    {
        double expectedValue = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
            }
            """;
        NumberScalar scalar = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteNumberScalar()
    {
        double expectedValue = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberReturning(new NumberScalar(expectedValue)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ihufd")]
    public void ThrowsExceptionOnWrongFieldType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "entity": "ufbrdeyhov",
              "field": "heuiyrndfosgv"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("ihufd")]
    public void ThrowsExceptionOnWrongParameterType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "name": "erfinjdhksgt"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "2025-12-24T15:20:36.6778291+03:00"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }
}
