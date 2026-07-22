using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class SingleValueReturningConverter : JsonConverter<SingleValueReturning>
{
    public override SingleValueReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out BooleanReturning? boolean)
                ? new SingleValueReturning(boolean!)
            : JsonExtensions.TryDeserialize(root, options, out DateReturning? date)
                ? new SingleValueReturning(date!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeReturning? dateTime
            )
                ? new SingleValueReturning(dateTime!)
            : JsonExtensions.TryDeserialize(root, options, out TimeReturning? time)
                ? new SingleValueReturning(time!)
            : JsonExtensions.TryDeserialize(root, options, out NumberReturning? number)
                ? new SingleValueReturning(number!)
            : JsonExtensions.TryDeserialize(root, options, out UuidReturning? uuid)
                ? new SingleValueReturning(uuid!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringReturning? stringValue
            )
                ? new SingleValueReturning(stringValue!)
            : throw new JsonException("Unable to determine SingleValueReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        SingleValueReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            boolean => JsonSerializer.Serialize(writer, boolean, options),
            date => JsonSerializer.Serialize(writer, date, options),
            dateTime => JsonSerializer.Serialize(writer, dateTime, options),
            number => JsonSerializer.Serialize(writer, number, options),
            stringValue => JsonSerializer.Serialize(writer, stringValue, options),
            time => JsonSerializer.Serialize(writer, time, options),
            uuid => JsonSerializer.Serialize(writer, uuid, options)
        );
    }
}
