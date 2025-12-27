using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

public sealed class NumberReturningConverter : JsonConverter<NumberReturning>
{
    public override NumberReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out NumberField? field)
                ? new NumberReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out NumberParameter? parameter)
                ? new NumberReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out INumberScalar? scalar)
                ? new NumberReturning(new NumberScalar(scalar!.Value))
            : throw new JsonException("Unable to determine BooleanReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberReturning value,
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
            JsonSerializer.Serialize<INumberScalar>(writer, value.AsT2, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
