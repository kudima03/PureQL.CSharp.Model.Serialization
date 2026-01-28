using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class StringArrayReturningConverter : JsonConverter<StringArrayReturning>
{
    public override StringArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out StringField? field)
                ? new StringArrayReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out IStringArrayScalar? scalar)
                ? new StringArrayReturning(new StringArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringArrayParameter? parameter
            )
                ? new StringArrayReturning(parameter!)
            : throw new JsonException("Unable to determine StringArrayReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out StringArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out StringField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out StringArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize<IStringArrayScalar>(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine StringArrayReturning type.");
        }
    }
}
