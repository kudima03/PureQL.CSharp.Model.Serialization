using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

public sealed class DateReturningConverter : JsonConverter<DateReturning>
{
    public override DateReturning? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out DateField? boolean)
                ? new DateReturning(boolean!)
            : JsonExtensions.TryDeserialize(root, options, out DateParameter? parameter)
                ? new DateReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out IDateScalar? scalar)
                ? new DateReturning(new DateScalar(scalar!.Value))
            : throw new JsonException("Unable to determine BooleanReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateReturning value,
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
            JsonSerializer.Serialize<IDateScalar>(writer, value.AsT2, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
