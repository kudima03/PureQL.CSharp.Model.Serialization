using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

public sealed class BooleanOperatorConverter
    : JsonConverter<Model.BooleanOperations.BooleanOperator>
{
    public override Model.BooleanOperations.BooleanOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out AndOperator? andOperator)
                ? new Model.BooleanOperations.BooleanOperator(andOperator!)
            : JsonExtensions.TryDeserialize(root, options, out OrOperator? orOperator)
                ? new Model.BooleanOperations.BooleanOperator(orOperator!)
            : JsonExtensions.TryDeserialize(root, options, out NotOperator? notOperator)
                ? new Model.BooleanOperations.BooleanOperator(notOperator!)
            : throw new JsonException("Unable to determine BooleanOperator type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Model.BooleanOperations.BooleanOperator value,
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
        else
        {
            throw new JsonException("Unable to determine BooleanOperator type.");
        }
    }
}
