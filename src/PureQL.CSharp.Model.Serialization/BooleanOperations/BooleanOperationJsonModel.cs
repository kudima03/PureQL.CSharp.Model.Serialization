using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
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
    public BooleanOperationJsonModel(OrOperator @operator)
        : this(BooleanOperator.Or, @operator.Conditions) { }

    public BooleanOperationJsonModel(AndOperator @operator)
        : this(BooleanOperator.And, @operator.Conditions) { }

    [JsonConstructor]
    public BooleanOperationJsonModel(
        BooleanOperator @operator,
        OneOf<IEnumerable<BooleanReturning>, BooleanArrayReturning>? conditions
    )
    {
        Operator =
            @operator == BooleanOperator.None ? throw new JsonException() : @operator;
        Conditions = conditions ?? throw new JsonException();
    }

    public BooleanOperator Operator { get; }

    public OneOf<
        IEnumerable<BooleanReturning>,
        BooleanArrayReturning
    >? Conditions
    { get; }
}
