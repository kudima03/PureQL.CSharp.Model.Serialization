using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

public sealed class TimeReturningConverter : JsonConverter<TimeReturning>
{
    public override TimeReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out TimeField? field)
                ? new TimeReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out TimeParameter? parameter)
                ? new TimeReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out ITimeScalar? scalar)
                ? new TimeReturning(new TimeScalar(scalar!.Value))
            : throw new JsonException("Unable to determine BooleanReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.IsT0)
        {
            JsonSerializer.Serialize(writer, value.AsT0, options);
        }
        else if (value.IsT1)
        {
            JsonSerializer.Serialize(writer, value.AsT1, options);
        }
        else if (value.IsT2)
        {
            JsonSerializer.Serialize<ITimeScalar>(writer, value.AsT2, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
