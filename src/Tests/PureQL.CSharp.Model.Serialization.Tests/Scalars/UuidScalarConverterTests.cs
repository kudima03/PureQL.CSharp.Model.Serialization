using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests.Scalars;

public sealed record UuidScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidScalarConverterTests()
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
        Guid expected = Guid.NewGuid();

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expected}}"
            }
            """;

        IUuidScalar scalar = JsonSerializer.Deserialize<IUuidScalar>(input, _options)!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        Guid expected = Guid.NewGuid();

        string expectedJson = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expected}}"
            }
            """;

        string output = JsonSerializer.Serialize<IUuidScalar>(
            new UuidScalar(expected),
            _options
        );

        Assert.Equal(expectedJson, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    [InlineData( /*lang=json,strict*/
            """
            {
              "type": {
                "name": "uuid"
              },
              "value": "afdkjgnhajlkhisfdbng"
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "uuid"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "uuid"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("null")]
    [InlineData("number")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "type": {
                "name": "{{type}}"
              },
              "value": "86246f01-f5be-4925-a699-ed1f988b0a7c"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidScalar>(input, _options)
        );
    }
}
