using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class ArrayReturningConverter : JsonConverter<ArrayReturning>
{
    public override ArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(
                root,
                options,
                out BooleanArrayReturning? booleanArray
            )
                ? new ArrayReturning(booleanArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateArrayReturning? dateArray
            )
                ? new ArrayReturning(dateArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeArrayReturning? dateTimeArray
            )
                ? new ArrayReturning(dateTimeArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberArrayReturning? numberArray
            )
                ? new ArrayReturning(numberArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringArrayReturning? stringArray
            )
                ? new ArrayReturning(stringArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out TimeArrayReturning? timeArray
            )
                ? new ArrayReturning(timeArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out UuidArrayReturning? uuidArray
            )
                ? new ArrayReturning(uuidArray!)
            : throw new JsonException("Unable to determine ArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        ArrayReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            booleanArray => JsonSerializer.Serialize(writer, booleanArray, options),
            dateArray => JsonSerializer.Serialize(writer, dateArray, options),
            dateTimeArray => JsonSerializer.Serialize(writer, dateTimeArray, options),
            numberArray => JsonSerializer.Serialize(writer, numberArray, options),
            stringArray => JsonSerializer.Serialize(writer, stringArray, options),
            timeArray => JsonSerializer.Serialize(writer, timeArray, options),
            uuidArray => JsonSerializer.Serialize(writer, uuidArray, options)
        );
    }
}
