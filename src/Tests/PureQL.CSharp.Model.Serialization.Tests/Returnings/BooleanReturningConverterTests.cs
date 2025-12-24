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
            new TypeConverter<BooleanType>(),
        },
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
}
