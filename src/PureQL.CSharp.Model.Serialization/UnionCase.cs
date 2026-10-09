namespace PureQL.CSharp.Model.Serialization;

/// <summary>One case of a OneOf union and the constructor that wraps it.</summary>
internal sealed class UnionCase<TUnion>(
    Type type,
    TypeFamily? family,
    Func<object, TUnion> create
)
{
    public Type Type { get; } = type;

    /// <summary>The operand family that selects this case in a probe union.</summary>
    public TypeFamily? Family { get; } = family;

    public Func<object, TUnion> Create { get; } = create;
}

internal static class UnionCase
{
    public static UnionCase<TUnion> Of<TCase, TUnion>(Func<TCase, TUnion> create)
        where TCase : class
    {
        return new UnionCase<TUnion>(typeof(TCase), null, value => create((TCase)value));
    }

    public static UnionCase<TUnion> Of<TCase, TUnion>(
        TypeFamily family,
        Func<TCase, TUnion> create
    )
        where TCase : class
    {
        return new UnionCase<TUnion>(
            typeof(TCase),
            family,
            value => create((TCase)value)
        );
    }
}
