using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Fields;

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
                    "name": "stringArray"
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
                    "name": "stringArray"
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
                    "name": "stringArray"
                  }
                },
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField2}}",
                  "type": {
                    "name": "stringArray"
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
                    "name": "stringArray"
                  }
                },
                {
                  "entity": "{{expectedAlias}}",
                  "field": "{{expectedField2}}",
                  "type": {
                    "name": "stringArray"
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
}
