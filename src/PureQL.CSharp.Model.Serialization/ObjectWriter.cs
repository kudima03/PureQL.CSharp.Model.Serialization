using System.Text.Json;
using OneOf;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Serialization.Literals;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Writes the properties of one JSON object. Values equal to the schema default
/// (<c>nullable: false</c>, the default <c>over</c> and <c>direction</c>,
/// <c>distinct: false</c>) and absent optional properties are omitted.
/// </summary>
internal sealed class ObjectWriter(ModelRegistry registry, Utf8JsonWriter writer)
{
    private readonly ModelRegistry _registry = registry;

    private readonly Utf8JsonWriter _writer = writer;

    public void One<T>(string name, T value)
        where T : class
    {
        _writer.WritePropertyName(name);
        _registry.Write(_writer, value);
    }

    public void Optional<T>(string name, T? value)
        where T : class
    {
        if (value is not null)
        {
            One(name, value);
        }
    }

    public void Many<T>(string name, IEnumerable<T> values)
        where T : class
    {
        _writer.WriteStartArray(name);
        foreach (T value in values)
        {
            _registry.Write(_writer, value);
        }

        _writer.WriteEndArray();
    }

    public void OptionalMany<T>(string name, IEnumerable<T>? values)
        where T : class
    {
        if (values is not null)
        {
            Many(name, values);
        }
    }

    public void String(string name, string value)
    {
        _writer.WriteString(name, value);
    }

    public void OptionalString(string name, string? value)
    {
        if (value is not null)
        {
            _writer.WriteString(name, value);
        }
    }

    public void Index(string name, int value)
    {
        _writer.WriteNumber(name, value);
    }

    public void Flag(string name, bool value)
    {
        if (value)
        {
            _writer.WriteBoolean(name, value);
        }
    }

    public void Type(IType type)
    {
        Type(type.Name, type.Nullable);
    }

    public void Type(string name, bool nullable)
    {
        _writer.WriteStartObject("type");
        _writer.WriteString("name", name);
        if (nullable)
        {
            _writer.WriteBoolean("nullable", true);
        }

        _writer.WriteEndObject();
    }

    public void Value<TValue>(LiteralValue<TValue> literal, TValue value)
    {
        _writer.WritePropertyName("value");
        literal.Write(_writer, value);
    }

    public void Values<TValue>(LiteralValue<TValue> literal, IEnumerable<TValue> values)
    {
        _writer.WriteStartArray("value");
        foreach (TValue value in values)
        {
            literal.Write(_writer, value);
        }

        _writer.WriteEndArray();
    }

    public void NullValue()
    {
        _writer.WriteNull("value");
    }

    public void Over(AggregateOver over)
    {
        if (over == AggregateOver.All)
        {
            _writer.WriteString("over", "all");
        }
    }

    public void Direction(SortDirection direction)
    {
        if (direction == SortDirection.Desc)
        {
            _writer.WriteString("direction", "desc");
        }
    }

    public void JoinType(JoinType type)
    {
        _writer.WriteString(
            "type",
            type switch
            {
                Model.JoinType.Inner => "inner",
                Model.JoinType.Left => "left",
                Model.JoinType.Right => "right",
                Model.JoinType.Full => "full",
                _ => throw new JsonException($"Unknown join type {type}"),
            }
        );
    }

    public void Bound(string name, OneOf<long, ParamInteger> bound)
    {
        if (bound.TryPickT0(out long number, out ParamInteger parameter))
        {
            _writer.WriteNumber(name, number);
        }
        else
        {
            One(name, parameter);
        }
    }
}
