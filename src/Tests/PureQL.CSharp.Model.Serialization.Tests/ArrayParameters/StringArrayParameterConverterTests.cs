using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayParameters;

public sealed record StringArrayParameterConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringArrayParameterConverterTests()
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
                "name": "stringArray"
              },
              "name": "{{expected}}"
            }
            """;

        StringArrayParameter parameter = JsonSerializer.Deserialize<StringArrayParameter>(
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
                "name": "stringArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new StringArrayParameter("auiheyrdsnf"),
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
                "name": "stringArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringArrayParameter>(input, _options)
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
            JsonSerializer.Deserialize<StringArrayParameter>(input, _options)
        );
    }

    [Theory]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("datetimeArray")]
    [InlineData("numberArray")]
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
            JsonSerializer.Deserialize<StringArrayParameter>(input, _options)
        );
    }
}
