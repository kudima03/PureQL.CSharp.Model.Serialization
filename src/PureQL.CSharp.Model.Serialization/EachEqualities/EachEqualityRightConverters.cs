using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachEqualities;

internal sealed class NumberReturningOrArrayConverter
    : JsonConverter<OneOf<NumberReturning, NumberArrayReturning>>
{
    public override OneOf<NumberReturning, NumberArrayReturning> Read(
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
                out NumberArrayReturning? array
            )
                ? OneOf<NumberReturning, NumberArrayReturning>.FromT1(array!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberReturning? single
            )
                ? OneOf<NumberReturning, NumberArrayReturning>.FromT0(single!)
            : throw new JsonException(
                "Unable to determine NumberReturningOrArray type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<NumberReturning, NumberArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out NumberReturning? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out NumberArrayReturning? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException("Unable to determine NumberReturningOrArray type.");
        }
    }
}

internal sealed class StringReturningOrArrayConverter
    : JsonConverter<OneOf<StringReturning, StringArrayReturning>>
{
    public override OneOf<StringReturning, StringArrayReturning> Read(
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
                out StringArrayReturning? array
            )
                ? OneOf<StringReturning, StringArrayReturning>.FromT1(array!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringReturning? single
            )
                ? OneOf<StringReturning, StringArrayReturning>.FromT0(single!)
            : throw new JsonException(
                "Unable to determine StringReturningOrArray type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<StringReturning, StringArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out StringReturning? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out StringArrayReturning? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException("Unable to determine StringReturningOrArray type.");
        }
    }
}

internal sealed class DateReturningOrArrayConverter
    : JsonConverter<OneOf<DateReturning, DateArrayReturning>>
{
    public override OneOf<DateReturning, DateArrayReturning> Read(
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
                out DateArrayReturning? array
            )
                ? OneOf<DateReturning, DateArrayReturning>.FromT1(array!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateReturning? single
            )
                ? OneOf<DateReturning, DateArrayReturning>.FromT0(single!)
            : throw new JsonException(
                "Unable to determine DateReturningOrArray type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<DateReturning, DateArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out DateReturning? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out DateArrayReturning? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException("Unable to determine DateReturningOrArray type.");
        }
    }
}

internal sealed class TimeReturningOrArrayConverter
    : JsonConverter<OneOf<TimeReturning, TimeArrayReturning>>
{
    public override OneOf<TimeReturning, TimeArrayReturning> Read(
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
                out TimeArrayReturning? array
            )
                ? OneOf<TimeReturning, TimeArrayReturning>.FromT1(array!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out TimeReturning? single
            )
                ? OneOf<TimeReturning, TimeArrayReturning>.FromT0(single!)
            : throw new JsonException(
                "Unable to determine TimeReturningOrArray type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<TimeReturning, TimeArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out TimeReturning? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out TimeArrayReturning? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException("Unable to determine TimeReturningOrArray type.");
        }
    }
}

internal sealed class DateTimeReturningOrArrayConverter
    : JsonConverter<OneOf<DateTimeReturning, DateTimeArrayReturning>>
{
    public override OneOf<DateTimeReturning, DateTimeArrayReturning> Read(
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
                out DateTimeArrayReturning? array
            )
                ? OneOf<DateTimeReturning, DateTimeArrayReturning>.FromT1(array!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out DateTimeReturning? single
            )
                ? OneOf<DateTimeReturning, DateTimeArrayReturning>.FromT0(single!)
            : throw new JsonException(
                "Unable to determine DateTimeReturningOrArray type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<DateTimeReturning, DateTimeArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out DateTimeReturning? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out DateTimeArrayReturning? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException(
                "Unable to determine DateTimeReturningOrArray type."
            );
        }
    }
}

internal sealed class UuidReturningOrArrayConverter
    : JsonConverter<OneOf<UuidReturning, UuidArrayReturning>>
{
    public override OneOf<UuidReturning, UuidArrayReturning> Read(
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
                out UuidArrayReturning? array
            )
                ? OneOf<UuidReturning, UuidArrayReturning>.FromT1(array!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out UuidReturning? single
            )
                ? OneOf<UuidReturning, UuidArrayReturning>.FromT0(single!)
            : throw new JsonException(
                "Unable to determine UuidReturningOrArray type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        OneOf<UuidReturning, UuidArrayReturning> value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out UuidReturning? single, out _))
        {
            JsonSerializer.Serialize(writer, single, options);
        }
        else if (value.TryPickT1(out UuidArrayReturning? array, out _))
        {
            JsonSerializer.Serialize(writer, array, options);
        }
        else
        {
            throw new JsonException("Unable to determine UuidReturningOrArray type.");
        }
    }
}
