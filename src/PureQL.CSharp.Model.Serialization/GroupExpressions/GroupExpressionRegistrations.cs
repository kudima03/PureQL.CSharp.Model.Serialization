using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.GroupExpressions;

internal static class GroupExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            new TypeDecimal(),
            r => new AddDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeDecimalNullable(),
            r => new AddDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeInteger(),
            r => new AddIntegerGroup(r.Many<IntegerGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeIntegerNullable(),
            r => new AddIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((MinBooleanGroup x) => new AggregateBooleanGroup(x)),
            UnionCase.Of((MaxBooleanGroup x) => new AggregateBooleanGroup(x)),
            UnionCase.Of((AnyGroup x) => new AggregateBooleanGroup(x)),
            UnionCase.Of((AllGroup x) => new AggregateBooleanGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinBooleanNullableGroup x) => new AggregateBooleanNullableGroup(x)
            ),
            UnionCase.Of(
                (MaxBooleanNullableGroup x) => new AggregateBooleanNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((AverageDateGroup x) => new AggregateDateGroup(x)),
            UnionCase.Of((MinDateGroup x) => new AggregateDateGroup(x)),
            UnionCase.Of((MaxDateGroup x) => new AggregateDateGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDateNullableGroup x) => new AggregateDateNullableGroup(x)
            ),
            UnionCase.Of((MinDateNullableGroup x) => new AggregateDateNullableGroup(x)),
            UnionCase.Of((MaxDateNullableGroup x) => new AggregateDateNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((AverageDatetimeGroup x) => new AggregateDatetimeGroup(x)),
            UnionCase.Of((MinDatetimeGroup x) => new AggregateDatetimeGroup(x)),
            UnionCase.Of((MaxDatetimeGroup x) => new AggregateDatetimeGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDatetimeNullableGroup x) => new AggregateDatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (MinDatetimeNullableGroup x) => new AggregateDatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (MaxDatetimeNullableGroup x) => new AggregateDatetimeNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((SumDecimalGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((AverageDecimalGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((MinDecimalGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((MaxDecimalGroup x) => new AggregateDecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDecimalNullableGroup x) => new AggregateDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (MinDecimalNullableGroup x) => new AggregateDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (MaxDecimalNullableGroup x) => new AggregateDecimalNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((CountGroup x) => new AggregateIntegerGroup(x)),
            UnionCase.Of((SumIntegerGroup x) => new AggregateIntegerGroup(x)),
            UnionCase.Of((MinIntegerGroup x) => new AggregateIntegerGroup(x)),
            UnionCase.Of((MaxIntegerGroup x) => new AggregateIntegerGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinIntegerNullableGroup x) => new AggregateIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (MaxIntegerNullableGroup x) => new AggregateIntegerNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((MinStringGroup x) => new AggregateStringGroup(x)),
            UnionCase.Of((MaxStringGroup x) => new AggregateStringGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinStringNullableGroup x) => new AggregateStringNullableGroup(x)
            ),
            UnionCase.Of(
                (MaxStringNullableGroup x) => new AggregateStringNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((AverageTimeGroup x) => new AggregateTimeGroup(x)),
            UnionCase.Of((MinTimeGroup x) => new AggregateTimeGroup(x)),
            UnionCase.Of((MaxTimeGroup x) => new AggregateTimeGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageTimeNullableGroup x) => new AggregateTimeNullableGroup(x)
            ),
            UnionCase.Of((MinTimeNullableGroup x) => new AggregateTimeNullableGroup(x)),
            UnionCase.Of((MaxTimeNullableGroup x) => new AggregateTimeNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((MinUuidGroup x) => new AggregateUuidGroup(x)),
            UnionCase.Of((MaxUuidGroup x) => new AggregateUuidGroup(x))
        );
        registry.Union(
            UnionCase.Of((MinUuidNullableGroup x) => new AggregateUuidNullableGroup(x)),
            UnionCase.Of((MaxUuidNullableGroup x) => new AggregateUuidNullableGroup(x))
        );
        registry.Operator(
            "all",
            new TypeBoolean(),
            r => new AllGroup(r.One<BooleanRow>("predicate"), r.Over()),
            (w, x) =>
            {
                w.One("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "and",
            new TypeBoolean(),
            r => new AndGroup(r.Many<BooleanGroup>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "any",
            new TypeBoolean(),
            r => new AnyGroup(r.One<BooleanRow>("predicate"), r.Over()),
            (w, x) =>
            {
                w.One("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Union(
            UnionCase.Of((AddDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((SubtractDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((MultiplyDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((DivideDecimalGroup x) => new ArithmeticDecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (AddDecimalNullableGroup x) => new ArithmeticDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (SubtractDecimalNullableGroup x) => new ArithmeticDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (MultiplyDecimalNullableGroup x) => new ArithmeticDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (DivideDecimalNullableGroup x) => new ArithmeticDecimalNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddIntegerGroup x) => new ArithmeticIntegerGroup(x)),
            UnionCase.Of((SubtractIntegerGroup x) => new ArithmeticIntegerGroup(x)),
            UnionCase.Of((MultiplyIntegerGroup x) => new ArithmeticIntegerGroup(x)),
            UnionCase.Of((IntegerDivideIntegerGroup x) => new ArithmeticIntegerGroup(x)),
            UnionCase.Of((ModuloIntegerGroup x) => new ArithmeticIntegerGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (AddIntegerNullableGroup x) => new ArithmeticIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (SubtractIntegerNullableGroup x) => new ArithmeticIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (MultiplyIntegerNullableGroup x) => new ArithmeticIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableGroup x) =>
                    new ArithmeticIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableGroup x) => new ArithmeticIntegerNullableGroup(x)
            )
        );
        registry.Operator(
            "average",
            new TypeDate(),
            r => new AverageDateGroup(r.One<DateRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeDateNullable(),
            r => new AverageDateNullableGroup(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeDatetime(),
            r => new AverageDatetimeGroup(r.One<DatetimeRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeDatetimeNullable(),
            r => new AverageDatetimeNullableGroup(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeDecimal(),
            r => new AverageDecimalGroup(r.One<DecimalRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeDecimalNullable(),
            r => new AverageDecimalNullableGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeTime(),
            r => new AverageTimeGroup(r.One<TimeRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "average",
            new TypeTimeNullable(),
            r => new AverageTimeNullableGroup(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Union(
            UnionCase.Of((KeyBoolean x) => new BooleanGroup(x)),
            UnionCase.Of((ParamBoolean x) => new BooleanGroup(x)),
            UnionCase.Of((LiteralBoolean x) => new BooleanGroup(x)),
            UnionCase.Of((LogicalGroup x) => new BooleanGroup(x)),
            UnionCase.Of((ComparisonGroup x) => new BooleanGroup(x)),
            UnionCase.Of((ConditionalBooleanGroup x) => new BooleanGroup(x)),
            UnionCase.Of((AggregateBooleanGroup x) => new BooleanGroup(x))
        );
        registry.Union(
            UnionCase.Of((BooleanGroup x) => new BooleanNullableGroup(x)),
            UnionCase.Of((ExactBooleanNullableGroup x) => new BooleanNullableGroup(x))
        );
        registry.Operator(
            "ceiling",
            new TypeInteger(),
            r => new CeilingIntegerGroup(r.One<DecimalGroup>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "ceiling",
            new TypeIntegerNullable(),
            r => new CeilingIntegerNullableGroup(r.One<DecimalNullableGroup>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "coalesce",
            new TypeBoolean(),
            r => new CoalesceBooleanGroup(r.Many<BooleanNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeBooleanNullable(),
            r => new CoalesceBooleanNullableGroup(r.Many<BooleanNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDate(),
            r => new CoalesceDateGroup(r.Many<DateNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDateNullable(),
            r => new CoalesceDateNullableGroup(r.Many<DateNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetime(),
            r => new CoalesceDatetimeGroup(r.Many<DatetimeNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetimeNullable(),
            r => new CoalesceDatetimeNullableGroup(
                r.Many<DatetimeNullableGroup>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimal(),
            r => new CoalesceDecimalGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimalNullable(),
            r => new CoalesceDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeInteger(),
            r => new CoalesceIntegerGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeIntegerNullable(),
            r => new CoalesceIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeString(),
            r => new CoalesceStringGroup(r.Many<StringNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeStringNullable(),
            r => new CoalesceStringNullableGroup(r.Many<StringNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTime(),
            r => new CoalesceTimeGroup(r.Many<TimeNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTimeNullable(),
            r => new CoalesceTimeNullableGroup(r.Many<TimeNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuid(),
            r => new CoalesceUuidGroup(r.Many<UuidNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuidNullable(),
            r => new CoalesceUuidNullableGroup(r.Many<UuidNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((EqualGroup x) => new ComparisonGroup(x)),
            UnionCase.Of((NotEqualGroup x) => new ComparisonGroup(x)),
            UnionCase.Of((InGroup x) => new ComparisonGroup(x)),
            UnionCase.Of((GreaterThanGroup x) => new ComparisonGroup(x)),
            UnionCase.Of((LessThanGroup x) => new ComparisonGroup(x)),
            UnionCase.Of((GreaterThanOrEqualGroup x) => new ComparisonGroup(x)),
            UnionCase.Of((LessThanOrEqualGroup x) => new ComparisonGroup(x))
        );
        registry.Operator(
            "concat",
            new TypeString(),
            r => new ConcatStringGroup(r.Many<StringGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
            new TypeStringNullable(),
            r => new ConcatStringNullableGroup(r.Many<StringNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((IfBooleanGroup x) => new ConditionalBooleanGroup(x)),
            UnionCase.Of((CoalesceBooleanGroup x) => new ConditionalBooleanGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfBooleanNullableGroup x) => new ConditionalBooleanNullableGroup(x)
            ),
            UnionCase.Of(
                (CoalesceBooleanNullableGroup x) => new ConditionalBooleanNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDateGroup x) => new ConditionalDateGroup(x)),
            UnionCase.Of((CoalesceDateGroup x) => new ConditionalDateGroup(x))
        );
        registry.Union(
            UnionCase.Of((IfDateNullableGroup x) => new ConditionalDateNullableGroup(x)),
            UnionCase.Of(
                (CoalesceDateNullableGroup x) => new ConditionalDateNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDatetimeGroup x) => new ConditionalDatetimeGroup(x)),
            UnionCase.Of((CoalesceDatetimeGroup x) => new ConditionalDatetimeGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDatetimeNullableGroup x) => new ConditionalDatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (CoalesceDatetimeNullableGroup x) =>
                    new ConditionalDatetimeNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDecimalGroup x) => new ConditionalDecimalGroup(x)),
            UnionCase.Of((CoalesceDecimalGroup x) => new ConditionalDecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDecimalNullableGroup x) => new ConditionalDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (CoalesceDecimalNullableGroup x) => new ConditionalDecimalNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfIntegerGroup x) => new ConditionalIntegerGroup(x)),
            UnionCase.Of((CoalesceIntegerGroup x) => new ConditionalIntegerGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfIntegerNullableGroup x) => new ConditionalIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (CoalesceIntegerNullableGroup x) => new ConditionalIntegerNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfStringGroup x) => new ConditionalStringGroup(x)),
            UnionCase.Of((CoalesceStringGroup x) => new ConditionalStringGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfStringNullableGroup x) => new ConditionalStringNullableGroup(x)
            ),
            UnionCase.Of(
                (CoalesceStringNullableGroup x) => new ConditionalStringNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfTimeGroup x) => new ConditionalTimeGroup(x)),
            UnionCase.Of((CoalesceTimeGroup x) => new ConditionalTimeGroup(x))
        );
        registry.Union(
            UnionCase.Of((IfTimeNullableGroup x) => new ConditionalTimeNullableGroup(x)),
            UnionCase.Of(
                (CoalesceTimeNullableGroup x) => new ConditionalTimeNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfUuidGroup x) => new ConditionalUuidGroup(x)),
            UnionCase.Of((CoalesceUuidGroup x) => new ConditionalUuidGroup(x))
        );
        registry.Union(
            UnionCase.Of((IfUuidNullableGroup x) => new ConditionalUuidNullableGroup(x)),
            UnionCase.Of(
                (CoalesceUuidNullableGroup x) => new ConditionalUuidNullableGroup(x)
            )
        );
        registry.Operator(
            "count",
            new TypeInteger(),
            r => new CountGroup(r.Optional<BooleanRow>("predicate"), r.Over()),
            (w, x) =>
            {
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "dateAddDays",
            new TypeDate(),
            r => new DateAddDaysDateGroup(
                r.One<DateGroup>("left"),
                r.One<IntegerGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateAddDays",
            new TypeDateNullable(),
            r => new DateAddDaysDateNullableGroup(
                r.One<DateNullableGroup>("left"),
                r.One<IntegerNullableGroup>("right")
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
            r => new DateDiffDaysIntegerGroup(
                r.One<DateGroup>("left"),
                r.One<DateGroup>("right")
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
            r => new DateDiffDaysIntegerNullableGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((KeyDate x) => new DateGroup(x)),
            UnionCase.Of((ParamDate x) => new DateGroup(x)),
            UnionCase.Of((LiteralDate x) => new DateGroup(x)),
            UnionCase.Of((DateAddDaysDateGroup x) => new DateGroup(x)),
            UnionCase.Of((ConditionalDateGroup x) => new DateGroup(x)),
            UnionCase.Of((AggregateDateGroup x) => new DateGroup(x))
        );
        registry.Union(
            UnionCase.Of((DateGroup x) => new DateNullableGroup(x)),
            UnionCase.Of((ExactDateNullableGroup x) => new DateNullableGroup(x))
        );
        registry.Operator(
            "datetimeAddSeconds",
            new TypeDatetime(),
            r => new DatetimeAddSecondsDatetimeGroup(
                r.One<DatetimeGroup>("left"),
                r.One<DecimalGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeAddSeconds",
            new TypeDatetimeNullable(),
            r => new DatetimeAddSecondsDatetimeNullableGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
            r => new DatetimeDiffSecondsDecimalGroup(
                r.One<DatetimeGroup>("left"),
                r.One<DatetimeGroup>("right")
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
            r => new DatetimeDiffSecondsDecimalNullableGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((KeyDatetime x) => new DatetimeGroup(x)),
            UnionCase.Of((ParamDatetime x) => new DatetimeGroup(x)),
            UnionCase.Of((LiteralDatetime x) => new DatetimeGroup(x)),
            UnionCase.Of((DatetimeAddSecondsDatetimeGroup x) => new DatetimeGroup(x)),
            UnionCase.Of((ConditionalDatetimeGroup x) => new DatetimeGroup(x)),
            UnionCase.Of((AggregateDatetimeGroup x) => new DatetimeGroup(x))
        );
        registry.Union(
            UnionCase.Of((DatetimeGroup x) => new DatetimeNullableGroup(x)),
            UnionCase.Of((ExactDatetimeNullableGroup x) => new DatetimeNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalGroup x) => new DecimalGroup(x)),
            UnionCase.Of((IntegerGroup x) => new DecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalGroup x) => new DecimalNullableGroup(x)),
            UnionCase.Of((ExactDecimalNullableGroup x) => new DecimalNullableGroup(x)),
            UnionCase.Of((IntegerGroup x) => new DecimalNullableGroup(x)),
            UnionCase.Of((ExactIntegerNullableGroup x) => new DecimalNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (TimeDiffSecondsDecimalGroup x) => new DifferenceDecimalGroup(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalGroup x) => new DifferenceDecimalGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (TimeDiffSecondsDecimalNullableGroup x) =>
                    new DifferenceDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalNullableGroup x) =>
                    new DifferenceDecimalNullableGroup(x)
            )
        );
        registry.Operator(
            "divide",
            new TypeDecimal(),
            r => new DivideDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            new TypeDecimalNullable(),
            r => new DivideDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
            new TypeBoolean(),
            r => new EqualBooleanGroup(
                r.One<BooleanNullableGroup>("left"),
                r.One<BooleanNullableGroup>("right")
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
            r => new EqualDateGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
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
            r => new EqualDatetimeGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
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
            r => new EqualDecimalGroup(
                r.One<DecimalNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Probe(
            "left",
            UnionCase.Of(TypeFamily.Decimal, (EqualDecimalGroup x) => new EqualGroup(x)),
            UnionCase.Of(TypeFamily.String, (EqualStringGroup x) => new EqualGroup(x)),
            UnionCase.Of(TypeFamily.Boolean, (EqualBooleanGroup x) => new EqualGroup(x)),
            UnionCase.Of(TypeFamily.Date, (EqualDateGroup x) => new EqualGroup(x)),
            UnionCase.Of(TypeFamily.Time, (EqualTimeGroup x) => new EqualGroup(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (EqualDatetimeGroup x) => new EqualGroup(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (EqualUuidGroup x) => new EqualGroup(x))
        );
        registry.Operator(
            "equal",
            new TypeBoolean(),
            r => new EqualStringGroup(
                r.One<StringNullableGroup>("left"),
                r.One<StringNullableGroup>("right")
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
            r => new EqualTimeGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
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
            r => new EqualUuidGroup(
                r.One<UuidNullableGroup>("left"),
                r.One<UuidNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((KeyBooleanNullable x) => new ExactBooleanNullableGroup(x)),
            UnionCase.Of((ParamBooleanNullable x) => new ExactBooleanNullableGroup(x)),
            UnionCase.Of((LiteralBooleanNullable x) => new ExactBooleanNullableGroup(x)),
            UnionCase.Of(
                (ConditionalBooleanNullableGroup x) => new ExactBooleanNullableGroup(x)
            ),
            UnionCase.Of(
                (AggregateBooleanNullableGroup x) => new ExactBooleanNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyDateNullable x) => new ExactDateNullableGroup(x)),
            UnionCase.Of((ParamDateNullable x) => new ExactDateNullableGroup(x)),
            UnionCase.Of((LiteralDateNullable x) => new ExactDateNullableGroup(x)),
            UnionCase.Of(
                (DateAddDaysDateNullableGroup x) => new ExactDateNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalDateNullableGroup x) => new ExactDateNullableGroup(x)
            ),
            UnionCase.Of((AggregateDateNullableGroup x) => new ExactDateNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyDatetimeNullable x) => new ExactDatetimeNullableGroup(x)),
            UnionCase.Of((ParamDatetimeNullable x) => new ExactDatetimeNullableGroup(x)),
            UnionCase.Of(
                (LiteralDatetimeNullable x) => new ExactDatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableGroup x) =>
                    new ExactDatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalDatetimeNullableGroup x) => new ExactDatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (AggregateDatetimeNullableGroup x) => new ExactDatetimeNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyDecimal x) => new ExactDecimalGroup(x)),
            UnionCase.Of((ParamDecimal x) => new ExactDecimalGroup(x)),
            UnionCase.Of((LiteralDecimal x) => new ExactDecimalGroup(x)),
            UnionCase.Of((ArithmeticDecimalGroup x) => new ExactDecimalGroup(x)),
            UnionCase.Of((RoundDecimalGroup x) => new ExactDecimalGroup(x)),
            UnionCase.Of((DifferenceDecimalGroup x) => new ExactDecimalGroup(x)),
            UnionCase.Of((ConditionalDecimalGroup x) => new ExactDecimalGroup(x)),
            UnionCase.Of((AggregateDecimalGroup x) => new ExactDecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyDecimalNullable x) => new ExactDecimalNullableGroup(x)),
            UnionCase.Of((ParamDecimalNullable x) => new ExactDecimalNullableGroup(x)),
            UnionCase.Of((LiteralDecimalNullable x) => new ExactDecimalNullableGroup(x)),
            UnionCase.Of(
                (ArithmeticDecimalNullableGroup x) => new ExactDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (RoundDecimalNullableGroup x) => new ExactDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (DifferenceDecimalNullableGroup x) => new ExactDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalNullableGroup x) => new ExactDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (AggregateDecimalNullableGroup x) => new ExactDecimalNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyIntegerNullable x) => new ExactIntegerNullableGroup(x)),
            UnionCase.Of((ParamIntegerNullable x) => new ExactIntegerNullableGroup(x)),
            UnionCase.Of((LiteralIntegerNullable x) => new ExactIntegerNullableGroup(x)),
            UnionCase.Of(
                (ArithmeticIntegerNullableGroup x) => new ExactIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (RoundingIntegerNullableGroup x) => new ExactIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (DateDiffDaysIntegerNullableGroup x) => new ExactIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalIntegerNullableGroup x) => new ExactIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (AggregateIntegerNullableGroup x) => new ExactIntegerNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyStringNullable x) => new ExactStringNullableGroup(x)),
            UnionCase.Of((ParamStringNullable x) => new ExactStringNullableGroup(x)),
            UnionCase.Of((LiteralStringNullable x) => new ExactStringNullableGroup(x)),
            UnionCase.Of(
                (ConcatStringNullableGroup x) => new ExactStringNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalStringNullableGroup x) => new ExactStringNullableGroup(x)
            ),
            UnionCase.Of(
                (AggregateStringNullableGroup x) => new ExactStringNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyTimeNullable x) => new ExactTimeNullableGroup(x)),
            UnionCase.Of((ParamTimeNullable x) => new ExactTimeNullableGroup(x)),
            UnionCase.Of((LiteralTimeNullable x) => new ExactTimeNullableGroup(x)),
            UnionCase.Of(
                (TimeAddSecondsTimeNullableGroup x) => new ExactTimeNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalTimeNullableGroup x) => new ExactTimeNullableGroup(x)
            ),
            UnionCase.Of((AggregateTimeNullableGroup x) => new ExactTimeNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyUuidNullable x) => new ExactUuidNullableGroup(x)),
            UnionCase.Of((ParamUuidNullable x) => new ExactUuidNullableGroup(x)),
            UnionCase.Of((LiteralUuidNullable x) => new ExactUuidNullableGroup(x)),
            UnionCase.Of(
                (ConditionalUuidNullableGroup x) => new ExactUuidNullableGroup(x)
            ),
            UnionCase.Of((AggregateUuidNullableGroup x) => new ExactUuidNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((DecimalNullableGroup x) => new ExprGroup(x)),
            UnionCase.Of((StringNullableGroup x) => new ExprGroup(x)),
            UnionCase.Of((BooleanNullableGroup x) => new ExprGroup(x)),
            UnionCase.Of((DateNullableGroup x) => new ExprGroup(x)),
            UnionCase.Of((TimeNullableGroup x) => new ExprGroup(x)),
            UnionCase.Of((DatetimeNullableGroup x) => new ExprGroup(x)),
            UnionCase.Of((UuidNullableGroup x) => new ExprGroup(x))
        );
        registry.Operator(
            "floor",
            new TypeInteger(),
            r => new FloorIntegerGroup(r.One<DecimalGroup>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "floor",
            new TypeIntegerNullable(),
            r => new FloorIntegerNullableGroup(r.One<DecimalNullableGroup>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanBooleanGroup(
                r.One<BooleanNullableGroup>("left"),
                r.One<BooleanNullableGroup>("right")
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
            r => new GreaterThanDateGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
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
            r => new GreaterThanDatetimeGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
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
            r => new GreaterThanDecimalGroup(
                r.One<DecimalNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
                (GreaterThanDecimalGroup x) => new GreaterThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanStringGroup x) => new GreaterThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (GreaterThanBooleanGroup x) => new GreaterThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanDateGroup x) => new GreaterThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanTimeGroup x) => new GreaterThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanDatetimeGroup x) => new GreaterThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanUuidGroup x) => new GreaterThanGroup(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
            r => new GreaterThanOrEqualBooleanGroup(
                r.One<BooleanNullableGroup>("left"),
                r.One<BooleanNullableGroup>("right")
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
            r => new GreaterThanOrEqualDateGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
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
            r => new GreaterThanOrEqualDatetimeGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
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
            r => new GreaterThanOrEqualDecimalGroup(
                r.One<DecimalNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
                (GreaterThanOrEqualDecimalGroup x) => new GreaterThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanOrEqualStringGroup x) => new GreaterThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (GreaterThanOrEqualBooleanGroup x) => new GreaterThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanOrEqualDateGroup x) => new GreaterThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanOrEqualTimeGroup x) => new GreaterThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanOrEqualDatetimeGroup x) => new GreaterThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanOrEqualUuidGroup x) => new GreaterThanOrEqualGroup(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
            r => new GreaterThanOrEqualStringGroup(
                r.One<StringNullableGroup>("left"),
                r.One<StringNullableGroup>("right")
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
            r => new GreaterThanOrEqualTimeGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
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
            r => new GreaterThanOrEqualUuidGroup(
                r.One<UuidNullableGroup>("left"),
                r.One<UuidNullableGroup>("right")
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
            r => new GreaterThanStringGroup(
                r.One<StringNullableGroup>("left"),
                r.One<StringNullableGroup>("right")
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
            r => new GreaterThanTimeGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
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
            r => new GreaterThanUuidGroup(
                r.One<UuidNullableGroup>("left"),
                r.One<UuidNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "if",
            new TypeBoolean(),
            r => new IfBooleanGroup(
                r.One<BooleanGroup>("condition"),
                r.One<BooleanGroup>("then"),
                r.One<BooleanGroup>("else")
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
            new TypeBooleanNullable(),
            r => new IfBooleanNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<BooleanNullableGroup>("then"),
                r.One<BooleanNullableGroup>("else")
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
            r => new IfDateGroup(
                r.One<BooleanGroup>("condition"),
                r.One<DateGroup>("then"),
                r.One<DateGroup>("else")
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
            r => new IfDateNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<DateNullableGroup>("then"),
                r.One<DateNullableGroup>("else")
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
            r => new IfDatetimeGroup(
                r.One<BooleanGroup>("condition"),
                r.One<DatetimeGroup>("then"),
                r.One<DatetimeGroup>("else")
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
            r => new IfDatetimeNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<DatetimeNullableGroup>("then"),
                r.One<DatetimeNullableGroup>("else")
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
            r => new IfDecimalGroup(
                r.One<BooleanGroup>("condition"),
                r.One<DecimalGroup>("then"),
                r.One<DecimalGroup>("else")
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
            r => new IfDecimalNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<DecimalNullableGroup>("then"),
                r.One<DecimalNullableGroup>("else")
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
            r => new IfIntegerGroup(
                r.One<BooleanGroup>("condition"),
                r.One<IntegerGroup>("then"),
                r.One<IntegerGroup>("else")
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
            r => new IfIntegerNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<IntegerNullableGroup>("then"),
                r.One<IntegerNullableGroup>("else")
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
            r => new IfStringGroup(
                r.One<BooleanGroup>("condition"),
                r.One<StringGroup>("then"),
                r.One<StringGroup>("else")
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
            r => new IfStringNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<StringNullableGroup>("then"),
                r.One<StringNullableGroup>("else")
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
            r => new IfTimeGroup(
                r.One<BooleanGroup>("condition"),
                r.One<TimeGroup>("then"),
                r.One<TimeGroup>("else")
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
            r => new IfTimeNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<TimeNullableGroup>("then"),
                r.One<TimeNullableGroup>("else")
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
            r => new IfUuidGroup(
                r.One<BooleanGroup>("condition"),
                r.One<UuidGroup>("then"),
                r.One<UuidGroup>("else")
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
            r => new IfUuidNullableGroup(
                r.One<BooleanGroup>("condition"),
                r.One<UuidNullableGroup>("then"),
                r.One<UuidNullableGroup>("else")
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
            r => new InBooleanGroup(
                r.One<BooleanNullableGroup>("operand"),
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
            r => new InDateGroup(
                r.One<DateNullableGroup>("operand"),
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
            r => new InDatetimeGroup(
                r.One<DatetimeNullableGroup>("operand"),
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
            r => new InDecimalGroup(
                r.One<DecimalNullableGroup>("operand"),
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
            UnionCase.Of(TypeFamily.Decimal, (InDecimalGroup x) => new InGroup(x)),
            UnionCase.Of(TypeFamily.String, (InStringGroup x) => new InGroup(x)),
            UnionCase.Of(TypeFamily.Boolean, (InBooleanGroup x) => new InGroup(x)),
            UnionCase.Of(TypeFamily.Date, (InDateGroup x) => new InGroup(x)),
            UnionCase.Of(TypeFamily.Time, (InTimeGroup x) => new InGroup(x)),
            UnionCase.Of(TypeFamily.Datetime, (InDatetimeGroup x) => new InGroup(x)),
            UnionCase.Of(TypeFamily.Uuid, (InUuidGroup x) => new InGroup(x))
        );
        registry.Operator(
            "in",
            new TypeBoolean(),
            r => new InStringGroup(
                r.One<StringNullableGroup>("operand"),
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
            r => new InTimeGroup(
                r.One<TimeNullableGroup>("operand"),
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
            r => new InUuidGroup(
                r.One<UuidNullableGroup>("operand"),
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
            new TypeInteger(),
            r => new IntegerDivideIntegerGroup(
                r.One<IntegerGroup>("left"),
                r.One<IntegerGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "integerDivide",
            new TypeIntegerNullable(),
            r => new IntegerDivideIntegerNullableGroup(
                r.One<IntegerNullableGroup>("left"),
                r.One<IntegerNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((KeyInteger x) => new IntegerGroup(x)),
            UnionCase.Of((ParamInteger x) => new IntegerGroup(x)),
            UnionCase.Of((LiteralInteger x) => new IntegerGroup(x)),
            UnionCase.Of((ArithmeticIntegerGroup x) => new IntegerGroup(x)),
            UnionCase.Of((RoundingIntegerGroup x) => new IntegerGroup(x)),
            UnionCase.Of((DateDiffDaysIntegerGroup x) => new IntegerGroup(x)),
            UnionCase.Of((ConditionalIntegerGroup x) => new IntegerGroup(x)),
            UnionCase.Of((AggregateIntegerGroup x) => new IntegerGroup(x))
        );
        registry.Union(
            UnionCase.Of((IntegerGroup x) => new IntegerNullableGroup(x)),
            UnionCase.Of((ExactIntegerNullableGroup x) => new IntegerNullableGroup(x))
        );
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
            r => new LessThanBooleanGroup(
                r.One<BooleanNullableGroup>("left"),
                r.One<BooleanNullableGroup>("right")
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
            r => new LessThanDateGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
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
            r => new LessThanDatetimeGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
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
            r => new LessThanDecimalGroup(
                r.One<DecimalNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
                (LessThanDecimalGroup x) => new LessThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanStringGroup x) => new LessThanGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (LessThanBooleanGroup x) => new LessThanGroup(x)
            ),
            UnionCase.Of(TypeFamily.Date, (LessThanDateGroup x) => new LessThanGroup(x)),
            UnionCase.Of(TypeFamily.Time, (LessThanTimeGroup x) => new LessThanGroup(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanDatetimeGroup x) => new LessThanGroup(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (LessThanUuidGroup x) => new LessThanGroup(x))
        );
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
            r => new LessThanOrEqualBooleanGroup(
                r.One<BooleanNullableGroup>("left"),
                r.One<BooleanNullableGroup>("right")
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
            r => new LessThanOrEqualDateGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
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
            r => new LessThanOrEqualDatetimeGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
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
            r => new LessThanOrEqualDecimalGroup(
                r.One<DecimalNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
                (LessThanOrEqualDecimalGroup x) => new LessThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanOrEqualStringGroup x) => new LessThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (LessThanOrEqualBooleanGroup x) => new LessThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (LessThanOrEqualDateGroup x) => new LessThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (LessThanOrEqualTimeGroup x) => new LessThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanOrEqualDatetimeGroup x) => new LessThanOrEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (LessThanOrEqualUuidGroup x) => new LessThanOrEqualGroup(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
            r => new LessThanOrEqualStringGroup(
                r.One<StringNullableGroup>("left"),
                r.One<StringNullableGroup>("right")
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
            r => new LessThanOrEqualTimeGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
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
            r => new LessThanOrEqualUuidGroup(
                r.One<UuidNullableGroup>("left"),
                r.One<UuidNullableGroup>("right")
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
            r => new LessThanStringGroup(
                r.One<StringNullableGroup>("left"),
                r.One<StringNullableGroup>("right")
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
            r => new LessThanTimeGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
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
            r => new LessThanUuidGroup(
                r.One<UuidNullableGroup>("left"),
                r.One<UuidNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((AndGroup x) => new LogicalGroup(x)),
            UnionCase.Of((OrGroup x) => new LogicalGroup(x)),
            UnionCase.Of((NotGroup x) => new LogicalGroup(x))
        );
        registry.Operator(
            "max",
            new TypeBoolean(),
            r => new MaxBooleanGroup(r.One<BooleanRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeBooleanNullable(),
            r => new MaxBooleanNullableGroup(
                r.One<BooleanNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeDate(),
            r => new MaxDateGroup(r.One<DateRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeDateNullable(),
            r => new MaxDateNullableGroup(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeDatetime(),
            r => new MaxDatetimeGroup(r.One<DatetimeRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeDatetimeNullable(),
            r => new MaxDatetimeNullableGroup(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeDecimal(),
            r => new MaxDecimalGroup(r.One<DecimalRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeDecimalNullable(),
            r => new MaxDecimalNullableGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeInteger(),
            r => new MaxIntegerGroup(r.One<IntegerRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeIntegerNullable(),
            r => new MaxIntegerNullableGroup(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeString(),
            r => new MaxStringGroup(r.One<StringRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeStringNullable(),
            r => new MaxStringNullableGroup(
                r.One<StringNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeTime(),
            r => new MaxTimeGroup(r.One<TimeRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeTimeNullable(),
            r => new MaxTimeNullableGroup(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeUuid(),
            r => new MaxUuidGroup(r.One<UuidRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "max",
            new TypeUuidNullable(),
            r => new MaxUuidNullableGroup(
                r.One<UuidNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeBoolean(),
            r => new MinBooleanGroup(r.One<BooleanRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeBooleanNullable(),
            r => new MinBooleanNullableGroup(
                r.One<BooleanNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeDate(),
            r => new MinDateGroup(r.One<DateRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeDateNullable(),
            r => new MinDateNullableGroup(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeDatetime(),
            r => new MinDatetimeGroup(r.One<DatetimeRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeDatetimeNullable(),
            r => new MinDatetimeNullableGroup(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeDecimal(),
            r => new MinDecimalGroup(r.One<DecimalRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeDecimalNullable(),
            r => new MinDecimalNullableGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeInteger(),
            r => new MinIntegerGroup(r.One<IntegerRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeIntegerNullable(),
            r => new MinIntegerNullableGroup(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeString(),
            r => new MinStringGroup(r.One<StringRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeStringNullable(),
            r => new MinStringNullableGroup(
                r.One<StringNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeTime(),
            r => new MinTimeGroup(r.One<TimeRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeTimeNullable(),
            r => new MinTimeNullableGroup(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeUuid(),
            r => new MinUuidGroup(r.One<UuidRow>("selector"), r.Over()),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "min",
            new TypeUuidNullable(),
            r => new MinUuidNullableGroup(
                r.One<UuidNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "modulo",
            new TypeInteger(),
            r => new ModuloIntegerGroup(
                r.One<IntegerGroup>("left"),
                r.One<IntegerGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "modulo",
            new TypeIntegerNullable(),
            r => new ModuloIntegerNullableGroup(
                r.One<IntegerNullableGroup>("left"),
                r.One<IntegerNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "multiply",
            new TypeDecimal(),
            r => new MultiplyDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeDecimalNullable(),
            r => new MultiplyDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeInteger(),
            r => new MultiplyIntegerGroup(r.Many<IntegerGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeIntegerNullable(),
            r => new MultiplyIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
            new TypeBoolean(),
            r => new NotEqualBooleanGroup(
                r.One<BooleanNullableGroup>("left"),
                r.One<BooleanNullableGroup>("right")
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
            r => new NotEqualDateGroup(
                r.One<DateNullableGroup>("left"),
                r.One<DateNullableGroup>("right")
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
            r => new NotEqualDatetimeGroup(
                r.One<DatetimeNullableGroup>("left"),
                r.One<DatetimeNullableGroup>("right")
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
            r => new NotEqualDecimalGroup(
                r.One<DecimalNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
                (NotEqualDecimalGroup x) => new NotEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (NotEqualStringGroup x) => new NotEqualGroup(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (NotEqualBooleanGroup x) => new NotEqualGroup(x)
            ),
            UnionCase.Of(TypeFamily.Date, (NotEqualDateGroup x) => new NotEqualGroup(x)),
            UnionCase.Of(TypeFamily.Time, (NotEqualTimeGroup x) => new NotEqualGroup(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (NotEqualDatetimeGroup x) => new NotEqualGroup(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (NotEqualUuidGroup x) => new NotEqualGroup(x))
        );
        registry.Operator(
            "notEqual",
            new TypeBoolean(),
            r => new NotEqualStringGroup(
                r.One<StringNullableGroup>("left"),
                r.One<StringNullableGroup>("right")
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
            r => new NotEqualTimeGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
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
            r => new NotEqualUuidGroup(
                r.One<UuidNullableGroup>("left"),
                r.One<UuidNullableGroup>("right")
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
            r => new NotGroup(r.One<BooleanGroup>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            new TypeBoolean(),
            r => new OrGroup(r.Many<BooleanGroup>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            new TypeDecimal(),
            r => new RoundDecimalGroup(
                r.One<DecimalGroup>("operand"),
                r.One<IntegerGroup>("digits")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("digits", x.Digits);
            }
        );
        registry.Operator(
            "round",
            new TypeDecimalNullable(),
            r => new RoundDecimalNullableGroup(
                r.One<DecimalNullableGroup>("operand"),
                r.One<IntegerGroup>("digits")
            ),
            (w, x) =>
            {
                w.One("operand", x.Operand);
                w.One("digits", x.Digits);
            }
        );
        registry.Operator(
            "round",
            new TypeInteger(),
            r => new RoundIntegerGroup(r.One<DecimalGroup>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "round",
            new TypeIntegerNullable(),
            r => new RoundIntegerNullableGroup(r.One<DecimalNullableGroup>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Union(
            UnionCase.Of((FloorIntegerGroup x) => new RoundingIntegerGroup(x)),
            UnionCase.Of((CeilingIntegerGroup x) => new RoundingIntegerGroup(x)),
            UnionCase.Of((RoundIntegerGroup x) => new RoundingIntegerGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableGroup x) => new RoundingIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableGroup x) => new RoundingIntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (RoundIntegerNullableGroup x) => new RoundingIntegerNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyString x) => new StringGroup(x)),
            UnionCase.Of((ParamString x) => new StringGroup(x)),
            UnionCase.Of((LiteralString x) => new StringGroup(x)),
            UnionCase.Of((ConcatStringGroup x) => new StringGroup(x)),
            UnionCase.Of((ConditionalStringGroup x) => new StringGroup(x)),
            UnionCase.Of((AggregateStringGroup x) => new StringGroup(x))
        );
        registry.Union(
            UnionCase.Of((StringGroup x) => new StringNullableGroup(x)),
            UnionCase.Of((ExactStringNullableGroup x) => new StringNullableGroup(x))
        );
        registry.Operator(
            "subtract",
            new TypeDecimal(),
            r => new SubtractDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeDecimalNullable(),
            r => new SubtractDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeInteger(),
            r => new SubtractIntegerGroup(r.Many<IntegerGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeIntegerNullable(),
            r => new SubtractIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "sum",
            new TypeDecimal(),
            r => new SumDecimalGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "sum",
            new TypeInteger(),
            r => new SumIntegerGroup(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
                w.Over(x.Over);
            }
        );
        registry.Operator(
            "timeAddSeconds",
            new TypeTime(),
            r => new TimeAddSecondsTimeGroup(
                r.One<TimeGroup>("left"),
                r.One<DecimalGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeAddSeconds",
            new TypeTimeNullable(),
            r => new TimeAddSecondsTimeNullableGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<DecimalNullableGroup>("right")
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
            r => new TimeDiffSecondsDecimalGroup(
                r.One<TimeGroup>("left"),
                r.One<TimeGroup>("right")
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
            r => new TimeDiffSecondsDecimalNullableGroup(
                r.One<TimeNullableGroup>("left"),
                r.One<TimeNullableGroup>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((KeyTime x) => new TimeGroup(x)),
            UnionCase.Of((ParamTime x) => new TimeGroup(x)),
            UnionCase.Of((LiteralTime x) => new TimeGroup(x)),
            UnionCase.Of((TimeAddSecondsTimeGroup x) => new TimeGroup(x)),
            UnionCase.Of((ConditionalTimeGroup x) => new TimeGroup(x)),
            UnionCase.Of((AggregateTimeGroup x) => new TimeGroup(x))
        );
        registry.Union(
            UnionCase.Of((TimeGroup x) => new TimeNullableGroup(x)),
            UnionCase.Of((ExactTimeNullableGroup x) => new TimeNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyUuid x) => new UuidGroup(x)),
            UnionCase.Of((ParamUuid x) => new UuidGroup(x)),
            UnionCase.Of((LiteralUuid x) => new UuidGroup(x)),
            UnionCase.Of((ConditionalUuidGroup x) => new UuidGroup(x)),
            UnionCase.Of((AggregateUuidGroup x) => new UuidGroup(x))
        );
        registry.Union(
            UnionCase.Of((UuidGroup x) => new UuidNullableGroup(x)),
            UnionCase.Of((ExactUuidNullableGroup x) => new UuidNullableGroup(x))
        );
    }
}
