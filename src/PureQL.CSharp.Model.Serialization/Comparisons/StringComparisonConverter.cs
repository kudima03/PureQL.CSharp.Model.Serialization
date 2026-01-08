using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Comparisons;
using PureQL.CSharp.Model.Returnings;
using StringComparison = PureQL.CSharp.Model.Comparisons.StringComparison;

namespace PureQL.CSharp.Model.Serialization.Comparisons;

internal sealed record StringComparisonJsonModel
{
    public StringComparisonJsonModel(StringComparison comparison)
        : this(
            (ComparisonOperatorJsonModel)(int)comparison.Operator + 1,
            comparison.Left,
            comparison.Right
        )
    { }

    [JsonConstructor]
    public StringComparisonJsonModel(
        ComparisonOperatorJsonModel @operator,
        StringReturning left,
        StringReturning right
    )
    {
        Operator = @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public ComparisonOperatorJsonModel Operator { get; }

    public StringReturning Left { get; }

    public StringReturning Right { get; }
}

internal sealed class StringComparisonConverter : JsonConverter<StringComparison>
{
    public override StringComparison Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringComparisonJsonModel comparison =
            JsonSerializer.Deserialize<StringComparisonJsonModel>(ref reader, options)!;

        return comparison.Operator == ComparisonOperatorJsonModel.None
            ? throw new JsonException()
            : new StringComparison(
                (ComparisonOperator)(int)comparison.Operator - 1,
                comparison.Left,
                comparison.Right
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringComparison value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StringComparisonJsonModel(value), options);
    }
}
