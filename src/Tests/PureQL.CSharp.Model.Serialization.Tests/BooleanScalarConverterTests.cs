using System.Text.Json;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record BooleanScalarConverterTests
{
    [Fact]
    public void Read()
    {
        const string input =
            /*lang=json,strict*/
            """
                {
                  "type": {
                    "name": "boolean"
                  },
                  "value": true
                }
                """;

        BooleanScalar scalar = JsonSerializer.Deserialize<BooleanScalar>(input, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new BooleanScalarConverter() },
        })!;

        Assert.True(scalar.Value);
    }

    [Fact]
    public void Write()
    {
        const string expected =
            /*lang=json,strict*/
            """{"type":{"name":"boolean"},"value":true}""";

        var output = JsonSerializer.Serialize(new BooleanScalar(true), new JsonSerializerOptions()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new BooleanScalarConverter() },
        })!;

        Assert.Equal(expected, output);
    }
}
