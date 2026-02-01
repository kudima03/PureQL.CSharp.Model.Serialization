using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayTypes;

public sealed record NumberArrayTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NumberArrayTypeConverterTests()
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
        const string input = /*lang=json,strict*/
            """
            {
              "name": "numberArray"
            }
            """;

        NumberArrayType type = JsonSerializer.Deserialize<NumberArrayType>(
            input,
            _options
        )!;

        Assert.Equal(type.Name, new NumberArrayType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new NumberArrayType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """
            {
              "name": "numberArray"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("uuidArray")]
    [InlineData("booleanArray")]
    [InlineData("dateArray")]
    [InlineData("nullArray")]
    [InlineData("datetimeArray")]
    [InlineData("stringArray")]
    [InlineData("timeArray")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "name": "{{type}}"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberArrayType>(input, _options)
        );
    }
}
