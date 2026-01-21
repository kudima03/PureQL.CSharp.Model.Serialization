using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Comparisons;

internal sealed record NumberComparisonJsonModel
{
    public NumberComparisonJsonModel(NumberComparison comparison)
        : this(
            (ComparisonOperatorJsonModel)(int)comparison.Operator + 1,
            comparison.Left,
            comparison.Right
        )
    { }

    [JsonConstructor]
    public NumberComparisonJsonModel(
        ComparisonOperatorJsonModel @operator,
        NumberReturning left,
        NumberReturning right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public ComparisonOperatorJsonModel Operator { get; }

    public NumberReturning Left { get; }

    public NumberReturning Right { get; }
}

internal sealed class NumberComparisonConverter : JsonConverter<NumberComparison>
{
    public override NumberComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberComparisonJsonModel comparison =
            JsonSerializer.Deserialize<NumberComparisonJsonModel>(ref reader, options)!;

        return comparison.Operator == ComparisonOperatorJsonModel.None
            ? throw new JsonException()
            : new NumberComparison(
                (ComparisonOperator)(int)comparison.Operator - 1,
                comparison.Left,
                comparison.Right
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumberComparisonJsonModel(value), options);
    }
}
