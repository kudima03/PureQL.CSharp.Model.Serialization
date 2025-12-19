using System.Text.Json;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Types;

public sealed record DateTypeConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new TypeConverter<DateType>() },
    };

    [Fact]
    public void Read()
    {
        const string input = /*lang=json,strict*/
            """{"name":"date"}""";

        DateType type = JsonSerializer.Deserialize<DateType>(input, _options)!;

        Assert.Equal(type.Name, new DateType().Name);
    }

    [Fact]
    public void Write()
    {
        const string expected = /*lang=json,strict*/
            """{"name":"date"}""";

        string output = JsonSerializer.Serialize(new DateType(), _options);

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnBooleanType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"boolean"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnDateTimeType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"datetime"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNullType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"null"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnNumberType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"number"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnStringType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"string"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnTimeType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"time"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnUuidType()
    {
        const string input = /*lang=json,strict*/
            """{"name":"uuid"}""";

        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateType>(input, _options)
        );
    }
}
