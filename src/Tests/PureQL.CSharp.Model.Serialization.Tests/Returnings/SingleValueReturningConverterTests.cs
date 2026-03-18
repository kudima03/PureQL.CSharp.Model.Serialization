using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record SingleValueReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SingleValueReturningConverterTests()
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
    public void ReadBooleanReturning()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "boolean"
              },
              "name": "{{paramName}}"
            }
            """;

        BooleanParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT0.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new BooleanType(), parameter.Type);
    }

    [Fact]
    public void WriteBooleanReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new BooleanReturning(new BooleanParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "boolean"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateReturning()
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
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT1.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateType(), parameter.Type);
    }

    [Fact]
    public void WriteDateReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new DateReturning(new DateParameter(expectedParamName))
            ),
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
    public void ReadDateTimeReturning()
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
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT2.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateTimeType(), parameter.Type);
    }

    [Fact]
    public void WriteDateTimeReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new DateTimeReturning(new DateTimeParameter(expectedParamName))
            ),
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
    public void ReadNumberReturning()
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
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT3.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new NumberType(), parameter.Type);
    }

    [Fact]
    public void WriteNumberReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new NumberReturning(new NumberParameter(expectedParamName))
            ),
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
    public void ReadStringReturning()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "string"
              },
              "name": "{{paramName}}"
            }
            """;

        StringParameter parameter = JsonSerializer
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT4.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new StringType(), parameter.Type);
    }

    [Fact]
    public void WriteStringReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new StringReturning(new StringParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "string"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeReturning()
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
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT5.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new TimeType(), parameter.Type);
    }

    [Fact]
    public void WriteTimeReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new TimeReturning(new TimeParameter(expectedParamName))
            ),
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
    public void ReadUuidReturning()
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
            .Deserialize<SingleValueReturning>(input, _options)!
            .AsT6.AsT0;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new UuidType(), parameter.Type);
    }

    [Fact]
    public void WriteUuidReturning()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SingleValueReturning(
                new UuidReturning(new UuidParameter(expectedParamName))
            ),
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

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SingleValueReturning>(input, _options)
        );
    }
}
