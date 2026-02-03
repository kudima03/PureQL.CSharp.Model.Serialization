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
        if (value.TryPickT0(out BooleanArrayReturning? booleanArray, out _))
        {
            JsonSerializer.Serialize(writer, booleanArray, options);
        }
        else if (value.TryPickT1(out DateArrayReturning? dateArray, out _))
        {
            JsonSerializer.Serialize(writer, dateArray, options);
        }
        else if (value.TryPickT2(out DateTimeArrayReturning? dateTimeArray, out _))
        {
            JsonSerializer.Serialize(writer, dateTimeArray, options);
        }
        else if (value.TryPickT3(out NumberArrayReturning? numberArray, out _))
        {
            JsonSerializer.Serialize(writer, numberArray, options);
        }
        else if (value.TryPickT4(out StringArrayReturning? stringArray, out _))
        {
            JsonSerializer.Serialize(writer, stringArray, options);
        }
        else if (value.TryPickT5(out TimeArrayReturning? timeArray, out _))
        {
            JsonSerializer.Serialize(writer, timeArray, options);
        }
        else if (value.TryPickT6(out UuidArrayReturning? uuidArray, out _))
        {
            JsonSerializer.Serialize(writer, uuidArray, options);
        }
        else
        {
            throw new JsonException("Unable to determine ArrayReturning type.");
        }
    }
}
