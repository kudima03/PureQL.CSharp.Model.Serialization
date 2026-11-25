using System.Text.Json;
using PureQL.CSharp.Model.Serialization.Literals;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Writes the properties records are made of. Values equal to the schema default
/// (<c>nullable: false</c>, the default <c>over</c>, <c>direction</c> and
/// <c>mode</c>, a <c>false</c> flag) and absent optional properties are omitted; a
/// missing required value, or one the schema does not accept, is a
/// <see cref="JsonException"/>.
/// </summary>
internal static class Writes
{
    private static readonly JsonEncodedText OperatorName = JsonEncodedText.Encode(
        "operator"
    );

    private static readonly JsonEncodedText TypeName = JsonEncodedText.Encode("type");

    private static readonly JsonEncodedText NameName = JsonEncodedText.Encode("name");

    private static readonly JsonEncodedText NullableName = JsonEncodedText.Encode(
        "nullable"
    );

    private static readonly JsonEncodedText ValueName = JsonEncodedText.Encode("value");

    private static readonly JsonEncodedText OverName = JsonEncodedText.Encode("over");

    private static readonly JsonEncodedText All = JsonEncodedText.Encode("all");

    private static readonly JsonEncodedText DirectionName = JsonEncodedText.Encode(
        "direction"
    );

    private static readonly JsonEncodedText Desc = JsonEncodedText.Encode("desc");

    private static readonly JsonEncodedText KindName = JsonEncodedText.Encode("kind");

    private static readonly JsonEncodedText HalfEven = JsonEncodedText.Encode("halfEven");

    private static readonly JsonEncodedText[] Forms =
    [
        JsonEncodedText.Encode("NFC"),
        JsonEncodedText.Encode("NFD"),
        JsonEncodedText.Encode("NFKC"),
        JsonEncodedText.Encode("NFKD"),
    ];

    private static readonly JsonEncodedText[] JoinKinds =
    [
        JsonEncodedText.Encode("inner"),
        JsonEncodedText.Encode("left"),
        JsonEncodedText.Encode("right"),
        JsonEncodedText.Encode("full"),
    ];

    /// <summary>A record of any model type, by its runtime type.</summary>
    public static void Record(Utf8JsonWriter writer, object? value)
    {
        ModelRegistry.Instance.Write(writer, value);
    }

    /// <summary>
    /// The case a union holds: a record, or a bare number of rows, levels or units.
    /// </summary>
    public static void Case(Utf8JsonWriter writer, object? value)
    {
        if (value is long count)
        {
            Check(Checks.Count(count));
            writer.WriteNumberValue(count);
        }
        else
        {
            Record(writer, value);
        }
    }

    public static T NotNull<T>(T? value)
        where T : class
    {
        return value ?? throw Missing();
    }

    /// <summary>Fails the write with the message of a check that failed.</summary>
    public static void Check(string? error)
    {
        if (error is not null)
        {
            throw new JsonException(error);
        }
    }

    /// <summary>
    /// Fails the write of a record whose properties are all optional when none is set,
    /// as the schema requires one.
    /// </summary>
    public static void AtLeastOne(bool any)
    {
        if (!any)
        {
            throw new JsonException("A PureQL model value has none of the properties JSON requires one of");
        }
    }

    public static JsonException Missing()
    {
        return new JsonException("A PureQL model value is null where JSON requires one");
    }

    public static void Operator(Utf8JsonWriter writer, int kind)
    {
        writer.WriteString(OperatorName, Kinds.Encoded(kind));
    }

    public static void Type(Utf8JsonWriter writer, int type)
    {
        writer.WriteStartObject(TypeName);
        writer.WriteString(NameName, TypeIds.EncodedName(type));
        if (TypeIds.IsNullable(type))
        {
            writer.WriteBoolean(NullableName, true);
        }

        writer.WriteEndObject();
    }

    public static void Optional<T>(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        T? value,
        Action<Utf8JsonWriter, T> write
    )
        where T : struct
    {
        if (value.HasValue)
        {
            writer.WritePropertyName(name);
            write(writer, value.GetValueOrDefault());
        }
    }

