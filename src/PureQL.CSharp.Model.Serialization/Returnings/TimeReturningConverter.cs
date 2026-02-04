using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class TimeReturningConverter : JsonConverter<TimeReturning>
{
    public override TimeReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out TimeParameter? parameter)
                ? new TimeReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out ITimeScalar? scalar)
                ? new TimeReturning(new TimeScalar(scalar!.Value))
            : throw new JsonException("Unable to determine TimeReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out TimeParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out TimeScalar? scalar, out _))
        {
            JsonSerializer.Serialize<ITimeScalar>(writer, scalar, options);
        }
        else
        {
            throw new JsonException("Unable to determine TimeReturning type.");
        }
    }
}
