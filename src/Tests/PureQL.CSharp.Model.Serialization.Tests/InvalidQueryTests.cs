using System.Diagnostics.CodeAnalysis;
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
        // operands only is not listed: the converters must reject it (#98).
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

    /// <summary>
    /// Invalid queries that declare a node nullable although its operands give it a
    /// non-null type. The model can represent them, but the schema defines every
    /// declared type as exact, so the converters must reject them; until they do (#98),
    /// their rejection is tested by a skipped theory.
    /// </summary>
    private static readonly HashSet<string> DeclaredNullableOverNonNull =
    [
        "0287_max_of_boolean_declared_nullable.jsonc",
        "0348_grouped_min_over_all_declared_nullable.jsonc",
        "0351_add_of_non_null_declared_nullable.jsonc",
        "0353_if_of_non_null_branches_declared_nullable.jsonc",
        "0355_coalesce_with_non_null_declared_nullable.jsonc",
        "0363_group_min_declared_nullable.jsonc",
        "0386_plain_min_declared_nullable.jsonc",
        "0691_declared_nullability_flipped_integer_divide_integer_group_by_key.jsonc",
        "0695_declared_nullability_flipped_integer_divide_integer_plain_select.jsonc",
        "0698_declared_nullability_flipped_integer_divide_integer_grouped_select.jsonc",
        "0702_declared_nullability_flipped_integer_divide_integer_total_query.jsonc",
        "0710_declared_nullability_flipped_modulo_integer_group_by_key.jsonc",
        "0714_declared_nullability_flipped_modulo_integer_plain_select.jsonc",
        "0717_declared_nullability_flipped_modulo_integer_grouped_select.jsonc",
        "0721_declared_nullability_flipped_modulo_integer_total_query.jsonc",
        "0729_declared_nullability_flipped_floor_integer_group_by_key.jsonc",
        "0733_declared_nullability_flipped_floor_integer_plain_select.jsonc",
        "0736_declared_nullability_flipped_floor_integer_grouped_select.jsonc",
        "0740_declared_nullability_flipped_floor_integer_total_query.jsonc",
        "0748_declared_nullability_flipped_ceiling_integer_group_by_key.jsonc",
        "0752_declared_nullability_flipped_ceiling_integer_plain_select.jsonc",
        "0756_declared_nullability_flipped_ceiling_integer_grouped_select.jsonc",
        "0760_declared_nullability_flipped_ceiling_integer_total_query.jsonc",
        "0768_declared_nullability_flipped_round_integer_group_by_key.jsonc",
        "0772_declared_nullability_flipped_round_integer_plain_select.jsonc",
        "0774_declared_nullability_flipped_round_integer_grouped_select.jsonc",
        "0778_declared_nullability_flipped_round_integer_total_query.jsonc",
        "0791_declared_nullability_flipped_round_decimal_group_by_key.jsonc",
        "0794_declared_nullability_flipped_round_decimal_plain_select.jsonc",
        "0797_declared_nullability_flipped_round_decimal_grouped_select.jsonc",
        "0800_declared_nullability_flipped_round_decimal_total_query.jsonc",
        "0824_declared_nullability_flipped_date_add_days_date_group_by_key.jsonc",
        "0828_declared_nullability_flipped_date_add_days_date_plain_select.jsonc",
        "0831_declared_nullability_flipped_date_add_days_date_grouped_select.jsonc",
        "0834_declared_nullability_flipped_date_add_days_date_total_query.jsonc",
        "0842_declared_nullability_flipped_date_diff_days_integer_group_by_key.jsonc",
        "0846_declared_nullability_flipped_date_diff_days_integer_plain_select.jsonc",
        "0848_declared_nullability_flipped_date_diff_days_integer_grouped_select.jsonc",
        "0852_declared_nullability_flipped_date_diff_days_integer_total_query.jsonc",
        "0860_declared_nullability_flipped_time_add_seconds_time_group_by_key.jsonc",
        "0864_declared_nullability_flipped_time_add_seconds_time_plain_select.jsonc",
        "0867_declared_nullability_flipped_time_add_seconds_time_grouped_select.jsonc",
        "0871_declared_nullability_flipped_time_add_seconds_time_total_query.jsonc",
        "0879_declared_nullability_flipped_time_diff_seconds_decimal_group_by_key.jsonc",
        "0883_declared_nullability_flipped_time_diff_seconds_decimal_plain_select.jsonc",
        "0885_declared_nullability_flipped_time_diff_seconds_decimal_grouped_"
            + "select.jsonc",
        "0889_declared_nullability_flipped_time_diff_seconds_decimal_total_query.jsonc",
        "0897_declared_nullability_flipped_datetime_add_seconds_datetime_group_by_"
            + "key.jsonc",
        "0901_declared_nullability_flipped_datetime_add_seconds_datetime_plain_"
            + "select.jsonc",
        "0904_declared_nullability_flipped_datetime_add_seconds_datetime_grouped_"
            + "select.jsonc",
        "0908_declared_nullability_flipped_datetime_add_seconds_datetime_total_"
            + "query.jsonc",
        "0916_declared_nullability_flipped_datetime_diff_seconds_decimal_group_by_"
            + "key.jsonc",
        "0920_declared_nullability_flipped_datetime_diff_seconds_decimal_plain_"
            + "select.jsonc",
        "0923_declared_nullability_flipped_datetime_diff_seconds_decimal_grouped_"
            + "select.jsonc",
        "0927_declared_nullability_flipped_datetime_diff_seconds_decimal_total_"
            + "query.jsonc",
        "1230_declared_nullability_flipped_if_integer_group_by_key.jsonc",
        "1232_declared_nullability_flipped_if_integer_plain_select.jsonc",
        "1233_declared_nullability_flipped_if_integer_grouped_select.jsonc",
        "1235_declared_nullability_flipped_if_integer_total_query.jsonc",
        "1241_declared_nullability_flipped_coalesce_integer_group_by_key.jsonc",
        "1242_declared_nullability_flipped_coalesce_integer_grouped_select.jsonc",
        "1244_declared_nullability_flipped_coalesce_integer_total_query.jsonc",
        "1251_declared_nullability_flipped_if_decimal_group_by_key.jsonc",
        "1253_declared_nullability_flipped_if_decimal_plain_select.jsonc",
        "1255_declared_nullability_flipped_if_decimal_grouped_select.jsonc",
        "1258_declared_nullability_flipped_if_decimal_total_query.jsonc",
        "1269_declared_nullability_flipped_coalesce_decimal_group_by_key.jsonc",
        "1270_declared_nullability_flipped_coalesce_decimal_plain_select.jsonc",
        "1273_declared_nullability_flipped_coalesce_decimal_grouped_select.jsonc",
        "1275_declared_nullability_flipped_coalesce_decimal_total_query.jsonc",
        "1281_declared_nullability_flipped_if_string_group_by_key.jsonc",
        "1283_declared_nullability_flipped_if_string_grouped_select.jsonc",
        "1285_declared_nullability_flipped_if_string_total_query.jsonc",
        "1291_declared_nullability_flipped_coalesce_string_group_by_key.jsonc",
        "1293_declared_nullability_flipped_coalesce_string_plain_select.jsonc",
        "1295_declared_nullability_flipped_coalesce_string_grouped_select.jsonc",
        "1297_declared_nullability_flipped_coalesce_string_total_query.jsonc",
        "1303_declared_nullability_flipped_if_boolean_group_by_key.jsonc",
        "1305_declared_nullability_flipped_if_boolean_plain_select.jsonc",
        "1307_declared_nullability_flipped_if_boolean_grouped_select.jsonc",
        "1309_declared_nullability_flipped_if_boolean_total_query.jsonc",
        "1316_declared_nullability_flipped_coalesce_boolean_group_by_key.jsonc",
        "1318_declared_nullability_flipped_coalesce_boolean_plain_select.jsonc",
        "1319_declared_nullability_flipped_coalesce_boolean_grouped_select.jsonc",
        "1320_declared_nullability_flipped_coalesce_boolean_total_query.jsonc",
        "1325_declared_nullability_flipped_if_date_group_by_key.jsonc",
        "1328_declared_nullability_flipped_if_date_plain_select.jsonc",
        "1330_declared_nullability_flipped_if_date_grouped_select.jsonc",
        "1332_declared_nullability_flipped_if_date_total_query.jsonc",
        "1338_declared_nullability_flipped_coalesce_date_group_by_key.jsonc",
        "1340_declared_nullability_flipped_coalesce_date_plain_select.jsonc",
        "1342_declared_nullability_flipped_coalesce_date_grouped_select.jsonc",
        "1346_declared_nullability_flipped_coalesce_date_total_query.jsonc",
        "1357_declared_nullability_flipped_if_time_group_by_key.jsonc",
        "1359_declared_nullability_flipped_if_time_plain_select.jsonc",
        "1361_declared_nullability_flipped_if_time_grouped_select.jsonc",
        "1364_declared_nullability_flipped_if_time_total_query.jsonc",
        "1370_declared_nullability_flipped_coalesce_time_group_by_key.jsonc",
        "1373_declared_nullability_flipped_coalesce_time_plain_select.jsonc",
        "1376_declared_nullability_flipped_coalesce_time_grouped_select.jsonc",
        "1379_declared_nullability_flipped_coalesce_time_total_query.jsonc",
        "1385_declared_nullability_flipped_if_datetime_group_by_key.jsonc",
        "1387_declared_nullability_flipped_if_datetime_plain_select.jsonc",
        "1389_declared_nullability_flipped_if_datetime_grouped_select.jsonc",
        "1391_declared_nullability_flipped_if_datetime_total_query.jsonc",
        "1397_declared_nullability_flipped_coalesce_datetime_group_by_key.jsonc",
        "1399_declared_nullability_flipped_coalesce_datetime_plain_select.jsonc",
        "1401_declared_nullability_flipped_coalesce_datetime_grouped_select.jsonc",
        "1403_declared_nullability_flipped_coalesce_datetime_total_query.jsonc",
        "1409_declared_nullability_flipped_if_uuid_group_by_key.jsonc",
        "1413_declared_nullability_flipped_if_uuid_plain_select.jsonc",
        "1415_declared_nullability_flipped_if_uuid_grouped_select.jsonc",
        "1418_declared_nullability_flipped_if_uuid_total_query.jsonc",
        "1426_declared_nullability_flipped_coalesce_uuid_group_by_key.jsonc",
        "1429_declared_nullability_flipped_coalesce_uuid_plain_select.jsonc",
        "1431_declared_nullability_flipped_coalesce_uuid_grouped_select.jsonc",
        "1433_declared_nullability_flipped_coalesce_uuid_total_query.jsonc",
        "1460_declared_nullability_flipped_average_date_of_date_plain_select.jsonc",
        "1463_declared_nullability_flipped_average_date_of_date_grouped_select.jsonc",
        "1471_declared_nullability_flipped_average_time_of_time_plain_select.jsonc",
        "1474_declared_nullability_flipped_average_time_of_time_grouped_select.jsonc",
        "1483_declared_nullability_flipped_average_datetime_of_datetime_plain_"
            + "select.jsonc",
        "1485_declared_nullability_flipped_average_datetime_of_datetime_grouped_"
            + "select.jsonc",
        "1500_declared_nullability_flipped_min_string_of_string_plain_select.jsonc",
        "1501_declared_nullability_flipped_min_string_of_string_grouped_select.jsonc",
        "1505_declared_nullability_flipped_min_boolean_of_boolean_plain_select.jsonc",
        "1507_declared_nullability_flipped_min_boolean_of_boolean_grouped_select.jsonc",
        "1514_declared_nullability_flipped_min_date_of_date_plain_select.jsonc",
        "1516_declared_nullability_flipped_min_date_of_date_grouped_select.jsonc",
        "1525_declared_nullability_flipped_min_time_of_time_plain_select.jsonc",
        "1528_declared_nullability_flipped_min_time_of_time_grouped_select.jsonc",
        "1536_declared_nullability_flipped_min_datetime_of_datetime_plain_select.jsonc",
        "1538_declared_nullability_flipped_min_datetime_of_datetime_grouped_select.jsonc",
        "1545_declared_nullability_flipped_min_uuid_of_uuid_plain_select.jsonc",
        "1547_declared_nullability_flipped_min_uuid_of_uuid_grouped_select.jsonc",
        "1558_declared_nullability_flipped_max_decimal_of_decimal_plain_select.jsonc",
        "1560_declared_nullability_flipped_max_decimal_of_decimal_grouped_select.jsonc",
        "1566_declared_nullability_flipped_max_string_of_string_plain_select.jsonc",
        "1568_declared_nullability_flipped_max_string_of_string_grouped_select.jsonc",
        "1572_declared_nullability_flipped_max_boolean_of_boolean_plain_select.jsonc",
        "1580_declared_nullability_flipped_max_date_of_date_plain_select.jsonc",
        "1582_declared_nullability_flipped_max_date_of_date_grouped_select.jsonc",
        "1591_declared_nullability_flipped_max_time_of_time_plain_select.jsonc",
        "1593_declared_nullability_flipped_max_time_of_time_grouped_select.jsonc",
        "1602_declared_nullability_flipped_max_datetime_of_datetime_plain_select.jsonc",
        "1604_declared_nullability_flipped_max_datetime_of_datetime_grouped_select.jsonc",
        "1611_declared_nullability_flipped_max_uuid_of_uuid_plain_select.jsonc",
        "1614_declared_nullability_flipped_max_uuid_of_uuid_grouped_select.jsonc",
        "1638_average_nullable_without_cause_average_plain_select.jsonc",
        "1639_average_nullable_without_cause_average_grouped_select.jsonc",
        "1645_min_nullable_without_cause_min_plain_select.jsonc",
        "1646_min_nullable_without_cause_min_grouped_select.jsonc",
        "1650_max_nullable_without_cause_max_plain_select.jsonc",
        "1651_max_nullable_without_cause_max_grouped_select.jsonc",
    ];

    [Theory]
    [MemberData(nameof(RejectedFiles))]
    public void InvalidQueryIsRejected(string name)
    {
        string input = SpecificationFiles.Read(name);

        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PureQLQuery>(input)
        );
    }

    [SuppressMessage("Usage", "xUnit1004", Justification = Issue98.Skip)]
    [Theory(Skip = Issue98.Skip)]
    [MemberData(nameof(DeclaredNullableFiles))]
    public void DeclaredNullableOverNonNullIsRejected(string name)
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
            AcceptedByModel.Keys.Concat(DeclaredNullableOverNonNull),
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

    public static TheoryData<string> DeclaredNullableFiles()
    {
        return SpecificationFiles.Data(
            SpecificationFiles
                .Invalid()
                .Where(name =>
                    DeclaredNullableOverNonNull.Contains(Path.GetFileName(name))
                )
        );
    }

    private static TheoryData<string> Select(bool accepted)
    {
        return SpecificationFiles.Data(
            SpecificationFiles
                .Invalid()
                .Where(name =>
                    !DeclaredNullableOverNonNull.Contains(Path.GetFileName(name))
                    && (AcceptedByModel.ContainsKey(Path.GetFileName(name)) == accepted)
                )
        );
    }
}
