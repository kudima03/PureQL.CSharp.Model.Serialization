using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Comparisons;

internal sealed record DateTimeComparisonJsonModel
{
    public DateTimeComparisonJsonModel(DateTimeComparison comparison)
        : this(
            (ComparisonOperatorJsonModel)(int)comparison.Operator + 1,
            comparison.Left,
            comparison.Right
        )
    { }

    [JsonConstructor]
    public DateTimeComparisonJsonModel(
        ComparisonOperatorJsonModel @operator,
        DateTimeReturning left,
        DateTimeReturning right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public ComparisonOperatorJsonModel Operator { get; }

    public DateTimeReturning Left { get; }

    public DateTimeReturning Right { get; }
}

internal sealed class DateTimeComparisonConverter : JsonConverter<DateTimeComparison>
{
    public override DateTimeComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeComparisonJsonModel comparison =
            JsonSerializer.Deserialize<DateTimeComparisonJsonModel>(ref reader, options)!;

        return comparison.Operator == ComparisonOperatorJsonModel.None
            ? throw new JsonException()
            : new DateTimeComparison(
                (ComparisonOperator)(int)comparison.Operator - 1,
                comparison.Left,
                comparison.Right
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeComparisonJsonModel(value), options);
    }
}
