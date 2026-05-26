using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record NullFieldConverterTests
{
    private readonly JsonSerializerOptions _options;

    public NullFieldConverterTests()
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
        const string expectedEntity = "ahbudnfs";
        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "nullArray"
              }
            }
            """;

        NullField value = JsonSerializer.Deserialize<NullField>(input, _options)!;
        Assert.Equal(new NullField(expectedEntity, expectedField), value);
    }

    [Fact]
    public void Write()
    {
        const string expectedEntity = "ahbudnfs";
        const string expectedField = "arfeinjuhg";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "nullArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NullField(expectedEntity, expectedField),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNullFieldWrappedInField()
    {
        const string expectedEntity = "ahbudnfs";
        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "nullArray"
              }
            }
            """;

        Field value = JsonSerializer.Deserialize<Field>(input, _options)!;
        Assert.Equal(new NullField(expectedEntity, expectedField), value.AsT3);
    }

    [Fact]
    public void WriteFieldWrappedNullField()
    {
        const string expectedEntity = "ahbudnfs";
        const string expectedField = "arfeinjuhg";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "nullArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Field(new NullField(expectedEntity, expectedField)),
            _options
        );
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
            JsonSerializer.Deserialize<NullField>(input, _options)
        );
    }
}
