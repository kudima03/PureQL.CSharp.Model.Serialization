using System.Globalization;
using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Literals;

/// <summary>
/// Literal formats of the specification. Strings are checked against the schema
/// patterns (ASCII digits only, no surrounding whitespace), and a value the model's CLR
/// type cannot hold exactly is rejected rather than altered.
/// </summary>
internal static class LiteralValues
{
    private const long TicksPerSecond = TimeSpan.TicksPerSecond;

    private static readonly TimeSpan MaxClrOffset = TimeSpan.FromHours(14);

    public static LiteralValue<long> Integer { get; } =
        new LiteralValue<long>(
            ReadInteger,
            (writer, value) => writer.WriteNumberValue(value)
        );

    public static LiteralValue<decimal> Decimal { get; } =
        new LiteralValue<decimal>(
            ReadDecimal,
            (writer, value) => writer.WriteNumberValue(value)
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
        return
            element.ValueKind == JsonValueKind.Number
            && element.TryGetDecimal(out decimal value)
            ? value
            : throw JsonErrors.At(path, "Expected a number that fits System.Decimal");
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
                $"'{text}' is not a time (HH:mm:ss[.f] to 100 ns)"
            )
            : new TimeOnly(ticks);
    }

    private static DateTimeOffset ParseDatetime(string text, string path)
    {
        if (
            text.Length < 20
            || (text[10] != 'T')
            || !TryParseDate(text[..10], out DateOnly date)
            || !TrySplitOffset(text[11..], out string time, out TimeSpan offset)
            || !TryParseTimeTicks(time, out long ticks)
        )
        {
            throw JsonErrors.At(
                path,
                $"'{text}' is not a datetime (yyyy-MM-ddTHH:mm:ss[.f] with Z or ±hh:mm)"
            );
        }

        long localTicks = date.ToDateTime(TimeOnly.MinValue).Ticks + ticks;
        long utcTicks = localTicks - offset.Ticks;
        if ((utcTicks < 0) || (utcTicks > DateTime.MaxValue.Ticks))
        {
            throw JsonErrors.At(path, $"'{text}' is outside the range of DateTimeOffset");
        }

        // The offset is notation only: an offset beyond ±14:00, which DateTimeOffset
        // cannot carry, keeps the instant and is written in UTC.
        return offset.Duration() <= MaxClrOffset
            ? new DateTimeOffset(localTicks, offset)
            : new DateTimeOffset(utcTicks, TimeSpan.Zero);
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
            if (hyphen ? (text[i] != '-') : !char.IsAsciiHexDigit(text[i]))
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
            if ((text[8] != '.') || (digits < 1) || (digits > 9))
            {
                return false;
            }

            for (int i = 0; i < digits; i++)
            {
                char digit = text[9 + i];
                if (!char.IsAsciiDigit(digit) || ((i >= 7) && (digit != '0')))
                {
                    // Beyond 100 ns, the precision of TimeOnly and DateTimeOffset.
                    return false;
                }

                if (i < 7)
                {
                    fraction = (fraction * 10) + (digit - '0');
                }
            }

            for (int i = digits; i < 7; i++)
            {
                fraction *= 10;
            }
        }

        ticks =
            (((((hours * 60L) + minutes) * 60L) + seconds) * TicksPerSecond) + fraction;
        return true;
    }

    private static bool TrySplitOffset(string text, out string time, out TimeSpan offset)
    {
        time = text;
        offset = TimeSpan.Zero;
        if (text.EndsWith('Z'))
        {
            time = text[..^1];
            return true;
        }

        // The caller guarantees at least 9 characters.
        string suffix = text[^6..];
        if (
            (suffix[0] is not ('+' or '-'))
            || (suffix[3] != ':')
            || !TryDigits(suffix, 1, 2, out int hours)
            || !TryDigits(suffix, 4, 2, out int minutes)
            || (hours > 23)
            || (minutes > 59)
            || ((suffix[0] == '-') && (hours == 0) && (minutes == 0))
        )
        {
            return false;
        }

        time = text[..^6];
        offset = new TimeSpan(hours, minutes, 0);
        offset = suffix[0] == '-' ? offset.Negate() : offset;
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
        TimeOnly time = new TimeOnly(ticks);
        string text = time.ToString("HH':'mm':'ss", CultureInfo.InvariantCulture);
        long fraction = ticks % TicksPerSecond;
        string digits = fraction
            .ToString("D7", CultureInfo.InvariantCulture)
            .TrimEnd('0');
        return fraction == 0 ? text : $"{text}.{digits}";
    }

    private static string FormatDatetime(DateTimeOffset value)
    {
        string text =
            $"{FormatDate(DateOnly.FromDateTime(value.DateTime))}T"
            + FormatTime(value.DateTime.TimeOfDay.Ticks);
        TimeSpan offset = value.Offset;
        if (offset == TimeSpan.Zero)
        {
            return $"{text}Z";
        }

        char sign = offset < TimeSpan.Zero ? '-' : '+';
        TimeSpan duration = offset.Duration();
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{text}{sign}{duration.Hours:D2}:{duration.Minutes:D2}"
        );
    }
}
