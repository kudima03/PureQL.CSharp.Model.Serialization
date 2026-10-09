using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Literals;

internal static class LiteralRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((LiteralBoolean x) => new LiteralAsBooleanNullable(x)),
            UnionCase.Of((LiteralBooleanNullable x) => new LiteralAsBooleanNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralDate x) => new LiteralAsDateNullable(x)),
            UnionCase.Of((LiteralDateNullable x) => new LiteralAsDateNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralDatetime x) => new LiteralAsDatetimeNullable(x)),
            UnionCase.Of((LiteralDatetimeNullable x) => new LiteralAsDatetimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralDecimal x) => new LiteralAsDecimal(x)),
            UnionCase.Of((LiteralInteger x) => new LiteralAsDecimal(x))
        );
        registry.Union(
            UnionCase.Of((LiteralDecimal x) => new LiteralAsDecimalNullable(x)),
            UnionCase.Of((LiteralDecimalNullable x) => new LiteralAsDecimalNullable(x)),
            UnionCase.Of((LiteralInteger x) => new LiteralAsDecimalNullable(x)),
            UnionCase.Of((LiteralIntegerNullable x) => new LiteralAsDecimalNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralInteger x) => new LiteralAsIntegerNullable(x)),
            UnionCase.Of((LiteralIntegerNullable x) => new LiteralAsIntegerNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralString x) => new LiteralAsStringNullable(x)),
            UnionCase.Of((LiteralStringNullable x) => new LiteralAsStringNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralTime x) => new LiteralAsTimeNullable(x)),
            UnionCase.Of((LiteralTimeNullable x) => new LiteralAsTimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((LiteralUuid x) => new LiteralAsUuidNullable(x)),
            UnionCase.Of((LiteralUuidNullable x) => new LiteralAsUuidNullable(x))
        );
        registry.Literal(
            new TypeBoolean(),
            LiteralValues.Boolean,
            v => new LiteralBoolean(v),
            x => x.Value
        );
        registry.NullLiteral(
            new TypeBooleanNullable(),
            () => new LiteralBooleanNullable()
        );
        registry.Literal(
            new TypeDate(),
            LiteralValues.Date,
            v => new LiteralDate(v),
            x => x.Value
        );
        registry.NullLiteral(new TypeDateNullable(), () => new LiteralDateNullable());
        registry.Literal(
            new TypeDatetime(),
            LiteralValues.Datetime,
            v => new LiteralDatetime(v),
            x => x.Value
        );
        registry.NullLiteral(
            new TypeDatetimeNullable(),
            () => new LiteralDatetimeNullable()
        );
        registry.Literal(
            new TypeDecimal(),
            LiteralValues.Decimal,
            v => new LiteralDecimal(v),
            x => x.Value
        );
        registry.NullLiteral(
            new TypeDecimalNullable(),
            () => new LiteralDecimalNullable()
        );
        registry.Literal(
            new TypeInteger(),
            LiteralValues.Integer,
            v => new LiteralInteger(v),
            x => x.Value
        );
        registry.NullLiteral(
            new TypeIntegerNullable(),
            () => new LiteralIntegerNullable()
        );
        registry.Literal(
            new TypeString(),
            LiteralValues.String,
            v => new LiteralString(v),
            x => x.Value
        );
        registry.NullLiteral(new TypeStringNullable(), () => new LiteralStringNullable());
        registry.Literal(
            new TypeTime(),
            LiteralValues.Time,
            v => new LiteralTime(v),
            x => x.Value
        );
        registry.NullLiteral(new TypeTimeNullable(), () => new LiteralTimeNullable());
        registry.Literal(
            new TypeUuid(),
            LiteralValues.Uuid,
            v => new LiteralUuid(v),
            x => x.Value
        );
        registry.NullLiteral(new TypeUuidNullable(), () => new LiteralUuidNullable());
    }
}
