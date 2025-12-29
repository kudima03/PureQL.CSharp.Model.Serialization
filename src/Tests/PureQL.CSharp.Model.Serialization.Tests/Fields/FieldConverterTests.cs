using System.Text.Json;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record FieldConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new FieldConverter(),
            new BooleanFieldConverter(),
            new DateFieldConverter(),
            new DateTimeFieldConverter(),
            new NumberFieldConverter(),
            new StringFieldConverter(),
            new TimeFieldConverter(),
            new UuidFieldConverter(),
            new TypeConverter<BooleanType>(),
            new TypeConverter<DateType>(),
            new TypeConverter<DateTimeType>(),
            new TypeConverter<NumberType>(),
            new TypeConverter<StringType>(),
            new TypeConverter<TimeType>(),
            new TypeConverter<UuidType>(),
        },
    };

    [Fact]
    public void ReadBooleanField()
    {
        const string expectedEntity = "ahbudnfs";

        const string expectedField = "arfeinjuhg";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"boolean"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        BooleanField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new BooleanType(), field.Type);
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
                "name": "boolean"
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
            $$"""{"type": {"name":"date"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        DateField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT1;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateType(), field.Type);
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
                "name": "date"
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
            $$"""{"type": {"name":"datetime"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        DateTimeField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT2;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new DateTimeType(), field.Type);
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
                "name": "datetime"
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
            $$"""{"type": {"name":"number"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        NumberField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT3;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new NumberType(), field.Type);
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
                "name": "number"
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
            $$"""{"type": {"name":"time"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        TimeField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT4;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new TimeType(), field.Type);
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
                "name": "time"
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
            $$"""{"type": {"name":"uuid"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        UuidField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT5;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new UuidType(), field.Type);
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
                "name": "uuid"
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
            $$"""{"type": {"name":"string"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        StringField field = JsonSerializer.Deserialize<Field>(input, _options)!.AsT6;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new StringType(), field.Type);
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
                "name": "string"
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
