using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class NumberArrayReturningConverter : JsonConverter<NumberArrayReturning>
{
    public override NumberArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out NumberField? field)
                ? new NumberArrayReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out INumberArrayScalar? scalar)
                ? new NumberArrayReturning(new NumberArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberArrayParameter? parameter
            )
                ? new NumberArrayReturning(parameter!)
            : throw new JsonException("Unable to determine NumberArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out NumberArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out NumberField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out NumberArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize<INumberArrayScalar>(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine NumberArrayReturning type.");
        }
    }
}
