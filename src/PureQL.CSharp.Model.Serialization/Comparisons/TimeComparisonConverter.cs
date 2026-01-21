using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Comparisons;

internal sealed record TimeComparisonJsonModel
{
    public TimeComparisonJsonModel(TimeComparison comparison)
        : this(
            (ComparisonOperatorJsonModel)(int)comparison.Operator + 1,
            comparison.Left,
            comparison.Right
        )
    { }

    [JsonConstructor]
    public TimeComparisonJsonModel(
        ComparisonOperatorJsonModel @operator,
        TimeReturning left,
        TimeReturning right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public ComparisonOperatorJsonModel Operator { get; }

    public TimeReturning Left { get; }

    public TimeReturning Right { get; }
}

internal sealed class TimeComparisonConverter : JsonConverter<TimeComparison>
{
    public override TimeComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeComparisonJsonModel comparison =
            JsonSerializer.Deserialize<TimeComparisonJsonModel>(ref reader, options)!;

        return comparison.Operator == ComparisonOperatorJsonModel.None
            ? throw new JsonException()
            : new TimeComparison(
                (ComparisonOperator)(int)comparison.Operator - 1,
                comparison.Left,
                comparison.Right
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeComparisonJsonModel(value), options);
    }
}
