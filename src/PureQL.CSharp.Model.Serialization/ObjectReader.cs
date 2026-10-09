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

    public IReadOnlyList<T> Many<T>(string name)
        where T : class
    {
        return Array(
            Required(name),
            Child(name),
            (item, path) => _registry.Read<T>(item, path)
        );
    }

    public IReadOnlyList<T>? OptionalMany<T>(string name)
        where T : class
    {
        return TryGet(name, out JsonElement value)
            ? Array(value, Child(name), (item, path) => _registry.Read<T>(item, path))
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

    public int Index(string name)
    {
        JsonElement value = Required(name);
        return value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out int index)
            ? index
            : throw JsonErrors.At(Child(name), "Expected a 32-bit integer");
    }

    public bool Flag(string name)
    {
        return TryGet(name, out JsonElement value)
            && LiteralValues.Boolean.Read(value, Child(name));
    }

    public TValue Value<TValue>(LiteralValue<TValue> literal)
    {
        return literal.Read(Required("value"), Child("value"));
    }

    public IReadOnlyList<TValue> Values<TValue>(LiteralValue<TValue> literal)
    {
        return Array(Required("value"), Child("value"), literal.Read);
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
    /// Reads <c>over</c> where the schema allows both values; it defaults to
    /// <c>group</c>.
    /// </summary>
    public AggregateOver Over()
    {
        return Enum(
            "over",
            AggregateOver.Group,
            ("group", AggregateOver.Group),
            ("all", AggregateOver.All)
        );
    }

    /// <summary>
    /// Accepts <c>over</c> where the schema allows only one value, which the model does
    /// not store.
    /// </summary>
    public void FixedOver(string value)
    {
        if (
            TryGet("over", out JsonElement over)
            && !string.Equals(
                LiteralValues.String.Read(over, Child("over")),
                value,
                StringComparison.Ordinal
            )
        )
        {
            throw JsonErrors.At(Child("over"), $"Only '{value}' is allowed here");
        }
    }

    public SortDirection Direction()
    {
        return Enum(
            "direction",
            SortDirection.Asc,
            ("asc", SortDirection.Asc),
            ("desc", SortDirection.Desc)
        );
    }

    public JoinType JoinType()
    {
        _ = Required("type");
        return Enum(
            "type",
            Model.JoinType.Inner,
            ("inner", Model.JoinType.Inner),
            ("left", Model.JoinType.Left),
            ("right", Model.JoinType.Right),
            ("full", Model.JoinType.Full)
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
        Func<JsonElement, string, TItem> read
    )
    {
        if (array.ValueKind != JsonValueKind.Array)
        {
            throw JsonErrors.At(path, $"Expected an array, found {array.ValueKind}");
        }

        TItem[] items = new TItem[array.GetArrayLength()];
        int index = 0;
        foreach (JsonElement item in array.EnumerateArray())
        {
            items[index] = read(item, $"{path}[{index}]");
            index++;
        }

        return items;
    }
}
