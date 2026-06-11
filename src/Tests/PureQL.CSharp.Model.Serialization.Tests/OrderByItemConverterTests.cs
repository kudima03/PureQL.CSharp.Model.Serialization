using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record OrderByItemConverterTests
{
    private readonly JsonSerializerOptions _options;

    public OrderByItemConverterTests()
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
    public void ReadAscDirection()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "field": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              },
              "direction": "asc"
            }
            """;

        OrderByItem value = JsonSerializer.Deserialize<OrderByItem>(input, _options)!;
        Assert.Equal(SortDirection.Asc, value.Direction);
        Assert.Equal(new NumberField(expectedEntity, expectedFieldName), value.Field.AsT4);
    }

    [Fact]
    public void ReadDescDirection()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "field": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              },
              "direction": "desc"
            }
            """;

        OrderByItem value = JsonSerializer.Deserialize<OrderByItem>(input, _options)!;
        Assert.Equal(SortDirection.Desc, value.Direction);
        Assert.Equal(new NumberField(expectedEntity, expectedFieldName), value.Field.AsT4);
    }

    [Fact]
    public void ReadDefaultDirectionIsAsc()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "field": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        OrderByItem value = JsonSerializer.Deserialize<OrderByItem>(input, _options)!;
        Assert.Equal(SortDirection.Asc, value.Direction);
    }

    [Fact]
    public void WriteAscDirection()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "field": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new OrderByItem(
                new Field(new NumberField(expectedEntity, expectedFieldName)),
                SortDirection.Asc
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void WriteDescDirection()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string expected = /*lang=json,strict*/
            $$"""
            {
              "field": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "number"
                }
              },
              "direction": "desc"
            }
            """;

        string output = JsonSerializer.Serialize(
            new OrderByItem(
                new Field(new NumberField(expectedEntity, expectedFieldName)),
                SortDirection.Desc
            ),
            _options
        );
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingField()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "direction": "asc"
            }
            """;

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<OrderByItem>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnEmptyObject()
    {
        const string input = "{}";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<OrderByItem>(input, _options)
        );
    }

    [Fact]
    public void ReadStringField()
    {
        const string expectedEntity = "entityName";
        const string expectedFieldName = "fieldName";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "field": {
                "entity": "{{expectedEntity}}",
                "field": "{{expectedFieldName}}",
                "type": {
                  "name": "string"
                }
              }
            }
            """;

        OrderByItem value = JsonSerializer.Deserialize<OrderByItem>(input, _options)!;
        Assert.Equal(new StringField(expectedEntity, expectedFieldName), value.Field.AsT7);
    }
}
