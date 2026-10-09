using System.Text.Json;
using System.Text.Json.Serialization;
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

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The descriptors of all model types, keyed by CLR type. Built once; every union's
/// dispatch table is computed and checked for ambiguity when it is built.
/// </summary>
internal sealed partial class ModelRegistry
{
    private readonly Dictionary<Type, ModelDescriptor> _descriptors = [];

    private readonly List<JsonConverter> _converters = [];

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

    public void ByPresence<T>(string property, UnionCase<T> present, UnionCase<T> absent)
        where T : class
    {
        Add<T>(new PresenceUnionDescriptor<T>(this, property, present, absent));
    }

    public void Record<T>(Func<ObjectReader, T> read, Action<ObjectWriter, T> write)
        where T : class
    {
        Add<T>(new RecordDescriptor<T>(this, [], null, read, write));
    }

    public void Operator<T>(
        string name,
        Func<ObjectReader, T> read,
        Action<ObjectWriter, T> write,
        bool digits = false
    )
        where T : class
    {
        Add<T>(
            new RecordDescriptor<T>(
                this,
                [Discriminators.Operator(name, digits)],
                name,
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
        Add<T>(new RecordDescriptor<T>(this, signatures, null, read, write));
    }

    private void Add<T>(ModelDescriptor descriptor)
        where T : class
    {
        _descriptors.Add(typeof(T), descriptor);
        _converters.Add(new PureQLConverter<T>(this));
    }
}
