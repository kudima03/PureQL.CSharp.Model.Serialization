using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The readers and writers of every model type. They are generated (see
/// <c>Generated/</c>) except here: the queries, chosen by which clauses are present as
/// the schema's conditions and dependencies choose them, windows, chosen by the type of
/// their sort key too, and the constant <c>"all"</c>.
/// </summary>
internal static partial class ModelSerializers
{
    private static readonly JsonEncodedText AllText = JsonEncodedText.Encode("all");

    /// <summary>
    /// A main query: combined with a set operation, else grouped with <c>groupBy</c>,
    /// else total with <c>aggregate</c>, else plain; a paginated one requires
    /// <c>orderBy</c>.
    /// </summary>
    internal static PureQLQuery ReadPureQLQuery(
        ref Utf8JsonReader reader,
        ReadContext context
    )
    {
        QueryClauses clauses = context.Clauses(ref reader);
        bool paginated = clauses.HasFlag(QueryClauses.Pagination);
        return clauses.HasFlag(QueryClauses.UnionAll)
                ? new PureQLQuery(ReadMainCombinedQueryUnionAll(ref reader, context))
            : clauses.HasFlag(QueryClauses.Union)
                ? new PureQLQuery(ReadMainCombinedQueryUnion(ref reader, context))
            : clauses.HasFlag(QueryClauses.Intersect)
                ? new PureQLQuery(ReadMainCombinedQueryIntersect(ref reader, context))
            : clauses.HasFlag(QueryClauses.Except)
                ? new PureQLQuery(ReadMainCombinedQueryExcept(ref reader, context))
            : clauses.HasFlag(QueryClauses.GroupBy)
                ? paginated
                    ? new PureQLQuery(ReadMainGroupedQueryWithPagination(ref reader, context))
                    : new PureQLQuery(
                        ReadMainGroupedQueryWithoutPagination(ref reader, context)
                    )
            : clauses.HasFlag(QueryClauses.Aggregate)
                ? new PureQLQuery(ReadMainTotalQuery(ref reader, context))
            : paginated ? new PureQLQuery(ReadMainPlainQueryWithPagination(ref reader, context))
            : new PureQLQuery(ReadMainPlainQueryWithoutPagination(ref reader, context));
    }

    /// <summary>
    /// A subquery, chosen as a main query is, where <c>orderBy</c> also requires
    /// <c>pagination</c> or <c>distinctBy</c>: without a pagination it is the variant
    /// with an <c>orderBy</c>, which requires a <c>distinctBy</c>.
    /// </summary>
    internal static Query ReadQuery(ref Utf8JsonReader reader, ReadContext context)
    {
        QueryClauses clauses = context.Clauses(ref reader);
        bool paginated = clauses.HasFlag(QueryClauses.Pagination);
        bool ordered = clauses.HasFlag(QueryClauses.OrderBy);
        return clauses.HasFlag(QueryClauses.UnionAll)
                ? new Query(ReadCombinedQueryUnionAll(ref reader, context))
            : clauses.HasFlag(QueryClauses.Union)
                ? new Query(ReadCombinedQueryUnion(ref reader, context))
            : clauses.HasFlag(QueryClauses.Intersect)
                ? new Query(ReadCombinedQueryIntersect(ref reader, context))
            : clauses.HasFlag(QueryClauses.Except)
                ? new Query(ReadCombinedQueryExcept(ref reader, context))
            : clauses.HasFlag(QueryClauses.GroupBy)
                ? paginated ? new Query(ReadGroupedQueryWithPagination(ref reader, context))
                : ordered
                    ? new Query(
                        ReadGroupedQueryWithoutPaginationWithOrderBy(ref reader, context)
                    )
                : new Query(
                    ReadGroupedQueryWithoutPaginationWithoutOrderBy(ref reader, context)
                )
            : clauses.HasFlag(QueryClauses.Aggregate)
                ? new Query(ReadTotalQuery(ref reader, context))
            : paginated ? new Query(ReadPlainQueryWithPagination(ref reader, context))
            : ordered
                ? new Query(ReadPlainQueryWithoutPaginationWithOrderBy(ref reader, context))
            : new Query(ReadPlainQueryWithoutPaginationWithoutOrderBy(ref reader, context));
    }

    /// <summary>
    /// A window: with a <c>frame</c> over rows, with a <c>range</c> over the values of
    /// its one sort key, whose declared type chooses the distances, or a partition
    /// alone.
    /// </summary>
    internal static Window ReadWindow(ref Utf8JsonReader reader, ReadContext context)
    {
        if (context.Has(ref reader, "frame"u8))
        {
            return new Window(ReadWindowWithFrame(ref reader, context));
        }

        if (!context.Has(ref reader, "range"u8))
        {
            return new Window(ReadWindowWithoutFrameWithoutRange(ref reader, context));
        }

        int key = context.RangeKey(ref reader) & ~TypeIds.NullableBit;
        return key switch
        {
            // Without a sort key that declares a type, the reader reports what is
            // missing.
            TypeIds.None or TypeIds.Integer => new Window(
                ReadWindowWithoutFrameWithRangeInteger(ref reader, context)
            ),
            TypeIds.Decimal => new Window(
                ReadWindowWithoutFrameWithRangeDecimal(ref reader, context)
            ),
            TypeIds.Date => new Window(
                ReadWindowWithoutFrameWithRangeDate(ref reader, context)
            ),
            TypeIds.Datetime => new Window(
                ReadWindowWithoutFrameWithRangeDatetime(ref reader, context)
            ),
            _ => throw context.Error(
                "A range window needs a sort key that declares an integer, a decimal, a "
                    + "date or a datetime"
            ),
        };
    }

    /// <summary>The constant <c>"all"</c>: every row on one side of a window.</summary>
    internal static All ReadAll(ref Utf8JsonReader reader, ReadContext context)
    {
        return context.All(ref reader);
    }

    internal static void WriteAll(Utf8JsonWriter writer, All value)
    {
        _ = Writes.NotNull(value);
        writer.WriteStringValue(AllText);
    }
}
