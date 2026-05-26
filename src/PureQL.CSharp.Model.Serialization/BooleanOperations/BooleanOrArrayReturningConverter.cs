using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

internal sealed class BooleanOrArrayReturningConverter
    : JsonConverter<OneOf<BooleanReturning, BooleanArrayReturning>>
{
    public override OneOf<BooleanReturning, BooleanArrayReturning> Read(
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
                ? OneOf<BooleanReturning, BooleanArrayReturning>.FromT1(booleanArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out BooleanReturning? booleanReturning
            )
                ? OneOf<BooleanReturning, BooleanArrayReturning>.FromT0(booleanReturning!)
            : throw new JsonException(
                "Unable to determine BooleanOrArrayReturning type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<BooleanReturning, BooleanArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out BooleanReturning? booleanReturning, out _))
        {
            JsonSerializer.Serialize(writer, booleanReturning, options);
        }
        else if (value.TryPickT1(out BooleanArrayReturning? booleanArray, out _))
        {
            JsonSerializer.Serialize(writer, booleanArray, options);
        }
        else
        {
            throw new JsonException(
                "Unable to determine BooleanOrArrayReturning type."
            );
        }
    }
}
