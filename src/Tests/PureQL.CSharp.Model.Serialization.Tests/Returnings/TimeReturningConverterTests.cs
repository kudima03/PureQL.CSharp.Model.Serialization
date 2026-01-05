using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record TimeReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public TimeReturningConverterTests()
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
    public void ReadTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        TimeField field = JsonSerializer
            .Deserialize<TimeReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new TimeType(), field.Type);
    }

    [Fact]
    public void WriteTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new TimeReturning(new TimeField(expectedEntity, expectedField)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "time"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "name": "{{paramName}}"
            }
            """;

        TimeParameter parameter = JsonSerializer
            .Deserialize<TimeReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new TimeType(), parameter.Type);
    }

    [Fact]
    public void WriteTimeParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new TimeReturning(new TimeParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "time"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeScalar()
    {
        TimeOnly expectedValue = TimeOnly.FromDateTime(DateTime.Now);

        string input = $$"""
            {
              "type": {
                "name": "time"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;
        TimeScalar scalar = JsonSerializer
            .Deserialize<TimeReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteTimeScalar()
    {
        TimeOnly expectedValue = TimeOnly.FromDateTime(DateTime.Now);

        string expected = $$"""
            {
              "type": {
                "name": "time"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new TimeReturning(new TimeScalar(expectedValue)),
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
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
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
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
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
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "hbgfrtdvsdhcif"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
        );
    }
}
