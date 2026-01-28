using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record DateReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateReturningConverterTests()
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
    public void ReadDateParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "name": "{{paramName}}"
            }
            """;

        DateParameter parameter = JsonSerializer
            .Deserialize<DateReturning>(input, _options)!
            .AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateType(), parameter.Type);
    }

    [Fact]
    public void WriteDateParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new DateReturning(new DateParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "date"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateScalar()
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{DateTime.Now:yyyy-MM-dd}}"
            }
            """;

        DateScalar scalar = JsonSerializer
            .Deserialize<DateReturning>(input, _options)!
            .AsT1;

        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), scalar.Value);
    }

    [Fact]
    public void WriteDateScalar()
    {
        string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{DateTime.Now:yyyy-MM-dd}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateReturning(new DateScalar(DateOnly.FromDateTime(DateTime.Now))),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("datetime")]
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
            JsonSerializer.Deserialize<DateReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("datetime")]
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
            JsonSerializer.Deserialize<DateReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("datetime")]
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
              "value": "2000-01-01"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdasd")]
    [InlineData("123123123123")]
    [InlineData("2000-01-01 asdasd")]
    [InlineData("ghjghjghj 2000-01-01")]
    [InlineData("ghjghjghj 2000-01-01 tyhjghj")]
    [InlineData("2026-01-20T17:04:58.6854976+03:00")]
    [InlineData("17:05:49.8658905")]
    public void ThrowsExceptionOnWrongValue(string value)
    {
        string input = $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{value}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateReturning>(input, _options)
        );
    }
}
