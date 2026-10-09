using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

internal static class JsonErrors
{
    public static JsonException At(string path, string message)
    {
        return new JsonException($"{message} (at {path})", path, null, null);
    }
}
