using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.Arithmetics;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

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

        Assert.Equal(
            new NumberParameter(paramName),
            JsonSerializer.Deserialize<NumberReturning>(input, _options)!.AsT0
        );
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
            .AsT1;

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
              "value": 10
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }

    [Fact]
    public void ReadArithmetic()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        Arithmetic arithmetic = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT2;
        Assert.True(arithmetic.IsT0);
    }

    [Fact]
    public void WriteArithmetic()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberReturning(new Arithmetic(new Add([]))),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNumberAggregate()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        NumberAggregate aggregate = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT3;
        Assert.Equal(
            new NumberField(entity, field),
            aggregate.AsT2.Argument.AsT1
        );
    }

    [Fact]
    public void WriteNumberAggregate()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberReturning(
                new NumberAggregate(
                    new MinNumber(new NumberArrayReturning(new NumberField(entity, field)))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadCount()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        Count count = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT4;
        Assert.Equal(
            new NumberField(entity, field),
            count.Argument.AsT3.AsT1
        );
    }

    [Fact]
    public void WriteCount()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "count",
              "arg": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberReturning(
                new Count(
                    new ArrayReturning(
                        new NumberArrayReturning(
                            new NumberField(entity, field)
                        )
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
