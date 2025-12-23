using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

internal sealed record JsonExtensions
{
    internal static bool TryDeserialize<T>(
        JsonElement element,
        JsonSerializerOptions options,
        out T? result
    )
    {
        try
        {
            result = JsonSerializer.Deserialize<T>(element.GetRawText(), options);
            return result != null;
        }
        catch (JsonException)
        {
            result = default;
            return false;
        }
    }
}
