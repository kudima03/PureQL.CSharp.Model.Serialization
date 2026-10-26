namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// What gives an operator declared nullable its nullable type. The schema makes every
/// declared type exact, and a nullable operand of the model also holds a non-null value,
/// so the cause is checked on the JSON node.
/// </summary>
internal enum NullableCause
{
    /// <summary>A nullable operand, or a predicate.</summary>
    AnyOperand,

    /// <summary>Only nullable operands, as of <c>coalesce</c>.</summary>
    EveryOperand,

    /// <summary>Nothing: an aggregate over all rows of a total query.</summary>
    Always,
}
