using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class UuidArrayReturningConverter : JsonConverter<UuidArrayReturning>
{
    public override UuidArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out UuidField? field)
                ? new UuidArrayReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out IUuidArrayScalar? scalar)
                ? new UuidArrayReturning(new UuidArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out UuidArrayParameter? parameter
            )
                ? new UuidArrayReturning(parameter!)
            : throw new JsonException("Unable to determine UuidArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out UuidArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out UuidField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out UuidArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize<IUuidArrayScalar>(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine UuidArrayReturning type.");
        }
    }
}
