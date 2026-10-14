using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Literals;

internal static class LiteralRegistrations
{
    public static void Register(ModelRegistry registry)
    {
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
