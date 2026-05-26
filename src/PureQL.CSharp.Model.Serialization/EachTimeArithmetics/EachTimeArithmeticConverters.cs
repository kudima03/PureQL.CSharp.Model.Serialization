using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachTimeArithmetics;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachTimeArithmetics;

internal enum EachTimeAddSecondsOperatorName
{
    None,
    eachTimeAddSeconds,
}

internal sealed record EachTimeAddSecondsJsonModel
{
    public EachTimeAddSecondsJsonModel(EachTimeAddSeconds value)
        : this(
            EachTimeAddSecondsOperatorName.eachTimeAddSeconds,
            value.Left,
            value.Right
        )
    { }

    [JsonConstructor]
    public EachTimeAddSecondsJsonModel(
        EachTimeAddSecondsOperatorName @operator,
        OneOf<TimeReturning, TimeArrayReturning> left,
        OneOf<NumberReturning, NumberArrayReturning> right
    )
    {
        Operator =
            @operator == EachTimeAddSecondsOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left;
        Right = right;
    }

    public EachTimeAddSecondsOperatorName Operator { get; }

    public OneOf<TimeReturning, TimeArrayReturning> Left { get; }

    public OneOf<NumberReturning, NumberArrayReturning> Right { get; }
}

internal sealed class EachTimeAddSecondsConverter : JsonConverter<EachTimeAddSeconds>
{
    public override EachTimeAddSeconds Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachTimeAddSecondsJsonModel model =
            JsonSerializer.Deserialize<EachTimeAddSecondsJsonModel>(ref reader, options)!;

        return model.Operator != EachTimeAddSecondsOperatorName.eachTimeAddSeconds
            ? throw new JsonException()
            : new EachTimeAddSeconds(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachTimeAddSeconds value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachTimeAddSecondsJsonModel(value),
            options
        );
    }
}

internal enum EachTimeDiffSecondsOperatorName
{
    None,
    eachTimeDiffSeconds,
}

internal sealed record EachTimeDiffSecondsJsonModel
{
    public EachTimeDiffSecondsJsonModel(EachTimeDiffSeconds value)
        : this(
            EachTimeDiffSecondsOperatorName.eachTimeDiffSeconds,
            value.Left,
            value.Right
        )
    { }

    [JsonConstructor]
    public EachTimeDiffSecondsJsonModel(
        EachTimeDiffSecondsOperatorName @operator,
        OneOf<TimeReturning, TimeArrayReturning> left,
        OneOf<TimeReturning, TimeArrayReturning> right
    )
    {
        Operator =
            @operator == EachTimeDiffSecondsOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left;
        Right = right;
    }

    public EachTimeDiffSecondsOperatorName Operator { get; }

    public OneOf<TimeReturning, TimeArrayReturning> Left { get; }

    public OneOf<TimeReturning, TimeArrayReturning> Right { get; }
}

internal sealed class EachTimeDiffSecondsConverter : JsonConverter<EachTimeDiffSeconds>
{
    public override EachTimeDiffSeconds Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachTimeDiffSecondsJsonModel model =
            JsonSerializer.Deserialize<EachTimeDiffSecondsJsonModel>(ref reader, options)!;

        return model.Operator != EachTimeDiffSecondsOperatorName.eachTimeDiffSeconds
            ? throw new JsonException()
            : new EachTimeDiffSeconds(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachTimeDiffSeconds value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachTimeDiffSecondsJsonModel(value),
            options
        );
    }
}
