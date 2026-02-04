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
        if (value.TryPickT0(out BooleanReturning? boolean, out _))
        {
            JsonSerializer.Serialize(writer, boolean, options);
        }
        else if (value.TryPickT1(out DateReturning? date, out _))
        {
            JsonSerializer.Serialize(writer, date, options);
        }
        else if (value.TryPickT2(out DateTimeReturning? dateTime, out _))
        {
            JsonSerializer.Serialize(writer, dateTime, options);
        }
        else if (value.TryPickT3(out NumberReturning? number, out _))
        {
            JsonSerializer.Serialize(writer, number, options);
        }
        else if (value.TryPickT4(out StringReturning? stringValue, out _))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else if (value.TryPickT5(out TimeReturning? time, out _))
        {
            JsonSerializer.Serialize(writer, time, options);
        }
        else if (value.TryPickT6(out UuidReturning? uuid, out _))
        {
            JsonSerializer.Serialize(writer, uuid, options);
        }
        else
        {
            throw new JsonException("Unable to determine SingleValueReturning type.");
        }
    }
}
