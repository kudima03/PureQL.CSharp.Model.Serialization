using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed class ArrayEqualityConverter : JsonConverter<ArrayEquality>
{
    public override ArrayEquality Read(
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
                out BooleanArrayEquality? booleanArray
            )
                ? new ArrayEquality(booleanArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateArrayEquality? dateArray
            )
                ? new ArrayEquality(dateArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeArrayEquality? dateTimeArray
            )
                ? new ArrayEquality(dateTimeArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberArrayEquality? numberArray
            )
                ? new ArrayEquality(numberArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringArrayEquality? stringArray
            )
                ? new ArrayEquality(stringArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out TimeArrayEquality? timeArray
            )
                ? new ArrayEquality(timeArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out UuidArrayEquality? uuidArray
            )
                ? new ArrayEquality(uuidArray!)
            : throw new JsonException("Unable to determine ArrayEquality type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        ArrayEquality value,
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
