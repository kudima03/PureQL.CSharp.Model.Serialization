using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Time;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Time;

internal enum TimeAggregateOperatorJsonModel
{
    None,
    min_time,
    max_time,
    average_time,
}

internal sealed record TimeAggregateJsonModel
{
    public TimeAggregateJsonModel(MinTime minDateTime)
        : this(TimeAggregateOperatorJsonModel.min_time, minDateTime.Argument) { }

    public TimeAggregateJsonModel(MaxTime maxDateTime)
        : this(TimeAggregateOperatorJsonModel.max_time, maxDateTime.Argument) { }

    public TimeAggregateJsonModel(AverageTime averageDateTime)
        : this(TimeAggregateOperatorJsonModel.average_time, averageDateTime.Argument) { }

    [JsonConstructor]
    public TimeAggregateJsonModel(
        TimeAggregateOperatorJsonModel @operator,
        TimeReturning arg
    )
    {
        Operator = @operator;
        Arg = arg ?? throw new JsonException();
    }

    public TimeAggregateOperatorJsonModel Operator { get; }

    public TimeReturning Arg { get; }
}
