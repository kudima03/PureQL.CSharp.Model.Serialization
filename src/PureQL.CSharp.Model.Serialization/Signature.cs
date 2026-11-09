namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// What a union reads from a node to choose its case: the <see cref="Kinds">kind</see>
/// of node, the <see cref="TypeIds">type</see> it declares and, for a comparison, the
/// <see cref="Families">family</see> of its operand. Every case of a union has a
/// different signature, so a union reads its case through one <c>switch</c> over the
/// constants of <c>Signatures</c>.
/// </summary>
internal static class Signature
{
    public static int Of(int kind, int type, int family)
    {
        return (kind << 16) | (type << 8) | family;
    }

    public static int Of(int kind, int type)
    {
        return Of(kind, type, Families.None);
    }
}
