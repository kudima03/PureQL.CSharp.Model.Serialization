namespace PureQL.CSharp.Model.Serialization;

/// <summary>The clauses whose presence chooses the variant of a query.</summary>
[Flags]
internal enum QueryClauses
{
    None = 0,
    GroupBy = 1,
    Aggregate = 2,
    Pagination = 4,
    OrderBy = 8,
}
