using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.SelectItems;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.SelectItems;

internal static class SelectItemRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((SelectItemGroupNonNullable x) => new SelectItemGroup(x)),
            UnionCase.Of((SelectItemGroupNullable x) => new SelectItemGroup(x))
        );
        registry.SelectItem(
            new TypeBoolean(),
            (string a, BooleanGroup e) => new SelectItemGroupBoolean(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeBooleanNullable(),
            (string a, BooleanNullableGroup e) =>
                new SelectItemGroupBooleanNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDate(),
            (string a, DateGroup e) => new SelectItemGroupDate(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDateNullable(),
            (string a, DateNullableGroup e) => new SelectItemGroupDateNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetime(),
            (string a, DatetimeGroup e) => new SelectItemGroupDatetime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetimeNullable(),
            (string a, DatetimeNullableGroup e) =>
                new SelectItemGroupDatetimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimal(),
            (string a, DecimalGroup e) => new SelectItemGroupDecimal(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimalNullable(),
            (string a, DecimalNullableGroup e) =>
                new SelectItemGroupDecimalNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeInteger(),
            (string a, IntegerGroup e) => new SelectItemGroupInteger(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeIntegerNullable(),
            (string a, IntegerNullableGroup e) =>
                new SelectItemGroupIntegerNullable(a, e),
            x => x.Expression
        );
        registry.Union(
            UnionCase.Of((SelectItemGroupInteger x) => new SelectItemGroupNonNullable(x)),
            UnionCase.Of((SelectItemGroupDecimal x) => new SelectItemGroupNonNullable(x)),
            UnionCase.Of((SelectItemGroupString x) => new SelectItemGroupNonNullable(x)),
            UnionCase.Of((SelectItemGroupBoolean x) => new SelectItemGroupNonNullable(x)),
            UnionCase.Of((SelectItemGroupDate x) => new SelectItemGroupNonNullable(x)),
            UnionCase.Of((SelectItemGroupTime x) => new SelectItemGroupNonNullable(x)),
            UnionCase.Of(
                (SelectItemGroupDatetime x) => new SelectItemGroupNonNullable(x)
            ),
            UnionCase.Of((SelectItemGroupUuid x) => new SelectItemGroupNonNullable(x))
        );
        registry.Union(
            UnionCase.Of(
                (SelectItemGroupIntegerNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupDecimalNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupStringNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupBooleanNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupDateNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupTimeNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupDatetimeNullable x) => new SelectItemGroupNullable(x)
            ),
            UnionCase.Of(
                (SelectItemGroupUuidNullable x) => new SelectItemGroupNullable(x)
            )
        );
        registry.SelectItem(
            new TypeString(),
            (string a, StringGroup e) => new SelectItemGroupString(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeStringNullable(),
            (string a, StringNullableGroup e) => new SelectItemGroupStringNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTime(),
            (string a, TimeGroup e) => new SelectItemGroupTime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTimeNullable(),
            (string a, TimeNullableGroup e) => new SelectItemGroupTimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuid(),
            (string a, UuidGroup e) => new SelectItemGroupUuid(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuidNullable(),
            (string a, UuidNullableGroup e) => new SelectItemGroupUuidNullable(a, e),
            x => x.Expression
        );
        registry.Union(
            UnionCase.Of(
                (SelectItemProjectionNonNullable x) => new SelectItemProjection(x)
            ),
            UnionCase.Of((SelectItemProjectionNullable x) => new SelectItemProjection(x))
        );
        registry.SelectItem(
            new TypeBoolean(),
            (string a, BooleanProjection e) => new SelectItemProjectionBoolean(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeBooleanNullable(),
            (string a, BooleanNullableProjection e) =>
                new SelectItemProjectionBooleanNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDate(),
            (string a, DateProjection e) => new SelectItemProjectionDate(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDateNullable(),
            (string a, DateNullableProjection e) =>
                new SelectItemProjectionDateNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetime(),
            (string a, DatetimeProjection e) => new SelectItemProjectionDatetime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetimeNullable(),
            (string a, DatetimeNullableProjection e) =>
                new SelectItemProjectionDatetimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimal(),
            (string a, DecimalProjection e) => new SelectItemProjectionDecimal(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimalNullable(),
            (string a, DecimalNullableProjection e) =>
                new SelectItemProjectionDecimalNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeInteger(),
            (string a, IntegerProjection e) => new SelectItemProjectionInteger(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeIntegerNullable(),
            (string a, IntegerNullableProjection e) =>
                new SelectItemProjectionIntegerNullable(a, e),
            x => x.Expression
        );
        registry.Union(
            UnionCase.Of(
                (SelectItemProjectionInteger x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionDecimal x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionString x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionBoolean x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionDate x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionTime x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionDatetime x) => new SelectItemProjectionNonNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionUuid x) => new SelectItemProjectionNonNullable(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (SelectItemProjectionIntegerNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionDecimalNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionStringNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionBooleanNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionDateNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionTimeNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionDatetimeNullable x) =>
                    new SelectItemProjectionNullable(x)
            ),
            UnionCase.Of(
                (SelectItemProjectionUuidNullable x) =>
                    new SelectItemProjectionNullable(x)
            )
        );
        registry.SelectItem(
            new TypeString(),
            (string a, StringProjection e) => new SelectItemProjectionString(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeStringNullable(),
            (string a, StringNullableProjection e) =>
                new SelectItemProjectionStringNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTime(),
            (string a, TimeProjection e) => new SelectItemProjectionTime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTimeNullable(),
            (string a, TimeNullableProjection e) =>
                new SelectItemProjectionTimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuid(),
            (string a, UuidProjection e) => new SelectItemProjectionUuid(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuidNullable(),
            (string a, UuidNullableProjection e) =>
                new SelectItemProjectionUuidNullable(a, e),
            x => x.Expression
        );
    }
}
