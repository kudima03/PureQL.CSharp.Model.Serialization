using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Date;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Date;

public sealed record DateAggregateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateAggregateConverterTests()
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
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "euhwyrfdbuyeghrfdb",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnAverageDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnAverageDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnAverageDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnAverageDate()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string input = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateArrayScalar(expectedDates), value.AsT2.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnAverageDate(string type)
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );
        string input = /*lang=json,strict*/
        $$"""
            {
              "operator": "average_date",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnAverageDate()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string expected = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new AverageDate(
                    new DateArrayReturning(new DateArrayScalar(expectedDates))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnAverageDate()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(value.AsT2.Argument.AsT0, new DateArrayParameter(expectedParamName));
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnAverageDate(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnAverageDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "average_date",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new AverageDate(
                    new DateArrayReturning(new DateArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnAverageDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(
            value.AsT2.Argument.AsT1,
            new DateField(expectedEntityName, expectedFieldName)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnAverageDate(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnAverageDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "average_date",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new AverageDate(
                    new DateArrayReturning(
                        new DateField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMaxDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMaxDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMaxDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "max_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMaxDate()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string input = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateArrayScalar(expectedDates), value.AsT0.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMaxDate(string type)
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );
        string input = /*lang=json,strict*/
        $$"""
            {
              "operator": "max_date",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMaxDate()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string expected = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MaxDate(new DateArrayReturning(new DateArrayScalar(expectedDates)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMaxDate()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateArrayParameter(expectedParamName), value.AsT0.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMaxDate(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMaxDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "max_date",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MaxDate(
                    new DateArrayReturning(new DateArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMaxDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(
            value.AsT0.Argument.AsT1,
            new DateField(expectedEntityName, expectedFieldName)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnMaxDate(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMaxDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "max_date",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MaxDate(
                    new DateArrayReturning(
                        new DateField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgumentOnMinDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgumentOnMinDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongTypeOnMinDate()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgumentOnMinDate()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string input = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateArrayScalar(expectedDates), value.AsT1.Argument.AsT2);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarTypeOnMinDate(string type)
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );
        string input = /*lang=json,strict*/
        $$"""
            {
              "operator": "min_date",
              "arg": {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgumentOnMinDate()
    {
        IEnumerable<DateOnly> expectedDates =
        [
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
        ];

        IEnumerable<string> formattedDates = expectedDates.Select(x =>
            x.ToString("yyyy-MM-dd")
        );

        string expected = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MinDate(new DateArrayReturning(new DateArrayScalar(expectedDates)))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgumentOnMinDate()
    {
        const string expectedParamName = "ashjlbd";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(new DateArrayParameter(expectedParamName), value.AsT1.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongParameterTypeOnMinDate(string type)
    {
        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgumentOnMinDate()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_date",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MinDate(
                    new DateArrayReturning(new DateArrayParameter(expectedParamName))
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgumentOnMinDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "dateArray"
                  }
                }
            }
            """;

        DateAggregate value = JsonSerializer.Deserialize<DateAggregate>(input, _options)!;
        Assert.Equal(
            value.AsT1.Argument.AsT1,
            new DateField(expectedEntityName, expectedFieldName)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongFieldTypeOnMinDate(string type)
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateAggregate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgumentOnMinDate()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_date",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "dateArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new DateAggregate(
                new MinDate(
                    new DateArrayReturning(
                        new DateField(expectedEntityName, expectedFieldName)
                    )
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
