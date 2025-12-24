using System.Text.Json;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Fields;

public sealed record UuidFieldConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new UuidFieldConverter(), new TypeConverter<UuidType>() },
    };

    [Fact]
    public void ReadEntity()
    {
        const string expected = "test";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"uuid"},"entity": "{{expected}}","field": "test"}""";

        UuidField field = JsonSerializer.Deserialize<UuidField>(input, _options)!;

        Assert.Equal(expected, field.Entity);
    }

    [Fact]
    public void ReadField()
    {
        const string expected = "test";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"uuid"},"entity": "test","field": "{{expected}}"}""";

        UuidField field = JsonSerializer.Deserialize<UuidField>(input, _options)!;

        Assert.Equal(expected, field.Field);
    }

    [Fact]
    public void Write()
    {
        const string expected =
            /*lang=json,strict*/
            """{"entity":"auiheyrdsnf","field":"jinaudferv","type":{"name":"uuid"}}""";

        string output = JsonSerializer.Serialize(
            new UuidField("auiheyrdsnf", "jinaudferv"),
            _options
        );

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ThrowsExceptionOnMissingEntityField()
    {
        const string input = /*lang=json,strict*/
            """{"field":"jinaudferv","type":{"name":"uuid"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidField>(input, _options)
        );
    }

    [Fact]
    public void ThrowsExceptionOnMissingFieldField()
    {
        const string input = /*lang=json,strict*/
            """{"entity":"auiheyrdsnf","type":{"name":"uuid"}}""";
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidField>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"entity": "auiheyrdsnf","field": "jinaudferv"}"""
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
        """{"entity": "auiheyrdsnf","field": "jinaudferv"}"""
    )]
    public void ThrowsExceptionOnWrongType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidField>(input, _options)
        );
    }
}
