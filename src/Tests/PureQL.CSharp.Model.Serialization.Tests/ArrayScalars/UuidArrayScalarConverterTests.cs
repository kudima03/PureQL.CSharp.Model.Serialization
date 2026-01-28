using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record UuidArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidArrayScalarConverterTests()
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
        IEnumerable<Guid> expected = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        string input = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "value": ["{{expected.First()}}", "{{expected.Skip(
                1
            ).First()}}", "{{expected.Skip(2).First()}}"]
            }
            """;

        IUuidArrayScalar scalar = JsonSerializer.Deserialize<IUuidArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(expected, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        IEnumerable<Guid> expected = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        string expectedJson = /*lang=json,strict*/
        $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "value": ["{{expected.First()}}", "{{expected.Skip(
                1
            ).First()}}", "{{expected.Skip(2).First()}}"]
            }
            """;

        string output = JsonSerializer.Serialize<IUuidArrayScalar>(
            new UuidArrayScalar(expected),
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
                "name": "uuidArray"
              },
              "value": ["afdkjgnhajlkhisfdbng"]
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "uuidArray"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "uuidArray"
              }
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<IUuidArrayScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
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
            JsonSerializer.Deserialize<IUuidArrayScalar>(input, _options)
        );
    }
}
