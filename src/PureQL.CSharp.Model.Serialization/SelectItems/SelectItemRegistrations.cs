using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.SelectItems;
using PureQL.CSharp.Model.TotalExpressions;
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
            (string a, ExactBooleanNullableGroup e) =>
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
            (string a, ExactDateNullableGroup e) => new SelectItemGroupDateNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetime(),
            (string a, DatetimeGroup e) => new SelectItemGroupDatetime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetimeNullable(),
            (string a, ExactDatetimeNullableGroup e) =>
                new SelectItemGroupDatetimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimal(),
            (string a, ExactDecimalGroup e) => new SelectItemGroupDecimal(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimalNullable(),
            (string a, ExactDecimalNullableGroup e) =>
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
            (string a, ExactIntegerNullableGroup e) =>
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
            (string a, ExactStringNullableGroup e) =>
                new SelectItemGroupStringNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTime(),
            (string a, TimeGroup e) => new SelectItemGroupTime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTimeNullable(),
            (string a, ExactTimeNullableGroup e) => new SelectItemGroupTimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuid(),
            (string a, UuidGroup e) => new SelectItemGroupUuid(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuidNullable(),
            (string a, ExactUuidNullableGroup e) => new SelectItemGroupUuidNullable(a, e),
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
            (string a, ExactBooleanNullableProjection e) =>
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
            (string a, ExactDateNullableProjection e) =>
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
            (string a, ExactDatetimeNullableProjection e) =>
                new SelectItemProjectionDatetimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimal(),
            (string a, ExactDecimalProjection e) => new SelectItemProjectionDecimal(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimalNullable(),
            (string a, ExactDecimalNullableProjection e) =>
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
            (string a, ExactIntegerNullableProjection e) =>
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
            (string a, ExactStringNullableProjection e) =>
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
            (string a, ExactTimeNullableProjection e) =>
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
            (string a, ExactUuidNullableProjection e) =>
                new SelectItemProjectionUuidNullable(a, e),
            x => x.Expression
        );
        registry.Union(
            UnionCase.Of((SelectItemTotalNonNullable x) => new SelectItemTotal(x)),
            UnionCase.Of((SelectItemTotalNullable x) => new SelectItemTotal(x))
        );
        registry.SelectItem(
            new TypeBoolean(),
            (string a, BooleanTotal e) => new SelectItemTotalBoolean(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeBooleanNullable(),
            (string a, ExactBooleanNullableTotal e) =>
                new SelectItemTotalBooleanNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDate(),
            (string a, DateTotal e) => new SelectItemTotalDate(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDateNullable(),
            (string a, ExactDateNullableTotal e) => new SelectItemTotalDateNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetime(),
            (string a, DatetimeTotal e) => new SelectItemTotalDatetime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDatetimeNullable(),
            (string a, ExactDatetimeNullableTotal e) =>
                new SelectItemTotalDatetimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimal(),
            (string a, ExactDecimalTotal e) => new SelectItemTotalDecimal(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeDecimalNullable(),
            (string a, ExactDecimalNullableTotal e) =>
                new SelectItemTotalDecimalNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeInteger(),
            (string a, IntegerTotal e) => new SelectItemTotalInteger(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeIntegerNullable(),
            (string a, ExactIntegerNullableTotal e) =>
                new SelectItemTotalIntegerNullable(a, e),
            x => x.Expression
        );
        registry.Union(
            UnionCase.Of((SelectItemTotalInteger x) => new SelectItemTotalNonNullable(x)),
            UnionCase.Of((SelectItemTotalDecimal x) => new SelectItemTotalNonNullable(x)),
            UnionCase.Of((SelectItemTotalString x) => new SelectItemTotalNonNullable(x)),
            UnionCase.Of((SelectItemTotalBoolean x) => new SelectItemTotalNonNullable(x)),
            UnionCase.Of((SelectItemTotalDate x) => new SelectItemTotalNonNullable(x)),
            UnionCase.Of((SelectItemTotalTime x) => new SelectItemTotalNonNullable(x)),
            UnionCase.Of(
                (SelectItemTotalDatetime x) => new SelectItemTotalNonNullable(x)
            ),
            UnionCase.Of((SelectItemTotalUuid x) => new SelectItemTotalNonNullable(x))
        );
        registry.Union(
            UnionCase.Of(
                (SelectItemTotalIntegerNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalDecimalNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalStringNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalBooleanNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalDateNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalTimeNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalDatetimeNullable x) => new SelectItemTotalNullable(x)
            ),
            UnionCase.Of(
                (SelectItemTotalUuidNullable x) => new SelectItemTotalNullable(x)
            )
        );
        registry.SelectItem(
            new TypeString(),
            (string a, StringTotal e) => new SelectItemTotalString(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeStringNullable(),
            (string a, ExactStringNullableTotal e) =>
                new SelectItemTotalStringNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTime(),
            (string a, TimeTotal e) => new SelectItemTotalTime(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeTimeNullable(),
            (string a, ExactTimeNullableTotal e) => new SelectItemTotalTimeNullable(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuid(),
            (string a, UuidTotal e) => new SelectItemTotalUuid(a, e),
            x => x.Expression
        );
        registry.SelectItem(
            new TypeUuidNullable(),
            (string a, ExactUuidNullableTotal e) => new SelectItemTotalUuidNullable(a, e),
            x => x.Expression
        );
    }
}
