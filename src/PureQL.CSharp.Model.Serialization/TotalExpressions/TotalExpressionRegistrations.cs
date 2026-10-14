using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.TotalExpressions;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.TotalExpressions;

internal static class TotalExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            new TypeDecimalNullable(),
            r => new AddDecimalNullableTotal(r.Many<DecimalNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeDecimal(),
            r => new AddDecimalTotal(r.Many<DecimalTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeIntegerNullable(),
            r => new AddIntegerNullableTotal(r.Many<IntegerNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeInteger(),
            r => new AddIntegerTotal(r.Many<IntegerTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of(
                (MinBooleanNullableTotal x) => new AggregateBooleanNullableTotal(x)
            ),
            UnionCase.Of(
                (MaxBooleanNullableTotal x) => new AggregateBooleanNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((AnyTotal x) => new AggregateBooleanTotal(x)),
            UnionCase.Of((AllTotal x) => new AggregateBooleanTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDateNullableTotal x) => new AggregateDateNullableTotal(x)
            ),
            UnionCase.Of((MinDateNullableTotal x) => new AggregateDateNullableTotal(x)),
            UnionCase.Of((MaxDateNullableTotal x) => new AggregateDateNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDatetimeNullableTotal x) => new AggregateDatetimeNullableTotal(x)
            ),
            UnionCase.Of(
                (MinDatetimeNullableTotal x) => new AggregateDatetimeNullableTotal(x)
            ),
            UnionCase.Of(
                (MaxDatetimeNullableTotal x) => new AggregateDatetimeNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (AverageDecimalNullableTotal x) => new AggregateDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (MinDecimalNullableTotal x) => new AggregateDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (MaxDecimalNullableTotal x) => new AggregateDecimalNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (MinIntegerNullableTotal x) => new AggregateIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (MaxIntegerNullableTotal x) => new AggregateIntegerNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((CountTotal x) => new AggregateIntegerTotal(x)),
            UnionCase.Of((SumIntegerTotal x) => new AggregateIntegerTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinStringNullableTotal x) => new AggregateStringNullableTotal(x)
            ),
            UnionCase.Of(
                (MaxStringNullableTotal x) => new AggregateStringNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (AverageTimeNullableTotal x) => new AggregateTimeNullableTotal(x)
            ),
            UnionCase.Of((MinTimeNullableTotal x) => new AggregateTimeNullableTotal(x)),
            UnionCase.Of((MaxTimeNullableTotal x) => new AggregateTimeNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((MinUuidNullableTotal x) => new AggregateUuidNullableTotal(x)),
            UnionCase.Of((MaxUuidNullableTotal x) => new AggregateUuidNullableTotal(x))
        );
        registry.Operator(
            "all",
            new TypeBoolean(),
            r => new AllTotal(r.One<BooleanRow>("predicate")),
            (w, x) => w.One("predicate", x.Predicate)
        );
        registry.Operator(
            "and",
            new TypeBoolean(),
            r => new AndTotal(r.Many<BooleanTotal>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "any",
            new TypeBoolean(),
            r => new AnyTotal(r.One<BooleanRow>("predicate")),
            (w, x) => w.One("predicate", x.Predicate)
        );
        registry.Union(
            UnionCase.Of(
                (AddDecimalNullableTotal x) => new ArithmeticDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (SubtractDecimalNullableTotal x) => new ArithmeticDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (MultiplyDecimalNullableTotal x) => new ArithmeticDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (DivideDecimalNullableTotal x) => new ArithmeticDecimalNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddDecimalTotal x) => new ArithmeticDecimalTotal(x)),
            UnionCase.Of((SubtractDecimalTotal x) => new ArithmeticDecimalTotal(x)),
            UnionCase.Of((MultiplyDecimalTotal x) => new ArithmeticDecimalTotal(x)),
            UnionCase.Of((DivideDecimalTotal x) => new ArithmeticDecimalTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (AddIntegerNullableTotal x) => new ArithmeticIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (SubtractIntegerNullableTotal x) => new ArithmeticIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (MultiplyIntegerNullableTotal x) => new ArithmeticIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableTotal x) =>
                    new ArithmeticIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableTotal x) => new ArithmeticIntegerNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddIntegerTotal x) => new ArithmeticIntegerTotal(x)),
            UnionCase.Of((SubtractIntegerTotal x) => new ArithmeticIntegerTotal(x)),
            UnionCase.Of((MultiplyIntegerTotal x) => new ArithmeticIntegerTotal(x)),
            UnionCase.Of((IntegerDivideIntegerTotal x) => new ArithmeticIntegerTotal(x)),
            UnionCase.Of((ModuloIntegerTotal x) => new ArithmeticIntegerTotal(x))
        );
        registry.Operator(
            "average",
            new TypeDateNullable(),
            r => new AverageDateNullableTotal(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            new TypeDatetimeNullable(),
            r => new AverageDatetimeNullableTotal(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            new TypeDecimalNullable(),
            r => new AverageDecimalNullableTotal(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            new TypeTimeNullable(),
            r => new AverageTimeNullableTotal(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Union(
            UnionCase.Of((BooleanTotal x) => new BooleanNullableTotal(x)),
            UnionCase.Of((ExactBooleanNullableTotal x) => new BooleanNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamBoolean x) => new BooleanTotal(x)),
            UnionCase.Of((LiteralBoolean x) => new BooleanTotal(x)),
            UnionCase.Of((LogicalTotal x) => new BooleanTotal(x)),
            UnionCase.Of((ComparisonTotal x) => new BooleanTotal(x)),
            UnionCase.Of((ConditionalBooleanTotal x) => new BooleanTotal(x)),
            UnionCase.Of((AggregateBooleanTotal x) => new BooleanTotal(x))
        );
        registry.Operator(
            "ceiling",
            new TypeIntegerNullable(),
            r => new CeilingIntegerNullableTotal(r.One<DecimalNullableTotal>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "ceiling",
            new TypeInteger(),
            r => new CeilingIntegerTotal(r.One<DecimalTotal>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "coalesce",
            new TypeBooleanNullable(),
            r => new CoalesceBooleanNullableTotal(r.Many<BooleanNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeBoolean(),
            r => new CoalesceBooleanTotal(r.Many<BooleanNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDateNullable(),
            r => new CoalesceDateNullableTotal(r.Many<DateNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDate(),
            r => new CoalesceDateTotal(r.Many<DateNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetimeNullable(),
            r => new CoalesceDatetimeNullableTotal(
                r.Many<DatetimeNullableTotal>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetime(),
            r => new CoalesceDatetimeTotal(r.Many<DatetimeNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimalNullable(),
            r => new CoalesceDecimalNullableTotal(r.Many<DecimalNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimal(),
            r => new CoalesceDecimalTotal(r.Many<DecimalNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeIntegerNullable(),
            r => new CoalesceIntegerNullableTotal(r.Many<IntegerNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeInteger(),
            r => new CoalesceIntegerTotal(r.Many<IntegerNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeStringNullable(),
            r => new CoalesceStringNullableTotal(r.Many<StringNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeString(),
            r => new CoalesceStringTotal(r.Many<StringNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTimeNullable(),
            r => new CoalesceTimeNullableTotal(r.Many<TimeNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTime(),
            r => new CoalesceTimeTotal(r.Many<TimeNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuidNullable(),
            r => new CoalesceUuidNullableTotal(r.Many<UuidNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuid(),
            r => new CoalesceUuidTotal(r.Many<UuidNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((EqualTotal x) => new ComparisonTotal(x)),
            UnionCase.Of((NotEqualTotal x) => new ComparisonTotal(x)),
            UnionCase.Of((InTotal x) => new ComparisonTotal(x)),
            UnionCase.Of((GreaterThanTotal x) => new ComparisonTotal(x)),
            UnionCase.Of((LessThanTotal x) => new ComparisonTotal(x)),
            UnionCase.Of((GreaterThanOrEqualTotal x) => new ComparisonTotal(x)),
            UnionCase.Of((LessThanOrEqualTotal x) => new ComparisonTotal(x))
        );
        registry.Operator(
            "concat",
            new TypeStringNullable(),
            r => new ConcatStringNullableTotal(r.Many<StringNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
            new TypeString(),
            r => new ConcatStringTotal(r.Many<StringTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of(
                (IfBooleanNullableTotal x) => new ConditionalBooleanNullableTotal(x)
            ),
            UnionCase.Of(
                (CoalesceBooleanNullableTotal x) => new ConditionalBooleanNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfBooleanTotal x) => new ConditionalBooleanTotal(x)),
            UnionCase.Of((CoalesceBooleanTotal x) => new ConditionalBooleanTotal(x))
        );
        registry.Union(
            UnionCase.Of((IfDateNullableTotal x) => new ConditionalDateNullableTotal(x)),
            UnionCase.Of(
                (CoalesceDateNullableTotal x) => new ConditionalDateNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDateTotal x) => new ConditionalDateTotal(x)),
            UnionCase.Of((CoalesceDateTotal x) => new ConditionalDateTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDatetimeNullableTotal x) => new ConditionalDatetimeNullableTotal(x)
            ),
            UnionCase.Of(
                (CoalesceDatetimeNullableTotal x) =>
                    new ConditionalDatetimeNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDatetimeTotal x) => new ConditionalDatetimeTotal(x)),
            UnionCase.Of((CoalesceDatetimeTotal x) => new ConditionalDatetimeTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDecimalNullableTotal x) => new ConditionalDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (CoalesceDecimalNullableTotal x) => new ConditionalDecimalNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDecimalTotal x) => new ConditionalDecimalTotal(x)),
            UnionCase.Of((CoalesceDecimalTotal x) => new ConditionalDecimalTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfIntegerNullableTotal x) => new ConditionalIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (CoalesceIntegerNullableTotal x) => new ConditionalIntegerNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfIntegerTotal x) => new ConditionalIntegerTotal(x)),
            UnionCase.Of((CoalesceIntegerTotal x) => new ConditionalIntegerTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfStringNullableTotal x) => new ConditionalStringNullableTotal(x)
            ),
            UnionCase.Of(
                (CoalesceStringNullableTotal x) => new ConditionalStringNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfStringTotal x) => new ConditionalStringTotal(x)),
            UnionCase.Of((CoalesceStringTotal x) => new ConditionalStringTotal(x))
        );
        registry.Union(
            UnionCase.Of((IfTimeNullableTotal x) => new ConditionalTimeNullableTotal(x)),
            UnionCase.Of(
                (CoalesceTimeNullableTotal x) => new ConditionalTimeNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfTimeTotal x) => new ConditionalTimeTotal(x)),
            UnionCase.Of((CoalesceTimeTotal x) => new ConditionalTimeTotal(x))
        );
        registry.Union(
            UnionCase.Of((IfUuidNullableTotal x) => new ConditionalUuidNullableTotal(x)),
            UnionCase.Of(
                (CoalesceUuidNullableTotal x) => new ConditionalUuidNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfUuidTotal x) => new ConditionalUuidTotal(x)),
            UnionCase.Of((CoalesceUuidTotal x) => new ConditionalUuidTotal(x))
        );
        registry.Operator(
            "count",
            new TypeInteger(),
            r => new CountTotal(r.Optional<BooleanRow>("predicate")),
            (w, x) => w.Optional("predicate", x.Predicate)
        );
        registry.Operator(
            "dateAddDays",
            new TypeDateNullable(),
            r => new DateAddDaysDateNullableTotal(
                r.One<DateNullableTotal>("left"),
                r.One<IntegerNullableTotal>("right")
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
            r => new DateAddDaysDateTotal(
                r.One<DateTotal>("left"),
                r.One<IntegerTotal>("right")
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
            r => new DateDiffDaysIntegerNullableTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new DateDiffDaysIntegerTotal(
                r.One<DateTotal>("left"),
                r.One<DateTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((DateTotal x) => new DateNullableTotal(x)),
            UnionCase.Of((ExactDateNullableTotal x) => new DateNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamDate x) => new DateTotal(x)),
            UnionCase.Of((LiteralDate x) => new DateTotal(x)),
            UnionCase.Of((DateAddDaysDateTotal x) => new DateTotal(x)),
            UnionCase.Of((ConditionalDateTotal x) => new DateTotal(x))
        );
        registry.Operator(
            "datetimeAddSeconds",
            new TypeDatetimeNullable(),
            r => new DatetimeAddSecondsDatetimeNullableTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new DatetimeAddSecondsDatetimeTotal(
                r.One<DatetimeTotal>("left"),
                r.One<DecimalTotal>("right")
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
            r => new DatetimeDiffSecondsDecimalNullableTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new DatetimeDiffSecondsDecimalTotal(
                r.One<DatetimeTotal>("left"),
                r.One<DatetimeTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((DatetimeTotal x) => new DatetimeNullableTotal(x)),
            UnionCase.Of((ExactDatetimeNullableTotal x) => new DatetimeNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamDatetime x) => new DatetimeTotal(x)),
            UnionCase.Of((LiteralDatetime x) => new DatetimeTotal(x)),
            UnionCase.Of((DatetimeAddSecondsDatetimeTotal x) => new DatetimeTotal(x)),
            UnionCase.Of((ConditionalDatetimeTotal x) => new DatetimeTotal(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalTotal x) => new DecimalNullableTotal(x)),
            UnionCase.Of((ExactDecimalNullableTotal x) => new DecimalNullableTotal(x)),
            UnionCase.Of((IntegerTotal x) => new DecimalNullableTotal(x)),
            UnionCase.Of((ExactIntegerNullableTotal x) => new DecimalNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalTotal x) => new DecimalTotal(x)),
            UnionCase.Of((IntegerTotal x) => new DecimalTotal(x))
        );
        registry.Union(
            UnionCase.Of(
                (TimeDiffSecondsDecimalNullableTotal x) =>
                    new DifferenceDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalNullableTotal x) =>
                    new DifferenceDecimalNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (TimeDiffSecondsDecimalTotal x) => new DifferenceDecimalTotal(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalTotal x) => new DifferenceDecimalTotal(x)
            )
        );
        registry.Operator(
            "divide",
            new TypeDecimalNullable(),
            r => new DivideDecimalNullableTotal(r.Many<DecimalNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            new TypeDecimal(),
            r => new DivideDecimalTotal(r.Many<DecimalTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
            new TypeBoolean(),
            r => new EqualBooleanTotal(
                r.One<BooleanNullableTotal>("left"),
                r.One<BooleanNullableTotal>("right")
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
            r => new EqualDateTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new EqualDatetimeTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new EqualDecimalTotal(
                r.One<DecimalNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new EqualStringTotal(
                r.One<StringNullableTotal>("left"),
                r.One<StringNullableTotal>("right")
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
            r => new EqualTimeTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(TypeFamily.Decimal, (EqualDecimalTotal x) => new EqualTotal(x)),
            UnionCase.Of(TypeFamily.String, (EqualStringTotal x) => new EqualTotal(x)),
            UnionCase.Of(TypeFamily.Boolean, (EqualBooleanTotal x) => new EqualTotal(x)),
            UnionCase.Of(TypeFamily.Date, (EqualDateTotal x) => new EqualTotal(x)),
            UnionCase.Of(TypeFamily.Time, (EqualTimeTotal x) => new EqualTotal(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (EqualDatetimeTotal x) => new EqualTotal(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (EqualUuidTotal x) => new EqualTotal(x))
        );
        registry.Operator(
            "equal",
            new TypeBoolean(),
            r => new EqualUuidTotal(
                r.One<UuidNullableTotal>("left"),
                r.One<UuidNullableTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((ParamBooleanNullable x) => new ExactBooleanNullableTotal(x)),
            UnionCase.Of((LiteralBooleanNullable x) => new ExactBooleanNullableTotal(x)),
            UnionCase.Of(
                (ConditionalBooleanNullableTotal x) => new ExactBooleanNullableTotal(x)
            ),
            UnionCase.Of(
                (AggregateBooleanNullableTotal x) => new ExactBooleanNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((ParamDateNullable x) => new ExactDateNullableTotal(x)),
            UnionCase.Of((LiteralDateNullable x) => new ExactDateNullableTotal(x)),
            UnionCase.Of(
                (DateAddDaysDateNullableTotal x) => new ExactDateNullableTotal(x)
            ),
            UnionCase.Of(
                (ConditionalDateNullableTotal x) => new ExactDateNullableTotal(x)
            ),
            UnionCase.Of((AggregateDateNullableTotal x) => new ExactDateNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamDatetimeNullable x) => new ExactDatetimeNullableTotal(x)),
            UnionCase.Of(
                (LiteralDatetimeNullable x) => new ExactDatetimeNullableTotal(x)
            ),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableTotal x) =>
                    new ExactDatetimeNullableTotal(x)
            ),
            UnionCase.Of(
                (ConditionalDatetimeNullableTotal x) => new ExactDatetimeNullableTotal(x)
            ),
            UnionCase.Of(
                (AggregateDatetimeNullableTotal x) => new ExactDatetimeNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((ParamDecimalNullable x) => new ExactDecimalNullableTotal(x)),
            UnionCase.Of((LiteralDecimalNullable x) => new ExactDecimalNullableTotal(x)),
            UnionCase.Of(
                (ArithmeticDecimalNullableTotal x) => new ExactDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (RoundDecimalNullableTotal x) => new ExactDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (DifferenceDecimalNullableTotal x) => new ExactDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalNullableTotal x) => new ExactDecimalNullableTotal(x)
            ),
            UnionCase.Of(
                (AggregateDecimalNullableTotal x) => new ExactDecimalNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((ParamDecimal x) => new ExactDecimalTotal(x)),
            UnionCase.Of((LiteralDecimal x) => new ExactDecimalTotal(x)),
            UnionCase.Of((ArithmeticDecimalTotal x) => new ExactDecimalTotal(x)),
            UnionCase.Of((RoundDecimalTotal x) => new ExactDecimalTotal(x)),
            UnionCase.Of((DifferenceDecimalTotal x) => new ExactDecimalTotal(x)),
            UnionCase.Of((ConditionalDecimalTotal x) => new ExactDecimalTotal(x)),
            UnionCase.Of((SumDecimalTotal x) => new ExactDecimalTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamIntegerNullable x) => new ExactIntegerNullableTotal(x)),
            UnionCase.Of((LiteralIntegerNullable x) => new ExactIntegerNullableTotal(x)),
            UnionCase.Of(
                (ArithmeticIntegerNullableTotal x) => new ExactIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (RoundingIntegerNullableTotal x) => new ExactIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (DateDiffDaysIntegerNullableTotal x) => new ExactIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (ConditionalIntegerNullableTotal x) => new ExactIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (AggregateIntegerNullableTotal x) => new ExactIntegerNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((ParamStringNullable x) => new ExactStringNullableTotal(x)),
            UnionCase.Of((LiteralStringNullable x) => new ExactStringNullableTotal(x)),
            UnionCase.Of(
                (ConcatStringNullableTotal x) => new ExactStringNullableTotal(x)
            ),
            UnionCase.Of(
                (ConditionalStringNullableTotal x) => new ExactStringNullableTotal(x)
            ),
            UnionCase.Of(
                (AggregateStringNullableTotal x) => new ExactStringNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((ParamTimeNullable x) => new ExactTimeNullableTotal(x)),
            UnionCase.Of((LiteralTimeNullable x) => new ExactTimeNullableTotal(x)),
            UnionCase.Of(
                (TimeAddSecondsTimeNullableTotal x) => new ExactTimeNullableTotal(x)
            ),
            UnionCase.Of(
                (ConditionalTimeNullableTotal x) => new ExactTimeNullableTotal(x)
            ),
            UnionCase.Of((AggregateTimeNullableTotal x) => new ExactTimeNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamUuidNullable x) => new ExactUuidNullableTotal(x)),
            UnionCase.Of((LiteralUuidNullable x) => new ExactUuidNullableTotal(x)),
            UnionCase.Of(
                (ConditionalUuidNullableTotal x) => new ExactUuidNullableTotal(x)
            ),
            UnionCase.Of((AggregateUuidNullableTotal x) => new ExactUuidNullableTotal(x))
        );
        registry.Operator(
            "floor",
            new TypeIntegerNullable(),
            r => new FloorIntegerNullableTotal(r.One<DecimalNullableTotal>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "floor",
            new TypeInteger(),
            r => new FloorIntegerTotal(r.One<DecimalTotal>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanBooleanTotal(
                r.One<BooleanNullableTotal>("left"),
                r.One<BooleanNullableTotal>("right")
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
            r => new GreaterThanDateTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new GreaterThanDatetimeTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new GreaterThanDecimalTotal(
                r.One<DecimalNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new GreaterThanOrEqualBooleanTotal(
                r.One<BooleanNullableTotal>("left"),
                r.One<BooleanNullableTotal>("right")
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
            r => new GreaterThanOrEqualDateTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new GreaterThanOrEqualDatetimeTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new GreaterThanOrEqualDecimalTotal(
                r.One<DecimalNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new GreaterThanOrEqualStringTotal(
                r.One<StringNullableTotal>("left"),
                r.One<StringNullableTotal>("right")
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
            r => new GreaterThanOrEqualTimeTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
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
                (GreaterThanOrEqualDecimalTotal x) => new GreaterThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanOrEqualStringTotal x) => new GreaterThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (GreaterThanOrEqualBooleanTotal x) => new GreaterThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanOrEqualDateTotal x) => new GreaterThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanOrEqualTimeTotal x) => new GreaterThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanOrEqualDatetimeTotal x) => new GreaterThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanOrEqualUuidTotal x) => new GreaterThanOrEqualTotal(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
            r => new GreaterThanOrEqualUuidTotal(
                r.One<UuidNullableTotal>("left"),
                r.One<UuidNullableTotal>("right")
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
            r => new GreaterThanStringTotal(
                r.One<StringNullableTotal>("left"),
                r.One<StringNullableTotal>("right")
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
            r => new GreaterThanTimeTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
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
                (GreaterThanDecimalTotal x) => new GreaterThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanStringTotal x) => new GreaterThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (GreaterThanBooleanTotal x) => new GreaterThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanDateTotal x) => new GreaterThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanTimeTotal x) => new GreaterThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanDatetimeTotal x) => new GreaterThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanUuidTotal x) => new GreaterThanTotal(x)
            )
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanUuidTotal(
                r.One<UuidNullableTotal>("left"),
                r.One<UuidNullableTotal>("right")
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
            r => new IfBooleanNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<BooleanNullableTotal>("then"),
                r.One<BooleanNullableTotal>("else")
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
            r => new IfBooleanTotal(
                r.One<BooleanTotal>("condition"),
                r.One<BooleanTotal>("then"),
                r.One<BooleanTotal>("else")
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
            r => new IfDateNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<DateNullableTotal>("then"),
                r.One<DateNullableTotal>("else")
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
            r => new IfDateTotal(
                r.One<BooleanTotal>("condition"),
                r.One<DateTotal>("then"),
                r.One<DateTotal>("else")
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
            r => new IfDatetimeNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<DatetimeNullableTotal>("then"),
                r.One<DatetimeNullableTotal>("else")
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
            r => new IfDatetimeTotal(
                r.One<BooleanTotal>("condition"),
                r.One<DatetimeTotal>("then"),
                r.One<DatetimeTotal>("else")
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
            r => new IfDecimalNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<DecimalNullableTotal>("then"),
                r.One<DecimalNullableTotal>("else")
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
            r => new IfDecimalTotal(
                r.One<BooleanTotal>("condition"),
                r.One<DecimalTotal>("then"),
                r.One<DecimalTotal>("else")
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
            r => new IfIntegerNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<IntegerNullableTotal>("then"),
                r.One<IntegerNullableTotal>("else")
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
            r => new IfIntegerTotal(
                r.One<BooleanTotal>("condition"),
                r.One<IntegerTotal>("then"),
                r.One<IntegerTotal>("else")
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
            r => new IfStringNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<StringNullableTotal>("then"),
                r.One<StringNullableTotal>("else")
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
            r => new IfStringTotal(
                r.One<BooleanTotal>("condition"),
                r.One<StringTotal>("then"),
                r.One<StringTotal>("else")
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
            r => new IfTimeNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<TimeNullableTotal>("then"),
                r.One<TimeNullableTotal>("else")
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
            r => new IfTimeTotal(
                r.One<BooleanTotal>("condition"),
                r.One<TimeTotal>("then"),
                r.One<TimeTotal>("else")
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
            r => new IfUuidNullableTotal(
                r.One<BooleanTotal>("condition"),
                r.One<UuidNullableTotal>("then"),
                r.One<UuidNullableTotal>("else")
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
            r => new IfUuidTotal(
                r.One<BooleanTotal>("condition"),
                r.One<UuidTotal>("then"),
                r.One<UuidTotal>("else")
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
            r => new InBooleanTotal(
                r.One<BooleanNullableTotal>("operand"),
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
            r => new InDateTotal(
                r.One<DateNullableTotal>("operand"),
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
            r => new InDatetimeTotal(
                r.One<DatetimeNullableTotal>("operand"),
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
            r => new InDecimalTotal(
                r.One<DecimalNullableTotal>("operand"),
                r.One<ListDecimal>("list")
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
            r => new InStringTotal(
                r.One<StringNullableTotal>("operand"),
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
            r => new InTimeTotal(
                r.One<TimeNullableTotal>("operand"),
                r.One<ListTime>("list")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("list", x.List);
            }
        );
        registry.Probe(
            "operand",
            UnionCase.Of(TypeFamily.Decimal, (InDecimalTotal x) => new InTotal(x)),
            UnionCase.Of(TypeFamily.String, (InStringTotal x) => new InTotal(x)),
            UnionCase.Of(TypeFamily.Boolean, (InBooleanTotal x) => new InTotal(x)),
            UnionCase.Of(TypeFamily.Date, (InDateTotal x) => new InTotal(x)),
            UnionCase.Of(TypeFamily.Time, (InTimeTotal x) => new InTotal(x)),
            UnionCase.Of(TypeFamily.Datetime, (InDatetimeTotal x) => new InTotal(x)),
            UnionCase.Of(TypeFamily.Uuid, (InUuidTotal x) => new InTotal(x))
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InUuidTotal(
                r.One<UuidNullableTotal>("operand"),
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
            r => new IntegerDivideIntegerNullableTotal(
                r.One<IntegerNullableTotal>("left"),
                r.One<IntegerNullableTotal>("right")
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
            r => new IntegerDivideIntegerTotal(
                r.One<IntegerTotal>("left"),
                r.One<IntegerTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((IntegerTotal x) => new IntegerNullableTotal(x)),
            UnionCase.Of((ExactIntegerNullableTotal x) => new IntegerNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamInteger x) => new IntegerTotal(x)),
            UnionCase.Of((LiteralInteger x) => new IntegerTotal(x)),
            UnionCase.Of((ArithmeticIntegerTotal x) => new IntegerTotal(x)),
            UnionCase.Of((RoundingIntegerTotal x) => new IntegerTotal(x)),
            UnionCase.Of((DateDiffDaysIntegerTotal x) => new IntegerTotal(x)),
            UnionCase.Of((ConditionalIntegerTotal x) => new IntegerTotal(x)),
            UnionCase.Of((AggregateIntegerTotal x) => new IntegerTotal(x))
        );
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
            r => new LessThanBooleanTotal(
                r.One<BooleanNullableTotal>("left"),
                r.One<BooleanNullableTotal>("right")
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
            r => new LessThanDateTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new LessThanDatetimeTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new LessThanDecimalTotal(
                r.One<DecimalNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new LessThanOrEqualBooleanTotal(
                r.One<BooleanNullableTotal>("left"),
                r.One<BooleanNullableTotal>("right")
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
            r => new LessThanOrEqualDateTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new LessThanOrEqualDatetimeTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new LessThanOrEqualDecimalTotal(
                r.One<DecimalNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new LessThanOrEqualStringTotal(
                r.One<StringNullableTotal>("left"),
                r.One<StringNullableTotal>("right")
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
            r => new LessThanOrEqualTimeTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
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
                (LessThanOrEqualDecimalTotal x) => new LessThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanOrEqualStringTotal x) => new LessThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (LessThanOrEqualBooleanTotal x) => new LessThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (LessThanOrEqualDateTotal x) => new LessThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (LessThanOrEqualTimeTotal x) => new LessThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanOrEqualDatetimeTotal x) => new LessThanOrEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (LessThanOrEqualUuidTotal x) => new LessThanOrEqualTotal(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
            r => new LessThanOrEqualUuidTotal(
                r.One<UuidNullableTotal>("left"),
                r.One<UuidNullableTotal>("right")
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
            r => new LessThanStringTotal(
                r.One<StringNullableTotal>("left"),
                r.One<StringNullableTotal>("right")
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
            r => new LessThanTimeTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
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
                (LessThanDecimalTotal x) => new LessThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanStringTotal x) => new LessThanTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (LessThanBooleanTotal x) => new LessThanTotal(x)
            ),
            UnionCase.Of(TypeFamily.Date, (LessThanDateTotal x) => new LessThanTotal(x)),
            UnionCase.Of(TypeFamily.Time, (LessThanTimeTotal x) => new LessThanTotal(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanDatetimeTotal x) => new LessThanTotal(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (LessThanUuidTotal x) => new LessThanTotal(x))
        );
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
            r => new LessThanUuidTotal(
                r.One<UuidNullableTotal>("left"),
                r.One<UuidNullableTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((AndTotal x) => new LogicalTotal(x)),
            UnionCase.Of((OrTotal x) => new LogicalTotal(x)),
            UnionCase.Of((NotTotal x) => new LogicalTotal(x))
        );
        registry.Operator(
            "max",
            new TypeBooleanNullable(),
            r => new MaxBooleanNullableTotal(
                r.One<BooleanNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeDateNullable(),
            r => new MaxDateNullableTotal(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeDatetimeNullable(),
            r => new MaxDatetimeNullableTotal(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeDecimalNullable(),
            r => new MaxDecimalNullableTotal(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeIntegerNullable(),
            r => new MaxIntegerNullableTotal(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeStringNullable(),
            r => new MaxStringNullableTotal(
                r.One<StringNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeTimeNullable(),
            r => new MaxTimeNullableTotal(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            new TypeUuidNullable(),
            r => new MaxUuidNullableTotal(
                r.One<UuidNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeBooleanNullable(),
            r => new MinBooleanNullableTotal(
                r.One<BooleanNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeDateNullable(),
            r => new MinDateNullableTotal(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeDatetimeNullable(),
            r => new MinDatetimeNullableTotal(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeDecimalNullable(),
            r => new MinDecimalNullableTotal(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeIntegerNullable(),
            r => new MinIntegerNullableTotal(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeStringNullable(),
            r => new MinStringNullableTotal(
                r.One<StringNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeTimeNullable(),
            r => new MinTimeNullableTotal(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            new TypeUuidNullable(),
            r => new MinUuidNullableTotal(
                r.One<UuidNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "modulo",
            new TypeIntegerNullable(),
            r => new ModuloIntegerNullableTotal(
                r.One<IntegerNullableTotal>("left"),
                r.One<IntegerNullableTotal>("right")
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
            r => new ModuloIntegerTotal(
                r.One<IntegerTotal>("left"),
                r.One<IntegerTotal>("right")
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
            r => new MultiplyDecimalNullableTotal(r.Many<DecimalNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeDecimal(),
            r => new MultiplyDecimalTotal(r.Many<DecimalTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeIntegerNullable(),
            r => new MultiplyIntegerNullableTotal(r.Many<IntegerNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeInteger(),
            r => new MultiplyIntegerTotal(r.Many<IntegerTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
            new TypeBoolean(),
            r => new NotEqualBooleanTotal(
                r.One<BooleanNullableTotal>("left"),
                r.One<BooleanNullableTotal>("right")
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
            r => new NotEqualDateTotal(
                r.One<DateNullableTotal>("left"),
                r.One<DateNullableTotal>("right")
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
            r => new NotEqualDatetimeTotal(
                r.One<DatetimeNullableTotal>("left"),
                r.One<DatetimeNullableTotal>("right")
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
            r => new NotEqualDecimalTotal(
                r.One<DecimalNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new NotEqualStringTotal(
                r.One<StringNullableTotal>("left"),
                r.One<StringNullableTotal>("right")
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
            r => new NotEqualTimeTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
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
                (NotEqualDecimalTotal x) => new NotEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (NotEqualStringTotal x) => new NotEqualTotal(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (NotEqualBooleanTotal x) => new NotEqualTotal(x)
            ),
            UnionCase.Of(TypeFamily.Date, (NotEqualDateTotal x) => new NotEqualTotal(x)),
            UnionCase.Of(TypeFamily.Time, (NotEqualTimeTotal x) => new NotEqualTotal(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (NotEqualDatetimeTotal x) => new NotEqualTotal(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (NotEqualUuidTotal x) => new NotEqualTotal(x))
        );
        registry.Operator(
            "notEqual",
            new TypeBoolean(),
            r => new NotEqualUuidTotal(
                r.One<UuidNullableTotal>("left"),
                r.One<UuidNullableTotal>("right")
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
            r => new NotTotal(r.One<BooleanTotal>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            new TypeBoolean(),
            r => new OrTotal(r.Many<BooleanTotal>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            new TypeDecimalNullable(),
            r => new RoundDecimalNullableTotal(
                r.One<DecimalNullableTotal>("operand"),
                r.One<IntegerTotal>("digits")
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
            r => new RoundDecimalTotal(
                r.One<DecimalTotal>("operand"),
                r.One<IntegerTotal>("digits")
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
            r => new RoundIntegerNullableTotal(r.One<DecimalNullableTotal>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "round",
            new TypeInteger(),
            r => new RoundIntegerTotal(r.One<DecimalTotal>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableTotal x) => new RoundingIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableTotal x) => new RoundingIntegerNullableTotal(x)
            ),
            UnionCase.Of(
                (RoundIntegerNullableTotal x) => new RoundingIntegerNullableTotal(x)
            )
        );
        registry.Union(
            UnionCase.Of((FloorIntegerTotal x) => new RoundingIntegerTotal(x)),
            UnionCase.Of((CeilingIntegerTotal x) => new RoundingIntegerTotal(x)),
            UnionCase.Of((RoundIntegerTotal x) => new RoundingIntegerTotal(x))
        );
        registry.Union(
            UnionCase.Of((StringTotal x) => new StringNullableTotal(x)),
            UnionCase.Of((ExactStringNullableTotal x) => new StringNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamString x) => new StringTotal(x)),
            UnionCase.Of((LiteralString x) => new StringTotal(x)),
            UnionCase.Of((ConcatStringTotal x) => new StringTotal(x)),
            UnionCase.Of((ConditionalStringTotal x) => new StringTotal(x))
        );
        registry.Operator(
            "subtract",
            new TypeDecimalNullable(),
            r => new SubtractDecimalNullableTotal(r.Many<DecimalNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeDecimal(),
            r => new SubtractDecimalTotal(r.Many<DecimalTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeIntegerNullable(),
            r => new SubtractIntegerNullableTotal(r.Many<IntegerNullableTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeInteger(),
            r => new SubtractIntegerTotal(r.Many<IntegerTotal>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "sum",
            new TypeDecimal(),
            r => new SumDecimalTotal(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "sum",
            new TypeInteger(),
            r => new SumIntegerTotal(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate")
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "timeAddSeconds",
            new TypeTimeNullable(),
            r => new TimeAddSecondsTimeNullableTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<DecimalNullableTotal>("right")
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
            r => new TimeAddSecondsTimeTotal(
                r.One<TimeTotal>("left"),
                r.One<DecimalTotal>("right")
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
            r => new TimeDiffSecondsDecimalNullableTotal(
                r.One<TimeNullableTotal>("left"),
                r.One<TimeNullableTotal>("right")
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
            r => new TimeDiffSecondsDecimalTotal(
                r.One<TimeTotal>("left"),
                r.One<TimeTotal>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((TimeTotal x) => new TimeNullableTotal(x)),
            UnionCase.Of((ExactTimeNullableTotal x) => new TimeNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamTime x) => new TimeTotal(x)),
            UnionCase.Of((LiteralTime x) => new TimeTotal(x)),
            UnionCase.Of((TimeAddSecondsTimeTotal x) => new TimeTotal(x)),
            UnionCase.Of((ConditionalTimeTotal x) => new TimeTotal(x))
        );
        registry.Union(
            UnionCase.Of((UuidTotal x) => new UuidNullableTotal(x)),
            UnionCase.Of((ExactUuidNullableTotal x) => new UuidNullableTotal(x))
        );
        registry.Union(
            UnionCase.Of((ParamUuid x) => new UuidTotal(x)),
            UnionCase.Of((LiteralUuid x) => new UuidTotal(x)),
            UnionCase.Of((ConditionalUuidTotal x) => new UuidTotal(x))
        );
    }
}
