using System.Text;
using System.Text.Json;
using PureQL.CSharp.Model.Serialization.Literals;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The state of one read: the JSON path to the value being read, kept as a stack and
/// spelled only when an error is reported, and the reading of objects, lists and the
/// values the records are made of. One context reads one value of a converter, nested
/// values included, from a single <see cref="Utf8JsonReader"/>.
/// </summary>
internal sealed partial class ReadContext
{
    private const string EndOfValue = "Unexpected end of the JSON value";

    private readonly List<(string? Name, int Index)> _path = [];

    /// <summary>The JSON path of the value being read, <c>$.select[0].expression</c>.</summary>
    public string Path
    {
        get
        {
            StringBuilder builder = new StringBuilder("$");
            foreach ((string? name, int index) in _path)
            {
                _ = name is null
                    ? builder.Append('[').Append(index).Append(']')
                    : builder.Append('.').Append(name);
            }

            return builder.ToString();
        }
    }

    public JsonException Error(string message)
    {
        return JsonErrors.At(Path, message);
    }

    public ObjectScope Begin(ref Utf8JsonReader reader, RecordShape shape)
    {
        return reader.TokenType == JsonTokenType.StartObject
            ? new ObjectScope(shape)
            : throw Error($"Expected an object, found {Describe(reader.TokenType)}");
    }

    /// <summary>
    /// Moves to the value of the next property of the record and gives its index in the
    /// shape, or returns <see langword="false"/> at the end of the object. The implied
    /// <c>operator</c> and <c>type</c> are checked here; a duplicate or unknown property
    /// is rejected, as <c>additionalProperties: false</c> does.
    /// </summary>
    public bool Next(ref Utf8JsonReader reader, ref ObjectScope scope, out int property)
    {
        if (scope.InProperty)
        {
            Pop();
            scope.InProperty = false;
        }

        RecordShape shape = scope.Shape;
        while (Read(ref reader) == JsonTokenType.PropertyName)
        {
            property = shape.Match(ref reader);
            if (property == RecordShape.UnknownProperty)
            {
                throw Error($"Unexpected property '{reader.GetString()}'");
            }

            ulong bit = RecordShape.Bit(property);
            string name = shape.PropertyName(property);
            if ((scope.Seen & bit) != 0)
            {
                throw Error($"Duplicate property '{name}'");
            }

            scope.Seen |= bit;
            Push(name);
            _ = Read(ref reader);
            if (property == RecordShape.OperatorProperty)
            {
                ExpectOperator(ref reader, shape);
                Pop();
            }
            else if (property == RecordShape.TypeProperty)
            {
                ExpectType(ref reader, shape);
                Pop();
            }
            else
            {
                scope.InProperty = true;
                return true;
            }
        }

        property = RecordShape.UnknownProperty;
        return false;
    }

    /// <summary>
    /// Checks that every required property of the record was read, and one at least of
    /// a record whose properties are all optional where the schema requires one
    /// (<c>minProperties: 1</c>).
    /// </summary>
    public void End(ref ObjectScope scope)
    {
        RecordShape shape = scope.Shape;
        ulong missing = shape.Required & ~scope.Seen;
        if (shape.AtLeastOne && (scope.Seen == 0))
        {
            throw Error($"Expected at least one of {string.Join(", ", shape.Properties)}");
        }

        if (missing == 0)
        {
            return;
        }

        int property =
            (missing & RecordShape.Bit(RecordShape.OperatorProperty)) != 0
                ? RecordShape.OperatorProperty
            : (missing & RecordShape.Bit(RecordShape.TypeProperty)) != 0
                ? RecordShape.TypeProperty
            : System.Numerics.BitOperations.TrailingZeroCount(missing);
        throw Error($"Missing property '{shape.PropertyName(property)}'");
    }

    /// <summary>
    /// A record declared nullable whose operands do not give it a nullable type, which is
    /// then wider than the exact type the schema requires.
    /// </summary>
    public JsonException NotNullable(RecordShape shape)
    {
        return Error(
            $"{shape.Name} is declared nullable, but its operands make it non-null"
        );
    }

    /// <summary>
    /// A non-null <c>coalesce</c> without exactly one non-null operand: none leaves it
    /// nullable, and one after the first would never be reached.
    /// </summary>
    public JsonException NotOneNonNull(RecordShape shape)
    {
        return Error($"{shape.Name} is declared non-null, so exactly one operand is non-null");
    }

    /// <summary>
    /// A record declared <c>decimal</c> whose number operands are all integers, which
    /// make it an <c>integer</c>: the declaration is then wider than the exact type the
    /// schema requires.
    /// </summary>
    public JsonException NotDecimal(RecordShape shape)
    {
        return Error($"{shape.Name} is declared decimal, but its operands make it an integer");
    }

    /// <summary>
    /// A list of at least <paramref name="minItems"/> items, the <c>minItems</c> the
    /// schema declares for it.
    /// </summary>
    public T[] Many<T>(ref Utf8JsonReader reader, int minItems, ReadValue<T> read)
    {
        return Many(ref reader, minItems, int.MaxValue, read);
    }

