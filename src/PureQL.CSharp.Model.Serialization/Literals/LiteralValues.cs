using System.Globalization;
using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Literals;

/// <summary>
/// Literal formats of the specification, each with one spelling. Strings are checked
/// against the schema patterns (ASCII digits only, no surrounding whitespace), and a
/// value that the schema or the model's CLR type cannot hold exactly is rejected rather
/// than altered, on read and on write.
/// </summary>
internal static class LiteralValues
{
    private const long TicksPerSecond = TimeSpan.TicksPerSecond;

    /// <summary>The schema holds times and datetimes to microseconds.</summary>
    private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

    private const int MaxDecimalDigits = 28;

    public static LiteralValue<long> Integer { get; } =
        new LiteralValue<long>(
            ReadInteger,
            (writer, value) => writer.WriteNumberValue(value)
        );

    public static LiteralValue<decimal> Decimal { get; } =
        new LiteralValue<decimal>(
            ReadDecimal,
            (writer, value) => writer.WriteStringValue(FormatDecimal(value))
        );

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
        new LiteralValue<DateOnly>(
            (element, path) => ParseDate(ReadString(element, path), path),
            (writer, value) => writer.WriteStringValue(FormatDate(value))
        );

    public static LiteralValue<TimeOnly> Time { get; } =
        new LiteralValue<TimeOnly>(
            (element, path) => ParseTime(ReadString(element, path), path),
            (writer, value) => writer.WriteStringValue(FormatTime(value.Ticks))
        );

    public static LiteralValue<DateTimeOffset> Datetime { get; } =
        new LiteralValue<DateTimeOffset>(
            (element, path) => ParseDatetime(ReadString(element, path), path),
            (writer, value) => writer.WriteStringValue(FormatDatetime(value))
        );

    public static LiteralValue<Guid> Uuid { get; } =
        new LiteralValue<Guid>(
            (element, path) => ParseUuid(ReadString(element, path), path),
            (writer, value) =>
                writer.WriteStringValue(value.ToString("D", CultureInfo.InvariantCulture))
        );

    public static long ReadInteger(JsonElement element, string path)
    {
        if (element.ValueKind == JsonValueKind.Number)
        {
            if (element.TryGetInt64(out long value))
            {
                return value;
            }

            // JSON Schema counts 5.0 as an integer.
            if (
                element.TryGetDecimal(out decimal number)
                && (decimal.Truncate(number) == number)
                && (number >= long.MinValue)
                && (number <= long.MaxValue)
            )
            {
                return (long)number;
            }
        }

        throw JsonErrors.At(path, "Expected a 64-bit integer");
    }

    private static decimal ReadDecimal(JsonElement element, string path)
    {
        string text = ReadString(element, path);
        return IsDecimal(text)
            ? decimal.Parse(
                text,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture
            )
            : throw JsonErrors.At(
                path,
                $"'{text}' is not a decimal (no exponent, no leading or trailing zeros, "
                    + $"at most {MaxDecimalDigits} digits)"
            );
    }

    private static string ReadString(JsonElement element, string path)
    {
        return element.ValueKind == JsonValueKind.String
            ? element.GetString()!
            : throw JsonErrors.At(path, $"Expected a string, found {element.ValueKind}");
    }

    private static bool ReadBoolean(JsonElement element, string path)
    {
        return element.ValueKind is JsonValueKind.True or JsonValueKind.False
            ? element.GetBoolean()
            : throw JsonErrors.At(path, $"Expected a boolean, found {element.ValueKind}");
    }

    private static DateOnly ParseDate(string text, string path)
    {
        return !TryParseDate(text, out DateOnly date)
            ? throw JsonErrors.At(
                path,
                $"'{text}' is not a date (yyyy-MM-dd, years 0001-9999)"
            )
            : date;
    }

    private static TimeOnly ParseTime(string text, string path)
    {
        return !TryParseTimeTicks(text, out long ticks)
            ? throw JsonErrors.At(
                path,
                $"'{text}' is not a time (HH:mm:ss[.f], at most 6 fraction digits)"
            )
            : new TimeOnly(ticks);
    }

