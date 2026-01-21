using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record DateTimeReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeReturningConverterTests()
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
    public void ReadDateTimeField()
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

        DateTimeField field = JsonSerializer
            .Deserialize<DateTimeReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateTimeType(), field.Type);
    }

    [Fact]
    public void WriteDateTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new DateTimeReturning(new DateTimeField(expectedEntity, expectedField)),
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
    public void ReadDateTimeParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "name": "{{paramName}}"
            }
            """;

        DateTimeParameter parameter = JsonSerializer
            .Deserialize<DateTimeReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateTimeType(), parameter.Type);
    }

    [Fact]
    public void WriteDateTimeParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new DateTimeReturning(new DateTimeParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "datetime"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateTimeScalar()
    {
        DateTime expectedValue = DateTime.Now;

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{JsonSerializer.Serialize(expectedValue, _options)}}
            }
            """;

        DateTimeScalar scalar = JsonSerializer
            .Deserialize<DateTimeReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteDateTimeScalar()
    {
        DateTime expectedValue = DateTime.Now;

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{JsonSerializer.Serialize(expectedValue, _options)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateTimeReturning(new DateTimeScalar(expectedValue)),
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
            JsonSerializer.Deserialize<DateTimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
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
            JsonSerializer.Deserialize<DateTimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
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
            JsonSerializer.Deserialize<DateTimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
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
            JsonSerializer.Deserialize<DateTimeReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdasd")]
    [InlineData("123123123123")]
    [InlineData("2000-01-01 asdasd")]
    [InlineData("ghjghjghj 2000-01-01")]
    [InlineData("ghjghjghj 2000-01-01 tyhjghj")]
    [InlineData("17:05:49.8658905")]
    public void ThrowsExceptionOnWrongValue(string value)
    {
        string input = $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": "{{value}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateTimeReturning>(input, _options)
        );
    }
}
