using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.GroupExpressions;
using PureQL.CSharp.Model.Serialization.GroupKeys;
using PureQL.CSharp.Model.Serialization.Keys;
using PureQL.CSharp.Model.Serialization.Lists;
using PureQL.CSharp.Model.Serialization.Literals;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.ProjectionExpressions;
using PureQL.CSharp.Model.Serialization.RowExpressions;
using PureQL.CSharp.Model.Serialization.SelectItems;
using PureQL.CSharp.Model.Serialization.TotalExpressions;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The descriptors of all model types, keyed by CLR type. Built once; every union's
/// dispatch table is computed and checked for ambiguity when it is built.
/// </summary>
internal sealed partial class ModelRegistry
{
    private readonly Dictionary<Type, ModelDescriptor> _descriptors = [];

    private readonly List<JsonConverter> _converters = [];

    private readonly Dictionary<
        Type,
        Func<JsonSerializerOptions, JsonTypeInfo>
    > _typeInfos = [];

    private ModelRegistry()
    {
        QueryRegistrations.Register(this);
        FieldRegistrations.Register(this);
        ParameterRegistrations.Register(this);
        LiteralRegistrations.Register(this);
        KeyRegistrations.Register(this);
        ListRegistrations.Register(this);
        RowExpressionRegistrations.Register(this);
        ProjectionExpressionRegistrations.Register(this);
        GroupExpressionRegistrations.Register(this);
        TotalExpressionRegistrations.Register(this);
        GroupKeyRegistrations.Register(this);
        SelectItemRegistrations.Register(this);
        foreach (ModelDescriptor descriptor in _descriptors.Values)
        {
            descriptor.Seal();
        }
    }

    public static ModelRegistry Instance { get; } = new ModelRegistry();

    public IReadOnlyList<JsonConverter> Converters => _converters;

    public T Read<T>(JsonElement element, string path)
        where T : class
    {
        return (T)Find(typeof(T)).Read(element, path);
    }

    public void Write(Utf8JsonWriter writer, object? value)
    {
        if (value is null)
        {
            throw new JsonException(
                "A PureQL model value is null where JSON requires one"
            );
        }

        Find(value.GetType()).Write(writer, value);
    }

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

    public ModelDescriptor Find(Type type)
    {
        return _descriptors.TryGetValue(type, out ModelDescriptor? descriptor)
            ? descriptor
            : throw new InvalidOperationException($"{type.Name} is not registered");
    }

    public void Union<T>(params UnionCase<T>[] cases)
        where T : class
    {
        Add<T>(new UnionDescriptor<T>(this, cases));
    }

    public void Probe<T>(string? operand, params UnionCase<T>[] cases)
        where T : class
    {
        Add<T>(new ProbeUnionDescriptor<T>(this, operand, cases));
    }

    public void ByPresence<T>(
        UnionCase<T> fallback,
        params (string Property, UnionCase<T> Case)[] present
    )
        where T : class
    {
        Add<T>(new PresenceUnionDescriptor<T>(this, present, fallback));
    }

    public void Record<T>(Func<ObjectReader, T> read, Action<ObjectWriter, T> write)
        where T : class
    {
        Add<T>(
            new RecordDescriptor<T>(
                this,
                [],
                null,
                null,
                NullableCause.AnyOperand,
                read,
                write
            )
        );
    }

    /// <summary>
    /// An operator node, which declares the type it yields: nullable only when an operand
    /// is (each of them for <c>coalesce</c>) or a predicate filters its rows.
    /// </summary>
    public void Operator<T>(
        string name,
        IType type,
        Func<ObjectReader, T> read,
        Action<ObjectWriter, T> write
    )
        where T : class
    {
        Operator(
            name,
            type,
            name == "coalesce" ? NullableCause.EveryOperand : NullableCause.AnyOperand,
            read,
            write
        );
    }

    /// <summary>
    /// An aggregate over all rows of a total query, which yields one row even when none
    /// is aggregated, so it is nullable whatever its operands are.
    /// </summary>
    public void TotalAggregate<T>(
        string name,
        IType type,
        Func<ObjectReader, T> read,
        Action<ObjectWriter, T> write
    )
        where T : class
    {
        Operator(name, type, NullableCause.Always, read, write);
    }

    private void Operator<T>(
        string name,
        IType type,
        NullableCause nullableCause,
        Func<ObjectReader, T> read,
        Action<ObjectWriter, T> write
    )
        where T : class
    {
        Add<T>(
            new RecordDescriptor<T>(
                this,
                [Discriminators.Operator(name, type.Name, type.Nullable)],
                name,
                type,
                nullableCause,
                read,
                write
            )
        );
    }

    private void Leaf<T>(
        IEnumerable<string> signatures,
        Func<ObjectReader, T> read,
        Action<ObjectWriter, T> write
    )
        where T : class
    {
        Add<T>(
            new RecordDescriptor<T>(
                this,
                signatures,
                null,
                null,
                NullableCause.AnyOperand,
                read,
                write
            )
        );
    }

    private void Add<T>(ModelDescriptor descriptor)
        where T : class
    {
        PureQLConverter<T> converter = new PureQLConverter<T>(this);
        _descriptors.Add(typeof(T), descriptor);
        _converters.Add(converter);
        _typeInfos.Add(
            typeof(T),
            options => JsonMetadataServices.CreateValueInfo<T>(options, converter)
        );
    }
}
