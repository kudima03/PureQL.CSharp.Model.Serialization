using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.BooleanOperations;

public sealed record BooleanOperatorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanOperatorConverterTests()
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
    public void ReadAndOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              ]
            }
            """;

        AndOperator andOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT0;

        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ],
            andOperator.Conditions.AsT0
        );
    }

    [Fact]
    public void WriteAndOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "and",
              "conditions": [
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new AndOperator([
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false)),
                ])
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadOrOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "or",
              "conditions": [
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              ]
            }
            """;

        OrOperator orOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT1;

        Assert.Equal(
            [
                new BooleanReturning(new BooleanScalar(true)),
                new BooleanReturning(new BooleanScalar(false)),
            ],
            orOperator.Conditions.AsT0
        );
    }

    [Fact]
    public void WriteOrOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "or",
              "conditions": [
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                },
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": false
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new OrOperator([
                    new BooleanReturning(new BooleanScalar(true)),
                    new BooleanReturning(new BooleanScalar(false)),
                ])
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNotOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        NotOperator notOperator = JsonSerializer
            .Deserialize<BooleanOperator>(input, _options)!
            .AsT2;

        Assert.Equal(new BooleanScalar(true), notOperator.Condition.AsT1);
    }

    [Fact]
    public void WriteNotOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "not",
              "condition": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanOperator(
                new NotOperator(new BooleanReturning(new BooleanScalar(true)))
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
            JsonSerializer.Deserialize<BooleanOperator>(input, _options)
        );
    }
}
