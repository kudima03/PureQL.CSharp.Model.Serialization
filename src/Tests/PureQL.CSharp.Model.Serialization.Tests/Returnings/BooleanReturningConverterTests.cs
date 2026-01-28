using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record BooleanReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanReturningConverterTests()
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
    public void ReadBooleanParameter()
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
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new BooleanType(), parameter.Type);
    }

    [Fact]
    public void WriteBooleanParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new BooleanReturning(new BooleanParameter(expectedParamName)),
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
    public void ReadBooleanScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        BooleanScalar scalar = JsonSerializer
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT2;

        Assert.True(scalar.Value);
    }

    [Fact]
    public void WriteBooleanScalar()
    {
        string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanReturning(new BooleanScalar(true)),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadEquality()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "entity": "u",
                "field": "active",
                "type": {
                  "name": "boolean"
                }
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        BooleanEquality equality = JsonSerializer
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT3.AsT0;

        Assert.Equal(new BooleanField("u", "active"), equality.Left.AsT0);
        Assert.Equal(new BooleanScalar(true), equality.Right.AsT2);
    }

    [Fact]
    public void ReadBooleanOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "entity": "u",
                  "field": "active",
                  "type": {
                    "name": "boolean"
                  }
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
              ]
            }
            """;

        BooleanReturning booleanReturning = JsonSerializer.Deserialize<BooleanReturning>(
            input,
            _options
        )!;

        AndOperator andOperator = booleanReturning.AsT4.AsT0;

        Assert.Equal(
            new BooleanField("u", "active"),
            andOperator.Conditions.First().AsT0
        );
        Assert.Equal(new BooleanScalar(true), andOperator.Conditions.Last().AsT2);
    }

    [Fact]
    public void ReadComparison()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedField = "efrnijk";
        const ushort rightValue = 12;
        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "greaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{rightValue}}
              }
            }
            """;

        NumberComparison comparison = JsonSerializer
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT5.AsT2;

        Assert.Equal(
            new NumberComparison(
                ComparisonOperator.GreaterThan,
                new NumberReturning(new NumberField(expectedEntity, expectedField)),
                new NumberReturning(new NumberScalar(rightValue))
            ),
            comparison
        );
    }

    [Fact]
    public void WriteComparison()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedField = "efrnijk";
        const ushort rightValue = 12;
        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "greaterThan",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "number"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": {{rightValue}}
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanReturning(
                new Comparison(
                    new NumberComparison(
                        ComparisonOperator.GreaterThan,
                        new NumberReturning(
                            new NumberField(expectedEntity, expectedField)
                        ),
                        new NumberReturning(new NumberScalar(rightValue))
                    )
                )
            ),
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
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("datetime")]
    [InlineData("date")]
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
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("datetime")]
    [InlineData("date")]
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
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData("datetime")]
    [InlineData("date")]
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
              "value": true
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }
}
