using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.EachComparisons;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayReturnings;

public sealed record BooleanArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanArrayReturningConverterTests()
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
    public void ReadField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "booleanArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new BooleanField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<BooleanArrayReturning>(input, _options)!.AsT1
        );
    }

    [Fact]
    public void WriteField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new BooleanField(expectedEntity, expectedField)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "booleanArray"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadArrayScalar()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "booleanArray"
              },
              "value": [
                true,
                false,
                true
              ]
            }
            """;

        BooleanArrayScalar scalar = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT0;

        Assert.Equal([true, false, true], scalar.Value);
    }

    [Fact]
    public void WriteArrayScalar()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "booleanArray"
                  },
                  "value": [
                    true,
                    false,
                    true
                  ]
                }
                """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new BooleanArrayScalar([true, false, true])),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadParameter()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "booleanArray"
              },
              "name": "{{expected}}"
            }
            """;

        BooleanArrayParameter parameter = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void WriteParameter()
    {
        const string name = "asudu";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "name": "{{name}}",
              "type": {
                "name": "booleanArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new BooleanArrayParameter(name)),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachComparison()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "numberArray"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 5
              }
            }
            """;

        EachComparison comparison = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT3;
        Assert.Equal(
            new NumberField(entity, field),
            comparison.AsT0.Left.AsT1
        );
    }

    [Fact]
    public void WriteEachComparison()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachGreaterThan",
              "left": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "numberArray"
                }
              },
              "right": {
                "type": {
                  "name": "number"
                },
                "value": 5
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(
                new EachComparison(
                    new EachNumberComparison(
                        EachComparisonOperator.EachGreaterThan,
                        new NumberArrayReturning(new NumberField(entity, field)),
                        new NumberReturning(new NumberScalar(5))
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachEquality()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "booleanArray"
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

        EachEquality equality = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT4;
        Assert.Equal(
            new BooleanField(entity, field),
            equality.AsT0.Left.AsT1
        );
    }

    [Fact]
    public void WriteEachEquality()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{entity}}",
                "field": "{{field}}",
                "type": {
                  "name": "booleanArray"
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

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(
                new EachEquality(
                    new EachBooleanEquality(
                        new BooleanArrayReturning(new BooleanField(entity, field)),
                        new BooleanReturning(new BooleanScalar(true))
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachAndOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd",
              "conditions": []
            }
            """;

        EachAndOperator op = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT5;
        Assert.Empty(op.Conditions);
    }

    [Fact]
    public void WriteEachAndOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachAnd",
              "conditions": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new EachAndOperator([])),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachOrOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": []
            }
            """;

        EachOrOperator op = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT6;
        Assert.Empty(op.Conditions);
    }

    [Fact]
    public void WriteEachOrOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachOr",
              "conditions": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(new EachOrOperator([])),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachNotOperator()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachNot",
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [true]
              }
            }
            """;

        EachNotOperator op = JsonSerializer
            .Deserialize<BooleanArrayReturning>(input, _options)!
            .AsT7;
        Assert.Equal([true], op.Condition.AsT0.Value);
    }

    [Fact]
    public void WriteEachNotOperator()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachNot",
              "condition": {
                "type": {
                  "name": "booleanArray"
                },
                "value": [
                  true
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanArrayReturning(
                new EachNotOperator(
                    new BooleanArrayReturning(new BooleanArrayScalar([true]))
                )
            ),
            _options
        );
        Assert.Equal(expected, output);
    }
}
