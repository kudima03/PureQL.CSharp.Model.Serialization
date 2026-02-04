using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record NullArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NullArrayScalarConverterTests()
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
                "name": "nullArray"
              },
              "value": [
                null,
                null,
                null
              ]
            }
            """;

        INullArrayScalar scalar = JsonSerializer.Deserialize<INullArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(new NullArrayScalar(3), scalar);
    }

    [Fact]
    public void Write()
    {
        string expected = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "nullArray"
              },
              "value": [
                null,
                null,
                null
              ]
            }
            """;

        string output = JsonSerializer.Serialize<INullArrayScalar>(
            new NullArrayScalar(3),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnNotNull()
    {
        string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "nullArray"
              },
              "value": [
                null,
                10,
                "dgefrknij"
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INullArrayScalar>(input, _options)!
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
            JsonSerializer.Deserialize<INullArrayScalar>(input, _options)
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
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("numberArray")]
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
              "value": [
                null,
                null,
                null
              ]
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INullArrayScalar>(input, _options)
        );
    }
}
