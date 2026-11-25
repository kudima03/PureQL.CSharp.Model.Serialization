using System.Text.Json;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.RowExpressions;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// The rules of the schema on values that the model's types do not express (names,
/// bare numbers, the literals of restricted positions, zones, windows), checked on read
/// and on write; and the choice of the variants that are told apart by more than their
/// declared type.
/// </summary>
public sealed record SpecificationRuleTests
{
    private const string Datetime = /*lang=json,strict*/
        """{"source":"o","field":"at","type":{"name":"datetime"}}""";

    private const string Integer = /*lang=json,strict*/
        """{"source":"o","field":"n","type":{"name":"integer"}}""";

    [Theory]
    [InlineData("")]
    [InlineData(" o")]
    [InlineData("o\t")]
    [InlineData("o\nrders")]
    [InlineData("o\r")]
    public void NamesFollowTheNamePattern(string name)
    {
        string json = JsonSerializer.Serialize(name);

        JsonException read = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<FieldDecimal>(
                $$$"""{"source":{{{json}}},"field":"f","type":{"name":"decimal"}}"""
            )
        );
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Serialize(new FieldDecimal(name, "f"))
        );
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Serialize(new FromEntity("orders", name))
        );
        Assert.EndsWith("(at $.source)", read.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void NamesMayHoldInnerSpaces()
    {
        FieldDecimal field = PureQLJson.Deserialize<FieldDecimal>(
            /*lang=json,strict*/
            """{"source":"order items","field":"f","type":{"name":"decimal"}}"""
        );

        Assert.Equal("order items", field.Source);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    public void BareNumbersStartFromOne(string number)
    {
        _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<FrameRows>(number));
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<Pagination>($$"""{"skip":{{number}}}""")
        );
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Serialize(new Pagination(new PaginationSkip(long.Parse(number))))
        );
    }

    [Fact]
    public void AllIsTheOneConstant()
    {
        _ = Assert.IsType<All>(PureQLJson.Deserialize<FrameRows>("\"all\"").Value);
        Assert.Equal("\"all\"", PureQLJson.Serialize(new FrameRows(new All())));
        Assert.Equal(3L, PureQLJson.Deserialize<FrameRows>("3").Value);
        _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<FrameRows>("\"none\""));
        _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<All>("\"All\""));
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PaginationSkip>("\"all\"")
        );
    }

    [Theory]
    [InlineData("{}")]
    [InlineData( /*lang=json,strict*/
        """{"preceding":"all","following":"all"}"""
    )]
    public void FramesHaveOneSideThatIsNotTheWholePartition(string json)
    {
        _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<Frame>(json));
        _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<RangeDate>(json));
    }

    [Fact]
    public void RecordsOfOptionalPropertiesRequireOneOnWrite()
    {
        FrameRows all = new All();

        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(new Pagination()));
        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(new Frame()));
        _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(new Frame(all, all)));
        Assert.Equal(
            /*lang=json,strict*/
            """{"preceding":"all","following":1}""",
            PureQLJson.Serialize(new Frame(all, new FrameRows(1L)))
        );
    }

    [Fact]
    public void PaginationIsReadEmptyNever()
    {
        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<Pagination>("{}")
        );

        Assert.Equal("Expected at least one of skip, take (at $)", exception.Message);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public void OffsetsStartFromTwo(long value, bool valid)
    {
        Check(
            $$"""{"type":{"name":"integer"},"value":{{value}}}""",
            new Offset(new LiteralInteger(value)),
            valid
        );
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void BucketsAndGroupsStartFromOne(long value, bool valid)
    {
        string json = $$"""{"type":{"name":"integer"},"value":{{value}}}""";

        Check(json, new Buckets(new LiteralInteger(value)), valid);
        Check(json, new RegexGroup(new LiteralInteger(value)), valid);
    }

    [Theory]
    [InlineData("0", true)]
    [InlineData("0.25", true)]
    [InlineData("1", true)]
    [InlineData("1.5", false)]
    [InlineData("-0.5", false)]
    public void FractionsAreFromZeroToOne(string value, bool valid)
    {
        Check(
            $$"""{"type":{"name":"decimal"},"value":"{{value}}"}""",
            new Fraction(new LiteralDecimal(decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture))),
            valid
        );
    }

    [Theory]
    [InlineData("0.5", true)]
    [InlineData("3", true)]
    [InlineData("0", false)]
    [InlineData("-2", false)]
    public void DistancesArePositive(string value, bool valid)
    {
        decimal distance = decimal.Parse(
            value,
            System.Globalization.CultureInfo.InvariantCulture
        );
        string json = $$"""{"type":{"name":"decimal"},"value":"{{value}}"}""";

        Check(json, new DistanceDecimal(new LiteralDecimal(distance)), valid);
        Check(
            json,
            new DistanceDatetimeSecondsSeconds(new LiteralDecimal(distance)),
            valid
        );
    }

    [Theory]
    [InlineData("^[a-z]+(-[0-9]{2,})?$", true)]
    [InlineData("(?:a|b)*\\.csv", true)]
    [InlineData("[^\\]x]+\\b", true)]
    [InlineData("(?=a)", false)]
    [InlineData("(?<name>a)", false)]
    [InlineData("\\1", false)]
    [InlineData("a++", false)]
    [InlineData("\\p{L}", false)]
    [InlineData("a\nb", false)]
    public void RegularExpressionsFollowTheDialect(string pattern, bool valid)
    {
        Check(
            $$"""{"type":{"name":"string"},"value":{{JsonSerializer.Serialize(pattern)}}}""",
            new RegexPattern(new LiteralString(pattern)),
            valid
        );
    }

    [Fact]
    public void ParametersOfRestrictedPositionsAreCheckedWhenBound()
    {
        Check(
            /*lang=json,strict*/
            """{"param":"offset","type":{"name":"integer"}}""",
            new Offset(new ParamInteger("offset")),
            true
        );
    }

    [Theory]
    [InlineData("Europe/Moscow", true)]
    [InlineData("America/Argentina/Buenos_Aires", true)]
    [InlineData("Etc/GMT-14", true)]
    [InlineData("UTC", false)]
    [InlineData("US/Eastern", false)]
    [InlineData("+03:00", false)]
    [InlineData("europe/moscow", false)]
    public void LiteralZonesAreTimeZoneNames(string zone, bool valid)
    {
        string json =
            $$$"""{"operator":"year","type":{"name":"integer"},"operand":{{{Datetime}}},"zone":{"type":{"name":"string"},"value":"{{{zone}}}"}}""";
        YearDatetimeIntegerRow year = new YearDatetimeIntegerRow(
            new FieldDatetime("o", "at"),
            new LiteralString(zone)
        );

        Check(json, year, valid);
    }

    [Fact]
    public void ComputedZonesAreAccepted()
    {
        string json =
            $$$$"""{"operator":"year","type":{"name":"integer"},"operand":{{{{Datetime}}}},"zone":{"source":"u","field":"zone","type":{"name":"string"}}}""";

        _ = Assert.IsType<YearDatetimeIntegerRow>(
            PureQLJson.Deserialize<IntegerRow>(json).Value
        );
    }

    [Fact]
    public void ADatePartOfADateHasNoZone()
    {
        string json = /*lang=json,strict*/
            """{"operator":"year","type":{"name":"integer"},"operand":{"source":"o","field":"d","type":{"name":"date"}},"zone":{"type":{"name":"string"},"value":"Europe/Moscow"}}""";

        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<IntegerRow>(json)
        );

        Assert.Equal("Unexpected property 'zone' (at $)", exception.Message);
    }

    [Theory]
    [InlineData("integer", typeof(AsDecimalIntegerDecimalRow))]
    [InlineData("string", typeof(AsDecimalStringDecimalRow))]
    public void AsDecimalIsChosenByTheTypeOfItsOperand(string operand, Type expected)
    {
        DecimalRow conversion = PureQLJson.Deserialize<DecimalRow>(
            $$$"""{"operand":{"source":"o","field":"f","type":{"name":"{{{operand}}}"}},"operator":"asDecimal","type":{"name":"decimal"}}"""
        );

        Assert.IsType(expected, conversion.Value);
    }

    [Fact]
    public void AsDecimalOfADecimalIsRejected()
    {
        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<DecimalRow>(
                /*lang=json,strict*/
                """{"operator":"asDecimal","type":{"name":"decimal"},"operand":{"source":"o","field":"f","type":{"name":"decimal"}}}"""
            )
        );

        Assert.Equal(
            "DecimalRow does not accept 'asDecimal decimal' of decimal (at $)",
            exception.Message
        );
    }

    [Fact]
    public void AsStringOfAnIntegerIsOfTheDecimalFamily()
    {
        StringRow conversion = PureQLJson.Deserialize<StringRow>(
            $$"""{"operator":"asString","type":{"name":"string"},"operand":{{Integer}}}"""
        );

        _ = Assert.IsType<AsStringDecimalStringRow>(conversion.Value);
    }

    [Fact]
    public void RangeWindowsAreChosenByTheTypeOfTheirSortKey()
    {
        Window date = PureQLJson.Deserialize<Window>(
            /*lang=json,strict*/
            """{"orderBy":[{"expression":{"source":"o","field":"d","type":{"name":"date","nullable":true}}}],"range":{"preceding":{"months":1}}}"""
        );
        Window number = PureQLJson.Deserialize<Window>(
            $$"""{"range":{"following":"all"},"orderBy":[{"expression":{{Integer}},"direction":"desc"}]}"""
        );

        _ = Assert.IsType<WindowWithoutFrameWithRangeDate>(date.Value);
        _ = Assert.IsType<WindowWithoutFrameWithRangeInteger>(number.Value);
    }

    [Theory]
    [InlineData( /*lang=json,strict*/
        """{"orderBy":[{"expression":{"source":"o","field":"s","type":{"name":"string"}}}],"range":{"preceding":1}}""",
        "A range window needs a sort key that declares an integer, a decimal, a date or a datetime (at $)"
    )]
    [InlineData( /*lang=json,strict*/
        """{"orderBy":[{"expression":{"source":"o","field":"n","type":{"name":"integer"}}},{"expression":{"source":"o","field":"n","type":{"name":"integer"}}}],"range":{"preceding":1}}""",
        "Expected at most 1 item, found 2 (at $.orderBy)"
    )]
    [InlineData( /*lang=json,strict*/
        """{"range":{"preceding":1}}""",
        "Missing property 'orderBy' (at $)"
    )]
    [InlineData( /*lang=json,strict*/
        """{"orderBy":[{"expression":{"source":"o","field":"n","type":{"name":"integer"}}}]}""",
        "Unexpected property 'orderBy' (at $)"
    )]
    [InlineData( /*lang=json,strict*/
        """{"orderBy":[{"expression":{"source":"o","field":"d","type":{"name":"date"}}}],"range":{"preceding":{"seconds":1}}}""",
        "Unexpected property 'seconds' (at $.range.preceding)"
    )]
    public void RangeWindowsHaveOneSortKeyOfARangeType(string json, string message)
    {
        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<Window>(json)
        );

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void PreviousIsNullableWithoutADefault()
    {
        string over = $$"""{"orderBy":[{"expression":{{Integer}}}]}""";
        string nullable =
            $$"""{"operator":"previous","type":{"name":"integer","nullable":true},"operand":{{Integer}},"over":{{over}}}""";
        string defaulted =
            $$"""{"operator":"previous","type":{"name":"integer","nullable":true},"operand":{{Integer}},"default":{"type":{"name":"integer"},"value":0},"over":{{over}}}""";

        _ = Assert.IsType<PreviousIntegerNullableProjection>(
            PureQLJson.Deserialize<ExprProjection>(nullable).Value
        );
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<ExprProjection>(defaulted)
        );
    }

    [Fact]
    public void InlineRowsAreListsOfLists()
    {
        string json = /*lang=json,strict*/
            """{"name":"tiers","rows":[[{"alias":"tier","expression":{"type":{"name":"string"},"value":"gold"}}],[]]}""";

        JsonException exception = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<Subquery>(json)
        );

        Assert.Equal("Expected at least 1 item, found 0 (at $.rows[1])", exception.Message);
        Subquery rows = PureQLJson.Deserialize<Subquery>(json.Replace(",[]", "", StringComparison.Ordinal));
        _ = Assert.Single(Assert.IsType<SubqueryRows>(rows.Value).Rows);
    }

    [Theory]
    [InlineData("unionAll", typeof(CombinedQueryUnionAll))]
    [InlineData("union", typeof(CombinedQueryUnion))]
    [InlineData("intersect", typeof(CombinedQueryIntersect))]
    [InlineData("except", typeof(CombinedQueryExcept))]
    public void CombinedQueriesAreChosenByTheirOperation(string operation, Type expected)
    {
        string plain = /*lang=json,strict*/
            """{"from":{"entity":"users"},"select":[{"alias":"id","expression":{"source":"users","field":"id","type":{"name":"uuid"}}}]}""";

        Query query = PureQLJson.Deserialize<Query>(
            $$"""{"{{operation}}":[{{plain}},{{plain}}]}"""
        );

        Assert.IsType(expected, query.Value);
    }

    [Fact]
    public void ACombinedQueryHasOneOperation()
    {
        string plain = /*lang=json,strict*/
            """{"from":{"entity":"users"},"select":[{"alias":"id","expression":{"source":"users","field":"id","type":{"name":"uuid"}}}]}""";

        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PureQLQuery>(
                $$"""{"union":[{{plain}},{{plain}}],"except":[{{plain}},{{plain}}]}"""
            )
        );
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<PureQLQuery>($$"""{"union":[{{plain}}]}""")
        );
    }

    [Fact]
    public void UnknownEnumValuesAreRejectedOnWrite()
    {
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Serialize(
                new RoundWithoutDigitsIntegerRow(new FieldDecimal("o", "f"), (RoundingMode)7)
            )
        );
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Serialize(
                new NormalizeStringRow(new FieldString("o", "f"), (NormalizationForm)9)
            )
        );
    }

    [Theory]
    [InlineData("NFC", NormalizationForm.NFC)]
    [InlineData("NFKD", NormalizationForm.NFKD)]
    public void NormalizationFormsAreNames(string name, NormalizationForm form)
    {
        string json =
            $$$"""{"operator":"normalize","type":{"name":"string"},"operand":{"source":"o","field":"f","type":{"name":"string"}},"form":"{{{name}}}"}""";

        NormalizeStringRow normalize = PureQLJson.Deserialize<NormalizeStringRow>(json);

        Assert.Equal(form, normalize.Form);
        Assert.Equal(json, PureQLJson.Serialize(normalize));
        _ = Assert.Throws<JsonException>(() =>
            PureQLJson.Deserialize<NormalizeStringRow>(
                json.Replace(name, name.ToLowerInvariant(), StringComparison.Ordinal)
            )
        );
    }

    /// <summary>
    /// Reads and writes a value of a restricted position, which either both accept or
    /// both reject.
    /// </summary>
    private static void Check<T>(string json, T value, bool valid)
    {
        if (valid)
        {
            Assert.Equal(json, PureQLJson.Serialize(PureQLJson.Deserialize<T>(json)));
            Assert.Equal(json, PureQLJson.Serialize(value));
        }
        else
        {
            _ = Assert.Throws<JsonException>(() => PureQLJson.Deserialize<T>(json));
            _ = Assert.Throws<JsonException>(() => PureQLJson.Serialize(value));
        }
    }
}
