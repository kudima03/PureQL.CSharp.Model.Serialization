using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.ProjectionExpressions;
using PureQL.CSharp.Model.RowExpressions;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.ProjectionExpressions;

internal static class ProjectionExpressionRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Operator(
            "add",
            new TypeDecimalNullable(),
            r => new AddDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeDecimal(),
            r => new AddDecimalProjection(r.Many<DecimalProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeIntegerNullable(),
            r => new AddIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "add",
            new TypeInteger(),
            r => new AddIntegerProjection(r.Many<IntegerProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Union(
            UnionCase.Of(
                (MinBooleanNullableProjection x) =>
                    new AggregateBooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxBooleanNullableProjection x) =>
                    new AggregateBooleanNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((MinBooleanProjection x) => new AggregateBooleanProjection(x)),
            UnionCase.Of((MaxBooleanProjection x) => new AggregateBooleanProjection(x)),
            UnionCase.Of((AnyProjection x) => new AggregateBooleanProjection(x)),
            UnionCase.Of((AllProjection x) => new AggregateBooleanProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDateNullableProjection x) =>
                    new AggregateDateNullableProjection(x)
            ),
            UnionCase.Of(
                (MinDateNullableProjection x) => new AggregateDateNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxDateNullableProjection x) => new AggregateDateNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((AverageDateProjection x) => new AggregateDateProjection(x)),
            UnionCase.Of((MinDateProjection x) => new AggregateDateProjection(x)),
            UnionCase.Of((MaxDateProjection x) => new AggregateDateProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageDatetimeNullableProjection x) =>
                    new AggregateDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (MinDatetimeNullableProjection x) =>
                    new AggregateDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxDatetimeNullableProjection x) =>
                    new AggregateDatetimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (AverageDatetimeProjection x) => new AggregateDatetimeProjection(x)
            ),
            UnionCase.Of((MinDatetimeProjection x) => new AggregateDatetimeProjection(x)),
            UnionCase.Of((MaxDatetimeProjection x) => new AggregateDatetimeProjection(x))
        );
        registry.Union(
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
            UnionCase.Of((SumDecimalProjection x) => new AggregateDecimalProjection(x)),
            UnionCase.Of(
                (AverageDecimalProjection x) => new AggregateDecimalProjection(x)
            ),
            UnionCase.Of((MinDecimalProjection x) => new AggregateDecimalProjection(x)),
            UnionCase.Of((MaxDecimalProjection x) => new AggregateDecimalProjection(x))
        );
        registry.Union(
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
            UnionCase.Of((SumIntegerProjection x) => new AggregateIntegerProjection(x)),
            UnionCase.Of((MinIntegerProjection x) => new AggregateIntegerProjection(x)),
            UnionCase.Of((MaxIntegerProjection x) => new AggregateIntegerProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinStringNullableProjection x) =>
                    new AggregateStringNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxStringNullableProjection x) =>
                    new AggregateStringNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((MinStringProjection x) => new AggregateStringProjection(x)),
            UnionCase.Of((MaxStringProjection x) => new AggregateStringProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (AverageTimeNullableProjection x) =>
                    new AggregateTimeNullableProjection(x)
            ),
            UnionCase.Of(
                (MinTimeNullableProjection x) => new AggregateTimeNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxTimeNullableProjection x) => new AggregateTimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((AverageTimeProjection x) => new AggregateTimeProjection(x)),
            UnionCase.Of((MinTimeProjection x) => new AggregateTimeProjection(x)),
            UnionCase.Of((MaxTimeProjection x) => new AggregateTimeProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (MinUuidNullableProjection x) => new AggregateUuidNullableProjection(x)
            ),
            UnionCase.Of(
                (MaxUuidNullableProjection x) => new AggregateUuidNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((MinUuidProjection x) => new AggregateUuidProjection(x)),
            UnionCase.Of((MaxUuidProjection x) => new AggregateUuidProjection(x))
        );
        registry.Operator(
            "all",
            new TypeBoolean(),
            r => new AllProjection(r.One<BooleanRow>("predicate")),
            (w, x) => w.One("predicate", x.Predicate)
        );
        registry.Operator(
            "and",
            new TypeBoolean(),
            r => new AndProjection(r.Many<BooleanProjection>("conditions", 2)),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "any",
            new TypeBoolean(),
            r => new AnyProjection(r.One<BooleanRow>("predicate")),
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
            new TypeDateNullable(),
            r => new AverageDateNullableProjection(
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
            new TypeDate(),
            r => new AverageDateProjection(r.One<DateRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            new TypeDatetimeNullable(),
            r => new AverageDatetimeNullableProjection(
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
            new TypeDatetime(),
            r => new AverageDatetimeProjection(r.One<DatetimeRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            new TypeDecimalNullable(),
            r => new AverageDecimalNullableProjection(
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
            new TypeDecimal(),
            r => new AverageDecimalProjection(r.One<DecimalRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "average",
            new TypeTimeNullable(),
            r => new AverageTimeNullableProjection(
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
            "average",
            new TypeTime(),
            r => new AverageTimeProjection(r.One<TimeRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Union(
            UnionCase.Of((BooleanProjection x) => new BooleanNullableProjection(x)),
            UnionCase.Of(
                (ExactBooleanNullableProjection x) => new BooleanNullableProjection(x)
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
            new TypeIntegerNullable(),
            r => new CeilingIntegerNullableProjection(
                r.One<DecimalNullableProjection>("operand")
            ),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "ceiling",
            new TypeInteger(),
            r => new CeilingIntegerProjection(r.One<DecimalProjection>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "coalesce",
            new TypeBooleanNullable(),
            r => new CoalesceBooleanNullableProjection(
                r.Many<BooleanNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeBoolean(),
            r => new CoalesceBooleanProjection(
                r.Many<BooleanNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDateNullable(),
            r => new CoalesceDateNullableProjection(
                r.Many<DateNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDate(),
            r => new CoalesceDateProjection(r.Many<DateNullableProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetimeNullable(),
            r => new CoalesceDatetimeNullableProjection(
                r.Many<DatetimeNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDatetime(),
            r => new CoalesceDatetimeProjection(
                r.Many<DatetimeNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimalNullable(),
            r => new CoalesceDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeDecimal(),
            r => new CoalesceDecimalProjection(
                r.Many<DecimalNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeIntegerNullable(),
            r => new CoalesceIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeInteger(),
            r => new CoalesceIntegerProjection(
                r.Many<IntegerNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeStringNullable(),
            r => new CoalesceStringNullableProjection(
                r.Many<StringNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeString(),
            r => new CoalesceStringProjection(
                r.Many<StringNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTimeNullable(),
            r => new CoalesceTimeNullableProjection(
                r.Many<TimeNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeTime(),
            r => new CoalesceTimeProjection(r.Many<TimeNullableProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuidNullable(),
            r => new CoalesceUuidNullableProjection(
                r.Many<UuidNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "coalesce",
            new TypeUuid(),
            r => new CoalesceUuidProjection(r.Many<UuidNullableProjection>("values", 2)),
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
            new TypeStringNullable(),
            r => new ConcatStringNullableProjection(
                r.Many<StringNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "concat",
            new TypeString(),
            r => new ConcatStringProjection(r.Many<StringProjection>("values", 2)),
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
            new TypeInteger(),
            r => new CountProjection(r.Optional<BooleanRow>("predicate")),
            (w, x) => w.Optional("predicate", x.Predicate)
        );
        registry.Operator(
            "dateAddDays",
            new TypeDateNullable(),
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
            new TypeDate(),
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
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            UnionCase.Of((DateProjection x) => new DateNullableProjection(x)),
            UnionCase.Of((ExactDateNullableProjection x) => new DateNullableProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldDate x) => new DateProjection(x)),
            UnionCase.Of((ParamDate x) => new DateProjection(x)),
            UnionCase.Of((LiteralDate x) => new DateProjection(x)),
            UnionCase.Of((DateAddDaysDateProjection x) => new DateProjection(x)),
            UnionCase.Of((ConditionalDateProjection x) => new DateProjection(x)),
            UnionCase.Of((AggregateDateProjection x) => new DateProjection(x))
        );
        registry.Operator(
            "datetimeAddSeconds",
            new TypeDatetimeNullable(),
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
            new TypeDatetime(),
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
            new TypeDecimalNullable(),
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
            new TypeDecimal(),
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
            UnionCase.Of((DatetimeProjection x) => new DatetimeNullableProjection(x)),
            UnionCase.Of(
                (ExactDatetimeNullableProjection x) => new DatetimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDatetime x) => new DatetimeProjection(x)),
            UnionCase.Of((ParamDatetime x) => new DatetimeProjection(x)),
            UnionCase.Of((LiteralDatetime x) => new DatetimeProjection(x)),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeProjection x) => new DatetimeProjection(x)
            ),
            UnionCase.Of((ConditionalDatetimeProjection x) => new DatetimeProjection(x)),
            UnionCase.Of((AggregateDatetimeProjection x) => new DatetimeProjection(x))
        );
        registry.Union(
            UnionCase.Of((ExactDecimalProjection x) => new DecimalNullableProjection(x)),
            UnionCase.Of(
                (ExactDecimalNullableProjection x) => new DecimalNullableProjection(x)
            ),
            UnionCase.Of((IntegerProjection x) => new DecimalNullableProjection(x)),
            UnionCase.Of(
                (ExactIntegerNullableProjection x) => new DecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((ExactDecimalProjection x) => new DecimalProjection(x)),
            UnionCase.Of((IntegerProjection x) => new DecimalProjection(x))
        );
        registry.Union(
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
                (TimeDiffSecondsDecimalProjection x) => new DifferenceDecimalProjection(x)
            ),
            UnionCase.Of(
                (DatetimeDiffSecondsDecimalProjection x) =>
                    new DifferenceDecimalProjection(x)
            )
        );
        registry.Operator(
            "divide",
            new TypeDecimalNullable(),
            r => new DivideDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "divide",
            new TypeDecimal(),
            r => new DivideDecimalProjection(r.Many<DecimalProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "equal",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Union(
            UnionCase.Of(
                (FieldBooleanNullable x) => new ExactBooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (ParamBooleanNullable x) => new ExactBooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (LiteralBooleanNullable x) => new ExactBooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalBooleanNullableProjection x) =>
                    new ExactBooleanNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateBooleanNullableProjection x) =>
                    new ExactBooleanNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDateNullable x) => new ExactDateNullableProjection(x)),
            UnionCase.Of((ParamDateNullable x) => new ExactDateNullableProjection(x)),
            UnionCase.Of((LiteralDateNullable x) => new ExactDateNullableProjection(x)),
            UnionCase.Of(
                (DateAddDaysDateNullableProjection x) =>
                    new ExactDateNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDateNullableProjection x) =>
                    new ExactDateNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateDateNullableProjection x) => new ExactDateNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (FieldDatetimeNullable x) => new ExactDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (ParamDatetimeNullable x) => new ExactDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (LiteralDatetimeNullable x) => new ExactDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (DatetimeAddSecondsDatetimeNullableProjection x) =>
                    new ExactDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDatetimeNullableProjection x) =>
                    new ExactDatetimeNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateDatetimeNullableProjection x) =>
                    new ExactDatetimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of(
                (FieldDecimalNullable x) => new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (ParamDecimalNullable x) => new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (LiteralDecimalNullable x) => new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (ArithmeticDecimalNullableProjection x) =>
                    new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundDecimalNullableProjection x) =>
                    new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (DifferenceDecimalNullableProjection x) =>
                    new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalNullableProjection x) =>
                    new ExactDecimalNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateDecimalNullableProjection x) =>
                    new ExactDecimalNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldDecimal x) => new ExactDecimalProjection(x)),
            UnionCase.Of((ParamDecimal x) => new ExactDecimalProjection(x)),
            UnionCase.Of((LiteralDecimal x) => new ExactDecimalProjection(x)),
            UnionCase.Of(
                (ArithmeticDecimalProjection x) => new ExactDecimalProjection(x)
            ),
            UnionCase.Of((RoundDecimalProjection x) => new ExactDecimalProjection(x)),
            UnionCase.Of(
                (DifferenceDecimalProjection x) => new ExactDecimalProjection(x)
            ),
            UnionCase.Of(
                (ConditionalDecimalProjection x) => new ExactDecimalProjection(x)
            ),
            UnionCase.Of((AggregateDecimalProjection x) => new ExactDecimalProjection(x))
        );
        registry.Union(
            UnionCase.Of(
                (FieldIntegerNullable x) => new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (ParamIntegerNullable x) => new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (LiteralIntegerNullable x) => new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (ArithmeticIntegerNullableProjection x) =>
                    new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (RoundingIntegerNullableProjection x) =>
                    new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (DateDiffDaysIntegerNullableProjection x) =>
                    new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalIntegerNullableProjection x) =>
                    new ExactIntegerNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateIntegerNullableProjection x) =>
                    new ExactIntegerNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldStringNullable x) => new ExactStringNullableProjection(x)),
            UnionCase.Of((ParamStringNullable x) => new ExactStringNullableProjection(x)),
            UnionCase.Of(
                (LiteralStringNullable x) => new ExactStringNullableProjection(x)
            ),
            UnionCase.Of(
                (ConcatStringNullableProjection x) => new ExactStringNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalStringNullableProjection x) =>
                    new ExactStringNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateStringNullableProjection x) =>
                    new ExactStringNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldTimeNullable x) => new ExactTimeNullableProjection(x)),
            UnionCase.Of((ParamTimeNullable x) => new ExactTimeNullableProjection(x)),
            UnionCase.Of((LiteralTimeNullable x) => new ExactTimeNullableProjection(x)),
            UnionCase.Of(
                (TimeAddSecondsTimeNullableProjection x) =>
                    new ExactTimeNullableProjection(x)
            ),
            UnionCase.Of(
                (ConditionalTimeNullableProjection x) =>
                    new ExactTimeNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateTimeNullableProjection x) => new ExactTimeNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldUuidNullable x) => new ExactUuidNullableProjection(x)),
            UnionCase.Of((ParamUuidNullable x) => new ExactUuidNullableProjection(x)),
            UnionCase.Of((LiteralUuidNullable x) => new ExactUuidNullableProjection(x)),
            UnionCase.Of(
                (ConditionalUuidNullableProjection x) =>
                    new ExactUuidNullableProjection(x)
            ),
            UnionCase.Of(
                (AggregateUuidNullableProjection x) => new ExactUuidNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((DecimalNullableProjection x) => new ExprProjection(x)),
            UnionCase.Of((StringNullableProjection x) => new ExprProjection(x)),
            UnionCase.Of((BooleanNullableProjection x) => new ExprProjection(x)),
            UnionCase.Of((DateNullableProjection x) => new ExprProjection(x)),
            UnionCase.Of((TimeNullableProjection x) => new ExprProjection(x)),
            UnionCase.Of((DatetimeNullableProjection x) => new ExprProjection(x)),
            UnionCase.Of((UuidNullableProjection x) => new ExprProjection(x))
        );
        registry.Operator(
            "floor",
            new TypeIntegerNullable(),
            r => new FloorIntegerNullableProjection(
                r.One<DecimalNullableProjection>("operand")
            ),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "floor",
            new TypeInteger(),
            r => new FloorIntegerProjection(r.One<DecimalProjection>("operand")),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanBooleanProjection(
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
            "greaterThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
            r => new GreaterThanOrEqualBooleanProjection(
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
            "greaterThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
                TypeFamily.Boolean,
                (GreaterThanOrEqualBooleanProjection x) =>
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
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanOrEqualUuidProjection x) =>
                    new GreaterThanOrEqualProjection(x)
            )
        );
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Operator(
            "greaterThanOrEqual",
            new TypeBoolean(),
            r => new GreaterThanOrEqualUuidProjection(
                r.One<UuidNullableProjection>("left"),
                r.One<UuidNullableProjection>("right")
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
                TypeFamily.Boolean,
                (GreaterThanBooleanProjection x) => new GreaterThanProjection(x)
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
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (GreaterThanUuidProjection x) => new GreaterThanProjection(x)
            )
        );
        registry.Operator(
            "greaterThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            "greaterThan",
            new TypeBoolean(),
            r => new GreaterThanUuidProjection(
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
            "if",
            new TypeBooleanNullable(),
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
            new TypeBoolean(),
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
            new TypeDateNullable(),
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
            new TypeDate(),
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
            new TypeDatetimeNullable(),
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
            new TypeDatetime(),
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
            new TypeDecimalNullable(),
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
            new TypeDecimal(),
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
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            new TypeStringNullable(),
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
            new TypeString(),
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
            new TypeTimeNullable(),
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
            new TypeTime(),
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
            new TypeUuidNullable(),
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
            new TypeUuid(),
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
            new TypeBoolean(),
            r => new InBooleanProjection(
                r.One<BooleanNullableProjection>("operand"),
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
            r => new InDateProjection(
                r.One<DateNullableProjection>("operand"),
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
            r => new InDatetimeProjection(
                r.One<DatetimeNullableProjection>("operand"),
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
            r => new InDecimalProjection(
                r.One<DecimalNullableProjection>("operand"),
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
            new TypeBoolean(),
            r => new InStringProjection(
                r.One<StringNullableProjection>("operand"),
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
            r => new InTimeProjection(
                r.One<TimeNullableProjection>("operand"),
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
            r => new InUuidProjection(
                r.One<UuidNullableProjection>("operand"),
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
            new TypeInteger(),
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
            UnionCase.Of((IntegerProjection x) => new IntegerNullableProjection(x)),
            UnionCase.Of(
                (ExactIntegerNullableProjection x) => new IntegerNullableProjection(x)
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
            new TypeBoolean(),
            r => new LessThanBooleanProjection(
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
            "lessThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
            r => new LessThanOrEqualBooleanProjection(
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
            "lessThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
                TypeFamily.Boolean,
                (LessThanOrEqualBooleanProjection x) => new LessThanOrEqualProjection(x)
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
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (LessThanOrEqualUuidProjection x) => new LessThanOrEqualProjection(x)
            )
        );
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Operator(
            "lessThanOrEqual",
            new TypeBoolean(),
            r => new LessThanOrEqualUuidProjection(
                r.One<UuidNullableProjection>("left"),
                r.One<UuidNullableProjection>("right")
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
                TypeFamily.Boolean,
                (LessThanBooleanProjection x) => new LessThanProjection(x)
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
            ),
            UnionCase.Of(
                TypeFamily.Uuid,
                (LessThanUuidProjection x) => new LessThanProjection(x)
            )
        );
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
        registry.Operator(
            "lessThan",
            new TypeBoolean(),
            r => new LessThanUuidProjection(
                r.One<UuidNullableProjection>("left"),
                r.One<UuidNullableProjection>("right")
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
            new TypeBooleanNullable(),
            r => new MaxBooleanNullableProjection(
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
            new TypeBoolean(),
            r => new MaxBooleanProjection(r.One<BooleanRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeDateNullable(),
            r => new MaxDateNullableProjection(
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
            new TypeDate(),
            r => new MaxDateProjection(r.One<DateRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeDatetimeNullable(),
            r => new MaxDatetimeNullableProjection(
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
            new TypeDatetime(),
            r => new MaxDatetimeProjection(r.One<DatetimeRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeDecimalNullable(),
            r => new MaxDecimalNullableProjection(
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
            new TypeDecimal(),
            r => new MaxDecimalProjection(r.One<DecimalRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeIntegerNullable(),
            r => new MaxIntegerNullableProjection(
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
            new TypeInteger(),
            r => new MaxIntegerProjection(r.One<IntegerRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeStringNullable(),
            r => new MaxStringNullableProjection(
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
            new TypeString(),
            r => new MaxStringProjection(r.One<StringRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeTimeNullable(),
            r => new MaxTimeNullableProjection(
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
            new TypeTime(),
            r => new MaxTimeProjection(r.One<TimeRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "max",
            new TypeUuidNullable(),
            r => new MaxUuidNullableProjection(
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
            "max",
            new TypeUuid(),
            r => new MaxUuidProjection(r.One<UuidRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeBooleanNullable(),
            r => new MinBooleanNullableProjection(
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
            new TypeBoolean(),
            r => new MinBooleanProjection(r.One<BooleanRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeDateNullable(),
            r => new MinDateNullableProjection(
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
            new TypeDate(),
            r => new MinDateProjection(r.One<DateRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeDatetimeNullable(),
            r => new MinDatetimeNullableProjection(
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
            new TypeDatetime(),
            r => new MinDatetimeProjection(r.One<DatetimeRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeDecimalNullable(),
            r => new MinDecimalNullableProjection(
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
            new TypeDecimal(),
            r => new MinDecimalProjection(r.One<DecimalRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeIntegerNullable(),
            r => new MinIntegerNullableProjection(
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
            new TypeInteger(),
            r => new MinIntegerProjection(r.One<IntegerRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeStringNullable(),
            r => new MinStringNullableProjection(
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
            new TypeString(),
            r => new MinStringProjection(r.One<StringRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeTimeNullable(),
            r => new MinTimeNullableProjection(
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
            new TypeTime(),
            r => new MinTimeProjection(r.One<TimeRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "min",
            new TypeUuidNullable(),
            r => new MinUuidNullableProjection(
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
            new TypeUuid(),
            r => new MinUuidProjection(r.One<UuidRow>("selector")),
            (w, x) => w.One("selector", x.Selector)
        );
        registry.Operator(
            "modulo",
            new TypeIntegerNullable(),
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
            new TypeInteger(),
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
            new TypeDecimalNullable(),
            r => new MultiplyDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeDecimal(),
            r => new MultiplyDecimalProjection(r.Many<DecimalProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeIntegerNullable(),
            r => new MultiplyIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "multiply",
            new TypeInteger(),
            r => new MultiplyIntegerProjection(r.Many<IntegerProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "notEqual",
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
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
            new TypeBoolean(),
            r => new NotProjection(r.One<BooleanProjection>("condition")),
            (w, x) => w.One("condition", x.Condition)
        );
        registry.Operator(
            "or",
            new TypeBoolean(),
            r => new OrProjection(r.Many<BooleanProjection>("conditions", 2)),
            (w, x) => w.Many("conditions", x.Conditions)
        );
        registry.Operator(
            "round",
            new TypeDecimalNullable(),
            r => new RoundDecimalNullableProjection(
                r.One<DecimalNullableProjection>("operand"),
                r.One<IntegerProjection>("digits")
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
            r => new RoundDecimalProjection(
                r.One<DecimalProjection>("operand"),
                r.One<IntegerProjection>("digits")
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
            r => new RoundIntegerNullableProjection(
                r.One<DecimalNullableProjection>("operand")
            ),
            (w, x) => w.One("operand", x.Operand)
        );
        registry.Operator(
            "round",
            new TypeInteger(),
            r => new RoundIntegerProjection(r.One<DecimalProjection>("operand")),
            (w, x) => w.One("operand", x.Operand)
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
            UnionCase.Of((StringProjection x) => new StringNullableProjection(x)),
            UnionCase.Of(
                (ExactStringNullableProjection x) => new StringNullableProjection(x)
            )
        );
        registry.Union(
            UnionCase.Of((FieldString x) => new StringProjection(x)),
            UnionCase.Of((ParamString x) => new StringProjection(x)),
            UnionCase.Of((LiteralString x) => new StringProjection(x)),
            UnionCase.Of((ConcatStringProjection x) => new StringProjection(x)),
            UnionCase.Of((ConditionalStringProjection x) => new StringProjection(x)),
            UnionCase.Of((AggregateStringProjection x) => new StringProjection(x))
        );
        registry.Operator(
            "subtract",
            new TypeDecimalNullable(),
            r => new SubtractDecimalNullableProjection(
                r.Many<DecimalNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeDecimal(),
            r => new SubtractDecimalProjection(r.Many<DecimalProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeIntegerNullable(),
            r => new SubtractIntegerNullableProjection(
                r.Many<IntegerNullableProjection>("values", 2)
            ),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "subtract",
            new TypeInteger(),
            r => new SubtractIntegerProjection(r.Many<IntegerProjection>("values", 2)),
            (w, x) => w.Many("values", x.Values)
        );
        registry.Operator(
            "sum",
            new TypeDecimal(),
            r => new SumDecimalProjection(
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
            r => new SumIntegerProjection(
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
            new TypeTime(),
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
            new TypeDecimalNullable(),
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
            new TypeDecimal(),
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
            UnionCase.Of((TimeProjection x) => new TimeNullableProjection(x)),
            UnionCase.Of((ExactTimeNullableProjection x) => new TimeNullableProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldTime x) => new TimeProjection(x)),
            UnionCase.Of((ParamTime x) => new TimeProjection(x)),
            UnionCase.Of((LiteralTime x) => new TimeProjection(x)),
            UnionCase.Of((TimeAddSecondsTimeProjection x) => new TimeProjection(x)),
            UnionCase.Of((ConditionalTimeProjection x) => new TimeProjection(x)),
            UnionCase.Of((AggregateTimeProjection x) => new TimeProjection(x))
        );
        registry.Union(
            UnionCase.Of((UuidProjection x) => new UuidNullableProjection(x)),
            UnionCase.Of((ExactUuidNullableProjection x) => new UuidNullableProjection(x))
        );
        registry.Union(
            UnionCase.Of((FieldUuid x) => new UuidProjection(x)),
            UnionCase.Of((ParamUuid x) => new UuidProjection(x)),
            UnionCase.Of((LiteralUuid x) => new UuidProjection(x)),
            UnionCase.Of((ConditionalUuidProjection x) => new UuidProjection(x)),
            UnionCase.Of((AggregateUuidProjection x) => new UuidProjection(x))
        );
    }
}
