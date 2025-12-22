using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.Fields;

public sealed class FieldConverter : JsonConverter<Field>
{
    public override Field? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return TryDeserialize(root, options, out BooleanFieldJsonModel? boolean)
                ? new Field(new BooleanField(boolean!.Entity, boolean.Field))
            : TryDeserialize(root, options, out DateFieldJsonModel? date)
                ? new Field(new DateField(date!.Entity, date.Field))
            : TryDeserialize(root, options, out DateTimeFieldJsonModel? dateTime)
                ? new Field(new DateTimeField(dateTime!.Entity, dateTime.Field))
            : TryDeserialize(root, options, out NumberFieldJsonModel? number)
                ? new Field(new NumberField(number!.Entity, number.Field))
            : TryDeserialize(root, options, out TimeFieldJsonModel? time)
                ? new Field(new TimeField(time!.Entity, time.Field))
            : TryDeserialize(root, options, out UuidFieldJsonModel? uuid)
                ? new Field(new UuidField(uuid!.Entity, uuid.Field))
            : TryDeserialize(root, options, out StringFieldJsonModel? stringModel)
                ? new Field(new StringField(stringModel!.Entity, stringModel.Field))
            : throw new JsonException("Unable to determine Field type.");
    }

    private static bool TryDeserialize<T>(
        JsonElement element,
        JsonSerializerOptions options,
        out T? result
    )
    {
        try
        {
            result = JsonSerializer.Deserialize<T>(element.GetRawText(), options);
            return result != null;
        }
        catch (JsonException)
        {
            result = default;
            return false;
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        Field value,
        JsonSerializerOptions options
    )
    {
        if (value.IsT0)
        {
            JsonSerializer.Serialize(
                writer,
                new BooleanFieldJsonModel(value.AsT0),
                options
            );
        }
        else if (value.IsT1)
        {
            JsonSerializer.Serialize(writer, new DateFieldJsonModel(value.AsT1), options);
        }
        else if (value.IsT2)
        {
            JsonSerializer.Serialize(
                writer,
                new DateTimeFieldJsonModel(value.AsT2),
                options
            );
        }
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(
                writer,
                new NumberFieldJsonModel(value.AsT3),
                options
            );
        }
        else if (value.IsT4)
        {
            JsonSerializer.Serialize(writer, new TimeFieldJsonModel(value.AsT4), options);
        }
        else if (value.IsT5)
        {
            JsonSerializer.Serialize(writer, new UuidFieldJsonModel(value.AsT5), options);
        }
        else if (value.IsT6)
        {
            JsonSerializer.Serialize(
                writer,
                new StringFieldJsonModel(value.AsT6),
                options
            );
        }
    }
}
