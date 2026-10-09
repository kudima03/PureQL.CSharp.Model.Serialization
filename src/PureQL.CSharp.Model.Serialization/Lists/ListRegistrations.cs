using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Serialization.Literals;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Lists;

internal static class ListRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((ListLiteralBoolean x) => new ListBoolean(x)),
            UnionCase.Of((ListParamBoolean x) => new ListBoolean(x)),
            UnionCase.Of((ListSubqueryColumnBoolean x) => new ListBoolean(x))
        );
        registry.Union(
            UnionCase.Of((ListLiteralDate x) => new ListDate(x)),
            UnionCase.Of((ListParamDate x) => new ListDate(x)),
            UnionCase.Of((ListSubqueryColumnDate x) => new ListDate(x))
        );
        registry.Union(
            UnionCase.Of((ListLiteralDatetime x) => new ListDatetime(x)),
            UnionCase.Of((ListParamDatetime x) => new ListDatetime(x)),
            UnionCase.Of((ListSubqueryColumnDatetime x) => new ListDatetime(x))
        );
        registry.Union(
            UnionCase.Of((ListLiteralDecimal x) => new ListDecimal(x)),
            UnionCase.Of((ListParamDecimal x) => new ListDecimal(x)),
            UnionCase.Of((ListSubqueryColumnDecimal x) => new ListDecimal(x)),
            UnionCase.Of((ListInteger x) => new ListDecimal(x))
        );
        registry.Union(
            UnionCase.Of((ListLiteralInteger x) => new ListInteger(x)),
            UnionCase.Of((ListParamInteger x) => new ListInteger(x)),
            UnionCase.Of((ListSubqueryColumnInteger x) => new ListInteger(x))
        );
        registry.ListLiteral(
            new TypeBooleanList(),
            LiteralValues.Boolean,
            v => new ListLiteralBoolean(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeDateList(),
            LiteralValues.Date,
            v => new ListLiteralDate(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeDatetimeList(),
            LiteralValues.Datetime,
            v => new ListLiteralDatetime(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeDecimalList(),
            LiteralValues.Decimal,
            v => new ListLiteralDecimal(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeIntegerList(),
            LiteralValues.Integer,
            v => new ListLiteralInteger(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeStringList(),
            LiteralValues.String,
            v => new ListLiteralString(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeTimeList(),
            LiteralValues.Time,
            v => new ListLiteralTime(v),
            x => x.Value
        );
        registry.ListLiteral(
            new TypeUuidList(),
            LiteralValues.Uuid,
            v => new ListLiteralUuid(v),
            x => x.Value
        );
        registry.Param(new TypeBooleanList(), p => new ListParamBoolean(p));
        registry.Param(new TypeDateList(), p => new ListParamDate(p));
        registry.Param(new TypeDatetimeList(), p => new ListParamDatetime(p));
        registry.Param(new TypeDecimalList(), p => new ListParamDecimal(p));
        registry.Param(new TypeIntegerList(), p => new ListParamInteger(p));
        registry.Param(new TypeStringList(), p => new ListParamString(p));
        registry.Param(new TypeTimeList(), p => new ListParamTime(p));
        registry.Param(new TypeUuidList(), p => new ListParamUuid(p));
        registry.Union(
            UnionCase.Of((ListLiteralString x) => new ListString(x)),
            UnionCase.Of((ListParamString x) => new ListString(x)),
            UnionCase.Of((ListSubqueryColumnString x) => new ListString(x))
        );
        registry.Column(
            new TypeBoolean(),
            (s, f, nullable) => new ListSubqueryColumnBoolean(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeDate(),
            (s, f, nullable) => new ListSubqueryColumnDate(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeDatetime(),
            (s, f, nullable) => new ListSubqueryColumnDatetime(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeDecimal(),
            (s, f, nullable) => new ListSubqueryColumnDecimal(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeInteger(),
            (s, f, nullable) => new ListSubqueryColumnInteger(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeString(),
            (s, f, nullable) => new ListSubqueryColumnString(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeTime(),
            (s, f, nullable) => new ListSubqueryColumnTime(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Column(
            new TypeUuid(),
            (s, f, nullable) => new ListSubqueryColumnUuid(s, f, nullable),
            x => (x.Subquery, x.Field, x.Nullable)
        );
        registry.Union(
            UnionCase.Of((ListLiteralTime x) => new ListTime(x)),
            UnionCase.Of((ListParamTime x) => new ListTime(x)),
            UnionCase.Of((ListSubqueryColumnTime x) => new ListTime(x))
        );
        registry.Union(
            UnionCase.Of((ListLiteralUuid x) => new ListUuid(x)),
            UnionCase.Of((ListParamUuid x) => new ListUuid(x)),
            UnionCase.Of((ListSubqueryColumnUuid x) => new ListUuid(x))
        );
    }
}
