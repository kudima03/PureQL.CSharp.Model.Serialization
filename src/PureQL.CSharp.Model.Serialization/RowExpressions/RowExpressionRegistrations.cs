using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.RowExpressions;

namespace PureQL.CSharp.Model.Serialization.RowExpressions;

internal static class RowExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            r => new AddDecimalNullableRow(r.Many<DecimalNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddDecimalRow(r.Many<DecimalRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddIntegerNullableRow(r.Many<IntegerNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddIntegerRow(r.Many<IntegerRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "and",
            r => new AndRow(r.Many<BooleanRow>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Union(
            UnionCase.Of(
                (AddDecimalNullableRow x) => new ArithmeticDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (SubtractDecimalNullableRow x) => new ArithmeticDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (MultiplyDecimalNullableRow x) => new ArithmeticDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (DivideDecimalNullableRow x) => new ArithmeticDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableRow x) => new ArithmeticDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableRow x) => new ArithmeticDecimalNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((SubtractDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((MultiplyDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((DivideDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((IntegerDivideIntegerRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((ModuloIntegerRow x) => new ArithmeticDecimalRow(x))
        );
        registry.Union(
            UnionCase.Of(
                (AddIntegerNullableRow x) => new ArithmeticIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (SubtractIntegerNullableRow x) => new ArithmeticIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (MultiplyIntegerNullableRow x) => new ArithmeticIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableRow x) => new ArithmeticIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableRow x) => new ArithmeticIntegerNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddIntegerRow x) => new ArithmeticIntegerRow(x)),
            UnionCase.Of((SubtractIntegerRow x) => new ArithmeticIntegerRow(x)),
            UnionCase.Of((MultiplyIntegerRow x) => new ArithmeticIntegerRow(x)),
            UnionCase.Of((IntegerDivideIntegerRow x) => new ArithmeticIntegerRow(x)),
            UnionCase.Of((ModuloIntegerRow x) => new ArithmeticIntegerRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsBooleanNullable x) => new BooleanNullableRow(x)),
            UnionCase.Of((ParamAsBooleanNullable x) => new BooleanNullableRow(x)),
            UnionCase.Of((LiteralAsBooleanNullable x) => new BooleanNullableRow(x)),
            UnionCase.Of((LogicalRow x) => new BooleanNullableRow(x)),
            UnionCase.Of((ComparisonRow x) => new BooleanNullableRow(x)),
            UnionCase.Of((ConditionalBooleanNullableRow x) => new BooleanNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldBoolean x) => new BooleanRow(x)),
            UnionCase.Of((ParamBoolean x) => new BooleanRow(x)),
            UnionCase.Of((LiteralBoolean x) => new BooleanRow(x)),
            UnionCase.Of((LogicalRow x) => new BooleanRow(x)),
            UnionCase.Of((ComparisonRow x) => new BooleanRow(x)),
            UnionCase.Of((ConditionalBooleanRow x) => new BooleanRow(x))
        );
        registry.Operator(
            "ceiling",
            r => new CeilingIntegerNullableRow(r.One<DecimalNullableRow>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "ceiling",
            r => new CeilingIntegerRow(r.One<DecimalRow>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceBooleanNullableRow(r.Many<BooleanNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceBooleanRow(r.Many<BooleanNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDateNullableRow(r.Many<DateNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDateRow(r.Many<DateNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDatetimeNullableRow(r.Many<DatetimeNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDatetimeRow(r.Many<DatetimeNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDecimalNullableRow(r.Many<DecimalNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDecimalRow(r.Many<DecimalNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceIntegerNullableRow(r.Many<IntegerNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceIntegerRow(r.Many<IntegerNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceStringNullableRow(r.Many<StringNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceStringRow(r.Many<StringNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceTimeNullableRow(r.Many<TimeNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceTimeRow(r.Many<TimeNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceUuidNullableRow(r.Many<UuidNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceUuidRow(r.Many<UuidNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((EqualRow x) => new ComparisonRow(x)),
            UnionCase.Of((NotEqualRow x) => new ComparisonRow(x)),
            UnionCase.Of((InRow x) => new ComparisonRow(x)),
            UnionCase.Of((GreaterThanRow x) => new ComparisonRow(x)),
            UnionCase.Of((LessThanRow x) => new ComparisonRow(x)),
            UnionCase.Of((GreaterThanOrEqualRow x) => new ComparisonRow(x)),
            UnionCase.Of((LessThanOrEqualRow x) => new ComparisonRow(x))
        );
        registry.Operator(
            "concat",
            r => new ConcatStringNullableRow(r.Many<StringNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
            r => new ConcatStringRow(r.Many<StringRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of(
                (IfBooleanNullableRow x) => new ConditionalBooleanNullableRow(x)
            ),
            UnionCase.Of(
                (CoalesceBooleanNullableRow x) => new ConditionalBooleanNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfBooleanRow x) => new ConditionalBooleanRow(x)),
            UnionCase.Of((CoalesceBooleanRow x) => new ConditionalBooleanRow(x))
        );
        registry.Union(
            UnionCase.Of((IfDateNullableRow x) => new ConditionalDateNullableRow(x)),
            UnionCase.Of((CoalesceDateNullableRow x) => new ConditionalDateNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((IfDateRow x) => new ConditionalDateRow(x)),
            UnionCase.Of((CoalesceDateRow x) => new ConditionalDateRow(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDatetimeNullableRow x) => new ConditionalDatetimeNullableRow(x)
            ),
            UnionCase.Of(
                (CoalesceDatetimeNullableRow x) => new ConditionalDatetimeNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDatetimeRow x) => new ConditionalDatetimeRow(x)),
            UnionCase.Of((CoalesceDatetimeRow x) => new ConditionalDatetimeRow(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDecimalNullableRow x) => new ConditionalDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (CoalesceDecimalNullableRow x) => new ConditionalDecimalNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDecimalRow x) => new ConditionalDecimalRow(x)),
            UnionCase.Of((CoalesceDecimalRow x) => new ConditionalDecimalRow(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfIntegerNullableRow x) => new ConditionalIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (CoalesceIntegerNullableRow x) => new ConditionalIntegerNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfIntegerRow x) => new ConditionalIntegerRow(x)),
            UnionCase.Of((CoalesceIntegerRow x) => new ConditionalIntegerRow(x))
        );
        registry.Union(
            UnionCase.Of((IfStringNullableRow x) => new ConditionalStringNullableRow(x)),
            UnionCase.Of(
                (CoalesceStringNullableRow x) => new ConditionalStringNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfStringRow x) => new ConditionalStringRow(x)),
            UnionCase.Of((CoalesceStringRow x) => new ConditionalStringRow(x))
        );
        registry.Union(
            UnionCase.Of((IfTimeNullableRow x) => new ConditionalTimeNullableRow(x)),
            UnionCase.Of((CoalesceTimeNullableRow x) => new ConditionalTimeNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((IfTimeRow x) => new ConditionalTimeRow(x)),
            UnionCase.Of((CoalesceTimeRow x) => new ConditionalTimeRow(x))
        );
        registry.Union(
            UnionCase.Of((IfUuidNullableRow x) => new ConditionalUuidNullableRow(x)),
            UnionCase.Of((CoalesceUuidNullableRow x) => new ConditionalUuidNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((IfUuidRow x) => new ConditionalUuidRow(x)),
            UnionCase.Of((CoalesceUuidRow x) => new ConditionalUuidRow(x))
        );
        registry.Operator(
            "dateAddDays",
            r => new DateAddDaysDateNullableRow(
                r.One<DateNullableRow>("left"),
                r.One<IntegerNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateAddDays",
            r => new DateAddDaysDateRow(
                r.One<DateRow>("left"),
                r.One<IntegerRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateDiffDays",
            r => new DateDiffDaysIntegerNullableRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateDiffDays",
            r => new DateDiffDaysIntegerRow(
                r.One<DateRow>("left"),
                r.One<DateRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsDateNullable x) => new DateNullableRow(x)),
            UnionCase.Of((ParamAsDateNullable x) => new DateNullableRow(x)),
            UnionCase.Of((LiteralAsDateNullable x) => new DateNullableRow(x)),
            UnionCase.Of((DateAddDaysDateNullableRow x) => new DateNullableRow(x)),
            UnionCase.Of((ConditionalDateNullableRow x) => new DateNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldDate x) => new DateRow(x)),
            UnionCase.Of((ParamDate x) => new DateRow(x)),
            UnionCase.Of((LiteralDate x) => new DateRow(x)),
            UnionCase.Of((DateAddDaysDateRow x) => new DateRow(x)),
            UnionCase.Of((ConditionalDateRow x) => new DateRow(x))
        );
        registry.Operator(
            "datetimeAddSeconds",
            r => new DatetimeAddSecondsDatetimeNullableRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeAddSeconds",
            r => new DatetimeAddSecondsDatetimeRow(
                r.One<DatetimeRow>("left"),
                r.One<DecimalRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeDiffSeconds",
            r => new DatetimeDiffSecondsDecimalNullableRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeDiffSeconds",
            r => new DatetimeDiffSecondsDecimalRow(
                r.One<DatetimeRow>("left"),
                r.One<DatetimeRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsDatetimeNullable x) => new DatetimeNullableRow(x)),
            UnionCase.Of((ParamAsDatetimeNullable x) => new DatetimeNullableRow(x)),
            UnionCase.Of((LiteralAsDatetimeNullable x) => new DatetimeNullableRow(x)),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableRow x) => new DatetimeNullableRow(x)
            ),
            UnionCase.Of((ConditionalDatetimeNullableRow x) => new DatetimeNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldDatetime x) => new DatetimeRow(x)),
            UnionCase.Of((ParamDatetime x) => new DatetimeRow(x)),
            UnionCase.Of((LiteralDatetime x) => new DatetimeRow(x)),
            UnionCase.Of((DatetimeAddSecondsDatetimeRow x) => new DatetimeRow(x)),
            UnionCase.Of((ConditionalDatetimeRow x) => new DatetimeRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsDecimalNullable x) => new DecimalNullableRow(x)),
            UnionCase.Of((ParamAsDecimalNullable x) => new DecimalNullableRow(x)),
            UnionCase.Of((LiteralAsDecimalNullable x) => new DecimalNullableRow(x)),
            UnionCase.Of((ArithmeticDecimalNullableRow x) => new DecimalNullableRow(x)),
            UnionCase.Of((RoundingDecimalNullableRow x) => new DecimalNullableRow(x)),
            UnionCase.Of((DifferenceDecimalNullableRow x) => new DecimalNullableRow(x)),
            UnionCase.Of((ConditionalDecimalNullableRow x) => new DecimalNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsDecimal x) => new DecimalRow(x)),
            UnionCase.Of((ParamAsDecimal x) => new DecimalRow(x)),
            UnionCase.Of((LiteralAsDecimal x) => new DecimalRow(x)),
            UnionCase.Of((ArithmeticDecimalRow x) => new DecimalRow(x)),
            UnionCase.Of((RoundingDecimalRow x) => new DecimalRow(x)),
            UnionCase.Of((DifferenceDecimalRow x) => new DecimalRow(x)),
            UnionCase.Of((ConditionalDecimalRow x) => new DecimalRow(x))
        );
        registry.Union(
            UnionCase.Of(
                (DateDiffDaysIntegerNullableRow x) => new DifferenceDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (TimeDiffSecondsDecimalNullableRow x) =>
                    new DifferenceDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalNullableRow x) =>
                    new DifferenceDecimalNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((DateDiffDaysIntegerRow x) => new DifferenceDecimalRow(x)),
            UnionCase.Of((TimeDiffSecondsDecimalRow x) => new DifferenceDecimalRow(x)),
            UnionCase.Of((DatetimeDiffSecondsDecimalRow x) => new DifferenceDecimalRow(x))
        );
        registry.Operator(
            "divide",
            r => new DivideDecimalNullableRow(r.Many<DecimalNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            r => new DivideDecimalRow(r.Many<DecimalRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
            r => new EqualBooleanRow(
                r.One<BooleanNullableRow>("left"),
                r.One<BooleanNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualDateRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualDatetimeRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualDecimalRow(
                r.One<DecimalNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(TypeFamily.Decimal, (EqualDecimalRow x) => new EqualRow(x)),
            UnionCase.Of(TypeFamily.String, (EqualStringRow x) => new EqualRow(x)),
            UnionCase.Of(TypeFamily.Boolean, (EqualBooleanRow x) => new EqualRow(x)),
            UnionCase.Of(TypeFamily.Date, (EqualDateRow x) => new EqualRow(x)),
            UnionCase.Of(TypeFamily.Time, (EqualTimeRow x) => new EqualRow(x)),
            UnionCase.Of(TypeFamily.Datetime, (EqualDatetimeRow x) => new EqualRow(x)),
            UnionCase.Of(TypeFamily.Uuid, (EqualUuidRow x) => new EqualRow(x))
        );
        registry.Operator(
            "equal",
            r => new EqualStringRow(
                r.One<StringNullableRow>("left"),
                r.One<StringNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualTimeRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualUuidRow(
                r.One<UuidNullableRow>("left"),
                r.One<UuidNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "floor",
            r => new FloorIntegerNullableRow(r.One<DecimalNullableRow>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "floor",
            r => new FloorIntegerRow(r.One<DecimalRow>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanDateRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanDatetimeRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanDecimalRow(
                r.One<DecimalNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualDateRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualDatetimeRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualDecimalRow(
                r.One<DecimalNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(
                TypeFamily.Decimal,
                (GreaterThanOrEqualDecimalRow x) => new GreaterThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanOrEqualStringRow x) => new GreaterThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanOrEqualDateRow x) => new GreaterThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanOrEqualTimeRow x) => new GreaterThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanOrEqualDatetimeRow x) => new GreaterThanOrEqualRow(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualStringRow(
                r.One<StringNullableRow>("left"),
                r.One<StringNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualTimeRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(
                TypeFamily.Decimal,
                (GreaterThanDecimalRow x) => new GreaterThanRow(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanStringRow x) => new GreaterThanRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanDateRow x) => new GreaterThanRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanTimeRow x) => new GreaterThanRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanDatetimeRow x) => new GreaterThanRow(x)
            )
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanStringRow(
                r.One<StringNullableRow>("left"),
                r.One<StringNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanTimeRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "if",
            r => new IfBooleanNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<BooleanNullableRow>("then"),
                r.One<BooleanNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfBooleanRow(
                r.One<BooleanRow>("condition"),
                r.One<BooleanRow>("then"),
                r.One<BooleanRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfDateNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<DateNullableRow>("then"),
                r.One<DateNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfDateRow(
                r.One<BooleanRow>("condition"),
                r.One<DateRow>("then"),
                r.One<DateRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfDatetimeNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<DatetimeNullableRow>("then"),
                r.One<DatetimeNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfDatetimeRow(
                r.One<BooleanRow>("condition"),
                r.One<DatetimeRow>("then"),
                r.One<DatetimeRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfDecimalNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<DecimalNullableRow>("then"),
                r.One<DecimalNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfDecimalRow(
                r.One<BooleanRow>("condition"),
                r.One<DecimalRow>("then"),
                r.One<DecimalRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfIntegerNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<IntegerNullableRow>("then"),
                r.One<IntegerNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfIntegerRow(
                r.One<BooleanRow>("condition"),
                r.One<IntegerRow>("then"),
                r.One<IntegerRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfStringNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<StringNullableRow>("then"),
                r.One<StringNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfStringRow(
                r.One<BooleanRow>("condition"),
                r.One<StringRow>("then"),
                r.One<StringRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfTimeNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<TimeNullableRow>("then"),
                r.One<TimeNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfTimeRow(
                r.One<BooleanRow>("condition"),
                r.One<TimeRow>("then"),
                r.One<TimeRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfUuidNullableRow(
                r.One<BooleanRow>("condition"),
                r.One<UuidNullableRow>("then"),
                r.One<UuidNullableRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "if",
            r => new IfUuidRow(
                r.One<BooleanRow>("condition"),
                r.One<UuidRow>("then"),
                r.One<UuidRow>("else")
            ),
            (w, x) =>
            {
                w.One("condition", x.Condition);
                w.One("then", x.Then);
                w.One("else", x.Else);
            }
        );
        registry.Operator(
            "in",
            r => new InBooleanRow(
                r.One<BooleanNullableRow>("value"),
                r.One<ListBoolean>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InDateRow(r.One<DateNullableRow>("value"), r.One<ListDate>("list")),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InDatetimeRow(
                r.One<DatetimeNullableRow>("value"),
                r.One<ListDatetime>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InDecimalRow(
                r.One<DecimalNullableRow>("value"),
                r.One<ListDecimal>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Probe(
            "value",
            UnionCase.Of(TypeFamily.Decimal, (InDecimalRow x) => new InRow(x)),
            UnionCase.Of(TypeFamily.String, (InStringRow x) => new InRow(x)),
            UnionCase.Of(TypeFamily.Boolean, (InBooleanRow x) => new InRow(x)),
            UnionCase.Of(TypeFamily.Date, (InDateRow x) => new InRow(x)),
            UnionCase.Of(TypeFamily.Time, (InTimeRow x) => new InRow(x)),
            UnionCase.Of(TypeFamily.Datetime, (InDatetimeRow x) => new InRow(x)),
            UnionCase.Of(TypeFamily.Uuid, (InUuidRow x) => new InRow(x))
        );
        registry.Operator(
            "in",
            r => new InStringRow(
                r.One<StringNullableRow>("value"),
                r.One<ListString>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InTimeRow(r.One<TimeNullableRow>("value"), r.One<ListTime>("list")),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InUuidRow(r.One<UuidNullableRow>("value"), r.One<ListUuid>("list")),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "integerDivide",
            r => new IntegerDivideIntegerNullableRow(
                r.One<IntegerNullableRow>("left"),
                r.One<IntegerNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "integerDivide",
            r => new IntegerDivideIntegerRow(
                r.One<IntegerRow>("left"),
                r.One<IntegerRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsIntegerNullable x) => new IntegerNullableRow(x)),
            UnionCase.Of((ParamAsIntegerNullable x) => new IntegerNullableRow(x)),
            UnionCase.Of((LiteralAsIntegerNullable x) => new IntegerNullableRow(x)),
            UnionCase.Of((ArithmeticIntegerNullableRow x) => new IntegerNullableRow(x)),
            UnionCase.Of((RoundingIntegerNullableRow x) => new IntegerNullableRow(x)),
            UnionCase.Of((DateDiffDaysIntegerNullableRow x) => new IntegerNullableRow(x)),
            UnionCase.Of((ConditionalIntegerNullableRow x) => new IntegerNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldInteger x) => new IntegerRow(x)),
            UnionCase.Of((ParamInteger x) => new IntegerRow(x)),
            UnionCase.Of((LiteralInteger x) => new IntegerRow(x)),
            UnionCase.Of((ArithmeticIntegerRow x) => new IntegerRow(x)),
            UnionCase.Of((RoundingIntegerRow x) => new IntegerRow(x)),
            UnionCase.Of((DateDiffDaysIntegerRow x) => new IntegerRow(x)),
            UnionCase.Of((ConditionalIntegerRow x) => new IntegerRow(x))
        );
        registry.Operator(
            "lessThan",
            r => new LessThanDateRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThan",
            r => new LessThanDatetimeRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThan",
            r => new LessThanDecimalRow(
                r.One<DecimalNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualDateRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualDatetimeRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualDecimalRow(
                r.One<DecimalNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(
                TypeFamily.Decimal,
                (LessThanOrEqualDecimalRow x) => new LessThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanOrEqualStringRow x) => new LessThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (LessThanOrEqualDateRow x) => new LessThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (LessThanOrEqualTimeRow x) => new LessThanOrEqualRow(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanOrEqualDatetimeRow x) => new LessThanOrEqualRow(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualStringRow(
                r.One<StringNullableRow>("left"),
                r.One<StringNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualTimeRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(
                TypeFamily.Decimal,
                (LessThanDecimalRow x) => new LessThanRow(x)
            ),
            UnionCase.Of(TypeFamily.String, (LessThanStringRow x) => new LessThanRow(x)),
            UnionCase.Of(TypeFamily.Date, (LessThanDateRow x) => new LessThanRow(x)),
            UnionCase.Of(TypeFamily.Time, (LessThanTimeRow x) => new LessThanRow(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanDatetimeRow x) => new LessThanRow(x)
            )
        );
        registry.Operator(
            "lessThan",
            r => new LessThanStringRow(
                r.One<StringNullableRow>("left"),
                r.One<StringNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThan",
            r => new LessThanTimeRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((AndRow x) => new LogicalRow(x)),
            UnionCase.Of((OrRow x) => new LogicalRow(x)),
            UnionCase.Of((NotRow x) => new LogicalRow(x))
        );
        registry.Operator(
            "modulo",
            r => new ModuloIntegerNullableRow(
                r.One<IntegerNullableRow>("left"),
                r.One<IntegerNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "modulo",
            r => new ModuloIntegerRow(
                r.One<IntegerRow>("left"),
                r.One<IntegerRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "multiply",
            r => new MultiplyDecimalNullableRow(r.Many<DecimalNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyDecimalRow(r.Many<DecimalRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyIntegerNullableRow(r.Many<IntegerNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyIntegerRow(r.Many<IntegerRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualBooleanRow(
                r.One<BooleanNullableRow>("left"),
                r.One<BooleanNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualDateRow(
                r.One<DateNullableRow>("left"),
                r.One<DateNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualDatetimeRow(
                r.One<DatetimeNullableRow>("left"),
                r.One<DatetimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualDecimalRow(
                r.One<DecimalNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(
                TypeFamily.Decimal,
                (NotEqualDecimalRow x) => new NotEqualRow(x)
            ),
            UnionCase.Of(TypeFamily.String, (NotEqualStringRow x) => new NotEqualRow(x)),
            UnionCase.Of(
                TypeFamily.Boolean,
                (NotEqualBooleanRow x) => new NotEqualRow(x)
            ),
            UnionCase.Of(TypeFamily.Date, (NotEqualDateRow x) => new NotEqualRow(x)),
            UnionCase.Of(TypeFamily.Time, (NotEqualTimeRow x) => new NotEqualRow(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (NotEqualDatetimeRow x) => new NotEqualRow(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (NotEqualUuidRow x) => new NotEqualRow(x))
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualStringRow(
                r.One<StringNullableRow>("left"),
                r.One<StringNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualTimeRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualUuidRow(
                r.One<UuidNullableRow>("left"),
                r.One<UuidNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "not",
            r => new NotRow(r.One<BooleanRow>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            r => new OrRow(r.Many<BooleanRow>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            r => new RoundDecimalDigitsNullableRow(
                r.One<DecimalNullableRow>("value"),
                r.One<IntegerRow>("digits")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("digits", x.Digits);
            },
            digits: true
        );
        registry.Operator(
            "round",
            r => new RoundDecimalDigitsRow(
                r.One<DecimalRow>("value"),
                r.One<IntegerRow>("digits")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("digits", x.Digits);
            },
            digits: true
        );
        registry.Operator(
            "round",
            r => new RoundIntegerNullableRow(r.One<DecimalNullableRow>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "round",
            r => new RoundIntegerRow(r.One<DecimalRow>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableRow x) => new RoundingDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableRow x) => new RoundingDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (RoundDecimalDigitsNullableRow x) => new RoundingDecimalNullableRow(x)
            ),
            UnionCase.Of((RoundIntegerNullableRow x) => new RoundingDecimalNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FloorIntegerRow x) => new RoundingDecimalRow(x)),
            UnionCase.Of((CeilingIntegerRow x) => new RoundingDecimalRow(x)),
            UnionCase.Of((RoundDecimalDigitsRow x) => new RoundingDecimalRow(x)),
            UnionCase.Of((RoundIntegerRow x) => new RoundingDecimalRow(x))
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableRow x) => new RoundingIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableRow x) => new RoundingIntegerNullableRow(x)
            ),
            UnionCase.Of((RoundIntegerNullableRow x) => new RoundingIntegerNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FloorIntegerRow x) => new RoundingIntegerRow(x)),
            UnionCase.Of((CeilingIntegerRow x) => new RoundingIntegerRow(x)),
            UnionCase.Of((RoundIntegerRow x) => new RoundingIntegerRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsStringNullable x) => new StringNullableRow(x)),
            UnionCase.Of((ParamAsStringNullable x) => new StringNullableRow(x)),
            UnionCase.Of((LiteralAsStringNullable x) => new StringNullableRow(x)),
            UnionCase.Of((ConcatStringNullableRow x) => new StringNullableRow(x)),
            UnionCase.Of((ConditionalStringNullableRow x) => new StringNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldString x) => new StringRow(x)),
            UnionCase.Of((ParamString x) => new StringRow(x)),
            UnionCase.Of((LiteralString x) => new StringRow(x)),
            UnionCase.Of((ConcatStringRow x) => new StringRow(x)),
            UnionCase.Of((ConditionalStringRow x) => new StringRow(x))
        );
        registry.Operator(
            "subtract",
            r => new SubtractDecimalNullableRow(r.Many<DecimalNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractDecimalRow(r.Many<DecimalRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractIntegerNullableRow(r.Many<IntegerNullableRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractIntegerRow(r.Many<IntegerRow>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "timeAddSeconds",
            r => new TimeAddSecondsTimeNullableRow(
                r.One<TimeNullableRow>("left"),
                r.One<DecimalNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeAddSeconds",
            r => new TimeAddSecondsTimeRow(
                r.One<TimeRow>("left"),
                r.One<DecimalRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeDiffSeconds",
            r => new TimeDiffSecondsDecimalNullableRow(
                r.One<TimeNullableRow>("left"),
                r.One<TimeNullableRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeDiffSeconds",
            r => new TimeDiffSecondsDecimalRow(
                r.One<TimeRow>("left"),
                r.One<TimeRow>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsTimeNullable x) => new TimeNullableRow(x)),
            UnionCase.Of((ParamAsTimeNullable x) => new TimeNullableRow(x)),
            UnionCase.Of((LiteralAsTimeNullable x) => new TimeNullableRow(x)),
            UnionCase.Of((TimeAddSecondsTimeNullableRow x) => new TimeNullableRow(x)),
            UnionCase.Of((ConditionalTimeNullableRow x) => new TimeNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldTime x) => new TimeRow(x)),
            UnionCase.Of((ParamTime x) => new TimeRow(x)),
            UnionCase.Of((LiteralTime x) => new TimeRow(x)),
            UnionCase.Of((TimeAddSecondsTimeRow x) => new TimeRow(x)),
            UnionCase.Of((ConditionalTimeRow x) => new TimeRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsUuidNullable x) => new UuidNullableRow(x)),
            UnionCase.Of((ParamAsUuidNullable x) => new UuidNullableRow(x)),
            UnionCase.Of((LiteralAsUuidNullable x) => new UuidNullableRow(x)),
            UnionCase.Of((ConditionalUuidNullableRow x) => new UuidNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldUuid x) => new UuidRow(x)),
            UnionCase.Of((ParamUuid x) => new UuidRow(x)),
            UnionCase.Of((LiteralUuid x) => new UuidRow(x)),
            UnionCase.Of((ConditionalUuidRow x) => new UuidRow(x))
        );
    }
}
