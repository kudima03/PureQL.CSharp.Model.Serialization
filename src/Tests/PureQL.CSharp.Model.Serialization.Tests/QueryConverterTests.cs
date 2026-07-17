using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record QueryConverterTests
{
    private readonly JsonSerializerOptions _options;

    public QueryConverterTests()
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
    public void ReadSelectCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedAlias = "dsfvnkjm";
        const string expectedField = "edrfghiujn";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}",
                "alias": "{{expectedAlias}}"
              },
              "select": [
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "string"
                  }
                }
              ]
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.Equal(new FromExpression(expectedEntity, expectedAlias), query.From);
        Assert.True(
            Enumerable
                .Empty<SelectExpression>()
                .Append(
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedAlias, expectedField)
                            )
                        )
                    )
                )
                .SequenceEqual(query.SelectExpressions)
        );
    }

    [Fact]
    public void WriteSelectCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedAlias = "dsfvnkjm";
        const string expectedField = "edrfghiujn";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}",
                "alias": "{{expectedAlias}}"
              },
              "select": [
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "string"
                  }
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new Query(
                new FromExpression(expectedEntity, expectedAlias),
                [
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedAlias, expectedField)
                            )
                        )
                    ),
                ]
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadMultipleSelectCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedAlias = "dsfvnkjm";
        const string expectedField1 = "edrfghiujn";
        const string expectedField2 = "edfrgin";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}",
                "alias": "{{expectedAlias}}"
              },
              "select": [
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField1}}",
                  "type": {
                    "name": "string"
                  }
                },
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField2}}",
                  "type": {
                    "name": "string"
                  }
                }
              ]
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.Equal(new FromExpression(expectedEntity, expectedAlias), query.From);
        Assert.True(
            Enumerable
                .Empty<SelectExpression>()
                .Append(
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedAlias, expectedField1)
                            )
                        )
                    )
                )
                .Append(
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedAlias, expectedField2)
                            )
                        )
                    )
                )
                .SequenceEqual(query.SelectExpressions)
        );
    }

    [Fact]
    public void WriteMultipleSelectCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedAlias = "dsfvnkjm";
        const string expectedField1 = "edrfghiujn";
        const string expectedField2 = "edfrgin";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}",
                "alias": "{{expectedAlias}}"
              },
              "select": [
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField1}}",
                  "type": {
                    "name": "string"
                  }
                },
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField2}}",
                  "type": {
                    "name": "string"
                  }
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new Query(
                new FromExpression(expectedEntity, expectedAlias),
                [
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedAlias, expectedField1)
                            )
                        )
                    ),
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedAlias, expectedField2)
                            )
                        )
                    ),
                ]
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadJoinsCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedJoinEntity = "refnhbdjusi";
        const string expectedField = "edrfghiujn";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}"
              },
              "select": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "string"
                  }
                }
              ],
              "joins": [
                {
                  "type": "inner",
                  "entity": "{{expectedJoinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
              ]
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.NotNull(query.Join);
        Assert.True(
            Enumerable
                .Empty<Join>()
                .Append(
                    new Join(
                        JoinType.Inner,
                        expectedJoinEntity,
                        new BooleanReturning(new BooleanScalar(true))
                    )
                )
                .SequenceEqual(query.Join!)
        );
    }

    [Fact]
    public void WriteJoinsCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedJoinEntity = "refnhbdjusi";
        const string expectedField = "edrfghiujn";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}"
              },
              "select": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "string"
                  }
                }
              ],
              "joins": [
                {
                  "type": "inner",
                  "entity": "{{expectedJoinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new Query(
                new FromExpression(expectedEntity),
                [
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedEntity, expectedField)
                            )
                        )
                    ),
                ],
                where: null,
                [
                    new Join(
                        JoinType.Inner,
                        expectedJoinEntity,
                        new BooleanReturning(new BooleanScalar(true))
                    ),
                ],
                groupBy: null,
                having: null,
                orderBy: null,
                pagination: null
            ),
            _options
        );

        Assert.Equal(expected, output);
    }
}
