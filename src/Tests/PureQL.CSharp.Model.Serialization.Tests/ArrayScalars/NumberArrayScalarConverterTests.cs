using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayScalars;

public sealed record NumberArrayScalarConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberArrayScalarConverterTests()
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
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "value": [{{values.First()}}, {{values.Skip(1).First()}}, {{values.Skip(
                2
            ).First()}}]
            }
            """;

        INumberArrayScalar scalar = JsonSerializer.Deserialize<INumberArrayScalar>(
            input,
            _options
        )!;

        Assert.Equal(values, scalar.Value);
    }

    [Fact]
    public void Write()
    {
        IEnumerable<double> values =
        [
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
            Random.Shared.NextDouble(),
        ];

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "value": [{{values.First()}}, {{values.Skip(1).First()}}, {{values.Skip(
                2
            ).First()}}]
            }
            """;

        string output = JsonSerializer.Serialize<INumberArrayScalar>(
            new NumberArrayScalar(values),
            _options
        );

        Assert.Equal(expected, output);
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
                "name": "numberArray"
              },
              "value": "35.16.10000"
            }
            """
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyValue()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "type": {
                "name": "numberArray"
              },
              "value": ""
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberArrayScalar>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingValueField()
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
            JsonSerializer.Deserialize<INumberArrayScalar>(input, _options)
        );
    }

    [Theory]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("stringArray")]
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
              "value": 10.12345
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<INumberArrayScalar>(input, _options)
        );
    }
}
