using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachDateTimeArithmetics;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachDateTimeArithmetics;

internal enum EachDateTimeAddSecondsOperatorName
{
    None,
    eachDatetimeAddSeconds,
}

internal sealed record EachDateTimeAddSecondsJsonModel
{
    public EachDateTimeAddSecondsJsonModel(EachDateTimeAddSeconds value)
        : this(
            EachDateTimeAddSecondsOperatorName.eachDatetimeAddSeconds,
            value.Left,
            value.Right
        )
    { }

    [JsonConstructor]
    public EachDateTimeAddSecondsJsonModel(
        EachDateTimeAddSecondsOperatorName @operator,
        OneOf<DateTimeReturning, DateTimeArrayReturning> left,
        OneOf<NumberReturning, NumberArrayReturning> right
    )
    {
        Operator =
            @operator == EachDateTimeAddSecondsOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left;
        Right = right;
    }

    public EachDateTimeAddSecondsOperatorName Operator { get; }

    public OneOf<DateTimeReturning, DateTimeArrayReturning> Left { get; }

    public OneOf<NumberReturning, NumberArrayReturning> Right { get; }
}

internal sealed class EachDateTimeAddSecondsConverter
    : JsonConverter<EachDateTimeAddSeconds>
{
    public override EachDateTimeAddSeconds Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateTimeAddSecondsJsonModel model =
            JsonSerializer.Deserialize<EachDateTimeAddSecondsJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator != EachDateTimeAddSecondsOperatorName.eachDatetimeAddSeconds
            ? throw new JsonException()
            : new EachDateTimeAddSeconds(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateTimeAddSeconds value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachDateTimeAddSecondsJsonModel(value),
            options
        );
    }
}

internal enum EachDateTimeDiffSecondsOperatorName
{
    None,
    eachDatetimeDiffSeconds,
}

internal sealed record EachDateTimeDiffSecondsJsonModel
{
    public EachDateTimeDiffSecondsJsonModel(EachDateTimeDiffSeconds value)
        : this(
            EachDateTimeDiffSecondsOperatorName.eachDatetimeDiffSeconds,
            value.Left,
            value.Right
        )
    { }

    [JsonConstructor]
    public EachDateTimeDiffSecondsJsonModel(
        EachDateTimeDiffSecondsOperatorName @operator,
        OneOf<DateTimeReturning, DateTimeArrayReturning> left,
        OneOf<DateTimeReturning, DateTimeArrayReturning> right
    )
    {
        Operator =
            @operator == EachDateTimeDiffSecondsOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left;
        Right = right;
    }

    public EachDateTimeDiffSecondsOperatorName Operator { get; }

    public OneOf<DateTimeReturning, DateTimeArrayReturning> Left { get; }

    public OneOf<DateTimeReturning, DateTimeArrayReturning> Right { get; }
}

internal sealed class EachDateTimeDiffSecondsConverter
    : JsonConverter<EachDateTimeDiffSeconds>
{
    public override EachDateTimeDiffSeconds Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateTimeDiffSecondsJsonModel model =
            JsonSerializer.Deserialize<EachDateTimeDiffSecondsJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator
            != EachDateTimeDiffSecondsOperatorName.eachDatetimeDiffSeconds
            ? throw new JsonException()
            : new EachDateTimeDiffSeconds(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateTimeDiffSeconds value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachDateTimeDiffSecondsJsonModel(value),
            options
        );
    }
}
