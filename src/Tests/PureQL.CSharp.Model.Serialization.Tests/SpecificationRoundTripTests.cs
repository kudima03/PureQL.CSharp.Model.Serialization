using System.Text.Json;
using System.Text.Json.Nodes;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record SpecificationRoundTripTests
{
    private const string LiteralEdgeValues = "valid/002_literal_edge_values.jsonc";

    /// <summary>
    /// Columns of <see cref="LiteralEdgeValues"/> whose literal the model's CLR types
    /// cannot hold: <c>DateOnly</c> starts at year 0001, and <c>TimeOnly</c> has a
    /// precision of 100 ns while the specification has nanoseconds.
    /// </summary>
    private static readonly string[] UnrepresentableColumns =
    [
        "year_zero",
        "nanoseconds",
    ];

    [Theory]
    [MemberData(nameof(ValidFiles))]
    public void RoundTripsToSemanticallyEqualJson(string name)
    {
        AssertRoundTrip(SpecificationFiles.Read(name));
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

    [Fact]
    public void LiteralEdgeValuesRoundTripExceptUnrepresentableColumns()
    {
        AssertRoundTrip(WithoutColumns(UnrepresentableColumns));
    }

    [Theory]
    [InlineData("year_zero", "'0000-01-01' is not a date")]
    [InlineData("nanoseconds", "'23:59:59.999999999' is not a time")]
    public void LiteralEdgeValueOutsideTheModelIsRejected(string column, string message)
    {
        string input = WithoutColumns(UnrepresentableColumns.Where(c => c != column));

        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PureQLQuery>(input)
        );
        Assert.StartsWith(message, exception.Message, StringComparison.Ordinal);
    }

    public static TheoryData<string> ValidFiles()
    {
        return SpecificationFiles.Data(
            SpecificationFiles.Valid().Where(name => name != LiteralEdgeValues)
        );
    }

    private static void AssertRoundTrip(string input)
    {
        string output = PureQLJson.Serialize(PureQLJson.Deserialize<PureQLQuery>(input));

        Assert.Equal(JsonSemantics.Canonical(input), JsonSemantics.Canonical(output));
    }

    private static string WithoutColumns(IEnumerable<string> aliases)
    {
        JsonObject query = JsonNode
            .Parse(
                SpecificationFiles.Read(LiteralEdgeValues),
                null,
                new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip }
            )!
            .AsObject();
        JsonArray select = query["select"]!.AsArray();
        foreach (string alias in aliases)
        {
            _ = select.Remove(
                select.Single(c => c!["alias"]!.GetValue<string>() == alias)
            );
        }

        return query.ToJsonString();
    }
}
