using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachDateTimeArithmetics;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class DateTimeArrayReturningConverter
    : JsonConverter<DateTimeArrayReturning>
{
    public override DateTimeArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out DateTimeField? field)
                ? new DateTimeArrayReturning(field!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out IDateTimeArrayScalar? scalar
            )
                ? new DateTimeArrayReturning(new DateTimeArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeArrayParameter? parameter
            )
                ? new DateTimeArrayReturning(parameter!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateTimeAddSeconds? addSeconds
            )
                ? new DateTimeArrayReturning(addSeconds!)
            : throw new JsonException(
                "Unable to determine DateTimeArrayReturning type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out DateTimeArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out DateTimeField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out DateTimeArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize<IDateTimeArrayScalar>(writer, scalar, options);
        }
        else if (value.TryPickT3(out EachDateTimeAddSeconds? addSeconds, out _))
        {
            JsonSerializer.Serialize(writer, addSeconds, options);
        }
        else
        {
            throw new JsonException(
                "Unable to determine DateTimeArrayReturning type."
            );
        }
    }
}
