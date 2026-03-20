using System.Collections;

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
}
