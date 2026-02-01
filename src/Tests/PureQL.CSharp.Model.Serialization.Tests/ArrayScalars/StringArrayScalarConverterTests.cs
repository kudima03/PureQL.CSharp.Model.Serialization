using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record StringArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringArrayScalarConverterTests()
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
    public void Read()
    {
        string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "stringArray"
              },
              "value": ["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"]
            }
            """;

        IStringArrayScalar scalar = JsonSerializer.Deserialize<IStringArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"], scalar.Value);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "stringArray"
              },
              "value": ["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"]
            }
            """;

        string output = JsonSerializer.Serialize<IStringArrayScalar>(
            new StringArrayScalar(["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"]),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
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
            JsonSerializer.Deserialize<IStringArrayScalar>(input, _options)
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
            JsonSerializer.Deserialize<IStringArrayScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("datetimeArray")]
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
              "value": ["ianhuedrfiuhaerfd", "sdkfnjilhnsjkd"]
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IStringArrayScalar>(input, _options)
        );
    }
}
