using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Literals;

/// <summary>Reads and writes the JSON form of one literal value type.</summary>
internal sealed class LiteralValue<T>(ReadValue<T> read, Action<Utf8JsonWriter, T> write)
{
    public ReadValue<T> Read { get; } = read;

    public Action<Utf8JsonWriter, T> Write { get; } = write;
}
