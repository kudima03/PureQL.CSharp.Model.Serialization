using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record InvalidQueryTests
{
    /// <summary>
    /// Invalid queries that the model can represent, so deserialization accepts them:
    /// the schema rejects them for a constraint that no C# type of the model expresses.
    /// Every other invalid query must fail to deserialize.
    /// </summary>
    private static readonly Dictionary<string, string> AcceptedByModel = new Dictionary<
        string,
        string
    >
    {
        // minItems: arrays are IEnumerable<T> of any length.
        ["003_empty_select.jsonc"] = "minItems",
        ["013_empty_group_by.jsonc"] = "minItems",
        ["044_and_without_conditions.jsonc"] = "minItems",
        ["045_add_single_value.jsonc"] = "minItems",
        ["047_coalesce_single_value.jsonc"] = "minItems",
        ["128_empty_subqueries.jsonc"] = "minItems",
        ["228_divide_single_value.jsonc"] = "minItems",
        ["236_concat_single_value.jsonc"] = "minItems",
        ["318_subquery_empty_select.jsonc"] = "minItems",
        ["334_empty_joins.jsonc"] = "minItems",
        ["335_empty_order_by.jsonc"] = "minItems",

        // contains: a non-null coalesce needs a non-null operand, but its operands are
        // typed nullable.
        ["080_coalesce_still_nullable.jsonc"] = "contains",
        ["261_coalesce_all_nullable_declared_non_null.jsonc"] = "contains",
        ["265_coalesce_nullable_boolean_in_and.jsonc"] = "contains",

        // minimum: skip, take and key are plain long / int.
        ["005_pagination_take_zero.jsonc"] = "minimum",
        ["006_pagination_negative_skip.jsonc"] = "minimum",
        ["116_negative_key_index.jsonc"] = "minimum",

        // NAME pattern: names, aliases and sources are plain strings.
        ["031_field_empty_name.jsonc"] = "name",
        ["120_column_empty_alias.jsonc"] = "name",
        ["171_from_empty_entity.jsonc"] = "name",
        ["173_from_empty_subquery.jsonc"] = "name",
        ["178_join_empty_alias.jsonc"] = "name",
        ["201_param_empty_name.jsonc"] = "name",
        ["205_field_empty_source.jsonc"] = "name",
        ["309_group_key_empty_alias.jsonc"] = "name",
        ["313_subquery_empty_name.jsonc"] = "name",
        ["336_whitespace_alias.jsonc"] = "name",
        ["337_alias_trailing_space.jsonc"] = "name",
        ["338_source_leading_tab.jsonc"] = "name",
        ["339_param_name_with_line_break.jsonc"] = "name",
        ["340_subquery_name_whitespace.jsonc"] = "name",
    };

    [Theory]
    [MemberData(nameof(RejectedFiles))]
    public void InvalidQueryIsRejected(string name)
    {
        string input = SpecificationFiles.Read(name);

        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PureQLQuery>(input)
        );
    }

    [Theory]
    [MemberData(nameof(AcceptedFiles))]
    public void InvalidQueryOutsideTheModelIsAccepted(string name)
    {
        string input = SpecificationFiles.Read(name);

        string output = PureQLJson.Serialize(PureQLJson.Deserialize<PureQLQuery>(input));

        Assert.Equal(JsonSemantics.Canonical(input), JsonSemantics.Canonical(output));
    }

    [Fact]
    public void AcceptedFilesExist()
    {
        Assert.All(
            AcceptedByModel.Keys,
            file => Assert.Contains($"tests/invalid/{file}", SpecificationFiles.Invalid())
        );
    }

    public static TheoryData<string> RejectedFiles()
    {
        return Select(accepted: false);
    }

    public static TheoryData<string> AcceptedFiles()
    {
        return Select(accepted: true);
    }

    private static TheoryData<string> Select(bool accepted)
    {
        return SpecificationFiles.Data(
            SpecificationFiles
                .Invalid()
                .Where(name =>
                    AcceptedByModel.ContainsKey(Path.GetFileName(name)) == accepted
                )
        );
    }
}
