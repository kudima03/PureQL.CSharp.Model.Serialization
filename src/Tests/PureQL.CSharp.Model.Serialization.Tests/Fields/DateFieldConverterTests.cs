using System.Text.Json;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record DateFieldConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new DateFieldConverter(), new TypeConverter<DateType>() },
    };

    [Fact]
    public void ReadEntity()
    {
        const string expected = "test";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"date"},"entity": "{{expected}}","field": "test"}""";

        DateField field = JsonSerializer.Deserialize<DateField>(input, _options)!;

        Assert.Equal(expected, field.Entity);
    }

    [Fact]
    public void ReadField()
    {
        const string expected = "test";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"date"},"entity": "test","field": "{{expected}}"}""";

        DateField field = JsonSerializer.Deserialize<DateField>(input, _options)!;

        Assert.Equal(expected, field.Field);
    }

    [Fact]
    public void Write()
    {
        const string expected =
            /*lang=json,strict*/
            """
                {"entity":"auiheyrdsnf","field":"jinaudferv","type":{"name":"date"}}
                """;

        string output = JsonSerializer.Serialize(
            new DateField("auiheyrdsnf", "jinaudferv"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingEntityField()
    {
        const string input = /*lang=json,strict*/
            """{"field":"jinaudferv","type":{"name":"date"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateField>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingFieldField()
    {
        const string input = /*lang=json,strict*/
            """{"entity":"auiheyrdsnf","type":{"name":"date"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateField>(input, _options)
        );
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(" ")]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateField>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<DateField>(input, _options)
        );
    }
}