    public static void Many<T>(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        IEnumerable<T>? values,
        Action<Utf8JsonWriter, T> write
    )
    {
        writer.WritePropertyName(name);
        Array(writer, values, write);
    }

    /// <summary>A list, the value of a property or an item of another list.</summary>
    public static void Array<T>(
        Utf8JsonWriter writer,
        IEnumerable<T>? values,
        Action<Utf8JsonWriter, T> write
    )
    {
        writer.WriteStartArray();
        foreach (T value in values ?? throw Missing())
        {
            write(writer, value);
        }

        writer.WriteEndArray();
    }

    public static void OptionalMany<T>(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        IEnumerable<T>? values,
        Action<Utf8JsonWriter, T> write
    )
    {
        if (values is not null)
        {
            Many(writer, name, values, write);
        }
    }

    /// <summary>A name, which the schema's <c>NAME</c> pattern restricts.</summary>
    public static void String(Utf8JsonWriter writer, JsonEncodedText name, string? value)
    {
        Check(Checks.Name(value ?? throw Missing()));
        writer.WriteString(name, value);
    }

    public static void OptionalString(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        string? value
    )
    {
        if (value is not null)
        {
            String(writer, name, value);
        }
    }

    public static void Strings(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        IEnumerable<string>? values
    )
    {
        writer.WriteStartArray(name);
        foreach (string? value in values ?? throw Missing())
        {
            Check(Checks.Name(value ?? throw Missing()));
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }

    public static void OptionalStrings(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        IEnumerable<string>? values
    )
    {
        if (values is not null)
        {
            Strings(writer, name, values);
        }
    }

    public static void Value<T>(Utf8JsonWriter writer, LiteralValue<T> literal, T value)
    {
        writer.WritePropertyName(ValueName);
        literal.Write(writer, value);
    }

    public static void Values<T>(
        Utf8JsonWriter writer,
        LiteralValue<T> literal,
        IEnumerable<T>? values
    )
    {
        writer.WriteStartArray(ValueName);
        foreach (T value in values ?? throw Missing())
        {
            literal.Write(writer, value);
        }

        writer.WriteEndArray();
    }

    /// <summary>The <c>value</c> of a nullable literal: <c>null</c>.</summary>
    public static void Null(Utf8JsonWriter writer)
    {
        writer.WriteNull(ValueName);
    }

    public static void Over(Utf8JsonWriter writer, AggregateOver over)
    {
        if (over == AggregateOver.All)
        {
            writer.WriteString(OverName, All);
        }
    }

    public static void Direction(Utf8JsonWriter writer, SortDirection direction)
    {
        if (direction == SortDirection.Desc)
        {
            writer.WriteString(DirectionName, Desc);
        }
    }

    public static void Mode(Utf8JsonWriter writer, JsonEncodedText name, RoundingMode mode)
    {
        if (mode == RoundingMode.HalfEven)
        {
            writer.WriteString(name, HalfEven);
        }
        else if (mode != RoundingMode.HalfAwayFromZero)
        {
            throw new JsonException($"Unknown rounding mode {mode}");
        }
    }

    public static void Form(Utf8JsonWriter writer, JsonEncodedText name, NormalizationForm form)
    {
        writer.WriteString(
            name,
            form is >= NormalizationForm.NFC and <= NormalizationForm.NFKD
                ? Forms[(int)form]
                : throw new JsonException($"Unknown normalization form {form}")
        );
    }

    /// <summary>A flag such as <c>distinct</c>, written only when it is set.</summary>
    public static void Flag(Utf8JsonWriter writer, JsonEncodedText name, bool flag)
    {
        if (flag)
        {
            writer.WriteBoolean(name, true);
        }
    }

    public static void Kind(Utf8JsonWriter writer, JoinKind kind)
    {
        writer.WriteString(
            KindName,
            kind is >= JoinKind.Inner and <= JoinKind.Full
                ? JoinKinds[(int)kind]
                : throw new JsonException($"Unknown join kind {kind}")
        );
    }
}
