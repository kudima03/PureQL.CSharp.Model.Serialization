using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Finds the id of a name that a JSON string spells, without allocating a string for
/// it: ids are consecutive from the first one, and <c>0</c> is no name.
/// </summary>
internal sealed class NameTable
{
    /// <summary>Longer strings, escaped or not, are none of the names.</summary>
    private const int MaxBytes = 128;

    private readonly Dictionary<string, int>.AlternateLookup<ReadOnlySpan<char>> _ids;

    public NameTable(IReadOnlyList<string> names, int firstId)
    {
        Dictionary<string, int> ids = [with(StringComparer.Ordinal)];
        for (int i = 0; i < names.Count; i++)
        {
            ids.Add(names[i], firstId + i);
        }

        _ids = ids.GetAlternateLookup<ReadOnlySpan<char>>();
    }

    /// <summary>The id of the string the reader is on, or <c>0</c>.</summary>
    public int Find(ref Utf8JsonReader reader)
    {
        long length = reader.HasValueSequence
            ? reader.ValueSequence.Length
            : reader.ValueSpan.Length;
        if (length > MaxBytes)
        {
            return 0;
        }

        Span<char> text = stackalloc char[MaxBytes];
        int written = reader.CopyString(text);
        return _ids.TryGetValue(text[..written], out int id) ? id : 0;
    }
}
