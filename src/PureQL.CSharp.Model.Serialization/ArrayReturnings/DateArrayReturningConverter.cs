using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class DateArrayReturningConverter : JsonConverter<DateArrayReturning>
{
    public override DateArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out DateField? field)
                ? new DateArrayReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out IDateArrayScalar? scalar)
                ? new DateArrayReturning(new DateArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateArrayParameter? parameter
            )
                ? new DateArrayReturning(parameter!)
            : throw new JsonException("Unable to determine DateArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out DateArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out DateField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out DateArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine DateArrayReturning type.");
        }
    }
}
