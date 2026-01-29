using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Date;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Date;

public sealed record AverageDateConverterTests
{
    private readonly JsonSerializerOptions _options;

    public AverageDateConverterTests()
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
    public void ThrowsExceptionOnOperatorNameAbsence()
    {
        const string expectedEntityName = "aruhybfe";
        const string expectedFieldName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
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
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
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

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
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
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_date"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_date",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "average_date",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgument()
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

        AverageDate value = JsonSerializer.Deserialize<AverageDate>(input, _options)!;
        Assert.Equal(new DateArrayScalar(expectedDates), value.Argument.AsT2);
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
    public void ThrowsExceptionOnWrongScalarType(string type)
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
                "name": "{{type}}"
              },
              "value": ["{{formattedDates.First()}}", "{{formattedDates.Skip(
                1
            ).First()}}", "{{formattedDates.Skip(2).First()}}"]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgument()
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
            new AverageDate(new DateArrayReturning(new DateArrayScalar(expectedDates))),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArgument()
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

        AverageDate value = JsonSerializer.Deserialize<AverageDate>(input, _options)!;
        Assert.Equal(value.Argument.AsT0, new DateArrayParameter(expectedParamName));
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
    public void ThrowsExceptionOnWrongParameterType(string type)
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
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgument()
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
            new AverageDate(
                new DateArrayReturning(new DateArrayParameter(expectedParamName))
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadFieldArgument()
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

        AverageDate value = JsonSerializer.Deserialize<AverageDate>(input, _options)!;
        Assert.Equal(
            value.Argument.AsT1,
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
    public void ThrowsExceptionOnWrongFieldType(string type)
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
            JsonSerializer.Deserialize<AverageDate>(input, _options)
        );
    }

    [Fact]
    public void WriteFieldArgument()
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
            new AverageDate(
                new DateArrayReturning(
                    new DateField(expectedEntityName, expectedFieldName)
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
