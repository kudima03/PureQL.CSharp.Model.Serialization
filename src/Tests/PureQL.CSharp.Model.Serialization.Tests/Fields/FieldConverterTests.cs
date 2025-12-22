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
        const string input = /*lang=json,strict*/
            """{"type": {"name":"boolean"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT0);
    }

    [Fact]
    public void ReadDateField()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"date"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT1);
    }

    [Fact]
    public void ReadDateTimeField()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"datetime"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT2);
    }

    [Fact]
    public void ReadNumberField()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"number"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT3);
    }

    [Fact]
    public void ReadTimeField()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"time"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT4);
    }

    [Fact]
    public void ReadUuidField()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"uuid"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT5);
    }

    [Fact]
    public void ReadStringField()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"string"},"entity": "test","field": "test"}""";

        Assert.NotNull(JsonSerializer.Deserialize<Field>(input, _options)!.AsT6);
    }
}
