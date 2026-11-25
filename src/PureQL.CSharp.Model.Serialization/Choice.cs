namespace PureQL.CSharp.Model.Serialization;

/// <summary>What chooses the case of an operator beside its declared type.</summary>
internal enum Choice
{
    None,

    /// <summary>The family of the type <c>left</c> declares.</summary>
    Left,

    /// <summary>The family of the type <c>operand</c> declares.</summary>
    Operand,

    /// <summary>Whether <c>digits</c> is present.</summary>
    Digits,
}
