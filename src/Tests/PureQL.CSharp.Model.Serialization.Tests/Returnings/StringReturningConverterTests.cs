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

public sealed record StringReturningConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new StringReturningConverter(),
            new StringFieldConverter(),
            new StringParameterConverter(),
            new StringScalarConverter(),
            new TypeConverter<StringType>()
        }
    };

    [Fact]
    public void ReadStringField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"string"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        StringField field = JsonSerializer
            .Deserialize<StringReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new StringType(), field.Type);
    }

    [Fact]
    public void WriteStringField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new StringReturning(new StringField(expectedEntity, expectedField)),
            _options
        );

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

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"string"},"name": "{{paramName}}"}""";

        StringParameter parameter = JsonSerializer
            .Deserialize<StringReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new StringType(), parameter.Type);
    }

    [Fact]
    public void WriteStringParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new StringReturning(new StringParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
              {
                "name": "{{expectedParamName}}",
                "type": {
                  "name": "string"
                }
              }
              """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadStringScalar()
    {
        const string expectedValue = "fbdhidflefbhbfdhvjz";

        const string input = $$"""
                               {
                                 "type": {
                                   "name": "string"
                                 },
                                 "value": "{{expectedValue}}"
                               }
                               """;
        StringScalar scalar = JsonSerializer
            .Deserialize<StringReturning>(input, _options)!
            .AsT2;

        Assert.Equal(expectedValue, scalar.Value);
    }

    [Fact]
    public void WriteStringScalar()
    {
        const string expectedValue = "fbdhidflefbhbfdhvjz";

        const string expected = $$"""
                                  {
                                    "type": {
                                      "name": "string"
                                    },
                                    "value": "{{expectedValue}}"
                                  }
                                  """;

        string output = JsonSerializer.Serialize(
            new StringReturning(new StringScalar(expectedValue)),
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
            JsonSerializer.Deserialize<StringReturning>(input, _options)
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
            JsonSerializer.Deserialize<StringReturning>(input, _options)
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
            JsonSerializer.Deserialize<StringReturning>(input, _options)
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
        """{"type":{"name":"time"},"value": "hbgfrtdvsdhcif"}"""
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
            JsonSerializer.Deserialize<StringReturning>(input, _options)
        );
    }
}
