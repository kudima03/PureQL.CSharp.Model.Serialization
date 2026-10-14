using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.GroupKeys;
using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.SelectItems;
using PureQL.CSharp.Model.Serialization.Literals;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization;

internal sealed partial class ModelRegistry
{
    public void Field<T>(IType type, Func<string, string, T> create)
        where T : class, IField
    {
        Leaf(
            [Discriminators.Leaf("field", type.Name, type.Nullable)],
            r =>
            {
                r.Skip("type");
                return create(r.String("source"), r.String("field"));
            },
            (w, x) =>
            {
                w.String("source", x.Source);
                w.String("field", x.Field);
                w.Type(type);
            }
        );
    }

    /// <summary>A scalar or list parameter.</summary>
    public void Param<T>(IType type, Func<string, T> create)
        where T : class, IParameter
    {
        Leaf(
            [Discriminators.Leaf("param", type.Name, type.Nullable)],
            r =>
            {
                r.Skip("type");
                return create(r.String("param"));
            },
            (w, x) =>
            {
                w.String("param", x.Name);
                w.Type(type);
            }
        );
    }

    public void Key<T>(IType type, Func<string, T> create)
        where T : class, IKey
    {
        Leaf(
            [Discriminators.Leaf("key", type.Name, type.Nullable)],
            r =>
            {
                r.Skip("type");
                return create(r.String("key"));
            },
            (w, x) =>
            {
                w.String("key", x.Key);
                w.Type(type);
            }
        );
    }

    public void Literal<T, TValue>(
        IType type,
        LiteralValue<TValue> literal,
        Func<TValue, T> create,
        Func<T, TValue> value
    )
        where T : class, ILiteral
    {
        Leaf(
            [Discriminators.Leaf("literal", type.Name, type.Nullable)],
            r =>
            {
                r.Skip("type");
                return create(r.Value(literal));
            },
            (w, x) =>
            {
                w.Type(type);
                w.Value(literal, value(x));
            }
        );
    }

    /// <summary>
    /// A typed null: <c>{ "type": { …, "nullable": true }, "value": null }</c>.
    /// </summary>
    public void NullLiteral<T>(IType type, Func<T> create)
        where T : class, ILiteral
    {
        Leaf(
            [Discriminators.Leaf("literal", type.Name, type.Nullable)],
            r =>
            {
                r.Skip("type");
                r.NullValue();
                return create();
            },
            (w, _) =>
            {
                w.Type(type);
                w.NullValue();
            }
        );
    }

    public void ListLiteral<T, TValue>(
        IType type,
        LiteralValue<TValue> literal,
        Func<IEnumerable<TValue>, T> create,
        Func<T, IEnumerable<TValue>> values
    )
        where T : class, ILiteral
    {
        Leaf(
            [Discriminators.Leaf("literal", type.Name, type.Nullable)],
            r =>
            {
                r.Skip("type");
                return create(r.Values(literal));
            },
            (w, x) =>
            {
                w.Type(type);
                w.Values(literal, values(x));
            }
        );
    }

    /// <summary>
    /// A subquery column used as a list, <c>{ subquery, field, type }</c>, where the
    /// column type may be nullable.
    /// </summary>
    public void Column<T>(
        IType type,
        Func<string, string, bool, T> create,
        Func<T, (string Subquery, string Field, bool Nullable)> column
    )
        where T : class
    {
        Leaf(
            [
                Discriminators.Leaf("column", type.Name, false),
                Discriminators.Leaf("column", type.Name, true),
            ],
            r => create(r.String("subquery"), r.String("field"), r.NullableType()),
            (w, x) =>
            {
                (string subquery, string field, bool nullable) = column(x);
                w.String("subquery", subquery);
                w.String("field", field);
                w.Type(type.Name, nullable);
            }
        );
    }

    /// <summary>
    /// A column <c>{ alias, expression }</c>, which has the type its expression
    /// declares.
    /// </summary>
    public void SelectItem<T, TExpression>(
        IType type,
        Func<string, TExpression, T> create,
        Func<T, TExpression> expression
    )
        where T : class, ISelectItem
        where TExpression : class
    {
        Leaf(
            [Discriminators.Leaf("item", type.Name, type.Nullable)],
            r => create(r.String("alias"), r.One<TExpression>("expression")),
            (w, x) =>
            {
                w.String("alias", x.Alias);
                w.One("expression", expression(x));
            }
        );
    }

    /// <summary>
    /// A group key <c>{ alias, expression }</c>, which has the type its expression
    /// declares.
    /// </summary>
    public void GroupKey<T, TExpression>(
        IType type,
        Func<string, TExpression, T> create,
        Func<T, TExpression> expression
    )
        where T : class, IGroupKey
        where TExpression : class
    {
        Leaf(
            [Discriminators.Leaf("item", type.Name, type.Nullable)],
            r => create(r.String("alias"), r.One<TExpression>("expression")),
            (w, x) =>
            {
                w.String("alias", x.Alias);
                w.One("expression", expression(x));
            }
        );
    }
}
