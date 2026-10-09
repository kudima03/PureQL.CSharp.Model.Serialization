using System.Collections;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record PureQLConvertersTests
{
    [Fact]
    public void ConvertsEveryModelType()
    {
        IEnumerable<Type> types = ModelSamples.Types;

        foreach (Type type in types)
        {
            _ = Assert.Single(
                new PureQLConverters(),
                converter => converter.CanConvert(type)
            );
        }
    }

    [Fact]
    public void HasOneConverterPerModelType()
    {
        Assert.Equal(ModelSamples.Types.Count, new PureQLConverters().Count());
    }

    [Fact]
    public void NonGenericEnumeratorYieldsTheSameConverters()
    {
        IEnumerator enumerator = ((IEnumerable)new PureQLConverters()).GetEnumerator();
        List<JsonConverter> converters = [];
        while (enumerator.MoveNext())
        {
            converters.Add((JsonConverter)enumerator.Current);
        }

        Assert.Equal(new PureQLConverters(), converters);
    }
}
