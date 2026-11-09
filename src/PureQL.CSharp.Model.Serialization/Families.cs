namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The operand families that choose a variant of <c>equal</c>, <c>notEqual</c>,
/// <c>in</c> and the comparisons: that of the type the operand declares, where
/// <c>integer</c> belongs to the decimal family. The last part of a
/// <see cref="Signature"/>, which is <see cref="None"/> for any other node.
/// </summary>
internal static class Families
{
    public const int None = 0;

    public const int Decimal = TypeIds.Decimal;

    public const int String = TypeIds.String;

    public const int Boolean = TypeIds.Boolean;

    public const int Date = TypeIds.Date;

    public const int Time = TypeIds.Time;

    public const int Datetime = TypeIds.Datetime;

    public const int Uuid = TypeIds.Uuid;

    /// <summary>The family of a declared type, <see cref="None"/> for a list.</summary>
    public static int Of(int type)
    {
        int name = type & ~TypeIds.NullableBit;
        return name switch
        {
            TypeIds.Integer => Decimal,
            >= TypeIds.Decimal and <= TypeIds.Uuid => name,
            _ => None,
        };
    }
}
