using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record JoinConverterTests
{
    private readonly JsonSerializerOptions _options;

    public JoinConverterTests()
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

    [Theory]
    [InlineData(JoinType.Inner)]
    [InlineData(JoinType.Full)]
    [InlineData(JoinType.Left)]
    [InlineData(JoinType.Right)]
    public void Read(JoinType type)
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {{JsonSerializer.Serialize(type, _options)}},
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        Join value = JsonSerializer.Deserialize<Join>(input, _options)!;
        Assert.Equal(
            new Join(type, expectedEntity, new BooleanReturning(new BooleanScalar(true))),
            value
        );
    }

    [Theory]
    [InlineData(JoinType.Inner)]
    [InlineData(JoinType.Full)]
    [InlineData(JoinType.Left)]
    [InlineData(JoinType.Right)]
    public void Write(JoinType type)
    {
        const string expectedEntity = "refnhbdjusi";

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {{JsonSerializer.Serialize(type, _options)}},
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new Join(type, expectedEntity, new BooleanReturning(new BooleanScalar(true))),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnWrongJoinType()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "bfheuwdrsj",
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedJoinType()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Theory]
    [InlineData("date")]
    [InlineData("datetime")]
    [InlineData("null")]
    [InlineData("string")]
    [InlineData("number")]
    [InlineData("time")]
    [InlineData("uuid")]
    [InlineData("")]
    public void ThrowsExceptionOnWrongOnType(string type)
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "{{type}}"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnInvalidOnType()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "{{expectedEntity}}",
              "on": {
                "type": {
                  "name": "rfwsneihjlbinhu"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedOn()
    {
        const string expectedEntity = "refnhbdjusi";

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "{{expectedEntity}}"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyJson()
    {
        const string input = "{}";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyString()
    {
        const string input = "";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUndefinedEntity()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEntityWrongType()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": {},
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullEntity()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": null,
              "on": {
                "type": {
                  "name": "boolean"
                },
                "value": true
              }
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullOn()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "dsijnuf",
              "on": null
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnOnWrongType()
    {
        const string expected = /*lang=json,strict*/
            $$"""
            {
              "type": "full",
              "entity": "dsijnuf",
              "on": {}
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Join>(expected, _options)
        );
    }
}
