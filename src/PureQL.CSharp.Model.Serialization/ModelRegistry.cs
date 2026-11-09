using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The converter of every model type, built once from the generated readers and
/// writers (<c>ModelSerializers.Register</c>), and the writer of every record by its
/// runtime type, which writes the value a union holds.
/// </summary>
internal sealed class ModelRegistry
{
    private readonly List<JsonConverter> _converters = [];

    private readonly Dictionary<
        Type,
        Func<JsonSerializerOptions, JsonTypeInfo>
    > _typeInfos = [];

    private readonly Dictionary<Type, Action<Utf8JsonWriter, object>> _writers = [];

    private readonly HashSet<Type> _nullable = [];

    private readonly FrozenDictionary<Type, Action<Utf8JsonWriter, object>> _records;

    private readonly FrozenSet<Type> _nullableRecords;

    private ModelRegistry()
    {
        ModelSerializers.Register(this);
        _records = _writers.ToFrozenDictionary();
        _nullableRecords = _nullable.ToFrozenSet();
    }

    public static ModelRegistry Instance { get; } = new ModelRegistry();

    public IReadOnlyList<JsonConverter> Converters => _converters;

    /// <summary>
    /// The <see cref="JsonTypeInfo"/> of a model type for one options instance, backed
    /// by its converter, or <see langword="null"/> for any other type.
    /// </summary>
    public JsonTypeInfo? CreateTypeInfo(Type type, JsonSerializerOptions options)
    {
        return _typeInfos.TryGetValue(
            type,
            out Func<JsonSerializerOptions, JsonTypeInfo>? create
        )
            ? create(options)
            : null;
    }

    /// <summary>Writes a record of any model type.</summary>
    public void Write(Utf8JsonWriter writer, object? value)
    {
        if (value is null)
        {
            throw Writes.Missing();
        }

        _records[value.GetType()](writer, value);
    }

    /// <summary>Whether a record declares a nullable type.</summary>
    public bool IsNullable(object? value)
    {
        return value is not null && _nullableRecords.Contains(value.GetType());
    }

    /// <param name="nullable">Whether the record declares a nullable type.</param>
    public void Record<T>(
        ReadValue<T> read,
        Action<Utf8JsonWriter, T> write,
        bool nullable = false
    )
        where T : class
    {
        Add(read, write);
        _writers.Add(typeof(T), (writer, value) => write(writer, (T)value));
        if (nullable)
        {
            _ = _nullable.Add(typeof(T));
        }
    }

    public void Union<T>(ReadValue<T> read, Action<Utf8JsonWriter, T> write)
        where T : struct, IUnion
    {
        Add(read, write);
    }

    private void Add<T>(ReadValue<T> read, Action<Utf8JsonWriter, T> write)
    {
        PureQLConverter<T> converter = new PureQLConverter<T>(read, write);
        _converters.Add(converter);
        _typeInfos.Add(
            typeof(T),
            options => JsonMetadataServices.CreateValueInfo<T>(options, converter)
        );
    }
}
