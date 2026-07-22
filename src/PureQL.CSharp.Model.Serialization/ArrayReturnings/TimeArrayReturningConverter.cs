using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachTimeArithmetics;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class TimeArrayReturningConverter : JsonConverter<TimeArrayReturning>
{
    public override TimeArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out TimeField? field)
                ? new TimeArrayReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out ITimeArrayScalar? scalar)
                ? new TimeArrayReturning(new TimeArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out TimeArrayParameter? parameter
            )
                ? new TimeArrayReturning(parameter!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachTimeAddSeconds? addSeconds
            )
                ? new TimeArrayReturning(addSeconds!)
            : throw new JsonException("Unable to determine TimeArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeArrayReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            parameter => JsonSerializer.Serialize(writer, parameter, options),
            field => JsonSerializer.Serialize(writer, field, options),
            scalar => JsonSerializer.Serialize<ITimeArrayScalar>(writer, scalar, options),
            addSeconds => JsonSerializer.Serialize(writer, addSeconds, options)
        );
    }
}
