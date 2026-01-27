using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.Tests.ArrayTypes;

public sealed record DateTimeArrayTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeArrayTypeConverterTests()
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
              "name": "datetimeArray"
            }
            """;

        DateTimeArrayType type = JsonSerializer.Deserialize<DateTimeArrayType>(
            input,
            _options
        )!;

        Assert.Equal(type.Name, new DateTimeArrayType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new DateTimeArrayType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """
            {
              "name": "datetimeArray"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("uuidArray")]
    [InlineData("booleanArray")]
    [InlineData("nullArray")]
    [InlineData("numberArray")]
    [InlineData("dateArray")]
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
            JsonSerializer.Deserialize<DateTimeArrayType>(input, _options)
        );
    }
}
