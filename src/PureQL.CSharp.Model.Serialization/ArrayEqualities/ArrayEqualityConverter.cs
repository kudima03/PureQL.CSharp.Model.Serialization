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
                out BooleanArrayEquality? timeArray
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
        if (value.TryPickT0(out BooleanArrayEquality? booleanArray, out _))
        {
            JsonSerializer.Serialize(writer, booleanArray, options);
        }
        else if (value.TryPickT1(out DateArrayEquality? dateArray, out _))
        {
            JsonSerializer.Serialize(writer, dateArray, options);
        }
        else if (value.TryPickT2(out DateTimeArrayEquality? dateTimeArray, out _))
        {
            JsonSerializer.Serialize(writer, dateTimeArray, options);
        }
        else if (value.TryPickT3(out NumberArrayEquality? numberArray, out _))
        {
            JsonSerializer.Serialize(writer, numberArray, options);
        }
        else if (value.TryPickT4(out StringArrayEquality? stringArray, out _))
        {
            JsonSerializer.Serialize(writer, stringArray, options);
        }
        else if (value.TryPickT5(out TimeArrayEquality? timeArray, out _))
        {
            JsonSerializer.Serialize(writer, timeArray, options);
        }
        else if (value.TryPickT6(out UuidArrayEquality? uuidArray, out _))
        {
            JsonSerializer.Serialize(writer, uuidArray, options);
        }
        else
        {
            throw new JsonException("Unable to determine ArrayEquality type.");
        }
    }
}
