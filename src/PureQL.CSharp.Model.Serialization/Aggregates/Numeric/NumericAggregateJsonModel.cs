using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Numeric;

internal enum NumericAggregateOperatorJsonModel
{
    None,
    min_number,
    max_number,
    average_number,
    sum,
}

internal sealed record NumericAggregateJsonModel
{
    public NumericAggregateJsonModel(MinNumber minNumber)
        : this(NumericAggregateOperatorJsonModel.min_number, minNumber.Argument) { }

    public NumericAggregateJsonModel(MaxNumber maxNumber)
        : this(NumericAggregateOperatorJsonModel.max_number, maxNumber.Argument) { }

    public NumericAggregateJsonModel(AverageNumber averageNumber)
        : this(NumericAggregateOperatorJsonModel.average_number, averageNumber.Argument)
    { }

    public NumericAggregateJsonModel(SumNumber sum)
        : this(NumericAggregateOperatorJsonModel.sum, sum.Argument) { }

    [JsonConstructor]
    public NumericAggregateJsonModel(
        NumericAggregateOperatorJsonModel @operator,
        NumberArrayReturning arg
    )
    {
        Operator = @operator;
        Arg = arg ?? throw new JsonException();
    }

    public NumericAggregateOperatorJsonModel Operator { get; }

    public NumberArrayReturning Arg { get; }
}
