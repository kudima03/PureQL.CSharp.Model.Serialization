using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record SelectExpressionConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SelectExpressionConverterTests()
    {
        _options = new JsonSerializerOptions()
        {
            NewLine = "\n",
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Encoder = System
                .Text
                .Encodings
                .Web
                .JavaScriptEncoder
                .UnsafeRelaxedJsonEscaping,
        };
        foreach (JsonConverter converter in new PureQLConverters())
        {
            _options.Converters.Add(converter);
        }
    }

    [Fact]
    public void ReadBooleanField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "boolean"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        BooleanField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT0.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new BooleanType(), field.Type);
    }

    [Fact]
    public void WriteBooleanField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new BooleanReturning(new BooleanField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "boolean"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
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
            .Deserialize<SelectExpression>(input, _options)!
            .AsT0.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new BooleanType(), parameter.Type);
    }

    [Fact]
    public void WriteBooleanParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new BooleanReturning(new BooleanParameter(expectedParamName))
            ),
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
            .Deserialize<SelectExpression>(input, _options)!
            .AsT0.AsT2;

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
            new SelectExpression(new BooleanReturning(new BooleanScalar(true))),
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
            .Deserialize<SelectExpression>(input, _options)!
            .AsT0.AsT3.AsT0;

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

        SelectExpression booleanReturning = JsonSerializer.Deserialize<SelectExpression>(
            input,
            _options
        )!;

        AndOperator andOperator = booleanReturning.AsT0.AsT4.AsT0;

        Assert.Equal(
            new BooleanField("u", "active"),
            andOperator.Conditions.First().AsT0
        );
        Assert.Equal(new BooleanScalar(true), andOperator.Conditions.Last().AsT2);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Theory]
    [InlineData("")]
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
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("datetime")]
    [InlineData("date")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ReadAlias(string typeName)
    {
        const string expectedEntity = "sadiJUNFH";
        const string expectedField = "dfijng";
        const string expectedAlias = "regtnhjlnijlhregft";

        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "alias": "{{expectedAlias}}"
            }
            """;

        SelectExpression value = JsonSerializer.Deserialize<SelectExpression>(
            input,
            _options
        )!;

        Assert.Equal(expectedAlias, value.Alias);
    }

    [Fact]
    public void WriteAlias()
    {
        const string expectedEntity = "sadiJUNFH";
        const string expectedField = "dfijng";
        const string expectedAlias = "regtnhjlnijlhregft";

        string expected = $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "boolean"
              },
              "alias": "{{expectedAlias}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new BooleanReturning(new BooleanField(expectedEntity, expectedField)),
                expectedAlias
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("datetime")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongBooleanScalarType(string typeName)
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
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Fact]
    public void ReadDateField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        DateField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT3.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateType(), field.Type);
    }

    [Fact]
    public void WriteDateField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new DateReturning(new DateField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "date"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "name": "{{paramName}}"
            }
            """;

        DateParameter parameter = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT3.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateType(), parameter.Type);
    }

    [Fact]
    public void WriteDateParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(new DateReturning(new DateParameter(expectedParamName))),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "date"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateScalar()
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{DateTime.Now:yyyy-MM-dd}}"
            }
            """;

        DateScalar scalar = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT3.AsT2;

        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), scalar.Value);
    }

    [Fact]
    public void WriteDateScalar()
    {
        string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "date"
              },
              "value": "{{DateTime.Now:yyyy-MM-dd}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new DateReturning(new DateScalar(DateOnly.FromDateTime(DateTime.Now)))
            ),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongDateScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "2000-01-01"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Fact]
    public void ReadDateTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        DateTimeField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT5.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateTimeType(), field.Type);
    }

    [Fact]
    public void WriteDateTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new DateTimeReturning(new DateTimeField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "datetime"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateTimeParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "name": "{{paramName}}"
            }
            """;

        DateTimeParameter parameter = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT5.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new DateTimeType(), parameter.Type);
    }

    [Fact]
    public void WriteDateTimeParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new DateTimeReturning(new DateTimeParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "datetime"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateTimeScalar()
    {
        DateTime expectedValue = DateTime.Now;

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;

        DateTimeScalar scalar = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT5.AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteDateTimeScalar()
    {
        DateTime expectedValue = DateTime.Now;

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetime"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new DateTimeReturning(new DateTimeScalar(expectedValue))
            ),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongDateTimeScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "2025-12-24T15:20:36.6778291+03:00"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Fact]
    public void ReadNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        NumberField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT1.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new NumberType(), field.Type);
    }

    [Fact]
    public void WriteNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new NumberReturning(new NumberField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "number"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
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

        NumberParameter parameter = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT1.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new NumberType(), parameter.Type);
    }

    [Fact]
    public void WriteNumberParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new NumberReturning(new NumberParameter(expectedParamName))
            ),
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
            .Deserialize<SelectExpression>(input, _options)!
            .AsT1.AsT2;

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
            new SelectExpression(new NumberReturning(new NumberScalar(expectedValue))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongNumberScalarType(string typeName)
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
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Fact]
    public void ReadStringField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "string"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        StringField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT2.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new StringType(), field.Type);
    }

    [Fact]
    public void WriteStringField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new StringReturning(new StringField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "string"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "string"
              },
              "name": "{{paramName}}"
            }
            """;

        StringParameter parameter = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT2.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new StringType(), parameter.Type);
    }

    [Fact]
    public void WriteStringParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new StringReturning(new StringParameter(expectedParamName))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "string"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringScalar()
    {
        const string expectedValue = "fbdhidflefbhbfdhvjz";

        const string input = $$"""
            {
              "type": {
                "name": "string"
              },
              "value": "{{expectedValue}}"
            }
            """;
        StringScalar scalar = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT2.AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteStringScalar()
    {
        const string expectedValue = "fbdhidflefbhbfdhvjz";

        const string expected = $$"""
            {
              "type": {
                "name": "string"
              },
              "value": "{{expectedValue}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SelectExpression(new StringReturning(new StringScalar(expectedValue))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongStringScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "hbgfrtdvsdhcif"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Fact]
    public void ReadTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        TimeField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT4.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new TimeType(), field.Type);
    }

    [Fact]
    public void WriteTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new TimeReturning(new TimeField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "time"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "time"
              },
              "name": "{{paramName}}"
            }
            """;

        TimeParameter parameter = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT4.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new TimeType(), parameter.Type);
    }

    [Fact]
    public void WriteTimeParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(new TimeReturning(new TimeParameter(expectedParamName))),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "time"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeScalar()
    {
        TimeOnly expectedValue = TimeOnly.FromDateTime(DateTime.Now);

        string input = $$"""
            {
              "type": {
                "name": "time"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;
        TimeScalar scalar = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT4.AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteTimeScalar()
    {
        TimeOnly expectedValue = TimeOnly.FromDateTime(DateTime.Now);

        string expected = $$"""
            {
              "type": {
                "name": "time"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new SelectExpression(new TimeReturning(new TimeScalar(expectedValue))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("string")]
    [InlineData("uuid")]
    public void ThrowsExceptionOnWrongTimeScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "{{JsonSerializer.Serialize(
                TimeOnly.FromDateTime(DateTime.Now),
                _options
            )}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }

    [Fact]
    public void ReadUuidField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        UuidField field = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT6.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new UuidType(), field.Type);
    }

    [Fact]
    public void WriteUuidField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new SelectExpression(
                new UuidReturning(new UuidField(expectedEntity, expectedField))
            ),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "uuid"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "name": "{{paramName}}"
            }
            """;

        UuidParameter parameter = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT6.AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new UuidType(), parameter.Type);
    }

    [Fact]
    public void WriteUuidParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new SelectExpression(new UuidReturning(new UuidParameter(expectedParamName))),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "uuid"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidScalar()
    {
        Guid expectedValue = Guid.NewGuid();

        string input = $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expectedValue}}"
            }
            """;
        UuidScalar scalar = JsonSerializer
            .Deserialize<SelectExpression>(input, _options)!
            .AsT6.AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteUuidScalar()
    {
        Guid expectedValue = Guid.NewGuid();

        string expected = $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expectedValue}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new SelectExpression(new UuidReturning(new UuidScalar(expectedValue))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("date")]
    [InlineData("boolean")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("number")]
    [InlineData("time")]
    public void ThrowsExceptionOnWrongUuidScalarType(string typeName)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{typeName}}"
              },
              "value": "{{Guid.CreateVersion7()}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SelectExpression>(input, _options)
        );
    }
}
