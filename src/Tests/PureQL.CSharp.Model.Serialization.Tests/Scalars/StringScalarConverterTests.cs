using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record StringScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringScalarConverterTests()
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
                "name": "string"
              },
              "value": "ianhuedrfiuhaerfd"
            }
            """;

        IStringScalar scalar = JsonSerializer.Deserialize<IStringScalar>(
            input,
            _options
        )!;

        Assert.Equal("ianhuedrfiuhaerfd", scalar.Value);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "string"
              },
              "value": "adsihuowbfohuasdfipsduF"
            }
            """;

        string output = JsonSerializer.Serialize<IStringScalar>(
            new StringScalar("adsihuowbfohuasdfipsduF"),
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
                "name": "string"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IStringScalar>(input, _options)
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
            JsonSerializer.Deserialize<IStringScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("datetime")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "value": "faijdhnjikabngf"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IStringScalar>(input, _options)
        );
    }
}
