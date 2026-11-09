using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Reads one value from <paramref name="reader"/>, which is on its first token, and
/// leaves the reader on its last token, as a <c>JsonConverter</c> does.
/// </summary>
internal delegate T ReadValue<T>(ref Utf8JsonReader reader, ReadContext context);
