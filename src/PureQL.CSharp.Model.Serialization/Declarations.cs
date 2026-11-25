using System.Runtime.CompilerServices;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Whether operands that have been read declare a <c>decimal</c>, which an operator
/// declared <c>decimal</c> of number operands takes its type from: the schema makes
/// every declared type exact, and a decimal position of the model also holds an
/// integer operand.
/// </summary>
internal static class Declarations
{
    public static bool IsDecimal<TUnion>(TUnion operand)
        where TUnion : struct, IUnion
    {
        return ModelRegistry.Instance.IsDecimal(operand.Value);
    }

    /// <summary>Whether an optional operand is present and declares a decimal.</summary>
    public static bool IsDecimal<TUnion>(TUnion? operand)
        where TUnion : struct, IUnion
    {
        return operand.HasValue && IsDecimal(operand.GetValueOrDefault());
    }

    public static bool AnyDecimal<TUnion>(TUnion[] operands)
        where TUnion : struct, IUnion
    {
        foreach (TUnion operand in operands)
        {
            if (IsDecimal(operand))
            {
                return true;
            }
        }

        return false;
    }
}
