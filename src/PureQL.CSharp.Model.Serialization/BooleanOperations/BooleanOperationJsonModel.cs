using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

internal enum BooleanOperator
{
    None,
    Or,
    And,
    Not,
}

internal sealed record BooleanOperationJsonModel
{
    public BooleanOperationJsonModel(NotOperator @operator)
        : this(BooleanOperator.Not, [@operator.Condition]) { }

    public BooleanOperationJsonModel(OrOperator @operator)
        : this(BooleanOperator.Or, @operator.Conditions) { }

    public BooleanOperationJsonModel(AndOperator @operator)
        : this(BooleanOperator.And, @operator.Conditions) { }

    [JsonConstructor]
    public BooleanOperationJsonModel(
        BooleanOperator @operator,
        IEnumerable<BooleanReturning> conditions
    )
    {
        Operator =
            @operator == BooleanOperator.None ? throw new JsonException() : @operator;
        Conditions = conditions ?? throw new JsonException();
    }

    public BooleanOperator Operator { get; }

    public IEnumerable<BooleanReturning> Conditions { get; }
}
