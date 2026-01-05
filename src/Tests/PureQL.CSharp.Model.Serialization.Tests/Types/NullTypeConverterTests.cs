using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Types;

public sealed record NullTypeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NullTypeConverterTests()
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
              "name": "null"
            }
            """;

        NullType type = JsonSerializer.Deserialize<NullType>(input, _options)!;

        Assert.Equal(type.Name, new NullType().Name);
    }

    [Fact]
    public void Write()
    {
        string output = JsonSerializer.Serialize(new NullType(), _options);

        Assert.Equal( /*lang=json,strict*/
            """
            {
              "name": "null"
            }
            """,
            output
        );
    }

    [Theory]
    [InlineData("uuid")]
    [InlineData("boolean")]
    [InlineData("date")]
    [InlineData("number")]
    [InlineData("datetime")]
    [InlineData("string")]
    [InlineData("time")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongType(string type)
    {
        string input = $$"""
            {
              "name": "{{type}}"
            }
            """;
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NullType>(input, _options)
        );
    }
}
