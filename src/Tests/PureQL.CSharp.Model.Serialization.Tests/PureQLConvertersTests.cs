using System.Collections;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record PureQLConvertersTests
{
    [Fact]
    public void NonGenericGetEnumeratorReturnsConverters()
    {
        IEnumerable converters = new PureQLConverters();
        IEnumerator enumerator = converters.GetEnumerator();
        Assert.True(enumerator.MoveNext());
    }

    /// <summary>
    /// Pins the exact number of converters registered. This is the single wiring
    /// point that plugs every converter in this assembly into a
    /// JsonSerializerOptions instance - a converter silently added twice or
    /// dropped during a refactor would not otherwise be caught by any other test,
    /// since per-type converter tests each build their own isolated options.
    /// If this intentionally changes (a converter added or removed), update the
    /// expected count together with a note of which converter changed.
    /// </summary>
    [Fact]
    public void ReturnsExpectedNumberOfConverters()
    {
        Assert.Equal(169, new PureQLConverters().Count());
    }

    [Fact]
    public void ContainsNoNullConverters()
    {
        Assert.All(new PureQLConverters(), Assert.NotNull);
    }

    /// <summary>
    /// System.Text.Json resolves a converter for a given type by taking the last
    /// matching entry in the Converters list, so an accidental duplicate
    /// registration of the same converter type would silently mask whichever
    /// copy was added first rather than failing loudly. This asserts every
    /// concrete converter type appears exactly once.
    /// </summary>
    [Fact]
    public void ContainsNoDuplicateConverterTypes()
    {
        List<Type> types = [.. new PureQLConverters().Select(c => c.GetType())];
        List<Type> distinctTypes = [.. types.Distinct()];

        Assert.Equal(distinctTypes.Count, types.Count);
    }

    [Fact]
    public void ContainsCamelCaseEnumConverter()
    {
        Assert.Contains(new PureQLConverters(), c => c is JsonStringEnumConverter);
    }

    [Fact]
    public void GenericGetEnumeratorReturnsConverters()
    {
        IEnumerator<JsonConverter> enumerator = new PureQLConverters().GetEnumerator();
        Assert.True(enumerator.MoveNext());
    }
}
