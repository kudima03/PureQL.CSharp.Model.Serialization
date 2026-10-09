using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization.Tests;

internal static class PureQLJson
{
    /// <summary>
    /// Options with every PureQL converter. The deepest fixtures nest past the default
    /// limit of 64, and the invalid fixtures carry a leading comment.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = CreateOptions();

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, Options);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, Options)!;
    }

    private static JsonSerializerOptions CreateOptions()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            MaxDepth = 256,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };
        foreach (JsonConverter converter in new PureQLConverters())
        {
            options.Converters.Add(converter);
        }

        return options;
    }
}
