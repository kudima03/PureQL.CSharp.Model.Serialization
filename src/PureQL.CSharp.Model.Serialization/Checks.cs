using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Serialization.Literals;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The rules of the schema on values that the model's types do not express, checked
/// on read and on write. Each check gives the message of the rule a value breaks, or
/// <see langword="null"/> when it keeps them all.
/// </summary>
internal static partial class Checks
{
    /// <summary>A pattern character of its own (<c>REGEX_ATOM</c> of the schema).</summary>
    private const string RegexAtom =
        @"[^\\()\[\]{}*+?|^$.]"
        + @"|\\([dDwWsSbBnrt]|[\\()\[\]{}*+?|^$./])"
        + @"|\[\^?([^\]\[\\^]|\\([dDwWsSnrt]|[\\()\[\]{}*+?|^$./-]))+\]"
        + @"|\.|\)";

    private const string RegexQuantifier = @"([*+?]|\{[0-9]+(,[0-9]*)?\})\??";

    /// <summary>
    /// A literal regular expression, token by token (<c>REGEX</c> of the schema): what
    /// a token pattern cannot see, such as balanced parentheses, is left to the
    /// interpreter.
    /// </summary>
    private const string RegexDialect =
        "^((" + RegexAtom + ")(" + RegexQuantifier + @")?|\((\?:)?|[|^$])*$";

    /// <summary>
    /// The schema's <c>NAME</c>: not empty, no leading or trailing space or tab, no line
    /// break.
    /// </summary>
    public static string? Name(string name)
    {
        return (name.Length > 0)
            && !IsBlank(name[0])
            && !IsBlank(name[^1])
            && !name.AsSpan().ContainsAny('\n', '\r')
            ? null
            : $"'{name}' is not a name: it is empty, starts or ends with a space, a tab "
                + "or a line break, or holds a line break";
    }

    /// <summary>
    /// A bare number of rows, levels or units (<c>skip</c>, <c>take</c>,
    /// <c>maxDepth</c>, a side of a window): from 1.
    /// </summary>
    public static string? Count(long count)
    {
        return count >= 1 ? null : $"Expected a number from 1, found {count}";
    }

    /// <summary>
    /// The <c>offset</c> of <c>previous</c> / <c>next</c>: from 2, as one row away is
    /// its absence.
    /// </summary>
    public static string? Offset(Offset value)
    {
        return value.Value is LiteralInteger { Value: < 2 } literal
            ? $"An offset is from 2, found {literal.Value}"
            : null;
    }

    /// <summary>The <c>buckets</c> of <c>ntile</c>: from 1.</summary>
    public static string? Positive(Buckets value)
    {
        return Positive(value.Value);
    }

    /// <summary>The capture <c>group</c> of <c>extractRegex</c>: from 1.</summary>
    public static string? Positive(RegexGroup value)
    {
        return Positive(value.Value);
    }

    /// <summary>The <c>fraction</c> of a percentile: from 0 to 1.</summary>
    public static string? Fraction(Fraction value)
    {
        return value.Value is LiteralDecimal { Value: < 0 or > 1 } literal
            ? $"A fraction is from 0 to 1, found {literal.Value}"
            : null;
    }

    /// <summary>A literal regular expression, in the dialect of the specification.</summary>
    public static string? Regex(RegexPattern value)
    {
        return
            value.Value is LiteralString literal
            && (literal.Value.Contains('\n', StringComparison.Ordinal)
                || !Dialect().IsMatch(literal.Value))
            ? $"'{literal.Value}' is not a regular expression of the specification's "
                + "dialect"
            : null;
    }

    /// <summary>A distance of a range window in a number type: more than 0.</summary>
    public static string? Distance(DistanceDecimal value)
    {
        return Distance(value.Value);
    }

    /// <summary>A distance of a range window in seconds: more than 0.</summary>
    public static string? Distance(DistanceDatetimeSecondsSeconds value)
    {
        return Distance(value.Value);
    }

    /// <summary>
    /// A literal <c>zone</c>: the current name of a time zone (see
    /// <see cref="ZoneNames"/>). A zone computed for the row is checked when it is
    /// read.
    /// </summary>
    public static string? Zone<TUnion>(TUnion? value)
        where TUnion : struct, IUnion
    {
        return
            value.GetValueOrDefault().Value is LiteralString literal
            && !ZoneNames.All.Contains(literal.Value)
            ? $"'{literal.Value}' is not the name of a time zone"
            : null;
    }

    /// <summary>
    /// The sides of a frame or a range: not both <c>"all"</c>, as the whole partition
    /// is a window without them.
    /// </summary>
    public static string? Sides<TUnion>(TUnion? preceding, TUnion? following)
        where TUnion : struct, IUnion
    {
        return
            preceding.GetValueOrDefault().Value is All
            && following.GetValueOrDefault().Value is All
            ? "A window has no \"all\" on both sides: the whole partition is a window "
                + "without them"
            : null;
    }

    private static string? Positive(object? value)
    {
        return value is LiteralInteger { Value: < 1 } literal
            ? $"Expected a number from 1, found {literal.Value}"
            : null;
    }

    private static string? Distance(object? value)
    {
        return value is LiteralDecimal { Value: <= 0 } literal
            ? $"A distance is more than 0, found {literal.Value}"
            : null;
    }

    private static bool IsBlank(char c)
    {
        return c is ' ' or '\t' or '\n' or '\r';
    }

    [GeneratedRegex(RegexDialect, RegexOptions.CultureInvariant)]
    private static partial Regex Dialect();
}
