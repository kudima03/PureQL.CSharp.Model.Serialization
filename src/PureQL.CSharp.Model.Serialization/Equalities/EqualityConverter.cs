using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;

namespace PureQL.CSharp.Model.Serialization.Equalities;

public sealed class EqualityConverter : JsonConverter<Equality>
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
                out BooleanEquality? booleanEquality
            )
                ? new Equality(booleanEquality!)
            : JsonExtensions.TryDeserialize(root, options, out DateEquality? dateEquality)
                ? new Equality(dateEquality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeEquality? dateTimeEquality
            )
                ? new Equality(dateTimeEquality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberEquality? numberEquality
            )
                ? new Equality(numberEquality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringEquality? stringEquality
            )
                ? new Equality(stringEquality!)
            : JsonExtensions.TryDeserialize(root, options, out TimeEquality? timeEquality)
                ? new Equality(timeEquality!)
            : JsonExtensions.TryDeserialize(root, options, out UuidEquality? uuidEquality)
                ? new Equality(uuidEquality!)
            : throw new JsonException("Unable to determine Equality type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Equality value,
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
            JsonSerializer.Serialize(writer, value.AsT2, options);
        }
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(writer, value.AsT3, options);
        }
        else if (value.IsT4)
        {
            JsonSerializer.Serialize(writer, value.AsT4, options);
        }
        else if (value.IsT5)
        {
            JsonSerializer.Serialize(writer, value.AsT5, options);
        }
        else if (value.IsT6)
        {
            JsonSerializer.Serialize(writer, value.AsT6, options);
        }
        else
        {
            throw new JsonException("Unable to determine Equality type.");
        }
    }
}
