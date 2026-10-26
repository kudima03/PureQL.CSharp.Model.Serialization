using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using PureQL.CSharp.Model;
using PureQL.CSharp.Model.Serialization;
using PureQL.CSharp.Model.Serialization.Tests;

// Every valid fixture of the specification must round-trip to semantically equal JSON
// through the type info resolver alone, on its own and inside a source-generated
// document, and every model type must resolve. Prints the counts and exits with 1 on
// the first failure.
JsonSerializerOptions options = new JsonSerializerOptions
{
    MaxDepth = 256,
    ReadCommentHandling = JsonCommentHandling.Skip,
    TypeInfoResolver = JsonTypeInfoResolver.Combine(
        new PureQLTypeInfoResolver(),
        EnvelopeContext.Default
    ),
};
JsonTypeInfo<PureQLQuery> queryInfo = (JsonTypeInfo<PureQLQuery>)
    options.GetTypeInfo(typeof(PureQLQuery));
JsonTypeInfo<Envelope> envelopeInfo = (JsonTypeInfo<Envelope>)
    options.GetTypeInfo(typeof(Envelope));

int types = 0;
foreach (JsonConverter converter in new PureQLConverters())
{
    JsonTypeInfo info = options.GetTypeInfo(converter.Type!);
    if (info.Converter != converter)
    {
        return Fail($"{converter.Type} does not resolve to its converter");
    }

    types++;
}

string folder = Path.Combine(
    AppContext.BaseDirectory,
    "Specification",
    "samples",
    "valid"
);
string[] files = [.. Directory
    .GetFiles(folder)
    .Where(f => f.EndsWith(".json", StringComparison.Ordinal)
        || f.EndsWith(".jsonc", StringComparison.Ordinal))
    .Order(StringComparer.Ordinal)];
if (files.Length == 0)
{
    return Fail($"no fixtures in {folder}");
}

foreach (string file in files)
{
    string input = File.ReadAllText(file);
    string expected = JsonSemantics.Canonical(input);
    string name = Path.GetFileName(file);
    try
    {
        PureQLQuery query = JsonSerializer.Deserialize(input, queryInfo)!;
        string output = JsonSerializer.Serialize(query, queryInfo);
        if (JsonSemantics.Canonical(output) != expected)
        {
            return Fail($"{name} does not round-trip:\n{output}");
        }

        Envelope envelope = new Envelope(name, query);
        string document = JsonSerializer.Serialize(envelope, envelopeInfo);
        Envelope back = JsonSerializer.Deserialize(document, envelopeInfo)!;
        if (
            back.Name != name
            || JsonSerializer.Serialize(back.Query, queryInfo) != output
        )
        {
            return Fail($"{name} does not round-trip inside a document:\n{document}");
        }
    }
    catch (JsonException error)
    {
        return Fail($"{name} is rejected: {error.Message}");
    }
}

Console.WriteLine(
    $"Native AOT: {types} model types resolved, {files.Length} fixtures round-tripped"
);
return 0;

static int Fail(string message)
{
    Console.Error.WriteLine($"Native AOT check failed: {message}");
    return 1;
}

/// <summary>A document of the application that carries a query.</summary>
internal sealed record Envelope(string Name, PureQLQuery Query);

[JsonSerializable(typeof(Envelope))]
internal sealed partial class EnvelopeContext : JsonSerializerContext;
