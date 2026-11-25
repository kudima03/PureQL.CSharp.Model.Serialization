using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record PureQLTypeInfoResolverTests
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        MaxDepth = 256,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver = new PureQLTypeInfoResolver(),
    };

    [Fact]
    public void ResolvesEveryModelTypeToItsConverter()
    {
        PureQLTypeInfoResolver resolver = new PureQLTypeInfoResolver();

        foreach (Type type in ModelSamples.Types)
        {
            JsonTypeInfo? info = resolver.GetTypeInfo(type, new JsonSerializerOptions());

            Assert.NotNull(info);
            Assert.Equal(type, info.Type);
            Assert.Equal(JsonTypeInfoKind.None, info.Kind);
            Assert.Contains(info.Converter, new PureQLConverters());
        }
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(int))]
    [InlineData(typeof(object))]
    [InlineData(typeof(IType))]
    [InlineData(typeof(PureQLQuery[]))]
    [InlineData(typeof(List<PureQLQuery>))]
    public void LeavesOtherTypesToTheNextResolver(Type type)
    {
        PureQLTypeInfoResolver resolver = new PureQLTypeInfoResolver();

        Assert.Null(resolver.GetTypeInfo(type, new JsonSerializerOptions()));
    }

    [Theory]
    [MemberData(nameof(ModelTypes))]
    public void EveryVariantRoundTripsThroughItsTypeInfo(string typeName)
    {
        Type type = ModelSamples.Types.Single(t => t.FullName == typeName);
        JsonTypeInfo info = Options.GetTypeInfo(type);
        foreach (object value in ModelSamples.Of(type))
        {
            string json = JsonSerializer.Serialize(value, info);

            object back = JsonSerializer.Deserialize(json, info)!;

            ModelAssert.Equivalent(value, back);
            Assert.Equal(json, JsonSerializer.Serialize(value, type, PureQLJson.Options));
        }
    }

    [Theory]
    [MemberData(nameof(ValidFiles))]
    public void WritesTheSameJsonAsTheConverters(string name)
    {
        string input = SpecificationFiles.Read(name);
        JsonTypeInfo<PureQLQuery> info = Options.GetTypeInfo<PureQLQuery>();

        PureQLQuery query = JsonSerializer.Deserialize(input, info);
        string output = JsonSerializer.Serialize(query, info);

        Assert.Equal(JsonSemantics.Canonical(input), JsonSemantics.Canonical(output));
        Assert.Equal(
            PureQLJson.Serialize(PureQLJson.Deserialize<PureQLQuery>(input)),
            output
        );
    }

    [Fact]
    public void CombinesWithASourceGeneratedContext()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            MaxDepth = 256,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                new PureQLTypeInfoResolver(),
                EnvelopeContext.Default
            ),
        };
        JsonTypeInfo<Envelope> info = options.GetTypeInfo<Envelope>();
        string json = SpecificationFiles.Read(SpecificationFiles.Valid()[0]);
        Envelope envelope = new Envelope(
            "first",
            PureQLJson.Deserialize<PureQLQuery>(json)
        );

        string written = JsonSerializer.Serialize(envelope, info);
        Envelope back = JsonSerializer.Deserialize(written, info)!;
        PureQLQuery query = ((JsonElement)back.Query).Deserialize(
            options.GetTypeInfo<PureQLQuery>()
        );

        Assert.Equal("first", back.Name);
        Assert.Equal(
            JsonSemantics.Canonical(json),
            JsonSemantics.Canonical(PureQLJson.Serialize(query))
        );
        Assert.Equal(PureQLJson.Serialize((PureQLQuery)envelope.Query), PureQLJson.Serialize(query));
    }

    [Fact]
    public void RejectsInvalidJsonThroughTypeInfo()
    {
        JsonTypeInfo<PureQLQuery> info = Options.GetTypeInfo<PureQLQuery>();

        JsonException error = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize("""{"from":{"entity":"a"}}""", info)
        );

        Assert.Contains("$", error.Message, StringComparison.Ordinal);
    }

    public static TheoryData<string> ModelTypes()
    {
        return SpecificationFiles.Data(ModelSamples.Types.Select(t => t.FullName!));
    }

    public static TheoryData<string> ValidFiles()
    {
        return SpecificationFiles.Data(SpecificationFiles.Valid());
    }
}

/// <summary>
/// A document of the application that carries a query, written through the runtime
/// type of <see cref="Query"/>, which <see cref="PureQLTypeInfoResolver"/> resolves. It
/// is typed <see cref="object"/> so that the source generator does not reach the
/// model: it walks every type a context reaches, which takes many minutes of build
/// time for the model's thousands of records and unions.
/// </summary>
internal sealed record Envelope(string Name, object Query);

[JsonSerializable(typeof(Envelope))]
internal sealed partial class EnvelopeContext : JsonSerializerContext;
