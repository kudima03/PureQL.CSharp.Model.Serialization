using System.Text.Json;
using OneOf;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Serialization.Literals;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Reads the properties of one JSON object and remembers which ones were read, so that
/// any other property is rejected, as <c>additionalProperties: false</c> does.
/// </summary>
internal sealed class ObjectReader
{
    private readonly ModelRegistry _registry;

    private readonly JsonElement _element;

    private readonly HashSet<string> _read = new HashSet<string>(StringComparer.Ordinal);

    public ObjectReader(ModelRegistry registry, JsonElement element, string path)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw JsonErrors.At(path, $"Expected an object, found {element.ValueKind}");
        }

        HashSet<string> names = new HashSet<string>(StringComparer.Ordinal);
        foreach (JsonProperty property in element.EnumerateObject())
        {
            if (!names.Add(property.Name))
            {
                throw JsonErrors.At(path, $"Duplicate property '{property.Name}'");
            }
        }

        _registry = registry;
        _element = element;
        Path = path;
    }

    public string Path { get; }

    public void Skip(string name)
    {
        _ = _read.Add(name);
    }

    public T One<T>(string name)
        where T : class
    {
        return _registry.Read<T>(Required(name), Child(name));
    }

    public T? Optional<T>(string name)
        where T : class
    {
        return TryGet(name, out JsonElement value)
            ? _registry.Read<T>(value, Child(name))
            : null;
    }

    /// <summary>
    /// A list of at least <paramref name="minItems"/> items, the <c>minItems</c> the
    /// schema declares for it.
    /// </summary>
    public IReadOnlyList<T> Many<T>(string name, int minItems)
        where T : class
    {
        return Array(
            Required(name),
            Child(name),
            minItems,
            (item, path) => _registry.Read<T>(item, path)
        );
    }

    /// <summary>
    /// An optional list of at least <paramref name="minItems"/> items, the
    /// <c>minItems</c> the schema declares for it.
    /// </summary>
    public IReadOnlyList<T>? OptionalMany<T>(string name, int minItems)
        where T : class
    {
        return TryGet(name, out JsonElement value)
            ? Array(
                value,
                Child(name),
                minItems,
                (item, path) => _registry.Read<T>(item, path)
            )
            : null;
    }

    public string String(string name)
    {
        return LiteralValues.String.Read(Required(name), Child(name));
    }

    public string? OptionalString(string name)
    {
        return TryGet(name, out JsonElement value)
            ? LiteralValues.String.Read(value, Child(name))
            : null;
    }

    /// <summary>
    /// An optional list of names, such as <c>distinctBy</c>: at least one, each of them
    /// once.
    /// </summary>
    public IReadOnlyList<string>? OptionalStrings(string name)
    {
        if (!TryGet(name, out JsonElement value))
        {
            return null;
        }

        IReadOnlyList<string> items = Array(
            value,
            Child(name),
            1,
            LiteralValues.String.Read
        );
        return items.Distinct(StringComparer.Ordinal).Count() == items.Count
            ? items
            : throw JsonErrors.At(Child(name), "Items must be unique");
    }

    public TValue Value<TValue>(LiteralValue<TValue> literal)
    {
        return literal.Read(Required("value"), Child("value"));
    }

    public IReadOnlyList<TValue> Values<TValue>(LiteralValue<TValue> literal)
    {
        IReadOnlyList<TValue> values = Array(
            Required("value"),
            Child("value"),
            0,
            literal.Read
        );
        return values.Distinct().Count() == values.Count
            ? values
            : throw JsonErrors.At(Child("value"), "A list literal repeats no value");
    }

    public void NullValue()
    {
        if (Required("value").ValueKind != JsonValueKind.Null)
        {
            throw JsonErrors.At(
                Child("value"),
                "A nullable literal must have the value null"
            );
        }
    }

    public bool NullableType()
    {
        return Discriminators.Type(Required("type"), Child("type")).Nullable;
    }

    /// <summary>
    /// Reads <c>over</c>, which the schema allows only as <c>all</c>: <c>group</c>, the
    /// default, is never written.
    /// </summary>
    public AggregateOver Over()
    {
        return Enum("over", AggregateOver.Group, ("all", AggregateOver.All));
    }

    public SortDirection Direction()
    {
        return Enum("direction", SortDirection.Asc, ("desc", SortDirection.Desc));
    }

    public JoinKind Kind()
    {
        _ = Required("kind");
        return Enum(
            "kind",
            JoinKind.Inner,
            ("inner", JoinKind.Inner),
            ("left", JoinKind.Left),
            ("right", JoinKind.Right),
            ("full", JoinKind.Full)
        );
    }

    /// <summary>A pagination bound: a number or a non-null integer parameter.</summary>
    public OneOf<long, ParamInteger> Bound(string name)
    {
        JsonElement value = Required(name);
        return value.ValueKind == JsonValueKind.Number
            ? LiteralValues.ReadInteger(value, Child(name))
            : _registry.Read<ParamInteger>(value, Child(name));
    }

    public void EnsureAllRead()
    {
        foreach (JsonProperty property in _element.EnumerateObject())
        {
            if (!_read.Contains(property.Name))
            {
                throw JsonErrors.At(Path, $"Unexpected property '{property.Name}'");
            }
        }
    }

    private TEnum Enum<TEnum>(
        string name,
        TEnum defaultValue,
        params (string Name, TEnum Value)[] values
    )
    {
        if (!TryGet(name, out JsonElement element))
        {
            return defaultValue;
        }

        string text = LiteralValues.String.Read(element, Child(name));
        foreach ((string Name, TEnum Value) value in values)
        {
            if (string.Equals(value.Name, text, StringComparison.Ordinal))
            {
                return value.Value;
            }
        }

        throw JsonErrors.At(Child(name), $"Unexpected value '{text}'");
    }

    private JsonElement Required(string name)
    {
        return TryGet(name, out JsonElement value)
            ? value
            : throw JsonErrors.At(Path, $"Missing property '{name}'");
    }

    private bool TryGet(string name, out JsonElement value)
    {
        _ = _read.Add(name);
        return _element.TryGetProperty(name, out value);
    }

    private string Child(string name)
    {
        return $"{Path}.{name}";
    }

    private static IReadOnlyList<TItem> Array<TItem>(
        JsonElement array,
        string path,
        int minItems,
        Func<JsonElement, string, TItem> read
    )
    {
        if (array.ValueKind != JsonValueKind.Array)
        {
            throw JsonErrors.At(path, $"Expected an array, found {array.ValueKind}");
        }

        int length = array.GetArrayLength();
        if (length < minItems)
        {
            throw JsonErrors.At(
                path,
                $"Expected at least {minItems} {(minItems == 1 ? "item" : "items")}, "
                    + $"found {length}"
            );
        }

        TItem[] items = new TItem[length];
        int index = 0;
        foreach (JsonElement item in array.EnumerateArray())
        {
            items[index] = read(item, $"{path}[{index}]");
            index++;
        }

        return items;
    }
}
