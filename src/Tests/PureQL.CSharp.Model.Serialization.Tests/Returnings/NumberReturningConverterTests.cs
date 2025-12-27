using System.Globalization;
using System.Text.Json;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Returnings;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests.Returnings;

public sealed record NumberReturningConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new NumberReturningConverter(),
            new NumberFieldConverter(),
            new NumberParameterConverter(),
            new NumberScalarConverter(),
            new TypeConverter<NumberType>()
        }
    };

    [Fact]
    public void ReadNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"number"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        NumberField field = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new NumberType(), field.Type);
    }

    [Fact]
    public void WriteNumberField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new NumberReturning(new NumberField(expectedEntity, expectedField)),
            _options
        );

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

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadNumberParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"number"},"name": "{{paramName}}"}""";

        NumberParameter parameter = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new NumberType(), parameter.Type);
    }

    [Fact]
    public void WriteNumberParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new NumberReturning(new NumberParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "number"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadNumberScalar()
    {
        double expectedValue = Random.Shared.NextDouble();

        string input = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
            }
            """;
        NumberScalar scalar = JsonSerializer
            .Deserialize<NumberReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteNumberScalar()
    {
        double expectedValue = Random.Shared.NextDouble();

        string expected = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "number"
              },
              "value": {{expectedValue.ToString(CultureInfo.InvariantCulture)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new NumberReturning(new NumberScalar(expectedValue)),
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
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
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
        """{"type":{"name":"string"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
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
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
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
        """{"type":{"name":"string"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"name": "erfinjdhksgt"}"""
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
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"boolean"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value": "2025-12-24T15:20:36.6778291+03:00"}"""
    )]
    public void ThrowsExceptionOnWrongScalarType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<NumberReturning>(input, _options)
        );
    }
}
