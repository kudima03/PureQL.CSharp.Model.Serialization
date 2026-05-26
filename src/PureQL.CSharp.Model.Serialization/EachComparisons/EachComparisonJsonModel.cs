using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachComparisons;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachComparisons;

internal sealed record EachNumberComparisonJsonModel
{
    public EachNumberComparisonJsonModel(EachNumberComparison comparison)
        : this(comparison.Operator, comparison.Left, comparison.Right) { }

    [JsonConstructor]
    public EachNumberComparisonJsonModel(
        EachComparisonOperator @operator,
        NumberArrayReturning left,
        OneOf<NumberReturning, NumberArrayReturning> right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachComparisonOperator Operator { get; }

    public NumberArrayReturning Left { get; }

    public OneOf<NumberReturning, NumberArrayReturning> Right { get; }
}

internal sealed record EachStringComparisonJsonModel
{
    public EachStringComparisonJsonModel(EachStringComparison comparison)
        : this(comparison.Operator, comparison.Left, comparison.Right) { }

    [JsonConstructor]
    public EachStringComparisonJsonModel(
        EachComparisonOperator @operator,
        StringArrayReturning left,
        OneOf<StringReturning, StringArrayReturning> right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachComparisonOperator Operator { get; }

    public StringArrayReturning Left { get; }

    public OneOf<StringReturning, StringArrayReturning> Right { get; }
}

internal sealed record EachDateComparisonJsonModel
{
    public EachDateComparisonJsonModel(EachDateComparison comparison)
        : this(comparison.Operator, comparison.Left, comparison.Right) { }

    [JsonConstructor]
    public EachDateComparisonJsonModel(
        EachComparisonOperator @operator,
        DateArrayReturning left,
        OneOf<DateReturning, DateArrayReturning> right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachComparisonOperator Operator { get; }

    public DateArrayReturning Left { get; }

    public OneOf<DateReturning, DateArrayReturning> Right { get; }
}

internal sealed record EachDateTimeComparisonJsonModel
{
    public EachDateTimeComparisonJsonModel(EachDateTimeComparison comparison)
        : this(comparison.Operator, comparison.Left, comparison.Right) { }

    [JsonConstructor]
    public EachDateTimeComparisonJsonModel(
        EachComparisonOperator @operator,
        DateTimeArrayReturning left,
        OneOf<DateTimeReturning, DateTimeArrayReturning> right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachComparisonOperator Operator { get; }

    public DateTimeArrayReturning Left { get; }

    public OneOf<DateTimeReturning, DateTimeArrayReturning> Right { get; }
}

internal sealed record EachTimeComparisonJsonModel
{
    public EachTimeComparisonJsonModel(EachTimeComparison comparison)
        : this(comparison.Operator, comparison.Left, comparison.Right) { }

    [JsonConstructor]
    public EachTimeComparisonJsonModel(
        EachComparisonOperator @operator,
        TimeArrayReturning left,
        OneOf<TimeReturning, TimeArrayReturning> right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right;
    }

    public EachComparisonOperator Operator { get; }

    public TimeArrayReturning Left { get; }

    public OneOf<TimeReturning, TimeArrayReturning> Right { get; }
}
