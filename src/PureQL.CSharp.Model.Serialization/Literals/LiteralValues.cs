using System.Globalization;
using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Literals;

/// <summary>
/// Literal formats of the specification, each with one spelling. Strings are checked
/// against the schema patterns (ASCII digits only, no surrounding whitespace), and a
/// value that the schema or the model's CLR type cannot hold exactly is rejected rather
/// than altered, on read and on write. Values are parsed and formatted on the stack.
/// </summary>
internal static class LiteralValues
{
    private const long TicksPerSecond = TimeSpan.TicksPerSecond;

    /// <summary>The schema holds times and datetimes to microseconds.</summary>
    private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

    private const int MaxDecimalDigits = 28;

    /// <summary>Longer strings are none of the formats.</summary>
    private const int MaxLength = 64;

    private delegate bool TryParse<T>(ReadOnlySpan<char> text, out T value);

    public static LiteralValue<long> Integer { get; } =
        new LiteralValue<long>(
            ReadInteger,
            (writer, value) => writer.WriteNumberValue(value)
        );

    public static LiteralValue<decimal> Decimal { get; } =
        new LiteralValue<decimal>(ReadDecimal, WriteDecimal);

    public static LiteralValue<string> String { get; } =
        new LiteralValue<string>(
            ReadString,
            (writer, value) => writer.WriteStringValue(value)
        );

    public static LiteralValue<bool> Boolean { get; } =
        new LiteralValue<bool>(
            ReadBoolean,
            (writer, value) => writer.WriteBooleanValue(value)
        );

    public static LiteralValue<DateOnly> Date { get; } =
        new LiteralValue<DateOnly>(ReadDate, WriteDate);

    public static LiteralValue<TimeOnly> Time { get; } =
        new LiteralValue<TimeOnly>(ReadTime, WriteTime);

    public static LiteralValue<DateTimeOffset> Datetime { get; } =
        new LiteralValue<DateTimeOffset>(ReadDatetime, WriteDatetime);

    public static LiteralValue<Guid> Uuid { get; } =
        new LiteralValue<Guid>(ReadUuid, WriteUuid);

