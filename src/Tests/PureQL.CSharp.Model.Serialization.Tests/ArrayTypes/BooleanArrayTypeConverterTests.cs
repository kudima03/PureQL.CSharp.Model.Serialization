using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayTypes;

public sealed record BooleanArrayTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public BooleanArrayTypeConverterTests()
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
              "name": "booleanArray"
            }
            """;

        BooleanArrayType type = JsonSerializer.Deserialize<BooleanArrayType>(
            input,
            _options
        )!;

        Assert.Equal(type.Name, new BooleanArrayType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new BooleanArrayType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """
            {
              "name": "booleanArray"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("uuidArray")]
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
              "name": "{{type}}"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanArrayType>(input, _options)
        );
    }
}
