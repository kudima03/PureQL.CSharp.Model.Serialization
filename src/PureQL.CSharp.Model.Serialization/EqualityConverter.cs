using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.Equalities;

namespace PureQL.CSharp.Model.Serialization;

internal sealed class EqualityConverter : JsonConverter<Equality>
{
    public override Equality Read(
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
                out SingleValueEquality? single
            )
                ? new Equality(single!)
            : JsonExtensions.TryDeserialize(root, options, out ArrayEquality? array)
                ? new Equality(array!)
            : throw new JsonException("Unable to determine Equality type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Equality value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out SingleValueEquality? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out ArrayEquality? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException("Unable to determine Equality type.");
        }
    }
}
