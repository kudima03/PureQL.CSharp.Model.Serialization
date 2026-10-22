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
        // contains: a non-null coalesce needs a non-null operand, but its operands are
        // typed nullable, and at most one of them may be non-null.
        ["261_coalesce_all_nullable_declared_non_null.jsonc"] = "contains",
        ["407_coalesce_two_non_null.jsonc"] = "contains",

        // declared type: an operator declares the exact type its operands give it, but
        // its operands are typed by position, so a wider or narrower declaration of the
        // same record is a valid model.
        ["287_max_of_boolean_declared_nullable.jsonc"] = "declared type",
        ["348_grouped_min_over_all_declared_nullable.jsonc"] = "declared type",
        ["350_add_of_integers_declared_decimal.jsonc"] = "declared type",
        ["351_add_of_non_null_declared_nullable.jsonc"] = "declared type",
        ["353_if_of_non_null_branches_declared_nullable.jsonc"] = "declared type",
        ["354_if_of_integers_declared_decimal.jsonc"] = "declared type",
        ["355_coalesce_with_non_null_declared_nullable.jsonc"] = "declared type",
        ["356_coalesce_of_nullables_declared_non_null.jsonc"] = "declared type",
        ["357_coalesce_of_integers_declared_decimal.jsonc"] = "declared type",
        ["361_sum_of_integers_declared_decimal.jsonc"] = "declared type",
        ["362_min_of_integers_declared_decimal.jsonc"] = "declared type",
        ["363_group_min_declared_nullable.jsonc"] = "declared type",
        ["386_plain_min_declared_nullable.jsonc"] = "declared type",

        // dependentRequired, dependentSchemas: a pagination needs an orderBy, and a
        // subquery's orderBy needs a pagination or a distinctBy, but each is a property.
        ["371_pagination_without_order_by.jsonc"] = "dependent",
        ["372_subquery_pagination_without_order_by.jsonc"] = "dependent",
        ["373_subquery_order_by_without_pagination.jsonc"] = "dependent",

        // minimum: skip and take are plain long.
        ["005_pagination_take_zero.jsonc"] = "minimum",
        ["006_pagination_negative_skip.jsonc"] = "minimum",

        // NAME pattern: names, aliases and sources are plain strings.
        ["031_field_empty_name.jsonc"] = "name",
        ["120_column_empty_alias.jsonc"] = "name",
        ["171_from_empty_entity.jsonc"] = "name",
        ["173_from_empty_subquery.jsonc"] = "name",
        ["178_join_empty_alias.jsonc"] = "name",
        ["201_param_empty_name.jsonc"] = "name",
        ["206_key_empty_name.jsonc"] = "name",
        ["205_field_empty_source.jsonc"] = "name",
        ["309_group_key_empty_alias.jsonc"] = "name",
        ["313_subquery_empty_name.jsonc"] = "name",
        ["336_whitespace_alias.jsonc"] = "name",
        ["337_alias_trailing_space.jsonc"] = "name",
        ["338_source_leading_tab.jsonc"] = "name",
        ["339_param_name_with_line_break.jsonc"] = "name",
        ["340_subquery_name_whitespace.jsonc"] = "name",
        ["396_distinct_by_empty_alias.jsonc"] = "name",
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
