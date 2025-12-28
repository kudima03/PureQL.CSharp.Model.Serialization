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

public sealed record TimeReturningConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new TimeReturningConverter(),
            new TimeFieldConverter(),
            new TimeParameterConverter(),
            new TimeScalarConverter(),
            new TypeConverter<TimeType>(),
        },
    };

    [Fact]
    public void ReadTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"time"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        TimeField field = JsonSerializer
            .Deserialize<TimeReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new TimeType(), field.Type);
    }

    [Fact]
    public void WriteTimeField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new TimeReturning(new TimeField(expectedEntity, expectedField)),
            _options
        );

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

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"time"},"name": "{{paramName}}"}""";

        TimeParameter parameter = JsonSerializer
            .Deserialize<TimeReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new TimeType(), parameter.Type);
    }

    [Fact]
    public void WriteTimeParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new TimeReturning(new TimeParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "time"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadTimeScalar()
    {
        TimeOnly expectedValue = TimeOnly.FromDateTime(DateTime.Now);

        string input = $$"""
            {
              "type": {
                "name": "time"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;
        TimeScalar scalar = JsonSerializer
            .Deserialize<TimeReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteTimeScalar()
    {
        TimeOnly expectedValue = TimeOnly.FromDateTime(DateTime.Now);

        string expected = $$"""
            {
              "type": {
                "name": "time"
              },
              "value": {{JsonSerializer.Serialize(expectedValue)}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new TimeReturning(new TimeScalar(expectedValue)),
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
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
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
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
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
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
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
        """{"type":{"name":"uuid"},"value": "hbgfrtdvsdhcif"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value": "hbgfrtdvsdhcif"}"""
    )]
    public void ThrowsExceptionOnWrongScalarType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TimeReturning>(input, _options)
        );
    }
}
