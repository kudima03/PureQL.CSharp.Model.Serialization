using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class UuidReturningConverter : JsonConverter<UuidReturning>
{
    public override UuidReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out UuidParameter? parameter)
                ? new UuidReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out IUuidScalar? scalar)
                ? new UuidReturning(new UuidScalar(scalar!.Value))
            : throw new JsonException("Unable to determine UuidReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out UuidParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out UuidScalar? scalar, out _))
        {
            JsonSerializer.Serialize<IUuidScalar>(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine UuidReturning type.");
        }
    }
}
