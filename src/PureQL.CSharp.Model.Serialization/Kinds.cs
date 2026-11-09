using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// What a JSON node is: an operator, named by its <c>operator</c>, or a leaf, told
/// apart by the property only that leaf has (<c>source</c>, <c>param</c>, <c>key</c>,
/// <c>subquery</c>, <c>value</c>). The first part of a <see cref="Signature"/>.
/// </summary>
internal static class Kinds
{
    public const int Unknown = 0;

    public const int Add = 1;

    public const int Subtract = 2;

    public const int Multiply = 3;

    public const int Divide = 4;

    public const int IntegerDivide = 5;

    public const int Modulo = 6;

    public const int Floor = 7;

    public const int Ceiling = 8;

    public const int Round = 9;

    public const int Concat = 10;

    public const int DateAddDays = 11;

    public const int DateDiffDays = 12;

    public const int TimeAddSeconds = 13;

    public const int TimeDiffSeconds = 14;

    public const int DatetimeAddSeconds = 15;

    public const int DatetimeDiffSeconds = 16;

    public const int And = 17;

    public const int Or = 18;

    public const int Not = 19;

    public const int Equal = 20;

    public const int NotEqual = 21;

    public const int In = 22;

    public const int GreaterThan = 23;

    public const int LessThan = 24;

    public const int GreaterThanOrEqual = 25;

    public const int LessThanOrEqual = 26;

    public const int If = 27;

    public const int Coalesce = 28;

    public const int Count = 29;

    public const int Sum = 30;

    public const int Average = 31;

    public const int Min = 32;

    public const int Max = 33;

    public const int Any = 34;

    public const int All = 35;

    public const int Field = 40;

    public const int Param = 41;

    public const int Key = 42;

    public const int Column = 43;

    public const int Literal = 44;

    /// <summary>The <c>operator</c> values, indexed by kind.</summary>
    private static readonly string[] Operators =
    [
        "",
        "add",
        "subtract",
        "multiply",
        "divide",
        "integerDivide",
        "modulo",
        "floor",
        "ceiling",
        "round",
        "concat",
        "dateAddDays",
        "dateDiffDays",
        "timeAddSeconds",
        "timeDiffSeconds",
        "datetimeAddSeconds",
        "datetimeDiffSeconds",
        "and",
        "or",
        "not",
        "equal",
        "notEqual",
        "in",
        "greaterThan",
        "lessThan",
        "greaterThanOrEqual",
        "lessThanOrEqual",
        "if",
        "coalesce",
        "count",
        "sum",
        "average",
        "min",
        "max",
        "any",
        "all",
    ];

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
        return kind is >= Add and <= All;
    }

    /// <summary>
    /// Whether the operator is chosen by the type family of an operand as well:
    /// <c>equal</c>, <c>notEqual</c> and the comparisons by <c>left</c>, <c>in</c> by
    /// <c>operand</c>.
    /// </summary>
    public static bool IsComparison(int kind)
    {
        return kind is >= Equal and <= LessThanOrEqual;
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
