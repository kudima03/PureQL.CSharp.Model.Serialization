using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Literals;

/// <summary>Reads and writes the JSON form of one literal value type.</summary>
internal sealed class LiteralValue<T>(
    Func<JsonElement, string, T> read,
    Action<Utf8JsonWriter, T> write
)
{
    private readonly Func<JsonElement, string, T> _read = read;

    private readonly Action<Utf8JsonWriter, T> _write = write;

    public T Read(JsonElement element, string path)
    {
        return _read(element, path);
    }

    public void Write(Utf8JsonWriter writer, T value)
    {
        _write(writer, value);
    }
}
