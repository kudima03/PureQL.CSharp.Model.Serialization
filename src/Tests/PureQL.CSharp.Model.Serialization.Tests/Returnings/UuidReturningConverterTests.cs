using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record UuidReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidReturningConverterTests()
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
    public void ReadUuidParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "name": "{{paramName}}"
            }
            """;

        UuidParameter parameter = JsonSerializer
            .Deserialize<UuidReturning>(input, _options)!
            .AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new UuidType(), parameter.Type);
    }

    [Fact]
    public void WriteUuidParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new UuidReturning(new UuidParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "uuid"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidScalar()
    {
        Guid expectedValue = Guid.NewGuid();

        string input = $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expectedValue}}"
            }
            """;
        UuidScalar scalar = JsonSerializer
            .Deserialize<UuidReturning>(input, _options)!
            .AsT1;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteUuidScalar()
    {
        Guid expectedValue = Guid.NewGuid();

        string expected = $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expectedValue}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new UuidReturning(new UuidScalar(expectedValue)),
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
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("ihufd")]
    public void ThrowsExceptionOnWrongParameterType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "{{Guid.CreateVersion7()}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    public void ThrowsExceptionOnWrongScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "{{Guid.CreateVersion7()}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }
}
