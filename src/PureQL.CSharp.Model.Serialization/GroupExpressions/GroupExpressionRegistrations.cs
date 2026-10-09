using PureQL.CSharp.Model.GroupExpressions;
using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.RowExpressions;

namespace PureQL.CSharp.Model.Serialization.GroupExpressions;

internal static class GroupExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            r => new AddDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddIntegerGroup(r.Many<IntegerGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((AnyGroup x) => new AggregateBooleanGroup(x)),
            UnionCase.Of((AllGroup x) => new AggregateBooleanGroup(x))
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
            UnionCase.Of((CountGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((SumDecimalGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((AverageDecimalGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((MinDecimalGroup x) => new AggregateDecimalGroup(x)),
            UnionCase.Of((MaxDecimalGroup x) => new AggregateDecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of((CountGroup x) => new AggregateDecimalNullableGroup(x)),
            UnionCase.Of((SumDecimalGroup x) => new AggregateDecimalNullableGroup(x)),
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
            UnionCase.Of((CountGroup x) => new AggregateIntegerNullableGroup(x)),
            UnionCase.Of((SumIntegerGroup x) => new AggregateIntegerNullableGroup(x)),
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
        registry.Operator(
            "all",
            r => new AllGroup(r.One<BooleanRow>("predicate"), r.Over()),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "and",
            r => new AndGroup(r.Many<BooleanGroup>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "any",
            r => new AnyGroup(r.One<BooleanRow>("predicate"), r.Over()),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("predicate", x.Predicate);
            }
        );
        registry.Union(
            UnionCase.Of((AddDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((SubtractDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((MultiplyDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((DivideDecimalGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((IntegerDivideIntegerGroup x) => new ArithmeticDecimalGroup(x)),
            UnionCase.Of((ModuloIntegerGroup x) => new ArithmeticDecimalGroup(x))
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
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableGroup x) =>
                    new ArithmeticDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableGroup x) => new ArithmeticDecimalNullableGroup(x)
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
            r =>
            {
                r.FixedOver("group");
                return new AverageDateGroup(r.One<DateRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            r => new AverageDateNullableGroup(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("group");
                return new AverageDatetimeGroup(r.One<DatetimeRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            r => new AverageDatetimeNullableGroup(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("group");
                return new AverageDecimalGroup(r.One<DecimalRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            r => new AverageDecimalNullableGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("group");
                return new AverageTimeGroup(r.One<TimeRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            r => new AverageTimeNullableGroup(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
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
            UnionCase.Of((KeyAsBooleanNullable x) => new BooleanNullableGroup(x)),
            UnionCase.Of((ParamAsBooleanNullable x) => new BooleanNullableGroup(x)),
            UnionCase.Of((LiteralAsBooleanNullable x) => new BooleanNullableGroup(x)),
            UnionCase.Of((LogicalGroup x) => new BooleanNullableGroup(x)),
            UnionCase.Of((ComparisonGroup x) => new BooleanNullableGroup(x)),
            UnionCase.Of(
                (ConditionalBooleanNullableGroup x) => new BooleanNullableGroup(x)
            ),
            UnionCase.Of((AggregateBooleanGroup x) => new BooleanNullableGroup(x))
        );
        registry.Operator(
            "ceiling",
            r => new CeilingIntegerGroup(r.One<DecimalGroup>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "ceiling",
            r => new CeilingIntegerNullableGroup(r.One<DecimalNullableGroup>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceBooleanGroup(r.Many<BooleanNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceBooleanNullableGroup(r.Many<BooleanNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDateGroup(r.Many<DateNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDateNullableGroup(r.Many<DateNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDatetimeGroup(r.Many<DatetimeNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDatetimeNullableGroup(
                r.Many<DatetimeNullableGroup>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDecimalGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceIntegerGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceStringGroup(r.Many<StringNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceStringNullableGroup(r.Many<StringNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceTimeGroup(r.Many<TimeNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceTimeNullableGroup(r.Many<TimeNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceUuidGroup(r.Many<UuidNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
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
            r => new ConcatStringGroup(r.Many<StringGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
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
            r => new CountGroup(r.Optional<BooleanRow>("predicate"), r.Over()),
            (w, x) =>
            {
                w.Over(x.Over);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "dateAddDays",
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
            UnionCase.Of((KeyAsDateNullable x) => new DateNullableGroup(x)),
            UnionCase.Of((ParamAsDateNullable x) => new DateNullableGroup(x)),
            UnionCase.Of((LiteralAsDateNullable x) => new DateNullableGroup(x)),
            UnionCase.Of((DateAddDaysDateNullableGroup x) => new DateNullableGroup(x)),
            UnionCase.Of((ConditionalDateNullableGroup x) => new DateNullableGroup(x)),
            UnionCase.Of((AggregateDateNullableGroup x) => new DateNullableGroup(x))
        );
        registry.Operator(
            "datetimeAddSeconds",
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
            UnionCase.Of((KeyAsDatetimeNullable x) => new DatetimeNullableGroup(x)),
            UnionCase.Of((ParamAsDatetimeNullable x) => new DatetimeNullableGroup(x)),
            UnionCase.Of((LiteralAsDatetimeNullable x) => new DatetimeNullableGroup(x)),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableGroup x) =>
                    new DatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalDatetimeNullableGroup x) => new DatetimeNullableGroup(x)
            ),
            UnionCase.Of(
                (AggregateDatetimeNullableGroup x) => new DatetimeNullableGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of((KeyAsDecimal x) => new DecimalGroup(x)),
            UnionCase.Of((ParamAsDecimal x) => new DecimalGroup(x)),
            UnionCase.Of((LiteralAsDecimal x) => new DecimalGroup(x)),
            UnionCase.Of((ArithmeticDecimalGroup x) => new DecimalGroup(x)),
            UnionCase.Of((RoundingDecimalGroup x) => new DecimalGroup(x)),
            UnionCase.Of((DifferenceDecimalGroup x) => new DecimalGroup(x)),
            UnionCase.Of((ConditionalDecimalGroup x) => new DecimalGroup(x)),
            UnionCase.Of((AggregateDecimalGroup x) => new DecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyAsDecimalNullable x) => new DecimalNullableGroup(x)),
            UnionCase.Of((ParamAsDecimalNullable x) => new DecimalNullableGroup(x)),
            UnionCase.Of((LiteralAsDecimalNullable x) => new DecimalNullableGroup(x)),
            UnionCase.Of(
                (ArithmeticDecimalNullableGroup x) => new DecimalNullableGroup(x)
            ),
            UnionCase.Of((RoundingDecimalNullableGroup x) => new DecimalNullableGroup(x)),
            UnionCase.Of(
                (DifferenceDecimalNullableGroup x) => new DecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalNullableGroup x) => new DecimalNullableGroup(x)
            ),
            UnionCase.Of((AggregateDecimalNullableGroup x) => new DecimalNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((DateDiffDaysIntegerGroup x) => new DifferenceDecimalGroup(x)),
            UnionCase.Of(
                (TimeDiffSecondsDecimalGroup x) => new DifferenceDecimalGroup(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalGroup x) => new DifferenceDecimalGroup(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (DateDiffDaysIntegerNullableGroup x) =>
                    new DifferenceDecimalNullableGroup(x)
            ),
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
            r => new DivideDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            r => new DivideDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
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
        registry.Operator(
            "floor",
            r => new FloorIntegerGroup(r.One<DecimalGroup>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "floor",
            r => new FloorIntegerNullableGroup(r.One<DecimalNullableGroup>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "greaterThan",
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
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
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
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
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
            "greaterThan",
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
            "if",
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
            r => new InBooleanGroup(
                r.One<BooleanNullableGroup>("value"),
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
            r => new InDateGroup(
                r.One<DateNullableGroup>("value"),
                r.One<ListDate>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InDatetimeGroup(
                r.One<DatetimeNullableGroup>("value"),
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
            r => new InDecimalGroup(
                r.One<DecimalNullableGroup>("value"),
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
            r => new InStringGroup(
                r.One<StringNullableGroup>("value"),
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
            r => new InTimeGroup(
                r.One<TimeNullableGroup>("value"),
                r.One<ListTime>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "in",
            r => new InUuidGroup(
                r.One<UuidNullableGroup>("value"),
                r.One<ListUuid>("list")
            ),
            (w, x) =>
            {
                w.One("value", x.Value);
                w.One("list", x.List);
            }
        );
        registry.Operator(
            "integerDivide",
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
            UnionCase.Of((KeyAsIntegerNullable x) => new IntegerNullableGroup(x)),
            UnionCase.Of((ParamAsIntegerNullable x) => new IntegerNullableGroup(x)),
            UnionCase.Of((LiteralAsIntegerNullable x) => new IntegerNullableGroup(x)),
            UnionCase.Of(
                (ArithmeticIntegerNullableGroup x) => new IntegerNullableGroup(x)
            ),
            UnionCase.Of((RoundingIntegerNullableGroup x) => new IntegerNullableGroup(x)),
            UnionCase.Of(
                (DateDiffDaysIntegerNullableGroup x) => new IntegerNullableGroup(x)
            ),
            UnionCase.Of(
                (ConditionalIntegerNullableGroup x) => new IntegerNullableGroup(x)
            ),
            UnionCase.Of((AggregateIntegerNullableGroup x) => new IntegerNullableGroup(x))
        );
        registry.Operator(
            "lessThan",
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
            UnionCase.Of(TypeFamily.Date, (LessThanDateGroup x) => new LessThanGroup(x)),
            UnionCase.Of(TypeFamily.Time, (LessThanTimeGroup x) => new LessThanGroup(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanDatetimeGroup x) => new LessThanGroup(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
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
            )
        );
        registry.Operator(
            "lessThanOrEqual",
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
            "lessThan",
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
        registry.Union(
            UnionCase.Of((AndGroup x) => new LogicalGroup(x)),
            UnionCase.Of((OrGroup x) => new LogicalGroup(x)),
            UnionCase.Of((NotGroup x) => new LogicalGroup(x))
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("group");
                return new MaxDateGroup(r.One<DateRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            r => new MaxDateNullableGroup(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("group");
                return new MaxDatetimeGroup(r.One<DatetimeRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            r => new MaxDatetimeNullableGroup(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("group");
                return new MaxDecimalGroup(r.One<DecimalRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            r => new MaxDecimalNullableGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("group");
                return new MaxIntegerGroup(r.One<IntegerRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            r => new MaxIntegerNullableGroup(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("group");
                return new MaxStringGroup(r.One<StringRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            r => new MaxStringNullableGroup(
                r.One<StringNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("group");
                return new MaxTimeGroup(r.One<TimeRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            r => new MaxTimeNullableGroup(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("group");
                return new MinDateGroup(r.One<DateRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            r => new MinDateNullableGroup(
                r.One<DateNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("group");
                return new MinDatetimeGroup(r.One<DatetimeRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            r => new MinDatetimeNullableGroup(
                r.One<DatetimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("group");
                return new MinDecimalGroup(r.One<DecimalRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            r => new MinDecimalNullableGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("group");
                return new MinIntegerGroup(r.One<IntegerRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            r => new MinIntegerNullableGroup(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("group");
                return new MinStringGroup(r.One<StringRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            r => new MinStringNullableGroup(
                r.One<StringNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("group");
                return new MinTimeGroup(r.One<TimeRow>("selector"));
            },
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            r => new MinTimeNullableGroup(
                r.One<TimeNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "modulo",
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
            r => new MultiplyDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyIntegerGroup(r.Many<IntegerGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
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
            r => new NotGroup(r.One<BooleanGroup>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            r => new OrGroup(r.Many<BooleanGroup>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            r => new RoundDecimalDigitsGroup(
                r.One<DecimalGroup>("value"),
                r.One<IntegerGroup>("digits")
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
            r => new RoundDecimalDigitsNullableGroup(
                r.One<DecimalNullableGroup>("value"),
                r.One<IntegerGroup>("digits")
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
            r => new RoundIntegerGroup(r.One<DecimalGroup>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "round",
            r => new RoundIntegerNullableGroup(r.One<DecimalNullableGroup>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Union(
            UnionCase.Of((FloorIntegerGroup x) => new RoundingDecimalGroup(x)),
            UnionCase.Of((CeilingIntegerGroup x) => new RoundingDecimalGroup(x)),
            UnionCase.Of((RoundDecimalDigitsGroup x) => new RoundingDecimalGroup(x)),
            UnionCase.Of((RoundIntegerGroup x) => new RoundingDecimalGroup(x))
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableGroup x) => new RoundingDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableGroup x) => new RoundingDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (RoundDecimalDigitsNullableGroup x) => new RoundingDecimalNullableGroup(x)
            ),
            UnionCase.Of(
                (RoundIntegerNullableGroup x) => new RoundingDecimalNullableGroup(x)
            )
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
            UnionCase.Of((KeyAsStringNullable x) => new StringNullableGroup(x)),
            UnionCase.Of((ParamAsStringNullable x) => new StringNullableGroup(x)),
            UnionCase.Of((LiteralAsStringNullable x) => new StringNullableGroup(x)),
            UnionCase.Of((ConcatStringNullableGroup x) => new StringNullableGroup(x)),
            UnionCase.Of(
                (ConditionalStringNullableGroup x) => new StringNullableGroup(x)
            ),
            UnionCase.Of((AggregateStringNullableGroup x) => new StringNullableGroup(x))
        );
        registry.Operator(
            "subtract",
            r => new SubtractDecimalGroup(r.Many<DecimalGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractDecimalNullableGroup(r.Many<DecimalNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractIntegerGroup(r.Many<IntegerGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractIntegerNullableGroup(r.Many<IntegerNullableGroup>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "sum",
            r => new SumDecimalGroup(
                r.One<DecimalNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "sum",
            r => new SumIntegerGroup(
                r.One<IntegerNullableRow>("selector"),
                r.Optional<BooleanRow>("predicate"),
                r.Over()
            ),
            (w, x) =>
            {
                w.Over(x.Over);
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "timeAddSeconds",
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
            UnionCase.Of((KeyAsTimeNullable x) => new TimeNullableGroup(x)),
            UnionCase.Of((ParamAsTimeNullable x) => new TimeNullableGroup(x)),
            UnionCase.Of((LiteralAsTimeNullable x) => new TimeNullableGroup(x)),
            UnionCase.Of((TimeAddSecondsTimeNullableGroup x) => new TimeNullableGroup(x)),
            UnionCase.Of((ConditionalTimeNullableGroup x) => new TimeNullableGroup(x)),
            UnionCase.Of((AggregateTimeNullableGroup x) => new TimeNullableGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyUuid x) => new UuidGroup(x)),
            UnionCase.Of((ParamUuid x) => new UuidGroup(x)),
            UnionCase.Of((LiteralUuid x) => new UuidGroup(x)),
            UnionCase.Of((ConditionalUuidGroup x) => new UuidGroup(x))
        );
        registry.Union(
            UnionCase.Of((KeyAsUuidNullable x) => new UuidNullableGroup(x)),
            UnionCase.Of((ParamAsUuidNullable x) => new UuidNullableGroup(x)),
            UnionCase.Of((LiteralAsUuidNullable x) => new UuidNullableGroup(x)),
            UnionCase.Of((ConditionalUuidNullableGroup x) => new UuidNullableGroup(x))
        );
        registry.Probe(
            null,
            UnionCase.Of(
                TypeFamily.Decimal,
                (DecimalNullableGroup x) => new ValueGroup(x)
            ),
            UnionCase.Of(TypeFamily.String, (StringNullableGroup x) => new ValueGroup(x)),
            UnionCase.Of(
                TypeFamily.Boolean,
                (BooleanNullableGroup x) => new ValueGroup(x)
            ),
            UnionCase.Of(TypeFamily.Date, (DateNullableGroup x) => new ValueGroup(x)),
            UnionCase.Of(TypeFamily.Time, (TimeNullableGroup x) => new ValueGroup(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (DatetimeNullableGroup x) => new ValueGroup(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (UuidNullableGroup x) => new ValueGroup(x))
        );
    }
}
