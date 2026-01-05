using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record NullScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NullScalarConverterTests()
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
                "name": "null"
              }
            }
            """;

        INullScalar scalar = JsonSerializer.Deserialize<INullScalar>(input, _options)!;

        Assert.Equal(new NullScalar(), scalar);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "null"
              }
            }
            """;

        string output = JsonSerializer.Serialize<INullScalar>(new NullScalar(), _options);

        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INullScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INullScalar>(input, _options)
        );
    }
}
