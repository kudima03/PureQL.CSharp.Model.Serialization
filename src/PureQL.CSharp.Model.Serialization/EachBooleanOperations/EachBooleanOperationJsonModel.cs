using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachBooleanOperations;

namespace PureQL.CSharp.Model.Serialization.EachBooleanOperations;

internal enum EachBooleanOperatorName
{
    None,
    eachAnd,
    eachOr,
}

internal sealed record EachAndOrOperatorJsonModel
{
    public EachAndOrOperatorJsonModel(EachAndOperator @operator)
        : this(EachBooleanOperatorName.eachAnd, @operator.Conditions) { }

    public EachAndOrOperatorJsonModel(EachOrOperator @operator)
        : this(EachBooleanOperatorName.eachOr, @operator.Conditions) { }

    [JsonConstructor]
    public EachAndOrOperatorJsonModel(
        EachBooleanOperatorName @operator,
        IEnumerable<BooleanArrayReturning> conditions
    )
    {
        Operator =
            @operator == EachBooleanOperatorName.None
                ? throw new JsonException()
                : @operator;
        Conditions = conditions ?? throw new JsonException();
    }

    public EachBooleanOperatorName Operator { get; }

    public IEnumerable<BooleanArrayReturning> Conditions { get; }
}

internal enum EachNotOperatorName
{
    None,
    eachNot,
}

internal sealed record EachNotOperatorJsonModel
{
    public EachNotOperatorJsonModel(EachNotOperator @operator)
        : this(EachNotOperatorName.eachNot, @operator.Condition) { }

    [JsonConstructor]
    public EachNotOperatorJsonModel(
        EachNotOperatorName @operator,
        BooleanArrayReturning condition
    )
    {
        Operator =
            @operator == EachNotOperatorName.None
                ? throw new JsonException()
                : @operator;
        Condition = condition ?? throw new JsonException();
    }

    public EachNotOperatorName Operator { get; }

    public BooleanArrayReturning Condition { get; }
}
