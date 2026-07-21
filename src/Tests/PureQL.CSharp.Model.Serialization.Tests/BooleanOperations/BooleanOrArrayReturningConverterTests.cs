using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.BooleanOperations;

/// <summary>
/// Covers <c>OneOf&lt;BooleanReturning, BooleanArrayReturning&gt;</c> - the type used for
/// <c>Query.Where</c>, <c>Query.Having</c> and <c>Join.On</c> - which previously had no
/// dedicated test coverage of its own, only incidental coverage through trivial
/// boolean-literal fixtures in <see cref="QueryConverterTests"/> and
/// <see cref="JoinConverterTests"/>.
/// </summary>
public sealed record BooleanOrArrayReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanOrArrayReturningConverterTests()
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
    public void ReadBooleanReturningBranch()
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

        OneOf<BooleanReturning, BooleanArrayReturning> value = JsonSerializer.Deserialize<
            OneOf<BooleanReturning, BooleanArrayReturning>
        >(input, _options);

        Assert.True(value.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteBooleanReturningBranch()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        string output = JsonSerializer.Serialize(
            OneOf<BooleanReturning, BooleanArrayReturning>.FromT0(
                new BooleanReturning(new BooleanScalar(true))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBareFieldPrefersBooleanArrayReturningBranch()
    {
        const string expectedEntity = "myEntity";
        const string expectedField = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "boolean"
              }
            }
            """;

        OneOf<BooleanReturning, BooleanArrayReturning> value = JsonSerializer.Deserialize<
            OneOf<BooleanReturning, BooleanArrayReturning>
        >(input, _options);

        Assert.True(value.IsT1);
        Assert.Equal(
            new BooleanField(expectedEntity, expectedField),
            value.AsT1.AsT1
        );
    }

    [Fact]
    public void ReadEachEqualityFieldToLiteralFiltersRows()
    {
        const string expectedEntity = "resources";
        const string expectedField = "name";
        const string expectedValue = "sand";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "string"
                }
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{expectedValue}}"
              }
            }
            """;

        OneOf<BooleanReturning, BooleanArrayReturning> value = JsonSerializer.Deserialize<
            OneOf<BooleanReturning, BooleanArrayReturning>
        >(input, _options);

        Assert.True(value.IsT1);
        EachStringEquality equality = value.AsT1.AsT4.AsT2;
        Assert.Equal(expectedEntity, equality.Left.AsT1.Entity);
        Assert.Equal(expectedField, equality.Left.AsT1.Field);
        Assert.Equal(expectedValue, equality.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteEachEqualityFieldToLiteralFiltersRows()
    {
        const string expectedEntity = "resources";
        const string expectedField = "name";
        const string expectedValue = "sand";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachEqual",
              "left": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedField}}",
                "type": {
                  "name": "string"
                }
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "{{expectedValue}}"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            OneOf<BooleanReturning, BooleanArrayReturning>.FromT1(
                new BooleanArrayReturning(
                    new EachEquality(
                        new EachStringEquality(
                            new StringArrayReturning(
                                new StringField(expectedEntity, expectedField)
                            ),
                            new StringReturning(new StringScalar(expectedValue))
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadCompoundEachAndOfTwoFieldToLiteralConditions()
    {
        const string entity = "people";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachAnd",
              "conditions": [
                {
                  "operator": "eachEqual",
                  "left": {
                    "entity": "{{entity}}",
                    "field": "lastName",
                    "type": {
                      "name": "string"
                    }
                  },
                  "right": {
                    "type": {
                      "name": "string"
                    },
                    "value": "Ivanov"
                  }
                },
                {
                  "operator": "eachEqual",
                  "left": {
                    "entity": "{{entity}}",
                    "field": "firstName",
                    "type": {
                      "name": "string"
                    }
                  },
                  "right": {
                    "type": {
                      "name": "string"
                    },
                    "value": "Petr"
                  }
                }
              ]
            }
            """;

        OneOf<BooleanReturning, BooleanArrayReturning> value = JsonSerializer.Deserialize<
            OneOf<BooleanReturning, BooleanArrayReturning>
        >(input, _options);

        Assert.True(value.IsT1);
        EachAndOperator and = value.AsT1.AsT5;
        Assert.Equal(2, and.Conditions.Count());
    }

    /// <summary>
    /// Documents the (currently intentional) boundary between the scalar
    /// "equal"/comparison family - which only accepts Parameter/Scalar/Aggregate
    /// operands - and the row-wise "eachEqual" family, which is the only one that
    /// accepts a bare Field operand. A field-vs-literal predicate using the plain
    /// (non-each) "equal" operator must keep failing; if this starts passing, the two
    /// families have been merged and the tests exercising the distinction (this one,
    /// and the eachEqual tests above) need to be revisited together.
    /// </summary>
    [Fact]
    public void ThrowsOnPlainEqualityBetweenFieldAndLiteral()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "equal",
              "left": {
                "entity": "resources",
                "field": "name",
                "type": {
                  "name": "string"
                }
              },
              "right": {
                "type": {
                  "name": "string"
                },
                "value": "sand"
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<OneOf<BooleanReturning, BooleanArrayReturning>>(
                input,
                _options
            )
        );
    }

    [Fact]
    public void ThrowsExceptionOnGarbageInput()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "unknownProperty": "erafuhyobdng"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<OneOf<BooleanReturning, BooleanArrayReturning>>(
                input,
                _options
            )
        );
    }
}
