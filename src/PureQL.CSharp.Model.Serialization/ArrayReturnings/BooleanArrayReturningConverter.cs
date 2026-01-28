using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class BooleanArrayReturningConverter
    : JsonConverter<BooleanArrayReturning>
{
    public override BooleanArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out BooleanField? field)
                ? new BooleanArrayReturning(field!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out IBooleanArrayScalar? scalar
            )
                ? new BooleanArrayReturning(new BooleanArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out BooleanArrayParameter? parameter
            )
                ? new BooleanArrayReturning(parameter!)
            : throw new JsonException("Unable to determine BooleanArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out BooleanArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize(writer, scalar, options);
        }
        if (value.TryPickT1(out BooleanField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        if (value.TryPickT2(out BooleanArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else
        {
            throw new JsonException("Unable to determine BooleanArrayReturning type.");
        }
    }
}
