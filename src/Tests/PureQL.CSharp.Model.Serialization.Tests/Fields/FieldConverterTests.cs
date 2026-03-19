using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayTypes;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record FieldConverterTests
{
    private readonly JsonSerializerOptions _options;

    public FieldConverterTests()
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
    public void ReadBooleanField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "booleanArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new BooleanField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT0
        );
    }

    [Fact]
    public void WriteBooleanField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "booleanArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "dateArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new DateField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT1
        );
    }

    [Fact]
    public void WriteDateField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "dateArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadDateTimeField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "datetimeArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new DateTimeField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT2
        );
    }

    [Fact]
    public void WriteDateTimeField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "datetimeArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new DateTimeField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadNumberField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "numberArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new NumberField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT3
        );
    }

    [Fact]
    public void WriteNumberField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "numberArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "timeArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new TimeField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT4
        );
    }

    [Fact]
    public void WriteTimeField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "timeArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new TimeField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "uuidArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new UuidField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT5
        );
    }

    [Fact]
    public void WriteUuidField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "uuidArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new UuidField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "stringArray"
              },
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}"
            }
            """;

        Assert.Equal(
            new StringField(expectedEntity, expectedField),
            JsonSerializer.Deserialize<Field>(input, _options)!.AsT6
        );
    }

    [Fact]
    public void WriteStringField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "entity": "{{expectedEntity}}",
              "field": "{{expectedField}}",
              "type": {
                "name": "stringArray"
              }
            }
            """;

        string output = JsonSerializer.Serialize(
            new StringField(expectedEntity, expectedField),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<StringField>(input, _options)
        );
    }
}
