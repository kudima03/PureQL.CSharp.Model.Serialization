using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.EachComparisons;

namespace PureQL.CSharp.Model.Serialization.EachComparisons;

internal sealed class EachNumberComparisonConverter : JsonConverter<EachNumberComparison>
{
    public override EachNumberComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachNumberComparisonJsonModel model =
            JsonSerializer.Deserialize<EachNumberComparisonJsonModel>(
                ref reader,
                options
            )!;

        return new EachNumberComparison(model.Operator, model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachNumberComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachNumberComparisonJsonModel(value),
            options
        );
    }
}

internal sealed class EachStringComparisonConverter : JsonConverter<EachStringComparison>
{
    public override EachStringComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachStringComparisonJsonModel model =
            JsonSerializer.Deserialize<EachStringComparisonJsonModel>(
                ref reader,
                options
            )!;

        return new EachStringComparison(model.Operator, model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachStringComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachStringComparisonJsonModel(value),
            options
        );
    }
}

internal sealed class EachDateComparisonConverter : JsonConverter<EachDateComparison>
{
    public override EachDateComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateComparisonJsonModel model =
            JsonSerializer.Deserialize<EachDateComparisonJsonModel>(ref reader, options)!;

        return new EachDateComparison(model.Operator, model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachDateComparisonJsonModel(value),
            options
        );
    }
}

internal sealed class EachDateTimeComparisonConverter
    : JsonConverter<EachDateTimeComparison>
{
    public override EachDateTimeComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateTimeComparisonJsonModel model =
            JsonSerializer.Deserialize<EachDateTimeComparisonJsonModel>(
                ref reader,
                options
            )!;

        return new EachDateTimeComparison(model.Operator, model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateTimeComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachDateTimeComparisonJsonModel(value),
            options
        );
    }
}

internal sealed class EachTimeComparisonConverter : JsonConverter<EachTimeComparison>
{
    public override EachTimeComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachTimeComparisonJsonModel model =
            JsonSerializer.Deserialize<EachTimeComparisonJsonModel>(ref reader, options)!;

        return new EachTimeComparison(model.Operator, model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachTimeComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachTimeComparisonJsonModel(value),
            options
        );
    }
}

internal sealed class EachComparisonConverter : JsonConverter<EachComparison>
{
    public override EachComparison Read(
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
                out EachNumberComparison? number
            )
                ? new EachComparison(number!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachStringComparison? stringComp
            )
                ? new EachComparison(stringComp!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateComparison? date
            )
                ? new EachComparison(date!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateTimeComparison? dateTime
            )
                ? new EachComparison(dateTime!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachTimeComparison? time
            )
                ? new EachComparison(time!)
            : throw new JsonException("Unable to determine EachComparison type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachComparison value,
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
        else
        {
            throw new JsonException("Unable to determine EachComparison type.");
        }
    }
}
