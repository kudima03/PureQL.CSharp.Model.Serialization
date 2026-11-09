using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The declared types of the specification, <c>{ "name": …, "nullable": true }</c>:
/// a name id, with <see cref="NullableBit"/> for a nullable type. The second part of a
/// <see cref="Signature"/>.
/// </summary>
internal static class TypeIds
{
    /// <summary>No <c>type</c> property.</summary>
    public const int None = 0;

    public const int Integer = 1;

    public const int Decimal = 2;

    public const int String = 3;

    public const int Boolean = 4;

    public const int Date = 5;

    public const int Time = 6;

    public const int Datetime = 7;

    public const int Uuid = 8;

    public const int IntegerList = 9;

    public const int DecimalList = 10;

    public const int StringList = 11;

    public const int BooleanList = 12;

    public const int DateList = 13;

    public const int TimeList = 14;

    public const int DatetimeList = 15;

    public const int UuidList = 16;

    /// <summary>A type whose name is none of the specification's.</summary>
    public const int Unknown = 31;

    public const int NullableBit = 32;

    public const int IntegerNullable = Integer | NullableBit;

    public const int DecimalNullable = Decimal | NullableBit;

    public const int StringNullable = String | NullableBit;

    public const int BooleanNullable = Boolean | NullableBit;

    public const int DateNullable = Date | NullableBit;

    public const int TimeNullable = Time | NullableBit;

    public const int DatetimeNullable = Datetime | NullableBit;

    public const int UuidNullable = Uuid | NullableBit;

    /// <summary>The type names, indexed by name id.</summary>
    private static readonly string[] Names =
    [
        "",
        "integer",
        "decimal",
        "string",
        "boolean",
        "date",
        "time",
        "datetime",
        "uuid",
        "integerList",
        "decimalList",
        "stringList",
        "booleanList",
        "dateList",
        "timeList",
        "datetimeList",
        "uuidList",
    ];

    private static readonly JsonEncodedText[] EncodedNames =
    [
        .. Names.Select(name => JsonEncodedText.Encode(name)),
    ];

    private static readonly NameTable Table = new NameTable(Names[1..], 1);

    /// <summary>The name id a JSON string spells, or <see cref="Unknown"/>.</summary>
    public static int Find(ref Utf8JsonReader reader)
    {
        int id = Table.Find(ref reader);
        return id == 0 ? Unknown : id;
    }

    public static bool IsNullable(int type)
    {
        return (type & NullableBit) != 0;
    }

    public static string Name(int type)
    {
        int name = type & ~NullableBit;
        return name < Names.Length ? Names[name] : "unknown";
    }

    public static JsonEncodedText EncodedName(int type)
    {
        return EncodedNames[type & ~NullableBit];
    }

    public static string Describe(int type)
    {
        return IsNullable(type) ? $"{Name(type)}?" : Name(type);
    }
}
