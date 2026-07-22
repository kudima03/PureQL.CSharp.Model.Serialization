using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachDateArithmetics;
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
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateAddDays? addDays
            )
                ? new DateArrayReturning(addDays!)
            : throw new JsonException("Unable to determine DateArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateArrayReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            parameter => JsonSerializer.Serialize(writer, parameter, options),
            field => JsonSerializer.Serialize(writer, field, options),
            scalar => JsonSerializer.Serialize<IDateArrayScalar>(writer, scalar, options),
            addDays => JsonSerializer.Serialize(writer, addDays, options)
        );
    }
}
