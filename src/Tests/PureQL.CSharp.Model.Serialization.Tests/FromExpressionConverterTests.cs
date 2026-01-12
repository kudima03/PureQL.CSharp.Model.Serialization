using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record FromExpressionConverterTests
{
    private readonly JsonSerializerOptions _options;

    public FromExpressionConverterTests()
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
        const string expectedEntity = "refnhbdjusi";
        const string expectedAlias = "earfjnik";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "alias": "{{expectedAlias}}"
            }
            """;

        FromExpression value = JsonSerializer.Deserialize<FromExpression>(
            input,
            _options
        )!;
        Assert.Equal(new FromExpression(expectedEntity, expectedAlias), value);
    }

    [Fact]
    public void ReadEmptyFields()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "entity": "",
              "alias": ""
            }
            """;

        FromExpression value = JsonSerializer.Deserialize<FromExpression>(
            input,
            _options
        )!;
        Assert.Equal(new FromExpression(string.Empty, string.Empty), value);
    }

    [Fact]
    public void Write()
    {
        const string expectedEntity = "refnhbdjusi";
        const string expectedAlias = "earfjnik";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "alias": "{{expectedAlias}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new FromExpression(expectedEntity, expectedAlias),
            _options
        )!;
        Assert.Equal(expected, output);
    }

    [Fact]
    public void WriteEmptyFields()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {
                  "entity": "",
                  "alias": ""
                }
                """;

        string output = JsonSerializer.Serialize(
            new FromExpression(string.Empty, string.Empty),
            _options
        )!;
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnEmptyJson()
    {
        const string input = "{}";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyString()
    {
        const string input = "";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedEntity()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "alias": "refjhniu"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEntityWrongType()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": {},
              "alias": "sfdnji"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedAlias()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": "refjhniu"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullAlias()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": "dfahnjib",
              "alias": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnAliasWrongType()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "entity": "dfahnjib",
              "alias": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FromExpression>(expected, _options)
        );
    }
}
