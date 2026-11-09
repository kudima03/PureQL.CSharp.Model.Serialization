using System.Text.Json;
using PureQL.CSharp.Model.Serialization.Literals;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Writes the properties records are made of. Values equal to the schema default
/// (<c>nullable: false</c>, the default <c>over</c> and <c>direction</c>) and absent
/// optional properties are omitted; a missing required value is a
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

    public static T NotNull<T>(T? value)
        where T : class
    {
        return value ?? throw Missing();
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
        writer.WriteStartArray(name);
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

    public static void String(Utf8JsonWriter writer, JsonEncodedText name, string? value)
    {
        writer.WriteString(name, value ?? throw Missing());
    }

    public static void OptionalString(
        Utf8JsonWriter writer,
        JsonEncodedText name,
        string? value
    )
    {
        if (value is not null)
        {
            writer.WriteString(name, value);
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
            writer.WriteStringValue(value ?? throw Missing());
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
