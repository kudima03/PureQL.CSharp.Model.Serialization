using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// What a JSON node is: an operator, named by its <c>operator</c>, or a leaf, told
/// apart by the property only that leaf has (<c>source</c>, <c>param</c>, <c>key</c>,
/// <c>subquery</c>, <c>value</c>). The first part of a <see cref="Signature"/>. The
/// operator kinds, from 1 to <see cref="LastOperator"/>, are generated
/// (<c>Kinds.g.cs</c>) in the order of the specification.
/// </summary>
internal static partial class Kinds
{
    public const int Unknown = 0;

    public const int Field = 200;

    public const int Param = 201;

    public const int Key = 202;

    public const int Column = 203;

    public const int Literal = 204;

    private static readonly JsonEncodedText[] EncodedOperators =
    [
        .. Operators.Select(name => JsonEncodedText.Encode(name)),
    ];

    private static readonly NameTable Table = new NameTable(Operators[1..], 1);

    /// <summary>The operator kind named by a JSON string, or <see cref="Unknown"/>.</summary>
    public static int Operator(ref Utf8JsonReader reader)
    {
        return Table.Find(ref reader);
    }

    public static bool IsOperator(int kind)
    {
        return kind is >= 1 and <= LastOperator;
    }

    /// <summary>
    /// What chooses the case of an operator beside its declared type: the family of the
    /// type <c>left</c> declares (<c>equal</c>, <c>notEqual</c>, the comparisons), that
    /// of <c>operand</c> (<c>in</c>, <c>year</c>, <c>asString</c>, …), or whether
    /// <c>digits</c> is present (<c>round</c>, <c>truncate</c>).
    /// </summary>
    public static Choice ChoiceOf(int kind)
    {
        return IsOperator(kind) ? Choices[kind] : Choice.None;
    }

    /// <summary>
    /// Whether an <c>integer</c> operand is a family of its own for the operator
    /// (<c>asDecimal</c>), rather than of the decimal family.
    /// </summary>
    public static bool HasIntegerFamily(int kind)
    {
        return IntegerFamilies.Contains(kind);
    }

    public static string Name(int kind)
    {
        return kind switch
        {
            Field => "field",
            Param => "param",
            Key => "key",
            Column => "column",
            Literal => "literal",
            _ => Operators[kind],
        };
    }

    /// <summary>The <c>operator</c> value of an operator kind, encoded once.</summary>
    public static JsonEncodedText Encoded(int kind)
    {
        return EncodedOperators[kind];
    }
}
