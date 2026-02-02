using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

internal sealed class BooleanOperatorArgumentsConverter
    : JsonConverter<OneOf<IEnumerable<BooleanReturning>, BooleanArrayReturning>>
{
    public override OneOf<IEnumerable<BooleanReturning>, BooleanArrayReturning> Read(
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
                ? OneOf<IEnumerable<BooleanReturning>, BooleanArrayReturning>.FromT1(booleanArray!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out IEnumerable<BooleanReturning>? returnings
            )
                ? OneOf<IEnumerable<BooleanReturning>, BooleanArrayReturning>.FromT0(returnings!) :
             throw new JsonException("Unable to determine BooleanOperatorArguments type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<IEnumerable<BooleanReturning>, BooleanArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out IEnumerable<BooleanReturning>? returnings, out _))
        {
            JsonSerializer.Serialize(writer, returnings, options);
        }
        else if (value.TryPickT1(out BooleanArrayReturning? booleanArray, out _))
        {
            JsonSerializer.Serialize(writer, booleanArray, options);
        }
        else
        {
            throw new JsonException("Unable to determine BooleanOperatorArguments type.");
        }
    }
}
