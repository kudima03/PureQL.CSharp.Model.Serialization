using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.EachEqualities;

namespace PureQL.CSharp.Model.Serialization.EachEqualities;

internal sealed class EachBooleanEqualityConverter : JsonConverter<EachBooleanEquality>
{
    public override EachBooleanEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachBooleanEqualityJsonModel model =
            JsonSerializer.Deserialize<EachBooleanEqualityJsonModel>(ref reader, options)!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachBooleanEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachBooleanEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachBooleanEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachNumberEqualityConverter : JsonConverter<EachNumberEquality>
{
    public override EachNumberEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachNumberEqualityJsonModel model =
            JsonSerializer.Deserialize<EachNumberEqualityJsonModel>(ref reader, options)!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachNumberEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachNumberEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachNumberEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachStringEqualityConverter : JsonConverter<EachStringEquality>
{
    public override EachStringEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachStringEqualityJsonModel model =
            JsonSerializer.Deserialize<EachStringEqualityJsonModel>(ref reader, options)!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachStringEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachStringEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachStringEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachDateEqualityConverter : JsonConverter<EachDateEquality>
{
    public override EachDateEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateEqualityJsonModel model =
            JsonSerializer.Deserialize<EachDateEqualityJsonModel>(ref reader, options)!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachDateEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachDateEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachTimeEqualityConverter : JsonConverter<EachTimeEquality>
{
    public override EachTimeEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachTimeEqualityJsonModel model =
            JsonSerializer.Deserialize<EachTimeEqualityJsonModel>(ref reader, options)!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachTimeEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachTimeEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachTimeEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachDateTimeEqualityConverter : JsonConverter<EachDateTimeEquality>
{
    public override EachDateTimeEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateTimeEqualityJsonModel model =
            JsonSerializer.Deserialize<EachDateTimeEqualityJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachDateTimeEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateTimeEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachDateTimeEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachUuidEqualityConverter : JsonConverter<EachUuidEquality>
{
    public override EachUuidEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachUuidEqualityJsonModel model =
            JsonSerializer.Deserialize<EachUuidEqualityJsonModel>(ref reader, options)!;

        return model.Operator != EachEqualityOperatorName.eachEqual
            ? throw new JsonException()
            : new EachUuidEquality(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachUuidEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachUuidEqualityJsonModel(value),
            options
        );
    }
}

internal sealed class EachEqualityConverter : JsonConverter<EachEquality>
{
    public override EachEquality Read(
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
                out EachBooleanEquality? booleanEq
            )
                ? new EachEquality(booleanEq!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachNumberEquality? numberEq
            )
                ? new EachEquality(numberEq!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachStringEquality? stringEq
            )
                ? new EachEquality(stringEq!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateEquality? dateEq
            )
                ? new EachEquality(dateEq!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachTimeEquality? timeEq
            )
                ? new EachEquality(timeEq!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateTimeEquality? dateTimeEq
            )
                ? new EachEquality(dateTimeEq!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachUuidEquality? uuidEq
            )
                ? new EachEquality(uuidEq!)
            : throw new JsonException("Unable to determine EachEquality type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachEquality value,
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
            throw new JsonException("Unable to determine EachEquality type.");
        }
    }
}
