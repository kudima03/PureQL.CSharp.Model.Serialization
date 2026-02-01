using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Date;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Date;

internal enum DateAggregateOperatorJsonModel
{
    None,
    min_date,
    max_date,
    average_date,
}

internal sealed record DateAggregateJsonModel
{
    public DateAggregateJsonModel(MinDate minDate)
        : this(DateAggregateOperatorJsonModel.min_date, minDate.Argument) { }

    public DateAggregateJsonModel(MaxDate maxDate)
        : this(DateAggregateOperatorJsonModel.max_date, maxDate.Argument) { }

    public DateAggregateJsonModel(AverageDate averageDate)
        : this(DateAggregateOperatorJsonModel.average_date, averageDate.Argument) { }

    [JsonConstructor]
    public DateAggregateJsonModel(
        DateAggregateOperatorJsonModel @operator,
        DateArrayReturning arg
    )
    {
        Operator = @operator;
        Arg = arg ?? throw new JsonException();
    }

    public DateAggregateOperatorJsonModel Operator { get; }

    public DateArrayReturning Arg { get; }
}
