using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Converts one model type with its generated reader and writer. The value is read in
/// one pass of the <see cref="Utf8JsonReader"/>, which the serializer gives the whole
/// value: a union reads ahead on a copy of the reader to choose its case.
/// </summary>
internal sealed class PureQLConverter<T>(ReadValue<T> read, Action<Utf8JsonWriter, T> write)
    : JsonConverter<T>
{
    private readonly ReadValue<T> _read = read;

    private readonly Action<Utf8JsonWriter, T> _write = write;

    public override T Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return _read(ref reader, new ReadContext());
    }

    public override void Write(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        _write(writer, value);
    }
}