    public static long ReadInteger(ref Utf8JsonReader reader, ReadContext context)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out long value))
            {
                return value;
            }

            // JSON Schema counts 5.0 as an integer.
            if (
                reader.TryGetDecimal(out decimal number)
                && (decimal.Truncate(number) == number)
                && (number >= long.MinValue)
                && (number <= long.MaxValue)
            )
            {
                return (long)number;
            }
        }

        throw context.Error("Expected a 64-bit integer");
    }

    private static decimal ReadDecimal(ref Utf8JsonReader reader, ReadContext context)
    {
        return Parse<decimal>(
            ref reader,
            context,
            TryParseDecimal,
            "a decimal (no exponent, no leading or trailing zeros, "
                + $"at most {MaxDecimalDigits} digits)"
        );
    }

    private static DateOnly ReadDate(ref Utf8JsonReader reader, ReadContext context)
    {
        return Parse<DateOnly>(
            ref reader,
            context,
            TryParseDate,
            "a date (yyyy-MM-dd, years 0001-9999)"
        );
    }

    private static TimeOnly ReadTime(ref Utf8JsonReader reader, ReadContext context)
    {
        return Parse<TimeOnly>(
            ref reader,
            context,
            TryParseTime,
            "a time (HH:mm:ss[.f], at most 6 fraction digits)"
        );
    }

    private static DateTimeOffset ReadDatetime(
        ref Utf8JsonReader reader,
        ReadContext context
    )
    {
        return Parse<DateTimeOffset>(
            ref reader,
            context,
            TryParseDatetime,
            "a datetime (yyyy-MM-ddTHH:mm:ss[.f]Z in UTC)"
        );
    }

    private static Guid ReadUuid(ref Utf8JsonReader reader, ReadContext context)
    {
        return Parse<Guid>(ref reader, context, TryParseUuid, "a uuid");
    }

    private static string ReadString(ref Utf8JsonReader reader, ReadContext context)
    {
        return reader.TokenType == JsonTokenType.String
            ? reader.GetString()!
            : throw context.Error(
                $"Expected a string, found {ReadContext.Describe(reader.TokenType)}"
            );
    }

    private static bool ReadBoolean(ref Utf8JsonReader reader, ReadContext context)
    {
        return reader.TokenType is JsonTokenType.True or JsonTokenType.False
            ? reader.GetBoolean()
            : throw context.Error(
                $"Expected a boolean, found {ReadContext.Describe(reader.TokenType)}"
            );
    }

    /// <summary>A value spelled as a string, parsed without allocating it.</summary>
    private static T Parse<T>(
        ref Utf8JsonReader reader,
        ReadContext context,
        TryParse<T> parse,
        string format
    )
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw context.Error(
                $"Expected a string, found {ReadContext.Describe(reader.TokenType)}"
            );
        }

        long length = reader.HasValueSequence
            ? reader.ValueSequence.Length
            : reader.ValueSpan.Length;
        if (length <= MaxLength)
        {
            Span<char> buffer = stackalloc char[MaxLength];
            int written = reader.CopyString(buffer);
            if (parse(buffer[..written], out T value))
            {
                return value;
            }
        }

        throw context.Error($"'{reader.GetString()}' is not {format}");
    }

    private static bool TryParseDecimal(ReadOnlySpan<char> text, out decimal value)
    {
        value = 0;
        return IsDecimal(text)
            && decimal.TryParse(
                text,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out value
            );
    }

    private static bool TryParseDate(ReadOnlySpan<char> text, out DateOnly date)
    {
        if (
            text.Length != 10
            || (text[4] != '-')
            || (text[7] != '-')
            || !TryDigits(text, 0, 4, out int year)
            || !TryDigits(text, 5, 2, out int month)
            || !TryDigits(text, 8, 2, out int day)
            || (year < 1)
            || (month is < 1 or > 12)
            || (day < 1)
            || (day > DateTime.DaysInMonth(year, month))
        )
        {
            date = default;
            return false;
        }

        date = new DateOnly(year, month, day);
        return true;
    }

    private static bool TryParseTime(ReadOnlySpan<char> text, out TimeOnly time)
    {
        bool parsed = TryParseTimeTicks(text, out long ticks);
        time = parsed ? new TimeOnly(ticks) : default;
        return parsed;
    }

    private static bool TryParseDatetime(
        ReadOnlySpan<char> text,
        out DateTimeOffset value
    )
    {
        if (
            (text.Length >= 20)
            && (text[10] == 'T')
            && (text[^1] == 'Z')
            && TryParseDate(text[..10], out DateOnly date)
            && TryParseTimeTicks(text[11..^1], out long ticks)
        )
        {
            value = new DateTimeOffset(
                date.ToDateTime(TimeOnly.MinValue).Ticks + ticks,
                TimeSpan.Zero
            );
            return true;
        }

        value = default;
        return false;
    }

    private static bool TryParseUuid(ReadOnlySpan<char> text, out Guid value)
    {
        value = default;
        if (text.Length != 36)
        {
            return false;
        }

        for (int i = 0; i < text.Length; i++)
        {
            bool hyphen = i is 8 or 13 or 18 or 23;
            if (hyphen ? (text[i] != '-') : !char.IsAsciiHexDigitLower(text[i]))
            {
                return false;
            }
        }

        value = Guid.ParseExact(text, "D");
        return true;
    }

    private static bool TryParseTimeTicks(ReadOnlySpan<char> text, out long ticks)
    {
        ticks = 0;
        if (
            text.Length < 8
            || (text[2] != ':')
            || (text[5] != ':')
            || !TryDigits(text, 0, 2, out int hours)
            || !TryDigits(text, 3, 2, out int minutes)
            || !TryDigits(text, 6, 2, out int seconds)
            || (hours > 23)
            || (minutes > 59)
            || (seconds > 59)
        )
        {
            return false;
        }

        long fraction = 0;
        if (text.Length > 8)
        {
            int digits = text.Length - 9;
            if (
                (text[8] != '.')
                || (digits < 1)
                || (digits > 6)
                || (text[^1] == '0')
                || !TryDigits(text, 9, digits, out int microseconds)
            )
            {
                return false;
            }

            fraction = microseconds;
            for (int i = digits; i < 6; i++)
            {
                fraction *= 10;
            }

            fraction *= TicksPerMicrosecond;
        }

        ticks =
            (((((hours * 60L) + minutes) * 60L) + seconds) * TicksPerSecond) + fraction;
        return true;
    }

    private static bool TryDigits(
        ReadOnlySpan<char> text,
        int start,
        int length,
        out int value
    )
    {
        value = 0;
        for (int i = start; i < (start + length); i++)
        {
            if (!char.IsAsciiDigit(text[i]))
            {
                return false;
            }

            value = (value * 10) + (text[i] - '0');
        }

        return true;
    }

    private static void WriteDate(Utf8JsonWriter writer, DateOnly value)
    {
        Span<char> text = stackalloc char[10];
        FormatDate(value, text);
        writer.WriteStringValue(text);
    }

    private static void WriteTime(Utf8JsonWriter writer, TimeOnly value)
    {
        Span<char> text = stackalloc char[15];
        writer.WriteStringValue(text[..FormatTime(value.Ticks, text)]);
    }

    private static void WriteDatetime(Utf8JsonWriter writer, DateTimeOffset value)
    {
        DateTime utc = value.UtcDateTime;
        Span<char> text = stackalloc char[27];
        FormatDate(DateOnly.FromDateTime(utc), text);
        text[10] = 'T';
        int length = 11 + FormatTime(utc.TimeOfDay.Ticks, text[11..]);
        text[length] = 'Z';
        writer.WriteStringValue(text[..(length + 1)]);
    }

    private static void WriteUuid(Utf8JsonWriter writer, Guid value)
    {
        Span<char> text = stackalloc char[36];
        _ = value.TryFormat(text, out _, "D");
        writer.WriteStringValue(text);
    }

    /// <summary>
    /// The spelling of <c>value.decimal</c>: no exponent, no trailing zeros, no negative
    /// zero.
    /// </summary>
    private static void WriteDecimal(Utf8JsonWriter writer, decimal value)
    {
        Span<char> buffer = stackalloc char[MaxLength];
        _ = value.TryFormat(buffer, out int length, default, CultureInfo.InvariantCulture);
        ReadOnlySpan<char> text = buffer[..length];
        if (text.Contains('.'))
        {
            text = text.TrimEnd('0').TrimEnd('.');
        }

        text = text is "-0" ? "0" : text;
        if (!IsDecimal(text))
        {
            throw new JsonException($"The decimal {text} has more than 28 digits");
        }

        writer.WriteStringValue(text);
    }

    private static void FormatDate(DateOnly value, Span<char> text)
    {
        _ = value.TryFormat(text, out _, "yyyy'-'MM'-'dd", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes <c>HH:mm:ss[.f]</c> to <paramref name="text"/> and returns its length.
    /// </summary>
    private static int FormatTime(long ticks, Span<char> text)
    {
        long fraction = ticks % TicksPerSecond;
        if ((fraction % TicksPerMicrosecond) != 0)
        {
            throw new JsonException(
                "A time or datetime finer than a microsecond cannot be written"
            );
        }

        _ = new TimeOnly(ticks).TryFormat(
            text,
            out _,
            "HH':'mm':'ss",
            CultureInfo.InvariantCulture
        );
        if (fraction == 0)
        {
            return 8;
        }

        text[8] = '.';
        _ = (fraction / TicksPerMicrosecond).TryFormat(
            text[9..],
            out _,
            "D6",
            CultureInfo.InvariantCulture
        );
        return 9 + text.Slice(9, 6).TrimEnd('0').Length;
    }

    /// <summary>
    /// The spelling of <c>value.decimal</c>: no exponent, no leading or trailing zeros,
    /// no negative zero, and at most 28 digits.
    /// </summary>
    private static bool IsDecimal(ReadOnlySpan<char> text)
    {
        int start = text.StartsWith('-') ? 1 : 0;
        int dot = text.IndexOf('.');
        ReadOnlySpan<char> whole = dot < 0 ? text[start..] : text[start..dot];
        ReadOnlySpan<char> fraction = dot < 0 ? [] : text[(dot + 1)..];
        bool isZero = whole is "0";
        bool valid =
            (whole.Length > 0)
            && !whole.ContainsAnyExceptInRange('0', '9')
            && !fraction.ContainsAnyExceptInRange('0', '9')
            && (isZero || (whole[0] != '0'))
            && ((dot < 0) || ((fraction.Length > 0) && (fraction[^1] != '0')))
            && !(isZero && (dot < 0) && (start == 1));
        return valid
            && ((isZero ? 0 : whole.Length) + fraction.Length <= MaxDecimalDigits);
    }
}
