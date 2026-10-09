using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.RowExpressions;

namespace PureQL.CSharp.Model.Serialization.ProjectionExpressions;

internal static class ProjectionExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            r => new AddDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddDecimalProjection(r.Many<DecimalProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            r => new AddIntegerProjection(r.Many<IntegerProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((AnyProjection x) => new AggregateBooleanProjection(x)),
            UnionCase.Of((AllProjection x) => new AggregateBooleanProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDateNullableProjection x) => new AggregateDateProjection(x)
            ),
            UnionCase.Of((MinDateNullableProjection x) => new AggregateDateProjection(x)),
            UnionCase.Of((MaxDateNullableProjection x) => new AggregateDateProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDatetimeNullableProjection x) =>
                    new AggregateDatetimeProjection(x)
            ),
            UnionCase.Of(
                (MinDatetimeNullableProjection x) => new AggregateDatetimeProjection(x)
            ),
            UnionCase.Of(
                (MaxDatetimeNullableProjection x) => new AggregateDatetimeProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (CountProjection x) => new AggregateDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (SumDecimalProjection x) => new AggregateDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (AverageDecimalNullableProjection x) =>
                    new AggregateDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (MinDecimalNullableProjection x) =>
                    new AggregateDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxDecimalNullableProjection x) =>
                    new AggregateDecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((CountProjection x) => new AggregateDecimalProjection(x)),
            UnionCase.Of((SumDecimalProjection x) => new AggregateDecimalProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (CountProjection x) => new AggregateIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (SumIntegerProjection x) => new AggregateIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (MinIntegerNullableProjection x) =>
                    new AggregateIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxIntegerNullableProjection x) =>
                    new AggregateIntegerNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((CountProjection x) => new AggregateIntegerProjection(x)),
            UnionCase.Of((SumIntegerProjection x) => new AggregateIntegerProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinStringNullableProjection x) => new AggregateStringProjection(x)
            ),
            UnionCase.Of(
                (MaxStringNullableProjection x) => new AggregateStringProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (AverageTimeNullableProjection x) => new AggregateTimeProjection(x)
            ),
            UnionCase.Of((MinTimeNullableProjection x) => new AggregateTimeProjection(x)),
            UnionCase.Of((MaxTimeNullableProjection x) => new AggregateTimeProjection(x))
        );
        registry.Operator(
            "all",
            r =>
            {
                r.FixedOver("all");
                return new AllProjection(r.One<BooleanRow>("predicate"));
            },
            (w, x) => w.One("predicate", x.Predicate)
        );
        registry.Operator(
            "and",
            r => new AndProjection(r.Many<BooleanProjection>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "any",
            r =>
            {
                r.FixedOver("all");
                return new AnyProjection(r.One<BooleanRow>("predicate"));
            },
            (w, x) => w.One("predicate", x.Predicate)
        );
        registry.Union(
            UnionCase.Of(
                (AddDecimalNullableProjection x) =>
                    new ArithmeticDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (SubtractDecimalNullableProjection x) =>
                    new ArithmeticDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (MultiplyDecimalNullableProjection x) =>
                    new ArithmeticDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (DivideDecimalNullableProjection x) =>
                    new ArithmeticDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableProjection x) =>
                    new ArithmeticDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableProjection x) =>
                    new ArithmeticDecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddDecimalProjection x) => new ArithmeticDecimalProjection(x)),
            UnionCase.Of(
                (SubtractDecimalProjection x) => new ArithmeticDecimalProjection(x)
            ),
            UnionCase.Of(
                (MultiplyDecimalProjection x) => new ArithmeticDecimalProjection(x)
            ),
            UnionCase.Of(
                (DivideDecimalProjection x) => new ArithmeticDecimalProjection(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerProjection x) => new ArithmeticDecimalProjection(x)
            ),
            UnionCase.Of(
                (ModuloIntegerProjection x) => new ArithmeticDecimalProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (AddIntegerNullableProjection x) =>
                    new ArithmeticIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (SubtractIntegerNullableProjection x) =>
                    new ArithmeticIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (MultiplyIntegerNullableProjection x) =>
                    new ArithmeticIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerNullableProjection x) =>
                    new ArithmeticIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (ModuloIntegerNullableProjection x) =>
                    new ArithmeticIntegerNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((AddIntegerProjection x) => new ArithmeticIntegerProjection(x)),
            UnionCase.Of(
                (SubtractIntegerProjection x) => new ArithmeticIntegerProjection(x)
            ),
            UnionCase.Of(
                (MultiplyIntegerProjection x) => new ArithmeticIntegerProjection(x)
            ),
            UnionCase.Of(
                (IntegerDivideIntegerProjection x) => new ArithmeticIntegerProjection(x)
            ),
            UnionCase.Of(
                (ModuloIntegerProjection x) => new ArithmeticIntegerProjection(x)
            )
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("all");
                return new AverageDateNullableProjection(
                    r.One<DateNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("all");
                return new AverageDatetimeNullableProjection(
                    r.One<DatetimeNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("all");
                return new AverageDecimalNullableProjection(
                    r.One<DecimalNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "average",
            r =>
            {
                r.FixedOver("all");
                return new AverageTimeNullableProjection(
                    r.One<TimeNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsBooleanNullable x) => new BooleanNullableProjection(x)),
            UnionCase.Of((ParamAsBooleanNullable x) => new BooleanNullableProjection(x)),
            UnionCase.Of(
                (LiteralAsBooleanNullable x) => new BooleanNullableProjection(x)
            ),
            UnionCase.Of((LogicalProjection x) => new BooleanNullableProjection(x)),
            UnionCase.Of((ComparisonProjection x) => new BooleanNullableProjection(x)),
            UnionCase.Of(
                (ConditionalBooleanNullableProjection x) =>
                    new BooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateBooleanProjection x) => new BooleanNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldBoolean x) => new BooleanProjection(x)),
            UnionCase.Of((ParamBoolean x) => new BooleanProjection(x)),
            UnionCase.Of((LiteralBoolean x) => new BooleanProjection(x)),
            UnionCase.Of((LogicalProjection x) => new BooleanProjection(x)),
            UnionCase.Of((ComparisonProjection x) => new BooleanProjection(x)),
            UnionCase.Of((ConditionalBooleanProjection x) => new BooleanProjection(x)),
            UnionCase.Of((AggregateBooleanProjection x) => new BooleanProjection(x))
        );
        registry.Operator(
            "ceiling",
            r => new CeilingIntegerNullableProjection(
                r.One<DecimalNullableProjection>("value")
            ),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "ceiling",
            r => new CeilingIntegerProjection(r.One<DecimalProjection>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceBooleanNullableProjection(
                r.Many<BooleanNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceBooleanProjection(
                r.Many<BooleanNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDateNullableProjection(
                r.Many<DateNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDateProjection(r.Many<DateNullableProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDatetimeNullableProjection(
                r.Many<DatetimeNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDatetimeProjection(
                r.Many<DatetimeNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceDecimalProjection(
                r.Many<DecimalNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceIntegerProjection(
                r.Many<IntegerNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceStringNullableProjection(
                r.Many<StringNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceStringProjection(r.Many<StringNullableProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceTimeNullableProjection(
                r.Many<TimeNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceTimeProjection(r.Many<TimeNullableProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceUuidNullableProjection(
                r.Many<UuidNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            r => new CoalesceUuidProjection(r.Many<UuidNullableProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of((EqualProjection x) => new ComparisonProjection(x)),
            UnionCase.Of((NotEqualProjection x) => new ComparisonProjection(x)),
            UnionCase.Of((InProjection x) => new ComparisonProjection(x)),
            UnionCase.Of((GreaterThanProjection x) => new ComparisonProjection(x)),
            UnionCase.Of((LessThanProjection x) => new ComparisonProjection(x)),
            UnionCase.Of((GreaterThanOrEqualProjection x) => new ComparisonProjection(x)),
            UnionCase.Of((LessThanOrEqualProjection x) => new ComparisonProjection(x))
        );
        registry.Operator(
            "concat",
            r => new ConcatStringNullableProjection(
                r.Many<StringNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
            r => new ConcatStringProjection(r.Many<StringProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of(
                (IfBooleanNullableProjection x) =>
                    new ConditionalBooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceBooleanNullableProjection x) =>
                    new ConditionalBooleanNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfBooleanProjection x) => new ConditionalBooleanProjection(x)),
            UnionCase.Of(
                (CoalesceBooleanProjection x) => new ConditionalBooleanProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (IfDateNullableProjection x) => new ConditionalDateNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceDateNullableProjection x) =>
                    new ConditionalDateNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDateProjection x) => new ConditionalDateProjection(x)),
            UnionCase.Of((CoalesceDateProjection x) => new ConditionalDateProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfDatetimeNullableProjection x) =>
                    new ConditionalDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceDatetimeNullableProjection x) =>
                    new ConditionalDatetimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (IfDatetimeProjection x) => new ConditionalDatetimeProjection(x)
            ),
            UnionCase.Of(
                (CoalesceDatetimeProjection x) => new ConditionalDatetimeProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (IfDecimalNullableProjection x) =>
                    new ConditionalDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceDecimalNullableProjection x) =>
                    new ConditionalDecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfDecimalProjection x) => new ConditionalDecimalProjection(x)),
            UnionCase.Of(
                (CoalesceDecimalProjection x) => new ConditionalDecimalProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (IfIntegerNullableProjection x) =>
                    new ConditionalIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceIntegerNullableProjection x) =>
                    new ConditionalIntegerNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfIntegerProjection x) => new ConditionalIntegerProjection(x)),
            UnionCase.Of(
                (CoalesceIntegerProjection x) => new ConditionalIntegerProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (IfStringNullableProjection x) =>
                    new ConditionalStringNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceStringNullableProjection x) =>
                    new ConditionalStringNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfStringProjection x) => new ConditionalStringProjection(x)),
            UnionCase.Of(
                (CoalesceStringProjection x) => new ConditionalStringProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (IfTimeNullableProjection x) => new ConditionalTimeNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceTimeNullableProjection x) =>
                    new ConditionalTimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfTimeProjection x) => new ConditionalTimeProjection(x)),
            UnionCase.Of((CoalesceTimeProjection x) => new ConditionalTimeProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (IfUuidNullableProjection x) => new ConditionalUuidNullableProjection(x)
            ),
            UnionCase.Of(
                (CoalesceUuidNullableProjection x) =>
                    new ConditionalUuidNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((IfUuidProjection x) => new ConditionalUuidProjection(x)),
            UnionCase.Of((CoalesceUuidProjection x) => new ConditionalUuidProjection(x))
        );
        registry.Operator(
            "count",
            r =>
            {
                r.FixedOver("all");
                return new CountProjection(r.Optional<BooleanRow>("predicate"));
            },
            (w, x) => w.Optional("predicate", x.Predicate)
        );
        registry.Operator(
            "dateAddDays",
            r => new DateAddDaysDateNullableProjection(
                r.One<DateNullableProjection>("left"),
                r.One<IntegerNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateAddDays",
            r => new DateAddDaysDateProjection(
                r.One<DateProjection>("left"),
                r.One<IntegerProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateDiffDays",
            r => new DateDiffDaysIntegerNullableProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "dateDiffDays",
            r => new DateDiffDaysIntegerProjection(
                r.One<DateProjection>("left"),
                r.One<DateProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsDateNullable x) => new DateNullableProjection(x)),
            UnionCase.Of((ParamAsDateNullable x) => new DateNullableProjection(x)),
            UnionCase.Of((LiteralAsDateNullable x) => new DateNullableProjection(x)),
            UnionCase.Of(
                (DateAddDaysDateNullableProjection x) => new DateNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDateNullableProjection x) => new DateNullableProjection(x)
            ),
            UnionCase.Of((AggregateDateProjection x) => new DateNullableProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldDate x) => new DateProjection(x)),
            UnionCase.Of((ParamDate x) => new DateProjection(x)),
            UnionCase.Of((LiteralDate x) => new DateProjection(x)),
            UnionCase.Of((DateAddDaysDateProjection x) => new DateProjection(x)),
            UnionCase.Of((ConditionalDateProjection x) => new DateProjection(x))
        );
        registry.Operator(
            "datetimeAddSeconds",
            r => new DatetimeAddSecondsDatetimeNullableProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeAddSeconds",
            r => new DatetimeAddSecondsDatetimeProjection(
                r.One<DatetimeProjection>("left"),
                r.One<DecimalProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeDiffSeconds",
            r => new DatetimeDiffSecondsDecimalNullableProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "datetimeDiffSeconds",
            r => new DatetimeDiffSecondsDecimalProjection(
                r.One<DatetimeProjection>("left"),
                r.One<DatetimeProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of(
                (FieldAsDatetimeNullable x) => new DatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (ParamAsDatetimeNullable x) => new DatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (LiteralAsDatetimeNullable x) => new DatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableProjection x) =>
                    new DatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDatetimeNullableProjection x) =>
                    new DatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateDatetimeProjection x) => new DatetimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDatetime x) => new DatetimeProjection(x)),
            UnionCase.Of((ParamDatetime x) => new DatetimeProjection(x)),
            UnionCase.Of((LiteralDatetime x) => new DatetimeProjection(x)),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeProjection x) => new DatetimeProjection(x)
            ),
            UnionCase.Of((ConditionalDatetimeProjection x) => new DatetimeProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsDecimalNullable x) => new DecimalNullableProjection(x)),
            UnionCase.Of((ParamAsDecimalNullable x) => new DecimalNullableProjection(x)),
            UnionCase.Of(
                (LiteralAsDecimalNullable x) => new DecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (ArithmeticDecimalNullableProjection x) =>
                    new DecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundingDecimalNullableProjection x) => new DecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (DifferenceDecimalNullableProjection x) =>
                    new DecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalNullableProjection x) =>
                    new DecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateDecimalNullableProjection x) => new DecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldAsDecimal x) => new DecimalProjection(x)),
            UnionCase.Of((ParamAsDecimal x) => new DecimalProjection(x)),
            UnionCase.Of((LiteralAsDecimal x) => new DecimalProjection(x)),
            UnionCase.Of((ArithmeticDecimalProjection x) => new DecimalProjection(x)),
            UnionCase.Of((RoundingDecimalProjection x) => new DecimalProjection(x)),
            UnionCase.Of((DifferenceDecimalProjection x) => new DecimalProjection(x)),
            UnionCase.Of((ConditionalDecimalProjection x) => new DecimalProjection(x)),
            UnionCase.Of((AggregateDecimalProjection x) => new DecimalProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (DateDiffDaysIntegerNullableProjection x) =>
                    new DifferenceDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (TimeDiffSecondsDecimalNullableProjection x) =>
                    new DifferenceDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalNullableProjection x) =>
                    new DifferenceDecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (DateDiffDaysIntegerProjection x) => new DifferenceDecimalProjection(x)
            ),
            UnionCase.Of(
                (TimeDiffSecondsDecimalProjection x) => new DifferenceDecimalProjection(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalProjection x) =>
                    new DifferenceDecimalProjection(x)
            )
        );
        registry.Operator(
            "divide",
            r => new DivideDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            r => new DivideDecimalProjection(r.Many<DecimalProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
            r => new EqualBooleanProjection(
                r.One<BooleanNullableProjection>("left"),
                r.One<BooleanNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualDateProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualDatetimeProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualDecimalProjection(
                r.One<DecimalNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
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
                (EqualDecimalProjection x) => new EqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (EqualStringProjection x) => new EqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (EqualBooleanProjection x) => new EqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (EqualDateProjection x) => new EqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (EqualTimeProjection x) => new EqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (EqualDatetimeProjection x) => new EqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (EqualUuidProjection x) => new EqualProjection(x)
            )
        );
        registry.Operator(
            "equal",
            r => new EqualStringProjection(
                r.One<StringNullableProjection>("left"),
                r.One<StringNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualTimeProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "equal",
            r => new EqualUuidProjection(
                r.One<UuidNullableProjection>("left"),
                r.One<UuidNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "floor",
            r => new FloorIntegerNullableProjection(
                r.One<DecimalNullableProjection>("value")
            ),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "floor",
            r => new FloorIntegerProjection(r.One<DecimalProjection>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanDateProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanDatetimeProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanDecimalProjection(
                r.One<DecimalNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualDateProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualDatetimeProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualDecimalProjection(
                r.One<DecimalNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
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
                (GreaterThanOrEqualDecimalProjection x) =>
                    new GreaterThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanOrEqualStringProjection x) =>
                    new GreaterThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanOrEqualDateProjection x) =>
                    new GreaterThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanOrEqualTimeProjection x) =>
                    new GreaterThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanOrEqualDatetimeProjection x) =>
                    new GreaterThanOrEqualProjection(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualStringProjection(
                r.One<StringNullableProjection>("left"),
                r.One<StringNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThanOrEqual",
            r => new GreaterThanOrEqualTimeProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
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
                (GreaterThanDecimalProjection x) => new GreaterThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (GreaterThanStringProjection x) => new GreaterThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (GreaterThanDateProjection x) => new GreaterThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (GreaterThanTimeProjection x) => new GreaterThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (GreaterThanDatetimeProjection x) => new GreaterThanProjection(x)
            )
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanStringProjection(
                r.One<StringNullableProjection>("left"),
                r.One<StringNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "greaterThan",
            r => new GreaterThanTimeProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "if",
            r => new IfBooleanNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<BooleanNullableProjection>("then"),
                r.One<BooleanNullableProjection>("else")
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
            r => new IfBooleanProjection(
                r.One<BooleanProjection>("condition"),
                r.One<BooleanProjection>("then"),
                r.One<BooleanProjection>("else")
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
            r => new IfDateNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<DateNullableProjection>("then"),
                r.One<DateNullableProjection>("else")
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
            r => new IfDateProjection(
                r.One<BooleanProjection>("condition"),
                r.One<DateProjection>("then"),
                r.One<DateProjection>("else")
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
            r => new IfDatetimeNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<DatetimeNullableProjection>("then"),
                r.One<DatetimeNullableProjection>("else")
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
            r => new IfDatetimeProjection(
                r.One<BooleanProjection>("condition"),
                r.One<DatetimeProjection>("then"),
                r.One<DatetimeProjection>("else")
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
            r => new IfDecimalNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<DecimalNullableProjection>("then"),
                r.One<DecimalNullableProjection>("else")
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
            r => new IfDecimalProjection(
                r.One<BooleanProjection>("condition"),
                r.One<DecimalProjection>("then"),
                r.One<DecimalProjection>("else")
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
            r => new IfIntegerNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<IntegerNullableProjection>("then"),
                r.One<IntegerNullableProjection>("else")
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
            r => new IfIntegerProjection(
                r.One<BooleanProjection>("condition"),
                r.One<IntegerProjection>("then"),
                r.One<IntegerProjection>("else")
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
            r => new IfStringNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<StringNullableProjection>("then"),
                r.One<StringNullableProjection>("else")
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
            r => new IfStringProjection(
                r.One<BooleanProjection>("condition"),
                r.One<StringProjection>("then"),
                r.One<StringProjection>("else")
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
            r => new IfTimeNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<TimeNullableProjection>("then"),
                r.One<TimeNullableProjection>("else")
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
            r => new IfTimeProjection(
                r.One<BooleanProjection>("condition"),
                r.One<TimeProjection>("then"),
                r.One<TimeProjection>("else")
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
            r => new IfUuidNullableProjection(
                r.One<BooleanProjection>("condition"),
                r.One<UuidNullableProjection>("then"),
                r.One<UuidNullableProjection>("else")
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
            r => new IfUuidProjection(
                r.One<BooleanProjection>("condition"),
                r.One<UuidProjection>("then"),
                r.One<UuidProjection>("else")
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
            r => new InBooleanProjection(
                r.One<BooleanNullableProjection>("value"),
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
            r => new InDateProjection(
                r.One<DateNullableProjection>("value"),
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
            r => new InDatetimeProjection(
                r.One<DatetimeNullableProjection>("value"),
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
            r => new InDecimalProjection(
                r.One<DecimalNullableProjection>("value"),
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
            UnionCase.Of(
                TypeFamily.Decimal,
                (InDecimalProjection x) => new InProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (InStringProjection x) => new InProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (InBooleanProjection x) => new InProjection(x)
            ),
            UnionCase.Of(TypeFamily.Date, (InDateProjection x) => new InProjection(x)),
            UnionCase.Of(TypeFamily.Time, (InTimeProjection x) => new InProjection(x)),
            UnionCase.Of(
                TypeFamily.Datetime,
                (InDatetimeProjection x) => new InProjection(x)
            ),
            UnionCase.Of(TypeFamily.Uuid, (InUuidProjection x) => new InProjection(x))
        );
        registry.Operator(
            "in",
            r => new InStringProjection(
                r.One<StringNullableProjection>("value"),
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
            r => new InTimeProjection(
                r.One<TimeNullableProjection>("value"),
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
            r => new InUuidProjection(
                r.One<UuidNullableProjection>("value"),
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
            r => new IntegerDivideIntegerNullableProjection(
                r.One<IntegerNullableProjection>("left"),
                r.One<IntegerNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "integerDivide",
            r => new IntegerDivideIntegerProjection(
                r.One<IntegerProjection>("left"),
                r.One<IntegerProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsIntegerNullable x) => new IntegerNullableProjection(x)),
            UnionCase.Of((ParamAsIntegerNullable x) => new IntegerNullableProjection(x)),
            UnionCase.Of(
                (LiteralAsIntegerNullable x) => new IntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (ArithmeticIntegerNullableProjection x) =>
                    new IntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundingIntegerNullableProjection x) => new IntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (DateDiffDaysIntegerNullableProjection x) =>
                    new IntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalIntegerNullableProjection x) =>
                    new IntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateIntegerNullableProjection x) => new IntegerNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldInteger x) => new IntegerProjection(x)),
            UnionCase.Of((ParamInteger x) => new IntegerProjection(x)),
            UnionCase.Of((LiteralInteger x) => new IntegerProjection(x)),
            UnionCase.Of((ArithmeticIntegerProjection x) => new IntegerProjection(x)),
            UnionCase.Of((RoundingIntegerProjection x) => new IntegerProjection(x)),
            UnionCase.Of((DateDiffDaysIntegerProjection x) => new IntegerProjection(x)),
            UnionCase.Of((ConditionalIntegerProjection x) => new IntegerProjection(x)),
            UnionCase.Of((AggregateIntegerProjection x) => new IntegerProjection(x))
        );
        registry.Operator(
            "lessThan",
            r => new LessThanDateProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThan",
            r => new LessThanDatetimeProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThan",
            r => new LessThanDecimalProjection(
                r.One<DecimalNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualDateProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualDatetimeProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualDecimalProjection(
                r.One<DecimalNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
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
                (LessThanOrEqualDecimalProjection x) => new LessThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanOrEqualStringProjection x) => new LessThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (LessThanOrEqualDateProjection x) => new LessThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (LessThanOrEqualTimeProjection x) => new LessThanOrEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanOrEqualDatetimeProjection x) => new LessThanOrEqualProjection(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualStringProjection(
                r.One<StringNullableProjection>("left"),
                r.One<StringNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThanOrEqual",
            r => new LessThanOrEqualTimeProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
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
                (LessThanDecimalProjection x) => new LessThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (LessThanStringProjection x) => new LessThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (LessThanDateProjection x) => new LessThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (LessThanTimeProjection x) => new LessThanProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (LessThanDatetimeProjection x) => new LessThanProjection(x)
            )
        );
        registry.Operator(
            "lessThan",
            r => new LessThanStringProjection(
                r.One<StringNullableProjection>("left"),
                r.One<StringNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "lessThan",
            r => new LessThanTimeProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((AndProjection x) => new LogicalProjection(x)),
            UnionCase.Of((OrProjection x) => new LogicalProjection(x)),
            UnionCase.Of((NotProjection x) => new LogicalProjection(x))
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("all");
                return new MaxDateNullableProjection(
                    r.One<DateNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("all");
                return new MaxDatetimeNullableProjection(
                    r.One<DatetimeNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("all");
                return new MaxDecimalNullableProjection(
                    r.One<DecimalNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("all");
                return new MaxIntegerNullableProjection(
                    r.One<IntegerNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("all");
                return new MaxStringNullableProjection(
                    r.One<StringNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "max",
            r =>
            {
                r.FixedOver("all");
                return new MaxTimeNullableProjection(
                    r.One<TimeNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("all");
                return new MinDateNullableProjection(
                    r.One<DateNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("all");
                return new MinDatetimeNullableProjection(
                    r.One<DatetimeNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("all");
                return new MinDecimalNullableProjection(
                    r.One<DecimalNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("all");
                return new MinIntegerNullableProjection(
                    r.One<IntegerNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("all");
                return new MinStringNullableProjection(
                    r.One<StringNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "min",
            r =>
            {
                r.FixedOver("all");
                return new MinTimeNullableProjection(
                    r.One<TimeNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "modulo",
            r => new ModuloIntegerNullableProjection(
                r.One<IntegerNullableProjection>("left"),
                r.One<IntegerNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "modulo",
            r => new ModuloIntegerProjection(
                r.One<IntegerProjection>("left"),
                r.One<IntegerProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "multiply",
            r => new MultiplyDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyDecimalProjection(r.Many<DecimalProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            r => new MultiplyIntegerProjection(r.Many<IntegerProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualBooleanProjection(
                r.One<BooleanNullableProjection>("left"),
                r.One<BooleanNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualDateProjection(
                r.One<DateNullableProjection>("left"),
                r.One<DateNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualDatetimeProjection(
                r.One<DatetimeNullableProjection>("left"),
                r.One<DatetimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualDecimalProjection(
                r.One<DecimalNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
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
                (NotEqualDecimalProjection x) => new NotEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (NotEqualStringProjection x) => new NotEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (NotEqualBooleanProjection x) => new NotEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (NotEqualDateProjection x) => new NotEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (NotEqualTimeProjection x) => new NotEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (NotEqualDatetimeProjection x) => new NotEqualProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (NotEqualUuidProjection x) => new NotEqualProjection(x)
            )
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualStringProjection(
                r.One<StringNullableProjection>("left"),
                r.One<StringNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualTimeProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "notEqual",
            r => new NotEqualUuidProjection(
                r.One<UuidNullableProjection>("left"),
                r.One<UuidNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "not",
            r => new NotProjection(r.One<BooleanProjection>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            r => new OrProjection(r.Many<BooleanProjection>("conditions")),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            r => new RoundDecimalDigitsNullableProjection(
                r.One<DecimalNullableProjection>("value"),
                r.One<IntegerProjection>("digits")
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
            r => new RoundDecimalDigitsProjection(
                r.One<DecimalProjection>("value"),
                r.One<IntegerProjection>("digits")
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
            r => new RoundIntegerNullableProjection(
                r.One<DecimalNullableProjection>("value")
            ),
            (w, x) => w.One("value", x.Value)
        );
        registry.Operator(
            "round",
            r => new RoundIntegerProjection(r.One<DecimalProjection>("value")),
            (w, x) => w.One("value", x.Value)
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableProjection x) =>
                    new RoundingDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableProjection x) =>
                    new RoundingDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundDecimalDigitsNullableProjection x) =>
                    new RoundingDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundIntegerNullableProjection x) =>
                    new RoundingDecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FloorIntegerProjection x) => new RoundingDecimalProjection(x)),
            UnionCase.Of(
                (CeilingIntegerProjection x) => new RoundingDecimalProjection(x)
            ),
            UnionCase.Of(
                (RoundDecimalDigitsProjection x) => new RoundingDecimalProjection(x)
            ),
            UnionCase.Of((RoundIntegerProjection x) => new RoundingDecimalProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (FloorIntegerNullableProjection x) =>
                    new RoundingIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (CeilingIntegerNullableProjection x) =>
                    new RoundingIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundIntegerNullableProjection x) =>
                    new RoundingIntegerNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FloorIntegerProjection x) => new RoundingIntegerProjection(x)),
            UnionCase.Of(
                (CeilingIntegerProjection x) => new RoundingIntegerProjection(x)
            ),
            UnionCase.Of((RoundIntegerProjection x) => new RoundingIntegerProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsStringNullable x) => new StringNullableProjection(x)),
            UnionCase.Of((ParamAsStringNullable x) => new StringNullableProjection(x)),
            UnionCase.Of((LiteralAsStringNullable x) => new StringNullableProjection(x)),
            UnionCase.Of(
                (ConcatStringNullableProjection x) => new StringNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalStringNullableProjection x) => new StringNullableProjection(x)
            ),
            UnionCase.Of((AggregateStringProjection x) => new StringNullableProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldString x) => new StringProjection(x)),
            UnionCase.Of((ParamString x) => new StringProjection(x)),
            UnionCase.Of((LiteralString x) => new StringProjection(x)),
            UnionCase.Of((ConcatStringProjection x) => new StringProjection(x)),
            UnionCase.Of((ConditionalStringProjection x) => new StringProjection(x))
        );
        registry.Operator(
            "subtract",
            r => new SubtractDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractDecimalProjection(r.Many<DecimalProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values")
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            r => new SubtractIntegerProjection(r.Many<IntegerProjection>("values")),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "sum",
            r =>
            {
                r.FixedOver("all");
                return new SumDecimalProjection(
                    r.One<DecimalNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "sum",
            r =>
            {
                r.FixedOver("all");
                return new SumIntegerProjection(
                    r.One<IntegerNullableRow>("selector"),
                    r.Optional<BooleanRow>("predicate")
                );
            },
            (w, x) =>
            {
                w.One("selector", x.Selector);
                w.Optional("predicate", x.Predicate);
            }
        );
        registry.Operator(
            "timeAddSeconds",
            r => new TimeAddSecondsTimeNullableProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<DecimalNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeAddSeconds",
            r => new TimeAddSecondsTimeProjection(
                r.One<TimeProjection>("left"),
                r.One<DecimalProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeDiffSeconds",
            r => new TimeDiffSecondsDecimalNullableProjection(
                r.One<TimeNullableProjection>("left"),
                r.One<TimeNullableProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Operator(
            "timeDiffSeconds",
            r => new TimeDiffSecondsDecimalProjection(
                r.One<TimeProjection>("left"),
                r.One<TimeProjection>("right")
            ),
            (w, x) =>
            {
                w.One("left", x.Left);
                w.One("right", x.Right);
            }
        );
        registry.Union(
            UnionCase.Of((FieldAsTimeNullable x) => new TimeNullableProjection(x)),
            UnionCase.Of((ParamAsTimeNullable x) => new TimeNullableProjection(x)),
            UnionCase.Of((LiteralAsTimeNullable x) => new TimeNullableProjection(x)),
            UnionCase.Of(
                (TimeAddSecondsTimeNullableProjection x) => new TimeNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalTimeNullableProjection x) => new TimeNullableProjection(x)
            ),
            UnionCase.Of((AggregateTimeProjection x) => new TimeNullableProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldTime x) => new TimeProjection(x)),
            UnionCase.Of((ParamTime x) => new TimeProjection(x)),
            UnionCase.Of((LiteralTime x) => new TimeProjection(x)),
            UnionCase.Of((TimeAddSecondsTimeProjection x) => new TimeProjection(x)),
            UnionCase.Of((ConditionalTimeProjection x) => new TimeProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldAsUuidNullable x) => new UuidNullableProjection(x)),
            UnionCase.Of((ParamAsUuidNullable x) => new UuidNullableProjection(x)),
            UnionCase.Of((LiteralAsUuidNullable x) => new UuidNullableProjection(x)),
            UnionCase.Of(
                (ConditionalUuidNullableProjection x) => new UuidNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldUuid x) => new UuidProjection(x)),
            UnionCase.Of((ParamUuid x) => new UuidProjection(x)),
            UnionCase.Of((LiteralUuid x) => new UuidProjection(x)),
            UnionCase.Of((ConditionalUuidProjection x) => new UuidProjection(x))
        );
        registry.Probe(
            null,
            UnionCase.Of(
                TypeFamily.Decimal,
                (DecimalNullableProjection x) => new ValueProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.String,
                (StringNullableProjection x) => new ValueProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Boolean,
                (BooleanNullableProjection x) => new ValueProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Date,
                (DateNullableProjection x) => new ValueProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Time,
                (TimeNullableProjection x) => new ValueProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Datetime,
                (DatetimeNullableProjection x) => new ValueProjection(x)
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (UuidNullableProjection x) => new ValueProjection(x)
            )
        );
    }
}