    private static DateTimeOffset ParseDatetime(string text, string path)
    {
        return
            (text.Length >= 20)
            && (text[10] == 'T')
            && (text[^1] == 'Z')
            && TryParseDate(text[..10], out DateOnly date)
            && TryParseTimeTicks(text[11..^1], out long ticks)
            ? new DateTimeOffset(
                date.ToDateTime(TimeOnly.MinValue).Ticks + ticks,
                TimeSpan.Zero
            )
            : throw JsonErrors.At(
                path,
                $"'{text}' is not a datetime (yyyy-MM-ddTHH:mm:ss[.f]Z in UTC)"
            );
    }

    private static Guid ParseUuid(string text, string path)
    {
        if (text.Length != 36)
        {
            throw JsonErrors.At(path, $"'{text}' is not a uuid");
        }

        for (int i = 0; i < text.Length; i++)
        {
            bool hyphen = i is 8 or 13 or 18 or 23;
            if (hyphen ? (text[i] != '-') : !char.IsAsciiHexDigitLower(text[i]))
            {
                throw JsonErrors.At(path, $"'{text}' is not a uuid");
            }
        }

        return Guid.ParseExact(text, "D");
    }

    private static bool TryParseDate(string text, out DateOnly date)
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

    private static bool TryParseTimeTicks(string text, out long ticks)
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

    private static bool TryDigits(string text, int start, int length, out int value)
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

    private static string FormatDate(DateOnly value)
    {
        return value.ToString("yyyy'-'MM'-'dd", CultureInfo.InvariantCulture);
    }

    private static string FormatTime(long ticks)
    {
        long fraction = ticks % TicksPerSecond;
        if ((fraction % TicksPerMicrosecond) != 0)
        {
            throw new JsonException(
                "A time or datetime finer than a microsecond cannot be written"
            );
        }

        TimeOnly time = new TimeOnly(ticks);
        string text = time.ToString("HH':'mm':'ss", CultureInfo.InvariantCulture);
        string digits = (fraction / TicksPerMicrosecond)
            .ToString("D6", CultureInfo.InvariantCulture)
            .TrimEnd('0');
        return fraction == 0 ? text : $"{text}.{digits}";
    }

    private static string FormatDatetime(DateTimeOffset value)
    {
        DateTime utc = value.UtcDateTime;
        return $"{FormatDate(DateOnly.FromDateTime(utc))}T"
            + $"{FormatTime(utc.TimeOfDay.Ticks)}Z";
    }

    /// <summary>
    /// The spelling of <c>value.decimal</c>: no exponent, no leading or trailing zeros,
    /// no negative zero, and at most 28 digits.
    /// </summary>
    private static bool IsDecimal(string text)
    {
        int start = text.StartsWith('-') ? 1 : 0;
        int dot = text.IndexOf('.', StringComparison.Ordinal);
        string whole = dot < 0 ? text[start..] : text[start..dot];
        string fraction = dot < 0 ? string.Empty : text[(dot + 1)..];
        bool isZero = whole == "0";
        bool valid =
            (whole.Length > 0)
            && whole.All(char.IsAsciiDigit)
            && fraction.All(char.IsAsciiDigit)
            && (isZero || (whole[0] != '0'))
            && ((dot < 0) || ((fraction.Length > 0) && (fraction[^1] != '0')))
            && !(isZero && (dot < 0) && (start == 1))
            && !(isZero && (dot >= 0) && (fraction.Length == 0));
        return valid
            && ((isZero ? 0 : whole.Length) + fraction.Length <= MaxDecimalDigits);
    }

    private static string FormatDecimal(decimal value)
    {
        string text = value.ToString(CultureInfo.InvariantCulture);
        if (text.Contains('.', StringComparison.Ordinal))
        {
            text = text.TrimEnd('0').TrimEnd('.');
        }

        text = text == "-0" ? "0" : text;
        return IsDecimal(text)
            ? text
            : throw new JsonException($"The decimal {text} has more than 28 digits");
    }
}
