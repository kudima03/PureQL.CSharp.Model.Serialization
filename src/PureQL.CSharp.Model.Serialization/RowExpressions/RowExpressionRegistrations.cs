using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.RowExpressions;

internal static class RowExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            new TypeDecimalNullable(),
            r => new AddDecimalNullableRow(r.Many<DecimalNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeDecimal(),
            r => new AddDecimalRow(r.Many<DecimalRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeIntegerNullable(),
            r => new AddIntegerNullableRow(r.Many<IntegerNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeInteger(),
            r => new AddIntegerRow(r.Many<IntegerRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "and",
            new TypeBoolean(),
            r => new AndRow(r.Many<BooleanRow>("conditions", 2)),
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
            )
        );
        registry.Union(
            UnionCase.Of((AddDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((SubtractDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((MultiplyDecimalRow x) => new ArithmeticDecimalRow(x)),
            UnionCase.Of((DivideDecimalRow x) => new ArithmeticDecimalRow(x))
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
            UnionCase.Of((BooleanRow x) => new BooleanNullableRow(x)),
            UnionCase.Of((ExactBooleanNullableRow x) => new BooleanNullableRow(x))
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
            new TypeIntegerNullable(),
            r => new CeilingIntegerNullableRow(r.One<DecimalNullableRow>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "ceiling",
            new TypeInteger(),
            r => new CeilingIntegerRow(r.One<DecimalRow>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "coalesce",
            new TypeBooleanNullable(),
            r => new CoalesceBooleanNullableRow(r.Many<BooleanNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeBoolean(),
            r => new CoalesceBooleanRow(r.Many<BooleanNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDateNullable(),
            r => new CoalesceDateNullableRow(r.Many<DateNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDate(),
            r => new CoalesceDateRow(r.Many<DateNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetimeNullable(),
            r => new CoalesceDatetimeNullableRow(
                r.Many<DatetimeNullableRow>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetime(),
            r => new CoalesceDatetimeRow(r.Many<DatetimeNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimalNullable(),
            r => new CoalesceDecimalNullableRow(r.Many<DecimalNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimal(),
            r => new CoalesceDecimalRow(r.Many<DecimalNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeIntegerNullable(),
            r => new CoalesceIntegerNullableRow(r.Many<IntegerNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeInteger(),
            r => new CoalesceIntegerRow(r.Many<IntegerNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeStringNullable(),
            r => new CoalesceStringNullableRow(r.Many<StringNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeString(),
            r => new CoalesceStringRow(r.Many<StringNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTimeNullable(),
            r => new CoalesceTimeNullableRow(r.Many<TimeNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTime(),
            r => new CoalesceTimeRow(r.Many<TimeNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuidNullable(),
            r => new CoalesceUuidNullableRow(r.Many<UuidNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuid(),
            r => new CoalesceUuidRow(r.Many<UuidNullableRow>("values", 2)),
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
            new TypeStringNullable(),
            r => new ConcatStringNullableRow(r.Many<StringNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
            new TypeString(),
            r => new ConcatStringRow(r.Many<StringRow>("values", 2)),
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
            new TypeDateNullable(),
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
            new TypeDate(),
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
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            UnionCase.Of((DateRow x) => new DateNullableRow(x)),
            UnionCase.Of((ExactDateNullableRow x) => new DateNullableRow(x))
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
            new TypeDatetimeNullable(),
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
            new TypeDatetime(),
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
            new TypeDecimalNullable(),
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
            new TypeDecimal(),
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
            UnionCase.Of((DatetimeRow x) => new DatetimeNullableRow(x)),
            UnionCase.Of((ExactDatetimeNullableRow x) => new DatetimeNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldDatetime x) => new DatetimeRow(x)),
            UnionCase.Of((ParamDatetime x) => new DatetimeRow(x)),
            UnionCase.Of((LiteralDatetime x) => new DatetimeRow(x)),
            UnionCase.Of((DatetimeAddSecondsDatetimeRow x) => new DatetimeRow(x)),
            UnionCase.Of((ConditionalDatetimeRow x) => new DatetimeRow(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalRow x) => new DecimalNullableRow(x)),
            UnionCase.Of((ExactDecimalNullableRow x) => new DecimalNullableRow(x)),
            UnionCase.Of((IntegerRow x) => new DecimalNullableRow(x)),
            UnionCase.Of((ExactIntegerNullableRow x) => new DecimalNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalRow x) => new DecimalRow(x)),
            UnionCase.Of((IntegerRow x) => new DecimalRow(x))
        );
        registry.Union(
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
            UnionCase.Of((TimeDiffSecondsDecimalRow x) => new DifferenceDecimalRow(x)),
            UnionCase.Of((DatetimeDiffSecondsDecimalRow x) => new DifferenceDecimalRow(x))
        );
        registry.Operator(
            "divide",
            new TypeDecimalNullable(),
            r => new DivideDecimalNullableRow(r.Many<DecimalNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            new TypeDecimal(),
            r => new DivideDecimalRow(r.Many<DecimalRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Union(
            UnionCase.Of((FieldBooleanNullable x) => new ExactBooleanNullableRow(x)),
            UnionCase.Of((ParamBooleanNullable x) => new ExactBooleanNullableRow(x)),
            UnionCase.Of((LiteralBooleanNullable x) => new ExactBooleanNullableRow(x)),
            UnionCase.Of(
                (ConditionalBooleanNullableRow x) => new ExactBooleanNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDateNullable x) => new ExactDateNullableRow(x)),
            UnionCase.Of((ParamDateNullable x) => new ExactDateNullableRow(x)),
            UnionCase.Of((LiteralDateNullable x) => new ExactDateNullableRow(x)),
            UnionCase.Of((DateAddDaysDateNullableRow x) => new ExactDateNullableRow(x)),
            UnionCase.Of((ConditionalDateNullableRow x) => new ExactDateNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldDatetimeNullable x) => new ExactDatetimeNullableRow(x)),
            UnionCase.Of((ParamDatetimeNullable x) => new ExactDatetimeNullableRow(x)),
            UnionCase.Of((LiteralDatetimeNullable x) => new ExactDatetimeNullableRow(x)),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableRow x) =>
                    new ExactDatetimeNullableRow(x)
            ),
            UnionCase.Of(
                (ConditionalDatetimeNullableRow x) => new ExactDatetimeNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDecimalNullable x) => new ExactDecimalNullableRow(x)),
            UnionCase.Of((ParamDecimalNullable x) => new ExactDecimalNullableRow(x)),
            UnionCase.Of((LiteralDecimalNullable x) => new ExactDecimalNullableRow(x)),
            UnionCase.Of(
                (ArithmeticDecimalNullableRow x) => new ExactDecimalNullableRow(x)
            ),
            UnionCase.Of((RoundDecimalNullableRow x) => new ExactDecimalNullableRow(x)),
            UnionCase.Of(
                (DifferenceDecimalNullableRow x) => new ExactDecimalNullableRow(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalNullableRow x) => new ExactDecimalNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDecimal x) => new ExactDecimalRow(x)),
            UnionCase.Of((ParamDecimal x) => new ExactDecimalRow(x)),
            UnionCase.Of((LiteralDecimal x) => new ExactDecimalRow(x)),
            UnionCase.Of((ArithmeticDecimalRow x) => new ExactDecimalRow(x)),
            UnionCase.Of((RoundDecimalRow x) => new ExactDecimalRow(x)),
            UnionCase.Of((DifferenceDecimalRow x) => new ExactDecimalRow(x)),
            UnionCase.Of((ConditionalDecimalRow x) => new ExactDecimalRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldIntegerNullable x) => new ExactIntegerNullableRow(x)),
            UnionCase.Of((ParamIntegerNullable x) => new ExactIntegerNullableRow(x)),
            UnionCase.Of((LiteralIntegerNullable x) => new ExactIntegerNullableRow(x)),
            UnionCase.Of(
                (ArithmeticIntegerNullableRow x) => new ExactIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (RoundingIntegerNullableRow x) => new ExactIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (DateDiffDaysIntegerNullableRow x) => new ExactIntegerNullableRow(x)
            ),
            UnionCase.Of(
                (ConditionalIntegerNullableRow x) => new ExactIntegerNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldStringNullable x) => new ExactStringNullableRow(x)),
            UnionCase.Of((ParamStringNullable x) => new ExactStringNullableRow(x)),
            UnionCase.Of((LiteralStringNullable x) => new ExactStringNullableRow(x)),
            UnionCase.Of((ConcatStringNullableRow x) => new ExactStringNullableRow(x)),
            UnionCase.Of(
                (ConditionalStringNullableRow x) => new ExactStringNullableRow(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldTimeNullable x) => new ExactTimeNullableRow(x)),
            UnionCase.Of((ParamTimeNullable x) => new ExactTimeNullableRow(x)),
            UnionCase.Of((LiteralTimeNullable x) => new ExactTimeNullableRow(x)),
            UnionCase.Of(
                (TimeAddSecondsTimeNullableRow x) => new ExactTimeNullableRow(x)
            ),
            UnionCase.Of((ConditionalTimeNullableRow x) => new ExactTimeNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldUuidNullable x) => new ExactUuidNullableRow(x)),
            UnionCase.Of((ParamUuidNullable x) => new ExactUuidNullableRow(x)),
            UnionCase.Of((LiteralUuidNullable x) => new ExactUuidNullableRow(x)),
            UnionCase.Of((ConditionalUuidNullableRow x) => new ExactUuidNullableRow(x))
        );
        registry.Operator(
            "floor",
            new TypeIntegerNullable(),
            r => new FloorIntegerNullableRow(r.One<DecimalNullableRow>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "floor",
            new TypeInteger(),
            r => new FloorIntegerRow(r.One<DecimalRow>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanBooleanRow(
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
            "greaterThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
            r => new GreaterThanOrEqualBooleanRow(
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
            "greaterThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
                TypeFamily.Boolean,
                (GreaterThanOrEqualBooleanRow x) => new GreaterThanOrEqualRow(x)
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
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanOrEqualUuidRow x) => new GreaterThanOrEqualRow(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
            r => new GreaterThanOrEqualUuidRow(
                r.One<UuidNullableRow>("left"),
                r.One<UuidNullableRow>("right")
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
                TypeFamily.Boolean,
                (GreaterThanBooleanRow x) => new GreaterThanRow(x)
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
            ),
            UnionCase.Of(TypeFamily.Uuid, (GreaterThanUuidRow x) => new GreaterThanRow(x))
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanUuidRow(
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
            "if",
            new TypeBooleanNullable(),
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
            new TypeBoolean(),
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
            new TypeDateNullable(),
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
            new TypeDate(),
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
            new TypeDatetimeNullable(),
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
            new TypeDatetime(),
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
            new TypeDecimalNullable(),
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
            new TypeDecimal(),
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
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            new TypeStringNullable(),
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
            new TypeString(),
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
            new TypeTimeNullable(),
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
            new TypeTime(),
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
            new TypeUuidNullable(),
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
            new TypeUuid(),
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
            new TypeBoolean(),
            r => new InBooleanRow(
                r.One<BooleanNullableRow>("operand"),
                r.One<ListBoolean>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InDateRow(
                r.One<DateNullableRow>("operand"),
                r.One<ListDate>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InDatetimeRow(
                r.One<DatetimeNullableRow>("operand"),
                r.One<ListDatetime>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InDecimalRow(
                r.One<DecimalNullableRow>("operand"),
                r.One<ListDecimal>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Probe(
            "operand",
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
            new TypeBoolean(),
            r => new InStringRow(
                r.One<StringNullableRow>("operand"),
                r.One<ListString>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InTimeRow(
                r.One<TimeNullableRow>("operand"),
                r.One<ListTime>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InUuidRow(
                r.One<UuidNullableRow>("operand"),
                r.One<ListUuid>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "integerDivide",
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            UnionCase.Of((IntegerRow x) => new IntegerNullableRow(x)),
            UnionCase.Of((ExactIntegerNullableRow x) => new IntegerNullableRow(x))
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
            new TypeBoolean(),
            r => new LessThanBooleanRow(
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
            "lessThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
            r => new LessThanOrEqualBooleanRow(
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
            "lessThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
                TypeFamily.Boolean,
                (LessThanOrEqualBooleanRow x) => new LessThanOrEqualRow(x)
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
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (LessThanOrEqualUuidRow x) => new LessThanOrEqualRow(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
            r => new LessThanOrEqualUuidRow(
                r.One<UuidNullableRow>("left"),
                r.One<UuidNullableRow>("right")
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
            UnionCase.Of(
                TypeFamily.Boolean,
                (LessThanBooleanRow x) => new LessThanRow(x)
            ),
            UnionCase.Of(TypeFamily.Date, (LessThanDateRow x) => new LessThanRow(x)),
            UnionCase.Of(TypeFamily.Time, (LessThanTimeRow x) => new LessThanRow(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanDatetimeRow x) => new LessThanRow(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (LessThanUuidRow x) => new LessThanRow(x))
        );
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
            r => new LessThanUuidRow(
                r.One<UuidNullableRow>("left"),
                r.One<UuidNullableRow>("right")
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
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            new TypeDecimalNullable(),
            r => new MultiplyDecimalNullableRow(r.Many<DecimalNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeDecimal(),
            r => new MultiplyDecimalRow(r.Many<DecimalRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeIntegerNullable(),
            r => new MultiplyIntegerNullableRow(r.Many<IntegerNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeInteger(),
            r => new MultiplyIntegerRow(r.Many<IntegerRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
            r => new NotRow(r.One<BooleanRow>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            new TypeBoolean(),
            r => new OrRow(r.Many<BooleanRow>("conditions", 2)),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            new TypeDecimalNullable(),
            r => new RoundDecimalNullableRow(
                r.One<DecimalNullableRow>("operand"),
                r.One<IntegerRow>("digits")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("digits", x.Digits);
            }
        );
        registry.Operator(
            "round",
            new TypeDecimal(),
            r => new RoundDecimalRow(
                r.One<DecimalRow>("operand"),
                r.One<IntegerRow>("digits")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("digits", x.Digits);
            }
        );
        registry.Operator(
            "round",
            new TypeIntegerNullable(),
            r => new RoundIntegerNullableRow(r.One<DecimalNullableRow>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "round",
            new TypeInteger(),
            r => new RoundIntegerRow(r.One<DecimalRow>("operand")),
            (w, x) => w.One("operand", x.Operand)
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
            UnionCase.Of((StringRow x) => new StringNullableRow(x)),
            UnionCase.Of((ExactStringNullableRow x) => new StringNullableRow(x))
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
            new TypeDecimalNullable(),
            r => new SubtractDecimalNullableRow(r.Many<DecimalNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeDecimal(),
            r => new SubtractDecimalRow(r.Many<DecimalRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeIntegerNullable(),
            r => new SubtractIntegerNullableRow(r.Many<IntegerNullableRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeInteger(),
            r => new SubtractIntegerRow(r.Many<IntegerRow>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "timeAddSeconds",
            new TypeTimeNullable(),
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
            new TypeTime(),
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
            new TypeDecimalNullable(),
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
            new TypeDecimal(),
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
            UnionCase.Of((TimeRow x) => new TimeNullableRow(x)),
            UnionCase.Of((ExactTimeNullableRow x) => new TimeNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldTime x) => new TimeRow(x)),
            UnionCase.Of((ParamTime x) => new TimeRow(x)),
            UnionCase.Of((LiteralTime x) => new TimeRow(x)),
            UnionCase.Of((TimeAddSecondsTimeRow x) => new TimeRow(x)),
            UnionCase.Of((ConditionalTimeRow x) => new TimeRow(x))
        );
        registry.Union(
            UnionCase.Of((UuidRow x) => new UuidNullableRow(x)),
            UnionCase.Of((ExactUuidNullableRow x) => new UuidNullableRow(x))
        );
        registry.Union(
            UnionCase.Of((FieldUuid x) => new UuidRow(x)),
            UnionCase.Of((ParamUuid x) => new UuidRow(x)),
            UnionCase.Of((LiteralUuid x) => new UuidRow(x)),
            UnionCase.Of((ConditionalUuidRow x) => new UuidRow(x))
        );
    }
}
