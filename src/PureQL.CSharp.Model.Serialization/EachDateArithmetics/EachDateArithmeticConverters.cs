using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachDateArithmetics;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachDateArithmetics;

internal enum EachDateAddDaysOperatorName
{
    None,
    eachDateAddDays,
}

internal sealed record EachDateAddDaysJsonModel
{
    public EachDateAddDaysJsonModel(EachDateAddDays value)
        : this(EachDateAddDaysOperatorName.eachDateAddDays, value.Left, value.Right) { }

    [JsonConstructor]
    public EachDateAddDaysJsonModel(
        EachDateAddDaysOperatorName @operator,
        OneOf<DateReturning, DateArrayReturning> left,
        OneOf<NumberReturning, NumberArrayReturning> right
    )
    {
        Operator =
            @operator == EachDateAddDaysOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left;
        Right = right;
    }

    public EachDateAddDaysOperatorName Operator { get; }

    public OneOf<DateReturning, DateArrayReturning> Left { get; }

    public OneOf<NumberReturning, NumberArrayReturning> Right { get; }
}

internal sealed class EachDateAddDaysConverter : JsonConverter<EachDateAddDays>
{
    public override EachDateAddDays Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateAddDaysJsonModel model =
            JsonSerializer.Deserialize<EachDateAddDaysJsonModel>(ref reader, options)!;

        return model.Operator != EachDateAddDaysOperatorName.eachDateAddDays
            ? throw new JsonException()
            : new EachDateAddDays(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateAddDays value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new EachDateAddDaysJsonModel(value), options);
    }
}

internal enum EachDateDiffDaysOperatorName
{
    None,
    eachDateDiffDays,
}

internal sealed record EachDateDiffDaysJsonModel
{
    public EachDateDiffDaysJsonModel(EachDateDiffDays value)
        : this(
            EachDateDiffDaysOperatorName.eachDateDiffDays,
            value.Left,
            value.Right
        )
    { }

    [JsonConstructor]
    public EachDateDiffDaysJsonModel(
        EachDateDiffDaysOperatorName @operator,
        OneOf<DateReturning, DateArrayReturning> left,
        OneOf<DateReturning, DateArrayReturning> right
    )
    {
        Operator =
            @operator == EachDateDiffDaysOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left;
        Right = right;
    }

    public EachDateDiffDaysOperatorName Operator { get; }

    public OneOf<DateReturning, DateArrayReturning> Left { get; }

    public OneOf<DateReturning, DateArrayReturning> Right { get; }
}

internal sealed class EachDateDiffDaysConverter : JsonConverter<EachDateDiffDays>
{
    public override EachDateDiffDays Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachDateDiffDaysJsonModel model =
            JsonSerializer.Deserialize<EachDateDiffDaysJsonModel>(ref reader, options)!;

        return model.Operator != EachDateDiffDaysOperatorName.eachDateDiffDays
            ? throw new JsonException()
            : new EachDateDiffDays(model.Left, model.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDateDiffDays value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new EachDateDiffDaysJsonModel(value), options);
    }
}
