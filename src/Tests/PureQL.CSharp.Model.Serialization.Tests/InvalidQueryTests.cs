using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Every invalid query of the specification fails to deserialize: the converters
/// check what the model's types cannot express (the exact declared type of an operator
/// over its operands, names, bare numbers, the literals of restricted positions), so no
/// invalid query is a valid model.
/// </summary>
public sealed record InvalidQueryTests
{
    [Theory]
    [MemberData(nameof(InvalidFiles))]
    public void InvalidQueryIsRejected(string name)
    {
        string input = SpecificationFiles.Read(name);

        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PureQLQuery>(input)
        );
    }

    public static TheoryData<string> InvalidFiles()
    {
        return SpecificationFiles.Data(SpecificationFiles.Invalid());
    }
}
