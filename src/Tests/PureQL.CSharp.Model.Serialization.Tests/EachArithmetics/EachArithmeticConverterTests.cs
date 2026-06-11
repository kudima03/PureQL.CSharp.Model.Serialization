using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachArithmetics;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.EachArithmetics;

public sealed record EachArithmeticConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EachArithmeticConverterTests()
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
              "values": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAdd>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorNameForAdd()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachSubtract",
              "values": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAdd>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorNameForSubtract()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachSubtract>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorNameForMultiply()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachMultiply>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnWrongOperatorNameForDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachDivide>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOperatorName()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "badOperator",
              "values": []
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAdd>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedValues()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAdd>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullValues()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachAdd>(input, _options)
        );
    }

    [Fact]
    public void ReadAddEmpty()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        EachAdd value = JsonSerializer.Deserialize<EachAdd>(input, _options)!;
        Assert.Empty(value.Values);
    }

    [Fact]
    public void WriteAddEmpty()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(new EachAdd([]), _options);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadSubtractEmpty()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachSubtract",
              "values": []
            }
            """;

        EachSubtract value = JsonSerializer.Deserialize<EachSubtract>(input, _options)!;
        Assert.Empty(value.Values);
    }

    [Fact]
    public void WriteSubtractEmpty()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachSubtract",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(new EachSubtract([]), _options);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadMultiplyEmpty()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachMultiply",
              "values": []
            }
            """;

        EachMultiply value = JsonSerializer.Deserialize<EachMultiply>(input, _options)!;
        Assert.Empty(value.Values);
    }

    [Fact]
    public void WriteMultiplyEmpty()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachMultiply",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(new EachMultiply([]), _options);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadDivideEmpty()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachDivide",
              "values": []
            }
            """;

        EachDivide value = JsonSerializer.Deserialize<EachDivide>(input, _options)!;
        Assert.Empty(value.Values);
    }

    [Fact]
    public void WriteDivideEmpty()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachDivide",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(new EachDivide([]), _options);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadAddWithScalarValues()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachAdd",
              "values": [
                {
                  "entity": "{{entity}}",
                  "field": "{{field}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "type": {
                    "name": "number"
                  },
                  "value": 5
                }
              ]
            }
            """;

        EachAdd value = JsonSerializer.Deserialize<EachAdd>(input, _options)!;
        OneOf<NumberReturning, NumberArrayReturning>[] values = [.. value.Values];
        Assert.Equal(new NumberField(entity, field), values[0].AsT1.AsT1);
        Assert.Equal(5, values[1].AsT0.AsT1.Value);
    }

    [Fact]
    public void WriteAddWithScalarValues()
    {
        const string entity = "myEntity";
        const string field = "myField";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "operator": "eachAdd",
              "values": [
                {
                  "entity": "{{entity}}",
                  "field": "{{field}}",
                  "type": {
                    "name": "number"
                  }
                },
                {
                  "type": {
                    "name": "number"
                  },
                  "value": 5
                }
              ]
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachAdd([
                new NumberArrayReturning(new NumberField(entity, field)),
                new NumberReturning(new NumberScalar(5)),
            ]),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachArithmeticAsAdd()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        EachArithmetic value = JsonSerializer.Deserialize<EachArithmetic>(
            input,
            _options
        )!;
        Assert.True(value.IsT0);
    }

    [Fact]
    public void WriteEachArithmeticAsAdd()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachAdd",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachArithmetic(new EachAdd([])),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachArithmeticAsSubtract()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachSubtract",
              "values": []
            }
            """;

        EachArithmetic value = JsonSerializer.Deserialize<EachArithmetic>(
            input,
            _options
        )!;
        Assert.True(value.IsT1);
    }

    [Fact]
    public void WriteEachArithmeticAsSubtract()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachSubtract",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachArithmetic(new EachSubtract([])),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachArithmeticAsMultiply()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachMultiply",
              "values": []
            }
            """;

        EachArithmetic value = JsonSerializer.Deserialize<EachArithmetic>(
            input,
            _options
        )!;
        Assert.True(value.IsT2);
    }

    [Fact]
    public void WriteEachArithmeticAsMultiply()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachMultiply",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachArithmetic(new EachMultiply([])),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadEachArithmeticAsDivide()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "operator": "eachDivide",
              "values": []
            }
            """;

        EachArithmetic value = JsonSerializer.Deserialize<EachArithmetic>(
            input,
            _options
        )!;
        Assert.True(value.IsT3);
    }

    [Fact]
    public void WriteEachArithmeticAsDivide()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "operator": "eachDivide",
              "values": []
            }
            """;

        string output = JsonSerializer.Serialize(
            new EachArithmetic(new EachDivide([])),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnEachArithmeticBadJson()
    {
        const string input = "{}";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<EachArithmetic>(input, _options)
        );
    }
}
