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

public sealed record BooleanReturningConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        NewLine = "\n",
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new BooleanReturningConverter(),
            new BooleanFieldConverter(),
            new BooleanParameterConverter(),
            new BooleanScalarConverter(),
            new TypeConverter<BooleanType>()
        }
    };

    [Fact]
    public void ReadBooleanField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";
        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"boolean"},"entity": "{{expectedEntity}}","field": "{{expectedField}}"}""";

        BooleanField field = JsonSerializer
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT0;

        Assert.Equal(expectedEntity, field.Entity);
        Assert.Equal(expectedField, field.Field);
        Assert.Equal(new BooleanType(), field.Type);
    }

    [Fact]
    public void WriteBooleanField()
    {
        const string expectedEntity = "uheayfodrbniJ";
        const string expectedField = "ubhjedwasuyhgbefrda";

        string output = JsonSerializer.Serialize(
            new BooleanReturning(new BooleanField(expectedEntity, expectedField)),
            _options
        );

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

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadBooleanParameter()
    {
        const string paramName = "auryehgfbduygbhaerf";

        const string input = /*lang=json,strict*/
            $$"""{"type": {"name":"boolean"},"name": "{{paramName}}"}""";

        BooleanParameter parameter = JsonSerializer
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT1;

        Assert.Equal(paramName, parameter.Name);
        Assert.Equal(new BooleanType(), parameter.Type);
    }

    [Fact]
    public void WriteBooleanParameter()
    {
        const string expectedParamName = "uheayfodrbniJ";

        string output = JsonSerializer.Serialize(
            new BooleanReturning(new BooleanParameter(expectedParamName)),
            _options
        );

        const string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "name": "{{expectedParamName}}",
              "type": {
                "name": "boolean"
              }
            }
            """;

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void ReadBooleanScalar()
    {
        const string input = /*lang=json,strict*/
            """{"type": {"name":"boolean"},"value": true}""";

        BooleanScalar scalar = JsonSerializer
            .Deserialize<BooleanReturning>(input, _options)!
            .AsT2;

        Assert.True(scalar.Value);
    }

    [Fact]
    public void WriteBooleanScalar()
    {
        string expectedOutput = /*lang=json,strict*/
            $$"""
            {
              "type": {
                "name": "boolean"
              },
              "value": true
            }
            """;

        string output = JsonSerializer.Serialize(
            new BooleanReturning(new BooleanScalar(true)),
            _options
        );

        Assert.Equal(expectedOutput, output);
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "NotImplemented")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public void ReadEquality()
    {
        const string input = /*lang=json,strict*/
            """{"operator":"equal","left":{"entity":"u","field":"active","type":{"name":"boolean"}},"right":{"type":{"name":"boolean"},"value":true}}""";

        Assert.NotNull(
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)!.AsT3
        );
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "NotImplemented")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public void ReadBooleanOperator()
    {
        const string input = /*lang=json,strict*/
            """{"operator":"and","left":{"entity":"u","field":"active","type":{"name":"boolean"}},"right":{"type":{"name":"boolean"},"value":true}}""";

        Assert.NotNull(
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)!.AsT4
        );
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "NotImplemented")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    [InlineData("{}")]
    [InlineData("{asdasdasd}")]
    [InlineData("""{"asdasd":   }""")]
    [InlineData(""" """)]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "NotImplemented")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"entity": "ufbrdeyhov","field": "heuiyrndfosgv"}"""
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
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "NotImplemented")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    [InlineData( /*lang=json,strict*/
        """{"type": {"name":"boolean"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"name": "erfinjdhksgt"}"""
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
        """{"type":{"name":"uuid"},"name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"entity": "ufbrdeyhov","name": "erfinjdhksgt"}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"ihufd"},"name": "erfinjdhksgt"}"""
    )]
    public void ThrowsExceptionOnWrongParameterType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "NotImplemented")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"datetime"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"date"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"null"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"number"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"string"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"time"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"type":{"name":"uuid"},"value": true}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"value": true}"""
    )]
    public void ThrowsExceptionOnWrongScalarType(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<BooleanReturning>(input, _options)
        );
    }
}
