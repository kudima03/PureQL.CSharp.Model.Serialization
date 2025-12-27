using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

public sealed class StringReturningConverter : JsonConverter<StringReturning>
{
    public override StringReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out StringField? field)
                ? new StringReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out StringParameter? parameter)
                ? new StringReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out IStringScalar? scalar)
                ? new StringReturning(new StringScalar(scalar!.Value))
            : throw new JsonException("Unable to determine BooleanReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringReturning value,
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
            JsonSerializer.Serialize<IStringScalar>(writer, value.AsT2, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
