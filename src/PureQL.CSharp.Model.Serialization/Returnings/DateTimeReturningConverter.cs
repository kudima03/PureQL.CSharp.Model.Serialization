using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

public sealed class DateTimeReturningConverter : JsonConverter<DateTimeReturning>
{
    public override DateTimeReturning? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out DateTimeField? field)
                ? new DateTimeReturning(field!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeParameter? parameter
            )
                ? new DateTimeReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out IDateTimeScalar? scalar)
                ? new DateTimeReturning(new DateTimeScalar(scalar!.Value))
            : throw new JsonException("Unable to determine BooleanReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeReturning value,
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
            JsonSerializer.Serialize<IDateTimeScalar>(writer, value.AsT2, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
