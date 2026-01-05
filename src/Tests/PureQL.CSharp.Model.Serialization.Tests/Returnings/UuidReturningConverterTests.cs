using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record UuidReturningConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UuidReturningConverterTests()
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
    public void ReadUuidField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"uuid"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        UuidField field = JsonSerializer
            .Deserialize<UuidReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new UuidType(), field.Type);
    }

    [Fact]
    public void WriteUuidField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new UuidReturning(new UuidField(expectedEntity, expectedField)),
            _options
        );

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

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"uuid"},"name": "{{paramName}}"}""";

        UuidParameter parameter = JsonSerializer
            .Deserialize<UuidReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new UuidType(), parameter.Type);
    }

    [Fact]
    public void WriteUuidParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new UuidReturning(new UuidParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "uuid"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadUuidScalar()
    {
        Guid expectedValue = Guid.NewGuid();

        string input = $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expectedValue}}"
            }
            """;
        UuidScalar scalar = JsonSerializer
            .Deserialize<UuidReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteUuidScalar()
    {
        Guid expectedValue = Guid.NewGuid();

        string expected = $$"""
            {
              "type": {
                "name": "uuid"
              },
              "value": "{{expectedValue}}"
            }
            """;

        string output = JsonSerializer.Serialize(
            new UuidReturning(new UuidScalar(expectedValue)),
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
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"ihufd"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    public void ThrowsExceptionOnWrongFieldType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type": {"name":"date"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"ihufd"},"name": "erfinjdhksgt"}"""
    )]
    public void ThrowsExceptionOnWrongParameterType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value": "hbgfrtdvsdhcif"}"""
    )]
    public void ThrowsExceptionOnWrongScalarType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<UuidReturning>(input, _options)
        );
    }
}
