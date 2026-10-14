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
            (string a, BooleanRow e) => new GroupKeyBoolean(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeBooleanNullable(),
            (string a, ExactBooleanNullableRow e) => new GroupKeyBooleanNullable(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDate(),
            (string a, DateRow e) => new GroupKeyDate(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDateNullable(),
            (string a, ExactDateNullableRow e) => new GroupKeyDateNullable(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDatetime(),
            (string a, DatetimeRow e) => new GroupKeyDatetime(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDatetimeNullable(),
            (string a, ExactDatetimeNullableRow e) => new GroupKeyDatetimeNullable(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDecimal(),
            (string a, ExactDecimalRow e) => new GroupKeyDecimal(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeDecimalNullable(),
            (string a, ExactDecimalNullableRow e) => new GroupKeyDecimalNullable(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeInteger(),
            (string a, IntegerRow e) => new GroupKeyInteger(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeIntegerNullable(),
            (string a, ExactIntegerNullableRow e) => new GroupKeyIntegerNullable(a, e),
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
            (string a, StringRow e) => new GroupKeyString(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeStringNullable(),
            (string a, ExactStringNullableRow e) => new GroupKeyStringNullable(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeTime(),
            (string a, TimeRow e) => new GroupKeyTime(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeTimeNullable(),
            (string a, ExactTimeNullableRow e) => new GroupKeyTimeNullable(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeUuid(),
            (string a, UuidRow e) => new GroupKeyUuid(a, e),
            x => x.Expression
        );
        registry.GroupKey(
            new TypeUuidNullable(),
            (string a, ExactUuidNullableRow e) => new GroupKeyUuidNullable(a, e),
            x => x.Expression
        );
    }
}
