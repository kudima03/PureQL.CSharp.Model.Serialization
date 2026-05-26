using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachEqualities;

internal enum EachEqualityOperatorName
{
    None,
    eachEqual,
}

internal sealed record EachBooleanEqualityJsonModel
{
    public EachBooleanEqualityJsonModel(EachBooleanEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachBooleanEqualityJsonModel(
        EachEqualityOperatorName @operator,
        BooleanArrayReturning left,
        OneOf<BooleanReturning, BooleanArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public BooleanArrayReturning Left { get; }

    public OneOf<BooleanReturning, BooleanArrayReturning> Right { get; }
}

internal sealed record EachNumberEqualityJsonModel
{
    public EachNumberEqualityJsonModel(EachNumberEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachNumberEqualityJsonModel(
        EachEqualityOperatorName @operator,
        NumberArrayReturning left,
        OneOf<NumberReturning, NumberArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public NumberArrayReturning Left { get; }

    public OneOf<NumberReturning, NumberArrayReturning> Right { get; }
}

internal sealed record EachStringEqualityJsonModel
{
    public EachStringEqualityJsonModel(EachStringEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachStringEqualityJsonModel(
        EachEqualityOperatorName @operator,
        StringArrayReturning left,
        OneOf<StringReturning, StringArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public StringArrayReturning Left { get; }

    public OneOf<StringReturning, StringArrayReturning> Right { get; }
}

internal sealed record EachDateEqualityJsonModel
{
    public EachDateEqualityJsonModel(EachDateEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachDateEqualityJsonModel(
        EachEqualityOperatorName @operator,
        DateArrayReturning left,
        OneOf<DateReturning, DateArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public DateArrayReturning Left { get; }

    public OneOf<DateReturning, DateArrayReturning> Right { get; }
}

internal sealed record EachTimeEqualityJsonModel
{
    public EachTimeEqualityJsonModel(EachTimeEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachTimeEqualityJsonModel(
        EachEqualityOperatorName @operator,
        TimeArrayReturning left,
        OneOf<TimeReturning, TimeArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public TimeArrayReturning Left { get; }

    public OneOf<TimeReturning, TimeArrayReturning> Right { get; }
}

internal sealed record EachDateTimeEqualityJsonModel
{
    public EachDateTimeEqualityJsonModel(EachDateTimeEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachDateTimeEqualityJsonModel(
        EachEqualityOperatorName @operator,
        DateTimeArrayReturning left,
        OneOf<DateTimeReturning, DateTimeArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public DateTimeArrayReturning Left { get; }

    public OneOf<DateTimeReturning, DateTimeArrayReturning> Right { get; }
}

internal sealed record EachUuidEqualityJsonModel
{
    public EachUuidEqualityJsonModel(EachUuidEquality equality)
        : this(EachEqualityOperatorName.eachEqual, equality.Left, equality.Right) { }

    [JsonConstructor]
    public EachUuidEqualityJsonModel(
        EachEqualityOperatorName @operator,
        UuidArrayReturning left,
        OneOf<UuidReturning, UuidArrayReturning> right
    )
    {
        Operator =
            @operator == EachEqualityOperatorName.None
                ? throw new JsonException()
                : @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachEqualityOperatorName Operator { get; }

    public UuidArrayReturning Left { get; }

    public OneOf<UuidReturning, UuidArrayReturning> Right { get; }
}
