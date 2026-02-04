using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record DateTimeArrayEqualityJsonModel
{
    public DateTimeArrayEqualityJsonModel(DateTimeArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public DateTimeArrayEqualityJsonModel(
        EqualityOperator @operator,
        DateTimeArrayReturning left,
        DateTimeArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public DateTimeArrayReturning Left { get; }

    public DateTimeArrayReturning Right { get; }
}

internal sealed class DateTimeArrayEqualityConverter
    : JsonConverter<DateTimeArrayEquality>
{
    public override DateTimeArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<DateTimeArrayEqualityJsonModel>(
                ref reader,
                options
            )!;

        return new DateTimeArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new DateTimeArrayEqualityJsonModel(value),
            options
        );
    }
}
