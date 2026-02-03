using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed class SingleValueEqualityConverter : JsonConverter<SingleValueEquality>
{
    public override SingleValueEquality Read(
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
                ? new SingleValueEquality(booleanEquality!)
            : JsonExtensions.TryDeserialize(root, options, out DateEquality? dateEquality)
                ? new SingleValueEquality(dateEquality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeEquality? dateTimeEquality
            )
                ? new SingleValueEquality(dateTimeEquality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberEquality? numberEquality
            )
                ? new SingleValueEquality(numberEquality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringEquality? stringEquality
            )
                ? new SingleValueEquality(stringEquality!)
            : JsonExtensions.TryDeserialize(root, options, out TimeEquality? timeEquality)
                ? new SingleValueEquality(timeEquality!)
            : JsonExtensions.TryDeserialize(root, options, out UuidEquality? uuidEquality)
                ? new SingleValueEquality(uuidEquality!)
            : throw new JsonException("Unable to determine SingleValueEquality type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        SingleValueEquality value,
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
            throw new JsonException("Unable to determine SingleValueEquality type.");
        }
    }
}
