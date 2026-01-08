using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using StringComparison = PureQL.CSharp.Model.Comparisons.StringComparison;

namespace PureQL.CSharp.Model.Serialization.Comparisons;

internal sealed class ComparisonConverter : JsonConverter<Comparison>
{
    public override Comparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out DateComparison? date)
                ? new Comparison(date!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeComparison? dateTime
            )
                ? new Comparison(dateTime!)
            : JsonExtensions.TryDeserialize(root, options, out NumberComparison? number)
                ? new Comparison(number!)
            : JsonExtensions.TryDeserialize(root, options, out TimeComparison? time)
                ? new Comparison(time!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringComparison? stringModel
            )
                ? new Comparison(stringModel!)
            : throw new JsonException("Unable to determine Comparison type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Comparison value,
        JsonSerializerOptions options
    )
    {
        if (value.IsT0)
        {
            JsonSerializer.Serialize(writer, value.AsT0, options);
        }
        else if (value.IsT1)
        {
            JsonSerializer.Serialize(writer, value.AsT1, options);
        }
        else if (value.IsT2)
        {
            JsonSerializer.Serialize(writer, value.AsT2, options);
        }
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(writer, value.AsT3, options);
        }
        else if (value.IsT4)
        {
            JsonSerializer.Serialize(writer, value.AsT4, options);
        }
        else
        {
            throw new JsonException("Unable to determine Comparison type.");
        }
    }
}
