using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class NumberReturningConverter : JsonConverter<NumberReturning>
{
    public override NumberReturning Read(
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
                out NumberParameter? parameter
            )
                ? new NumberReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out INumberScalar? scalar)
                ? new NumberReturning(new NumberScalar(scalar!.Value))
            : throw new JsonException("Unable to determine NumberReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out NumberParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out NumberScalar? scalar, out _))
        {
            JsonSerializer.Serialize<INumberScalar>(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine NumberReturning type.");
        }
    }
}
