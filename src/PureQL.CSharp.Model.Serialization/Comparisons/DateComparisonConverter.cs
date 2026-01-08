using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Comparisons;

internal sealed record DateComparisonJsonModel
{
    public DateComparisonJsonModel(DateComparison comparison)
        : this(
            (ComparisonOperatorJsonModel)(int)comparison.Operator + 1,
            comparison.Left,
            comparison.Right
        )
    { }

    [JsonConstructor]
    public DateComparisonJsonModel(
        ComparisonOperatorJsonModel @operator,
        DateReturning left,
        DateReturning right
    )
    {
        Operator = @operator;
        Left = left?? throw new JsonException();
        Right = right?? throw new JsonException();
    }

    public ComparisonOperatorJsonModel Operator { get; }

    public DateReturning Left { get; }

    public DateReturning Right { get; }
}

internal sealed class DateComparisonConverter : JsonConverter<DateComparison>
{
    public override DateComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateComparisonJsonModel comparison =
            JsonSerializer.Deserialize<DateComparisonJsonModel>(ref reader, options)!;

        return comparison.Operator == ComparisonOperatorJsonModel.None
            ? throw new JsonException()
            : new DateComparison(
                (ComparisonOperator)(int)comparison.Operator - 1,
                comparison.Left,
                comparison.Right
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateComparisonJsonModel(value), options);
    }
}
