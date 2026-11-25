namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// What chooses the case of an operator beside its declared type (see
/// <see cref="Choice"/>): the family of the type an operand declares, where
/// <c>integer</c> belongs to the decimal family unless the operator has a case of its
/// own for it, or whether <c>digits</c> is present. The last part of a
/// <see cref="Signature"/>, which is <see cref="None"/> for any other node.
/// </summary>
internal static class Families
{
    public const int None = 0;

    public const int Integer = TypeIds.Integer;

    public const int Decimal = TypeIds.Decimal;

    public const int String = TypeIds.String;

    public const int Boolean = TypeIds.Boolean;

    public const int Date = TypeIds.Date;

    public const int Time = TypeIds.Time;

    public const int Datetime = TypeIds.Datetime;

    public const int Uuid = TypeIds.Uuid;

    public const int WithDigits = 64;

    public const int WithoutDigits = 65;

    /// <summary>
    /// The family of a type an operand of the operator declares, <see cref="None"/> for
    /// a list or no type.
    /// </summary>
    public static int Of(int kind, int type)
    {
        int name = type & ~TypeIds.NullableBit;
        return name switch
        {
            TypeIds.Integer => Kinds.HasIntegerFamily(kind) ? Integer : Decimal,
            >= TypeIds.Decimal and <= TypeIds.Uuid => name,
            _ => None,
        };
    }

    public static string Describe(int family)
    {
        return family switch
        {
            WithDigits => "with digits",
            WithoutDigits => "without digits",
            _ => $"of {TypeIds.Name(family)}",
        };
    }
}
