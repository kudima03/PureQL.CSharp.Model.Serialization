using System.Text.Json;
using PureQL.CSharp.Model.Serialization.Literals;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The readers and writers of every model type. They are generated (see
/// <c>Generated/</c>) except here: the unions chosen by which properties are present
/// rather than by a signature, as the schema's dependencies choose them, and the
/// pagination bounds, which are a number or a parameter.
/// </summary>
internal static partial class ModelSerializers
{
    /// <summary>
    /// A main query: grouped with <c>groupBy</c>, else total with <c>aggregate</c>, else
    /// plain; a paginated one requires <c>orderBy</c>.
    /// </summary>
    internal static PureQLQuery ReadPureQLQuery(
        ref Utf8JsonReader reader,
        ReadContext context
    )
    {
        QueryClauses clauses = context.Clauses(ref reader);
        bool paginated = clauses.HasFlag(QueryClauses.Pagination);
        return clauses.HasFlag(QueryClauses.GroupBy)
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
        return clauses.HasFlag(QueryClauses.GroupBy)
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

    internal static From ReadFrom(ref Utf8JsonReader reader, ReadContext context)
    {
        return context.Has(ref reader, "subquery"u8)
            ? new From(ReadFromSubquery(ref reader, context))
            : new From(ReadFromEntity(ref reader, context));
    }

    internal static Join ReadJoin(ref Utf8JsonReader reader, ReadContext context)
    {
        return context.Has(ref reader, "subquery"u8)
            ? new Join(ReadJoinSubquery(ref reader, context))
            : new Join(ReadJoinEntity(ref reader, context));
    }

    internal static PaginationSkip ReadPaginationSkip(
        ref Utf8JsonReader reader,
        ReadContext context
    )
    {
        return reader.TokenType == JsonTokenType.Number
            ? new PaginationSkip(LiteralValues.ReadInteger(ref reader, context))
            : new PaginationSkip(ReadParamInteger(ref reader, context));
    }

    internal static PaginationTake ReadPaginationTake(
        ref Utf8JsonReader reader,
        ReadContext context
    )
    {
        return reader.TokenType == JsonTokenType.Number
            ? new PaginationTake(LiteralValues.ReadInteger(ref reader, context))
            : new PaginationTake(ReadParamInteger(ref reader, context));
    }

    internal static void WritePaginationSkip(Utf8JsonWriter writer, PaginationSkip value)
    {
        WriteBound(writer, value.Value);
    }

    internal static void WritePaginationTake(Utf8JsonWriter writer, PaginationTake value)
    {
        WriteBound(writer, value.Value);
    }

    private static void WriteBound(Utf8JsonWriter writer, object? bound)
    {
        if (bound is long number)
        {
            writer.WriteNumberValue(number);
        }
        else
        {
            Writes.Record(writer, bound);
        }
    }
}
