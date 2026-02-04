using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Aggregates.Numeric;

public sealed record MinNumberConverterTests
{
    private readonly JsonSerializerOptions _options;

    public MinNumberConverterTests()
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
                    "name": "numberArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
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
              "operator": "average_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
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
                    "name": "numberArray"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_number"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArgument()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_number",
              "arg": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "min_number",
              "arg": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
        );
    }

    [Fact]
    public void ReadScalarArgument()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        MinNumber value = JsonSerializer.Deserialize<MinNumber>(input, _options)!;
        Assert.Equal(values, value.Argument.AsT2.Value);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("refhyuabogs")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "type": {
                  "name": "{{type}}"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArgument()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        IEnumerable<string> formattedValues = values.Select(x =>
            x.ToString(CultureInfo.InvariantCulture)
        );

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "min_number",
              "arg": {
                "type": {
                  "name": "numberArray"
                },
                "value": [
                  {{formattedValues.First()}},
                  {{formattedValues.Skip(1).First()}},
                  {{formattedValues.Skip(2).First()}}
                ]
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new MinNumber(new NumberArrayReturning(new NumberArrayScalar(values))),
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
              "operator": "min_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        MinNumber value = JsonSerializer.Deserialize<MinNumber>(input, _options)!;
        Assert.Equal(new NumberArrayParameter(expectedParamName), value.Argument.AsT0);
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
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
              "operator": "min_number",
              "arg": {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<MinNumber>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArgument()
    {
        const string expectedParamName = "ashjlbd";

        const string expected = $$"""
            {
              "operator": "min_number",
              "arg": {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new MinNumber(
                new NumberArrayReturning(new NumberArrayParameter(expectedParamName))
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
              "operator": "min_number",
              "arg": {
                  "entity": "{{expectedEntityName}}",
                  "field": "{{expectedFieldName}}",
                  "type": {
                    "name": "numberArray"
                  }
                }
            }
            """;

        MinNumber value = JsonSerializer.Deserialize<MinNumber>(input, _options)!;
        Assert.Equal(
            new NumberField(expectedEntityName, expectedFieldName),
            value.Argument.AsT1
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
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
              "operator": "min_number",
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
            JsonSerializer.Deserialize<MinNumber>(input, _options)
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
              "operator": "min_number",
              "arg": {
                "entity": "{{expectedEntityName}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "numberArray"
                }
              }
            }
            """;

        string value = JsonSerializer.Serialize(
            new MinNumber(
                new NumberArrayReturning(
                    new NumberField(expectedEntityName, expectedFieldName)
                )
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
