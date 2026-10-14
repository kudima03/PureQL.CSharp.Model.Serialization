namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record SpecificationRoundTripTests
{
    [Theory]
    [MemberData(nameof(ValidFiles))]
    public void RoundTripsToSemanticallyEqualJson(string name)
    {
        string input = SpecificationFiles.Read(name);

        string output = PureQLJson.Serialize(PureQLJson.Deserialize<PureQLQuery>(input));

        Assert.Equal(JsonSemantics.Canonical(input), JsonSemantics.Canonical(output));
    }

    [Theory]
    [MemberData(nameof(ValidFiles))]
    public void WritesStableJson(string name)
    {
        string output = PureQLJson.Serialize(
            PureQLJson.Deserialize<PureQLQuery>(SpecificationFiles.Read(name))
        );

        Assert.Equal(
            output,
            PureQLJson.Serialize(PureQLJson.Deserialize<PureQLQuery>(output))
        );
    }

    public static TheoryData<string> ValidFiles()
    {
        return SpecificationFiles.Data(SpecificationFiles.Valid());
    }
}
