using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record EqualityConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EqualityConverterTests()
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
    public void ReadSingleValueEquality()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              }
            }
            """;

        SingleValueEquality equality = JsonSerializer
            .Deserialize<Equality>(input, _options)!
            .AsT0;

        Assert.Equal(new BooleanScalar(true), equality.AsT0.Left.AsT1);
        Assert.Equal(new BooleanScalar(false), equality.AsT0.Right.AsT1);
    }

    [Fact]
    public void WriteSingleValueEquality()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              },
              "right": {
                "type": {
                  "name": "boolean"
                },
                "value": false
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Equality(
                new SingleValueEquality(
                    new BooleanEquality(
                        new BooleanReturning(new BooleanScalar(true)),
                        new BooleanReturning(new BooleanScalar(false))
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "type": {
                  "name": "booleanArray"
                },
                "name": "{{leftParamName}}"
              },
              "right": {
                "type": {
                  "name": "booleanArray"
                },
                "name": "{{rightParamName}}"
              }
            }
            """;

        ArrayEquality equality = JsonSerializer
            .Deserialize<Equality>(input, _options)!
            .AsT1;

        Assert.Equal(new BooleanArrayParameter(leftParamName), equality.AsT0.Left.AsT2);
        Assert.Equal(new BooleanArrayParameter(rightParamName), equality.AsT0.Right.AsT2);
    }

    [Fact]
    public void WriteArrayEquality()
    {
        const string leftParamName = "ashjlbd";
        const string rightParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "equal",
              "left": {
                "name": "{{leftParamName}}",
                "type": {
                  "name": "booleanArray"
                }
              },
              "right": {
                "name": "{{rightParamName}}",
                "type": {
                  "name": "booleanArray"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Equality(
                new ArrayEquality(
                    new BooleanArrayEquality(
                        new BooleanArrayReturning(
                            new BooleanArrayParameter(leftParamName)
                        ),
                        new BooleanArrayReturning(
                            new BooleanArrayParameter(rightParamName)
                        )
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
            JsonSerializer.Deserialize<Equality>(input, _options)
        );
    }
}
