using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization;

internal sealed class SelectExpressionConverter : JsonConverter<SelectExpression>
{
    public override SelectExpression Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out BooleanReturning? boolean)
                ? new SelectExpression(boolean!)
            : JsonExtensions.TryDeserialize(root, options, out NumberReturning? number)
                ? new SelectExpression(number!)
            : JsonExtensions.TryDeserialize(root, options, out UuidReturning? uuid)
                ? new SelectExpression(uuid!)
            : JsonExtensions.TryDeserialize(root, options, out DateReturning? date)
                ? new SelectExpression(date!)
            : JsonExtensions.TryDeserialize(root, options, out TimeReturning? time)
                ? new SelectExpression(time!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeReturning? dateTime
            )
                ? new SelectExpression(dateTime!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringReturning? stringModel
            )
                ? new SelectExpression(stringModel!)
            : throw new JsonException("Unable to determine SelectExpression type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        SelectExpression value,
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
        else if (value.IsT5)
        {
            JsonSerializer.Serialize(writer, value.AsT5, options);
        }
        else if (value.IsT6)
        {
            JsonSerializer.Serialize(writer, value.AsT6, options);
        }
        else
        {
            throw new JsonException("Unable to determine SelectExpression type.");
        }
    }
}
