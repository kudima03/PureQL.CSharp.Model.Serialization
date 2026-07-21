using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.EachEqualities;
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

    /// <summary>
    /// Realistic "WHERE column = 'value'" filter. Query.Where only round-tripped
    /// through <see cref="Comparison"/>/"equal"-style boolean literals before -
    /// never through the row-wise "eachEqual" family that is the only one able to
    /// compare a field against a literal.
    /// </summary>
    [Fact]
    public void ReadWhereEachEqualityFieldToLiteralCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedField = "edrfghiujn";
        const string expectedValue = "vfgtdcbhuji";

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
              "where": {
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
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.True(query.Where!.Value.IsT1);
        EachStringEquality equality = query.Where.Value.AsT1.AsT4.AsT2;
        Assert.Equal(expectedEntity, equality.Left.AsT1.Entity);
        Assert.Equal(expectedField, equality.Left.AsT1.Field);
        Assert.Equal(expectedValue, equality.Right.AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteWhereEachEqualityFieldToLiteralCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedField = "edrfghiujn";
        const string expectedValue = "vfgtdcbhuji";

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
              "where": {
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
                where: new BooleanArrayReturning(
                    new EachEquality(
                        new EachStringEquality(
                            new StringArrayReturning(
                                new StringField(expectedEntity, expectedField)
                            ),
                            new StringReturning(new StringScalar(expectedValue))
                        )
                    )
                ),
                join: null,
                groupBy: null,
                having: null,
                orderBy: null,
                pagination: null
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Compound row-wise filter ("WHERE a = 'x' AND b = 'y'") built from two
    /// eachEqual conditions combined with eachAnd.
    /// </summary>
    [Fact]
    public void ReadWhereCompoundEachAndCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string field1 = "edrfghiujn";
        const string field2 = "edfrgin";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}"
              },
              "select": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{field1}}",
                  "type": {
                    "name": "string"
                  }
                }
              ],
              "where": {
                "operator": "eachAnd",
                "conditions": [
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{expectedEntity}}",
                      "field": "{{field1}}",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "type": {
                        "name": "string"
                      },
                      "value": "x"
                    }
                  },
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{expectedEntity}}",
                      "field": "{{field2}}",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "type": {
                        "name": "string"
                      },
                      "value": "y"
                    }
                  }
                ]
              }
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.True(query.Where!.Value.IsT1);
        EachAndOperator and = query.Where.Value.AsT1.AsT5;
        Assert.Equal(2, and.Conditions.Count());
    }

    [Fact]
    public void WriteWhereCompoundEachAndCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string field1 = "edrfghiujn";
        const string field2 = "edfrgin";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "from": {
                "entity": "{{expectedEntity}}"
              },
              "select": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{field1}}",
                  "type": {
                    "name": "string"
                  }
                }
              ],
              "where": {
                "operator": "eachAnd",
                "conditions": [
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{expectedEntity}}",
                      "field": "{{field1}}",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "type": {
                        "name": "string"
                      },
                      "value": "x"
                    }
                  },
                  {
                    "operator": "eachEqual",
                    "left": {
                      "entity": "{{expectedEntity}}",
                      "field": "{{field2}}",
                      "type": {
                        "name": "string"
                      }
                    },
                    "right": {
                      "type": {
                        "name": "string"
                      },
                      "value": "y"
                    }
                  }
                ]
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Query(
                new FromExpression(expectedEntity),
                [
                    new SelectExpression(
                        new ArrayReturning(
                            new StringArrayReturning(
                                new StringField(expectedEntity, field1)
                            )
                        )
                    ),
                ],
                where: new BooleanArrayReturning(
                    new EachAndOperator(
                        [
                            new BooleanArrayReturning(
                                new EachEquality(
                                    new EachStringEquality(
                                        new StringArrayReturning(
                                            new StringField(expectedEntity, field1)
                                        ),
                                        new StringReturning(new StringScalar("x"))
                                    )
                                )
                            ),
                            new BooleanArrayReturning(
                                new EachEquality(
                                    new EachStringEquality(
                                        new StringArrayReturning(
                                            new StringField(expectedEntity, field2)
                                        ),
                                        new StringReturning(new StringScalar("y"))
                                    )
                                )
                            ),
                        ]
                    )
                ),
                join: null,
                groupBy: null,
                having: null,
                orderBy: null,
                pagination: null
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Realistic "HAVING SUM(field) > 100" clause. Query.Having had no coverage at
    /// the Query level at all before - only the underlying NumberComparison/SumNumber
    /// converters were tested in isolation.
    /// </summary>
    [Fact]
    public void ReadHavingComparisonCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedField = "edrfghiujn";
        const double expectedThreshold = 100;

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
              "having": {
                "operator": "greaterThan",
                "left": {
                  "operator": "sum",
                  "arg": {
                    "entity": "{{expectedEntity}}",
                    "field": "{{expectedField}}",
                    "type": {
                      "name": "number"
                    }
                  }
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 100
                }
              }
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.NotNull(query.Having);
        NumberComparison comparison = query.Having!.AsT4.AsT2;
        Assert.Equal(ComparisonOperator.GreaterThan, comparison.Operator);
        Assert.Equal(expectedThreshold, comparison.Right.AsT1.Value);
    }

    [Fact]
    public void WriteHavingComparisonCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string expectedField = "edrfghiujn";
        const double expectedThreshold = 100;

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
              "having": {
                "operator": "greaterThan",
                "left": {
                  "operator": "sum",
                  "arg": {
                    "entity": "{{expectedEntity}}",
                    "field": "{{expectedField}}",
                    "type": {
                      "name": "number"
                    }
                  }
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 100
                }
              }
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
                join: null,
                groupBy: null,
                orderBy: null,
                pagination: null,
                having: new BooleanReturning(
                    new Comparison(
                        new NumberComparison(
                            ComparisonOperator.GreaterThan,
                            new NumberReturning(
                                new NumberAggregate(
                                    new SumNumber(
                                        new NumberArrayReturning(
                                            new NumberField(expectedEntity, expectedField)
                                        )
                                    )
                                )
                            ),
                            new NumberReturning(new NumberScalar(expectedThreshold))
                        )
                    )
                )
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Query.GroupBy had no round-trip coverage through Query at all before this -
    /// only the underlying Field converters were tested in isolation.
    /// </summary>
    [Fact]
    public void ReadGroupByCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "groupBy": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "string"
                  }
                }
              ]
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.NotNull(query.GroupBy);
        Assert.Equal(
            new StringField(expectedEntity, expectedField),
            query.GroupBy!.Single().AsT7
        );
    }

    [Fact]
    public void WriteGroupByCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "groupBy": [
                {
                  "entity": "{{expectedEntity}}",
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
                join: null,
                groupBy: [new Field(new StringField(expectedEntity, expectedField))],
                having: null,
                orderBy: null,
                pagination: null
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Query.OrderBy had no round-trip coverage through Query at all before this -
    /// only OrderByItemConverter was tested in isolation. Uses "desc" explicitly
    /// since the default ("asc") is omitted on write, so this also exercises the
    /// non-default direction path end-to-end.
    /// </summary>
    [Fact]
    public void ReadOrderByCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "orderBy": [
                {
                  "field": {
                    "entity": "{{expectedEntity}}",
                    "field": "{{expectedField}}",
                    "type": {
                      "name": "string"
                    }
                  },
                  "direction": "desc"
                }
              ]
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.NotNull(query.OrderBy);
        OrderByItem item = query.OrderBy!.Single();
        Assert.Equal(SortDirection.Desc, item.Direction);
        Assert.Equal(new StringField(expectedEntity, expectedField), item.Field.AsT7);
    }

    [Fact]
    public void WriteOrderByCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "orderBy": [
                {
                  "field": {
                    "entity": "{{expectedEntity}}",
                    "field": "{{expectedField}}",
                    "type": {
                      "name": "string"
                    }
                  },
                  "direction": "desc"
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
                join: null,
                groupBy: null,
                having: null,
                orderBy:
                [
                    new OrderByItem(
                        new Field(new StringField(expectedEntity, expectedField)),
                        SortDirection.Desc
                    ),
                ],
                pagination: null
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Query.Pagination had no round-trip coverage through Query at all before this -
    /// only PaginationConverter was tested in isolation.
    /// </summary>
    [Fact]
    public void ReadPaginationCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "pagination": {
                "skip": 10,
                "take": 25
              }
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.NotNull(query.Pagination);
        Assert.Equal(10, query.Pagination!.Skip);
        Assert.Equal(25, query.Pagination!.Take);
    }

    [Fact]
    public void WritePaginationCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "pagination": {
                "skip": 10,
                "take": 25
              }
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
                join: null,
                groupBy: null,
                having: null,
                orderBy: null,
                pagination: new Pagination(10, 25)
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Query.Distinct had no coverage anywhere in QueryConverterTests before this -
    /// not even as an incidental default-value filler.
    /// </summary>
    [Fact]
    public void ReadDistinctCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "distinct": true
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.True(query.Distinct);
    }

    [Fact]
    public void WriteDistinctCase()
    {
        const string expectedEntity = "erfhduibgn";
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
              "distinct": true
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
                join: null,
                groupBy: null,
                having: null,
                orderBy: null,
                pagination: null,
                distinct: true
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    /// <summary>
    /// Every existing joins fixture (ReadJoinsCase/WriteJoinsCase) used exactly one
    /// join. A multi-table query - two joins in the same array - was never
    /// exercised, despite the "joins" property name itself having been the subject
    /// of a real production bug (kudima03/PureQL.CSharp.Model.Serialization#49):
    /// nothing here would catch a regression that drops all but the first element.
    /// </summary>
    [Fact]
    public void ReadMultipleJoinsCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string firstJoinEntity = "refnhbdjusi";
        const string secondJoinEntity = "dfeuionmvbg";
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
                  "entity": "{{firstJoinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                },
                {
                  "type": "left",
                  "entity": "{{secondJoinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
                  }
                }
              ]
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.NotNull(query.Join);
        Assert.Equal(2, query.Join!.Count());
        Assert.True(
            Enumerable
                .Empty<Join>()
                .Append(
                    new Join(
                        JoinType.Inner,
                        firstJoinEntity,
                        new BooleanReturning(new BooleanScalar(true))
                    )
                )
                .Append(
                    new Join(
                        JoinType.Left,
                        secondJoinEntity,
                        new BooleanReturning(new BooleanScalar(false))
                    )
                )
                .SequenceEqual(query.Join!)
        );
    }

    [Fact]
    public void WriteMultipleJoinsCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string firstJoinEntity = "refnhbdjusi";
        const string secondJoinEntity = "dfeuionmvbg";
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
                  "entity": "{{firstJoinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                },
                {
                  "type": "left",
                  "entity": "{{secondJoinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": false
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
                join:
                [
                    new Join(
                        JoinType.Inner,
                        firstJoinEntity,
                        new BooleanReturning(new BooleanScalar(true))
                    ),
                    new Join(
                        JoinType.Left,
                        secondJoinEntity,
                        new BooleanReturning(new BooleanScalar(false))
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

    /// <summary>
    /// A "kitchen sink" query combining every optional clause in one payload -
    /// where, joins, groupBy, having, orderBy, pagination and distinct together.
    /// No existing fixture combines more than two of these, so a property that
    /// accidentally overwrites or shadows another during serialization (e.g. a
    /// JsonIgnore condition or property-ordering bug) would not be caught by the
    /// single-clause tests alone.
    /// </summary>
    [Fact]
    public void ReadFullQueryWithAllClausesCase()
    {
        const string expectedEntity = "erfhduibgn";
        const string joinEntity = "refnhbdjusi";
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
              "distinct": true,
              "where": {
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
                  "value": "x"
                }
              },
              "joins": [
                {
                  "type": "inner",
                  "entity": "{{joinEntity}}",
                  "on": {
                    "type": {
                      "name": "boolean"
                    },
                    "value": true
                  }
                }
              ],
              "groupBy": [
                {
                  "entity": "{{expectedEntity}}",
                  "field": "{{expectedField}}",
                  "type": {
                    "name": "string"
                  }
                }
              ],
              "having": {
                "operator": "greaterThan",
                "left": {
                  "operator": "sum",
                  "arg": {
                    "entity": "{{expectedEntity}}",
                    "field": "{{expectedField}}",
                    "type": {
                      "name": "number"
                    }
                  }
                },
                "right": {
                  "type": {
                    "name": "number"
                  },
                  "value": 1
                }
              },
              "orderBy": [
                {
                  "field": {
                    "entity": "{{expectedEntity}}",
                    "field": "{{expectedField}}",
                    "type": {
                      "name": "string"
                    }
                  },
                  "direction": "desc"
                }
              ],
              "pagination": {
                "skip": 0,
                "take": 20
              }
            }
            """;

        Query query = JsonSerializer.Deserialize<Query>(input, _options)!;

        Assert.True(query.Distinct);
        Assert.True(query.Where!.Value.IsT1);
        Assert.NotNull(query.Having);

        _ = Assert.Single(query.Join!);
        _ = Assert.Single(query.GroupBy!);
        _ = Assert.Single(query.OrderBy!);
        Assert.Equal(0, query.Pagination!.Skip);
        Assert.Equal(20, query.Pagination!.Take);
    }
}
