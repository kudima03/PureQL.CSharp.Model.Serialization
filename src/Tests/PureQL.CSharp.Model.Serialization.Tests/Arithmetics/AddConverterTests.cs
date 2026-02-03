using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Arithmetics;

public sealed record AddConverterTests
{
    private readonly JsonSerializerOptions _options;

    public AddConverterTests()
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
        const string input = /*lang=json,strict*/
            """
            {
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOtherOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "subtract",
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "bduasfehrygcvdubfsahycv",
              "arguments": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedArguments()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullArguments()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnArgumentsWrongType()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void ReadEmptyArguments()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        Add value = JsonSerializer.Deserialize<Add>(input, _options)!;
        Assert.Empty(value.Arguments);
    }

    [Fact]
    public void WriteEmptyArguments()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "add",
              "arguments": []
            }
            """;

        string value = JsonSerializer.Serialize(new Add([]), _options);
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadScalarArguments()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue1.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue2.ToString(CultureInfo.InvariantCulture)}}
                }
              ]
            }
            """;

        Add value = JsonSerializer.Deserialize<Add>(input, _options)!;
        Assert.Equal(value.Arguments.First().AsT1, new NumberScalar(expectedValue1));
        Assert.Equal(value.Arguments.Last().AsT1, new NumberScalar(expectedValue2));
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("dateArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("booleanArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    public void ThrowsExceptionOnWrongScalarType(string type)
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{expectedValue1.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "type": {
                    "name": "{{type}}"
                  },
                  "value": {{expectedValue2.ToString(CultureInfo.InvariantCulture)}}
                }
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void WriteScalarArguments()
    {
        double expectedValue1 = Random.Shared.NextDouble();
        double expectedValue2 = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue1.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue2.ToString(CultureInfo.InvariantCulture)}}
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Add(
                [
                    new NumberReturning(new NumberScalar(expectedValue1)),
                    new NumberReturning(new NumberScalar(expectedValue2)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadParameterArguments()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        Add value = JsonSerializer.Deserialize<Add>(input, _options)!;
        Assert.Equal(
            value.Arguments.First().AsT0,
            new NumberParameter(expectedFirstParamName)
        );
        Assert.Equal(
            value.Arguments.Last().AsT0,
            new NumberParameter(expectedSecondParamName)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("boolean")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("dateArray")]
    [InlineData("datetimeArray")]
    [InlineData("nullArray")]
    [InlineData("booleanArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    public void ThrowsExceptionOnWrongParameterType(string type)
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "{{type}}"
                  }
                }
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Add>(input, _options)
        );
    }

    [Fact]
    public void WriteParameterArguments()
    {
        const string expectedFirstParamName = "ashjlbd";
        const string expectedSecondParamName = "erafuhyobdng";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "name": "{{expectedFirstParamName}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "name": "{{expectedSecondParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Add(
                [
                    new NumberReturning(new NumberParameter(expectedFirstParamName)),
                    new NumberReturning(new NumberParameter(expectedSecondParamName)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadMixedArguments()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedParamName = "ashjlbd";

        string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        Add value = JsonSerializer.Deserialize<Add>(input, _options)!;
        Assert.Equal(new NumberScalar(expectedValue), value.Arguments.First().AsT1);
        Assert.Equal(
            value.Arguments.Skip(1).First().AsT0,
            new NumberParameter(expectedParamName)
        );
    }

    [Fact]
    public void WriteMixedConditions()
    {
        double expectedValue = Random.Shared.NextDouble();

        const string expectedParamName = "ashjlbd";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "add",
              "arguments": [
                {
                  "type": {
                    "name": "number"
                  },
                  "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
                },
                {
                  "name": "{{expectedParamName}}",
                  "type": {
                    "name": "number"
                  }
                }
              ]
            }
            """;

        string value = JsonSerializer.Serialize(
            new Add(
                [
                    new NumberReturning(new NumberScalar(expectedValue)),
                    new NumberReturning(new NumberParameter(expectedParamName)),
                ]
            ),
            _options
        );
        Assert.Equal(expected, value);
    }
}
