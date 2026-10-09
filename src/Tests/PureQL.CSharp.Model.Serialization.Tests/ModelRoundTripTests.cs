using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record ModelRoundTripTests
{
    [Theory]
    [MemberData(nameof(ModelTypes))]
    public void EveryVariantRoundTripsToTheSameModel(string typeName)
    {
        Type type = ModelSamples.Types.Single(t => t.FullName == typeName);
        foreach (object value in ModelSamples.Of(type))
        {
            string json = JsonSerializer.Serialize(value, type, PureQLJson.Options);

            object back = JsonSerializer.Deserialize(json, type, PureQLJson.Options)!;

            ModelAssert.Equivalent(value, back);
            Assert.Equal(json, JsonSerializer.Serialize(back, type, PureQLJson.Options));
        }
    }

    public static TheoryData<string> ModelTypes()
    {
        return SpecificationFiles.Data(ModelSamples.Types.Select(t => t.FullName!));
    }
}
