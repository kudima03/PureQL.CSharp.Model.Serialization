using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayParameters;

public sealed record NumberArrayParameterConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberArrayParameterConverterTests()
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
    public void ReadName()
    {
        const string expected = "iurhgndfjsb";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "name": "{{expected}}"
            }
            """;

        NumberArrayParameter parameter = JsonSerializer.Deserialize<NumberArrayParameter>(
            input,
            _options
        )!;

        Assert.Equal(expected, parameter.Name);
    }

    [Fact]
    public void Write()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "name": "auiheyrdsnf",
              "type": {
                "name": "numberArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberArrayParameter("auiheyrdsnf"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingNameField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "numberArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberArrayParameter>(input, _options)
        );
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberArrayParameter>(input, _options)
        );
    }

    [Theory]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("nullArray")]
    [InlineData("timeArray")]
    [InlineData("uuidArray")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "name": "auiheyrdsnf"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberArrayParameter>(input, _options)
        );
    }
}