    /// <summary>
    /// A list of <paramref name="minItems"/> to <paramref name="maxItems"/> items, the
    /// <c>minItems</c> and <c>maxItems</c> the schema declares for it.
    /// </summary>
    public T[] Many<T>(
        ref Utf8JsonReader reader,
        int minItems,
        int maxItems,
        ReadValue<T> read
    )
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw Error($"Expected an array, found {Describe(reader.TokenType)}");
        }

        // Most lists are the two operands of an operator.
        T[] items = new T[2];
        int count = 0;
        while (Read(ref reader) != JsonTokenType.EndArray)
        {
            if (count == items.Length)
            {
                Array.Resize(ref items, count * 2);
            }

            Push(count);
            items[count] = read(ref reader, this);
            Pop();
            count++;
        }

        if (count < minItems)
        {
            throw Error(
                $"Expected at least {minItems} {(minItems == 1 ? "item" : "items")}, "
                    + $"found {count}"
            );
        }

        if (count > maxItems)
        {
            throw Error(
                $"Expected at most {maxItems} {(maxItems == 1 ? "item" : "items")}, "
                    + $"found {count}"
            );
        }

        if (count != items.Length)
        {
            Array.Resize(ref items, count);
        }

        return items;
    }

    /// <summary>
    /// A name of an entity, a field, an alias, a parameter or a subquery: not empty, no
    /// leading or trailing space or tab, no line break (the schema's <c>NAME</c>).
    /// </summary>
    public string String(ref Utf8JsonReader reader)
    {
        string name = LiteralValues.String.Read(ref reader, this);
        return Checks.Name(name) is string error ? throw Error(error) : name;
    }

    /// <summary>A list of names, such as <c>distinctBy</c>: at least one, each once.</summary>
    public string[] Strings(ref Utf8JsonReader reader)
    {
        string[] items = Many(ref reader, 1, ReadName);
        return items.Distinct(StringComparer.Ordinal).Count() == items.Length
            ? items
            : throw Error("Items must be unique");
    }

    /// <summary>
    /// A bare number of rows, levels or units, as <c>skip</c>, <c>take</c>,
    /// <c>maxDepth</c> and the sides of a window are written: from 1, as the absence of
    /// the property is none of them.
    /// </summary>
    public long Count(ref Utf8JsonReader reader)
    {
        long count = LiteralValues.ReadInteger(ref reader, this);
        return Checks.Count(count) is string error ? throw Error(error) : count;
    }

    /// <summary>The constant <c>"all"</c>, every row on one side of a window.</summary>
    public All All(ref Utf8JsonReader reader)
    {
        return Enum(ref reader, "all"u8, new All());
    }

    /// <summary>Fails the read with the message of a check that failed.</summary>
    public void Check(string? error)
    {
        if (error is not null)
        {
            throw Error(error);
        }
    }

    /// <summary>The value, if the check of it found no error.</summary>
    public T Check<T>(string? error, T value)
    {
        Check(error);
        return value;
    }

    /// <summary>The values of a list literal: at least one, each of them once.</summary>
    public TValue[] Values<TValue>(ref Utf8JsonReader reader, LiteralValue<TValue> literal)
    {
        TValue[] values = Many(ref reader, 1, literal.Read);
        return values.Distinct().Count() == values.Length
            ? values
            : throw Error("A list literal repeats no value");
    }

    /// <summary>The <c>value</c> of a nullable literal, which is <c>null</c>.</summary>
    public void Null(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.Null)
        {
            throw Error("A nullable literal must have the value null");
        }
    }

    /// <summary>
    /// Reads <c>over</c>, which the schema allows only as <c>all</c>: <c>group</c>, the
    /// default, is never written.
    /// </summary>
    public AggregateOver Over(ref Utf8JsonReader reader)
    {
        return Enum(ref reader, "all"u8, AggregateOver.All);
    }

    /// <summary>
    /// Reads <c>direction</c>, which the schema allows only as <c>desc</c>: <c>asc</c>,
    /// the default, is never written.
    /// </summary>
    public SortDirection Direction(ref Utf8JsonReader reader)
    {
        return Enum(ref reader, "desc"u8, SortDirection.Desc);
    }

    /// <summary>
    /// Reads <c>mode</c>, which the schema allows only as <c>halfEven</c>: half away from
    /// zero, the default, is never written.
    /// </summary>
    public RoundingMode Mode(ref Utf8JsonReader reader)
    {
        return Enum(ref reader, "halfEven"u8, RoundingMode.HalfEven);
    }

    /// <summary>
    /// Reads a flag such as <c>distinct</c>, which the schema allows only as
    /// <c>true</c>: <c>false</c>, the default, is never written.
    /// </summary>
    public bool Flag(ref Utf8JsonReader reader)
    {
        return reader.TokenType == JsonTokenType.True
            ? true
            : throw Error(
                reader.TokenType == JsonTokenType.False
                    ? "A flag is written only when true"
                    : $"Expected true, found {Describe(reader.TokenType)}"
            );
    }

    /// <summary>Reads <c>form</c>, the name of a Unicode normalization form.</summary>
    public NormalizationForm Form(ref Utf8JsonReader reader)
    {
        string text = LiteralValues.String.Read(ref reader, this);
        return text switch
        {
            "NFC" => NormalizationForm.NFC,
            "NFD" => NormalizationForm.NFD,
            "NFKC" => NormalizationForm.NFKC,
            "NFKD" => NormalizationForm.NFKD,
            _ => throw Error($"Unexpected value '{text}'"),
        };
    }

    public JoinKind Kind(ref Utf8JsonReader reader)
    {
        string text = LiteralValues.String.Read(ref reader, this);
        return text switch
        {
            "inner" => JoinKind.Inner,
            "left" => JoinKind.Left,
            "right" => JoinKind.Right,
            "full" => JoinKind.Full,
            _ => throw Error($"Unexpected value '{text}'"),
        };
    }

    /// <summary>
    /// Reads a type object, <c>{ "name": …, "nullable": true }</c>, strictly: a
    /// non-null type has no <c>nullable</c>, as every type has one spelling.
    /// </summary>
    public int ReadType(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw Error($"Expected a type object, found {Describe(reader.TokenType)}");
        }

        int name = TypeIds.None;
        bool nullable = false;
        while (Read(ref reader) == JsonTokenType.PropertyName)
        {
            if (reader.ValueTextEquals("name"u8))
            {
                _ = Read(ref reader);
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw Error("Unexpected type property 'name'");
                }

                name = TypeIds.Find(ref reader);
                UnknownType = name == TypeIds.Unknown ? reader.GetString() : UnknownType;
            }
            else if (reader.ValueTextEquals("nullable"u8))
            {
                JsonTokenType value = Read(ref reader);
                nullable =
                    value == JsonTokenType.True ? true
                    : value == JsonTokenType.False
                        ? throw Error("A non-null type is written without 'nullable'")
                    : throw Error("Unexpected type property 'nullable'");
            }
            else
            {
                throw Error($"Unexpected type property '{reader.GetString()}'");
            }
        }

        return name == TypeIds.None ? throw Error("Missing type name")
            : nullable ? name | TypeIds.NullableBit
            : name;
    }

    /// <summary>The name of the last type that is none of the specification's.</summary>
    private string? UnknownType { get; set; }

    private void ExpectOperator(ref Utf8JsonReader reader, RecordShape shape)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw Error("Expected a string");
        }

        if (Kinds.Operator(ref reader) != shape.Kind)
        {
            throw Error($"{shape.Name} does not accept '{reader.GetString()}'");
        }
    }

    private void ExpectType(ref Utf8JsonReader reader, RecordShape shape)
    {
        int type = ReadType(ref reader);
        if (type != shape.Type)
        {
            throw Error(
                $"{shape.Name} does not accept "
                    + $"'{Kinds.Name(shape.Kind)} {DescribeType(type)}'"
            );
        }
    }

    private string DescribeType(int type)
    {
        return (type & ~TypeIds.NullableBit) == TypeIds.Unknown
            ? $"{UnknownType}{(TypeIds.IsNullable(type) ? "?" : "")}"
            : TypeIds.Describe(type);
    }

    private TEnum Enum<TEnum>(ref Utf8JsonReader reader, ReadOnlySpan<byte> name, TEnum value)
    {
        if (reader.TokenType == JsonTokenType.String && reader.ValueTextEquals(name))
        {
            return value;
        }

        string text = LiteralValues.String.Read(ref reader, this);
        throw Error($"Unexpected value '{text}'");
    }

    private static string ReadName(ref Utf8JsonReader reader, ReadContext context)
    {
        return context.String(ref reader);
    }

    private void Push(string name)
    {
        _path.Add((name, 0));
    }

    private void Push(int index)
    {
        _path.Add((null, index));
    }

    private void Pop()
    {
        _path.RemoveAt(_path.Count - 1);
    }

    /// <summary>
    /// The next token of the value being read: the serializer gives a converter its
    /// whole value, so the reader runs out of data inside it only when the value is cut
    /// short, which is an error.
    /// </summary>
    private JsonTokenType Read(ref Utf8JsonReader reader)
    {
        return reader.Read() ? reader.TokenType : throw Error(EndOfValue);
    }

    /// <summary>
    /// Moves the reader to the last token of the value it is on, or of the value of the
    /// property it is on. <see cref="Utf8JsonReader.Skip"/> cannot be used: it throws
    /// on any reader whose block is not final, and the serializer gives a converter
    /// such a reader when it reads a stream or a pipe, even though the block holds the
    /// converter's whole value.
    /// </summary>
    private void Skip(ref Utf8JsonReader reader)
    {
        if (!reader.TrySkip())
        {
            throw Error(EndOfValue);
        }
    }

    public static string Describe(JsonTokenType token)
    {
        // The other tokens a value starts with are spelled as their JsonValueKind.
        return token == JsonTokenType.StartObject ? "Object"
            : token == JsonTokenType.StartArray ? "Array"
            : token.ToString();
    }
}
