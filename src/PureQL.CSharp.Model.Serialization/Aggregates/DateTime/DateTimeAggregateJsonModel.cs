using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.DateTime;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.Aggregates.DateTime;

internal enum DateTimeAggregateOperatorJsonModel
{
    None,
    min_datetime,
    max_datetime,
    average_datetime,
}

internal sealed record DateTimeAggregateJsonModel
{
    public DateTimeAggregateJsonModel(MinDateTime minDateTime)
        : this(DateTimeAggregateOperatorJsonModel.min_datetime, minDateTime.Argument) { }

    public DateTimeAggregateJsonModel(MaxDateTime maxDateTime)
        : this(DateTimeAggregateOperatorJsonModel.max_datetime, maxDateTime.Argument) { }

    public DateTimeAggregateJsonModel(AverageDateTime averageDateTime)
        : this(
            DateTimeAggregateOperatorJsonModel.average_datetime,
            averageDateTime.Argument
        )
    { }

    [JsonConstructor]
    public DateTimeAggregateJsonModel(
        DateTimeAggregateOperatorJsonModel @operator,
        DateTimeArrayReturning arg
    )
    {
        Operator = @operator;
        Arg = arg ?? throw new JsonException();
    }

    public DateTimeAggregateOperatorJsonModel Operator { get; }

    public DateTimeArrayReturning Arg { get; }
}
