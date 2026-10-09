using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.GroupKeys;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.SelectItems;

namespace PureQL.CSharp.Model.Serialization;

internal static class QueryRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.ByPresence(
            "groupBy",
            UnionCase.Of((MainGroupedQuery x) => new PureQLQuery(x)),
            UnionCase.Of((MainPlainQuery x) => new PureQLQuery(x))
        );
        registry.ByPresence(
            "groupBy",
            UnionCase.Of((GroupedQuery x) => new Query(x)),
            UnionCase.Of((PlainQuery x) => new Query(x))
        );
        registry.Record(
            r => new MainGroupedQuery(
                r.One<From>("from"),
                r.Many<GroupKey>("groupBy"),
                r.Many<SelectItemGroup>("select"),
                r.OptionalMany<Subquery>("subqueries"),
                r.OptionalMany<Join>("joins"),
                r.Optional<BooleanRow>("where"),
                r.Optional<BooleanGroup>("having"),
                r.OptionalMany<OrderItemGroup>("orderBy"),
                r.Optional<Pagination>("pagination"),
                r.Flag("distinct")
            ),
            (w, x) =>
            {
                w.OptionalMany("subqueries", x.Subqueries);
                w.One("from", x.From);
                w.OptionalMany("joins", x.Joins);
                w.Optional("where", x.Where);
                w.Many("groupBy", x.GroupBy);
                w.Optional("having", x.Having);
                w.Many("select", x.Select);
                w.Flag("distinct", x.Distinct);
                w.OptionalMany("orderBy", x.OrderBy);
                w.Optional("pagination", x.Pagination);
            }
        );
        registry.Record(
            r => new MainPlainQuery(
                r.One<From>("from"),
                r.Many<SelectItemProjection>("select"),
                r.OptionalMany<Subquery>("subqueries"),
                r.OptionalMany<Join>("joins"),
                r.Optional<BooleanRow>("where"),
                r.OptionalMany<OrderItemProjection>("orderBy"),
                r.Optional<Pagination>("pagination"),
                r.Flag("distinct")
            ),
            (w, x) =>
            {
                w.OptionalMany("subqueries", x.Subqueries);
                w.One("from", x.From);
                w.OptionalMany("joins", x.Joins);
                w.Optional("where", x.Where);
                w.Many("select", x.Select);
                w.Flag("distinct", x.Distinct);
                w.OptionalMany("orderBy", x.OrderBy);
                w.Optional("pagination", x.Pagination);
            }
        );
        registry.Record(
            r => new GroupedQuery(
                r.One<From>("from"),
                r.Many<GroupKey>("groupBy"),
                r.Many<SelectItemGroup>("select"),
                r.OptionalMany<Join>("joins"),
                r.Optional<BooleanRow>("where"),
                r.Optional<BooleanGroup>("having"),
                r.OptionalMany<OrderItemGroup>("orderBy"),
                r.Optional<Pagination>("pagination"),
                r.Flag("distinct")
            ),
            (w, x) =>
            {
                w.One("from", x.From);
                w.OptionalMany("joins", x.Joins);
                w.Optional("where", x.Where);
                w.Many("groupBy", x.GroupBy);
                w.Optional("having", x.Having);
                w.Many("select", x.Select);
                w.Flag("distinct", x.Distinct);
                w.OptionalMany("orderBy", x.OrderBy);
                w.Optional("pagination", x.Pagination);
            }
        );
        registry.Record(
            r => new PlainQuery(
                r.One<From>("from"),
                r.Many<SelectItemProjection>("select"),
                r.OptionalMany<Join>("joins"),
                r.Optional<BooleanRow>("where"),
                r.OptionalMany<OrderItemProjection>("orderBy"),
                r.Optional<Pagination>("pagination"),
                r.Flag("distinct")
            ),
            (w, x) =>
            {
                w.One("from", x.From);
                w.OptionalMany("joins", x.Joins);
                w.Optional("where", x.Where);
                w.Many("select", x.Select);
                w.Flag("distinct", x.Distinct);
                w.OptionalMany("orderBy", x.OrderBy);
                w.Optional("pagination", x.Pagination);
            }
        );
        registry.Record(
            r => new Subquery(r.String("name"), r.One<Query>("query")),
            (w, x) =>
            {
                w.String("name", x.Name);
                w.One("query", x.Query);
            }
        );
        RegisterSources(registry);
        registry.Record(
            r => new Pagination(r.Bound("skip"), r.Bound("take")),
            (w, x) =>
            {
                w.Bound("skip", x.Skip);
                w.Bound("take", x.Take);
            }
        );
        registry.Record(
            r => new OrderItemGroup(r.One<ValueGroup>("expression"), r.Direction()),
            (w, x) =>
            {
                w.One("expression", x.Expression);
                w.Direction(x.Direction);
            }
        );
        registry.Record(
            r => new OrderItemProjection(
                r.One<ValueProjection>("expression"),
                r.Direction()
            ),
            (w, x) =>
            {
                w.One("expression", x.Expression);
                w.Direction(x.Direction);
            }
        );
    }

    private static void RegisterSources(ModelRegistry registry)
    {
        registry.ByPresence(
            "subquery",
            UnionCase.Of((FromSubquery x) => new From(x)),
            UnionCase.Of((FromEntity x) => new From(x))
        );
        registry.Record(
            r => new FromEntity(r.String("entity"), r.OptionalString("alias")),
            (w, x) =>
            {
                w.String("entity", x.Entity);
                w.OptionalString("alias", x.Alias);
            }
        );
        registry.Record(
            r => new FromSubquery(r.String("subquery"), r.OptionalString("alias")),
            (w, x) =>
            {
                w.String("subquery", x.Subquery);
                w.OptionalString("alias", x.Alias);
            }
        );
        registry.ByPresence(
            "subquery",
            UnionCase.Of((JoinSubquery x) => new Join(x)),
            UnionCase.Of((JoinEntity x) => new Join(x))
        );
        registry.Record(
            r => new JoinEntity(
                r.JoinType(),
                r.String("entity"),
                r.One<BooleanRow>("on"),
                r.OptionalString("alias")
            ),
            (w, x) =>
            {
                w.JoinType(x.Type);
                w.String("entity", x.Entity);
                w.OptionalString("alias", x.Alias);
                w.One("on", x.On);
            }
        );
        registry.Record(
            r => new JoinSubquery(
                r.JoinType(),
                r.String("subquery"),
                r.One<BooleanRow>("on"),
                r.OptionalString("alias")
            ),
            (w, x) =>
            {
                w.JoinType(x.Type);
                w.String("subquery", x.Subquery);
                w.OptionalString("alias", x.Alias);
                w.One("on", x.On);
            }
        );
    }
}
