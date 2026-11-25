using System.Runtime.CompilerServices;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Whether operands that have been read declare a nullable type, which an operator
/// declared nullable takes its type from: the schema makes every declared type exact,
/// and a nullable position of the model also holds a non-null operand.
/// </summary>
internal static class Nullability
{
    public static bool Of<TUnion>(TUnion operand)
        where TUnion : struct, IUnion
    {
        return ModelRegistry.Instance.IsNullable(operand.Value);
    }

    /// <summary>Whether an optional operand is present and declares a nullable type.</summary>
    public static bool Of<TUnion>(TUnion? operand)
        where TUnion : struct, IUnion
    {
        return operand.HasValue && Of(operand.GetValueOrDefault());
    }

    public static bool Any<TUnion>(TUnion[] operands)
        where TUnion : struct, IUnion
    {
        foreach (TUnion operand in operands)
        {
            if (Of(operand))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Whether exactly one of the operands declares a non-null type.</summary>
    public static bool OneNonNull<TUnion>(TUnion[] operands)
        where TUnion : struct, IUnion
    {
        int count = 0;
        foreach (TUnion operand in operands)
        {
            count += Of(operand) ? 0 : 1;
        }

        return count == 1;
    }

    public static bool All<TUnion>(TUnion[] operands)
        where TUnion : struct, IUnion
    {
        foreach (TUnion operand in operands)
        {
            if (!Of(operand))
            {
                return false;
            }
        }

        return true;
    }
}
