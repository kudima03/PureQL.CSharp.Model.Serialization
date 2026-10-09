using PureQL.CSharp.Model.GroupKeys;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.GroupKeys;

internal static class GroupKeyRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((GroupKeyNonNullable x) => new GroupKey(x)),
            UnionCase.Of((GroupKeyNullable x) => new GroupKey(x))
        );
        registry.GroupKey(
            new TypeBoolean(),
            (BooleanRow e, string? a) => new GroupKeyBoolean(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeBooleanNullable(),
            (BooleanNullableRow e, string? a) => new GroupKeyBooleanNullable(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDate(),
            (DateRow e, string? a) => new GroupKeyDate(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDateNullable(),
            (DateNullableRow e, string? a) => new GroupKeyDateNullable(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDatetime(),
            (DatetimeRow e, string? a) => new GroupKeyDatetime(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDatetimeNullable(),
            (DatetimeNullableRow e, string? a) => new GroupKeyDatetimeNullable(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDecimal(),
            (DecimalRow e, string? a) => new GroupKeyDecimal(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDecimalNullable(),
            (DecimalNullableRow e, string? a) => new GroupKeyDecimalNullable(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeInteger(),
            (IntegerRow e, string? a) => new GroupKeyInteger(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeIntegerNullable(),
            (IntegerNullableRow e, string? a) => new GroupKeyIntegerNullable(e, a),
            x => x.Expression
        );
        registry.Union(
            UnionCase.Of((GroupKeyInteger x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyDecimal x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyString x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyBoolean x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyDate x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyTime x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyDatetime x) => new GroupKeyNonNullable(x)),
            UnionCase.Of((GroupKeyUuid x) => new GroupKeyNonNullable(x))
        );
        registry.Union(
            UnionCase.Of((GroupKeyIntegerNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyDecimalNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyStringNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyBooleanNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyDateNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyTimeNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyDatetimeNullable x) => new GroupKeyNullable(x)),
            UnionCase.Of((GroupKeyUuidNullable x) => new GroupKeyNullable(x))
        );
        registry.GroupKey(
            new TypeString(),
            (StringRow e, string? a) => new GroupKeyString(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeStringNullable(),
            (StringNullableRow e, string? a) => new GroupKeyStringNullable(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeTime(),
            (TimeRow e, string? a) => new GroupKeyTime(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeTimeNullable(),
            (TimeNullableRow e, string? a) => new GroupKeyTimeNullable(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeUuid(),
            (UuidRow e, string? a) => new GroupKeyUuid(e, a),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeUuidNullable(),
            (UuidNullableRow e, string? a) => new GroupKeyUuidNullable(e, a),
            x => x.Expression
        );
    }
}
