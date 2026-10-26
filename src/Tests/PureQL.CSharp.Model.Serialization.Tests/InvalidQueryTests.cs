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
        // contains: a non-null coalesce needs exactly one non-null operand, but its
        // operands are typed nullable, so none or several of them may be non-null.
        ["0261_coalesce_all_nullable_declared_non_null.jsonc"] = "contains",
        ["0356_coalesce_of_nullables_declared_non_null.jsonc"] = "contains",
        ["0407_coalesce_two_non_null.jsonc"] = "contains",
        ["1344_two_non_null_operands_coalesce_date_grouped_select.jsonc"] = "contains",
        ["1349_two_non_null_operands_coalesce_date_total_query.jsonc"] = "contains",
        ["1371_two_non_null_operands_coalesce_time_group_by_key.jsonc"] = "contains",

        // declared type: an operator declares the exact number type its operands give it,
        // but an integer operand is a valid decimal operand, so a decimal declaration
        // over integers only is a valid model. A nullable declaration over non-null
        // operands only is not listed: the converters reject it.
        ["0350_add_of_integers_declared_decimal.jsonc"] = "declared type",
        ["0354_if_of_integers_declared_decimal.jsonc"] = "declared type",
        ["0357_coalesce_of_integers_declared_decimal.jsonc"] = "declared type",
        ["0361_sum_of_integers_declared_decimal.jsonc"] = "declared type",
        ["0362_min_of_integers_declared_decimal.jsonc"] = "declared type",
        ["0596_decimal_from_integers_add_decimal_group_by_key.jsonc"] = "declared type",
        ["0597_decimal_from_integers_add_decimal_grouped_select.jsonc"] = "declared type",
        ["0600_decimal_from_integers_add_decimal_total_query.jsonc"] = "declared type",
        [
            "0603_decimal_from_integers_add_decimal_nullable_group_by_key.jsonc"
        ] = "declared type",
        [
            "0605_decimal_from_integers_add_decimal_nullable_plain_select.jsonc"
        ] = "declared type",
        [
            "0607_decimal_from_integers_add_decimal_nullable_grouped_select.jsonc"
        ] = "declared type",
        [
            "0609_decimal_from_integers_add_decimal_nullable_total_query.jsonc"
        ] = "declared type",
        [
            "0624_decimal_from_integers_subtract_decimal_group_by_key.jsonc"
        ] = "declared type",
        [
            "0626_decimal_from_integers_subtract_decimal_plain_select.jsonc"
        ] = "declared type",
        [
            "0629_decimal_from_integers_subtract_decimal_grouped_select.jsonc"
        ] = "declared type",
        [
            "0632_decimal_from_integers_subtract_decimal_total_query.jsonc"
        ] = "declared type",
        [
            "0634_decimal_from_integers_subtract_decimal_nullable_group_by_key.jsonc"
        ] = "declared type",
        [
            "0636_decimal_from_integers_subtract_decimal_nullable_plain_select.jsonc"
        ] = "declared type",
        [
            "0638_decimal_from_integers_subtract_decimal_nullable_grouped_select.jsonc"
        ] = "declared type",
        [
            "0640_decimal_from_integers_subtract_decimal_nullable_total_query.jsonc"
        ] = "declared type",
        [
            "0656_decimal_from_integers_multiply_decimal_group_by_key.jsonc"
        ] = "declared type",
        [
            "0658_decimal_from_integers_multiply_decimal_plain_select.jsonc"
        ] = "declared type",
        [
            "0661_decimal_from_integers_multiply_decimal_grouped_select.jsonc"
        ] = "declared type",
        [
            "0663_decimal_from_integers_multiply_decimal_total_query.jsonc"
        ] = "declared type",
        [
            "0666_decimal_from_integers_multiply_decimal_nullable_group_by_key.jsonc"
        ] = "declared type",
        [
            "0668_decimal_from_integers_multiply_decimal_nullable_plain_select.jsonc"
        ] = "declared type",
        [
            "0670_decimal_from_integers_multiply_decimal_nullable_grouped_select.jsonc"
        ] = "declared type",
        [
            "0672_decimal_from_integers_multiply_decimal_nullable_total_query.jsonc"
        ] = "declared type",
        ["1250_decimal_from_integers_if_decimal_group_by_key.jsonc"] = "declared type",
        ["1254_decimal_from_integers_if_decimal_grouped_select.jsonc"] = "declared type",
        ["1257_decimal_from_integers_if_decimal_total_query.jsonc"] = "declared type",
        [
            "1260_decimal_from_integers_if_decimal_nullable_group_by_key.jsonc"
        ] = "declared type",
        [
            "1262_decimal_from_integers_if_decimal_nullable_plain_select.jsonc"
        ] = "declared type",
        [
            "1264_decimal_from_integers_if_decimal_nullable_grouped_select.jsonc"
        ] = "declared type",
        [
            "1266_decimal_from_integers_if_decimal_nullable_total_query.jsonc"
        ] = "declared type",
        [
            "1268_decimal_from_integers_coalesce_decimal_group_by_key.jsonc"
        ] = "declared type",
        [
            "1272_decimal_from_integers_coalesce_decimal_grouped_select.jsonc"
        ] = "declared type",
        [
            "1274_decimal_from_integers_coalesce_decimal_total_query.jsonc"
        ] = "declared type",
        [
            "1277_decimal_from_integers_coalesce_decimal_nullable_group_by_key.jsonc"
        ] = "declared type",
        [
            "1278_decimal_from_integers_coalesce_decimal_nullable_plain_select.jsonc"
        ] = "declared type",
        [
            "1279_decimal_from_integers_coalesce_decimal_nullable_grouped_select.jsonc"
        ] = "declared type",
        [
            "1280_decimal_from_integers_coalesce_decimal_nullable_total_query.jsonc"
        ] = "declared type",
        [
            "1451_declared_other_number_sum_integer_of_integer_total_query.jsonc"
        ] = "declared type",

        // dependentRequired, dependentSchemas: a pagination needs an orderBy, and a
        // subquery's orderBy needs a pagination or a distinctBy, but each is a property.
        ["0371_pagination_without_order_by.jsonc"] = "dependent",
        ["0372_subquery_pagination_without_order_by.jsonc"] = "dependent",
        ["0373_subquery_order_by_without_pagination.jsonc"] = "dependent",
        ["0552_pagination_no_order_by_main_grouped_query.jsonc"] = "dependent",
        ["0570_order_by_only_sub_grouped_query.jsonc"] = "dependent",
        ["0571_pagination_no_order_by_sub_grouped_query.jsonc"] = "dependent",

        // minimum: skip and take are plain long.
        ["0005_pagination_take_zero.jsonc"] = "minimum",
        ["0006_pagination_negative_skip.jsonc"] = "minimum",

        // NAME pattern: names, aliases and sources are plain strings.
        ["0031_field_empty_name.jsonc"] = "name",
        ["0120_column_empty_alias.jsonc"] = "name",
        ["0171_from_empty_entity.jsonc"] = "name",
        ["0173_from_empty_subquery.jsonc"] = "name",
        ["0178_join_empty_alias.jsonc"] = "name",
        ["0201_param_empty_name.jsonc"] = "name",
        ["0205_field_empty_source.jsonc"] = "name",
        ["0206_key_empty_name.jsonc"] = "name",
        ["0309_group_key_empty_alias.jsonc"] = "name",
        ["0313_subquery_empty_name.jsonc"] = "name",
        ["0336_whitespace_alias.jsonc"] = "name",
        ["0337_alias_trailing_space.jsonc"] = "name",
        ["0338_source_leading_tab.jsonc"] = "name",
        ["0339_param_name_with_line_break.jsonc"] = "name",
        ["0340_subquery_name_whitespace.jsonc"] = "name",
        ["0396_distinct_by_empty_alias.jsonc"] = "name",
        ["0578_group_by_item_empty_alias_sub_grouped_query.jsonc"] = "name",
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
            file =>
                Assert.Contains($"samples/invalid/{file}", SpecificationFiles.Invalid())
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
