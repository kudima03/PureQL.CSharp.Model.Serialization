using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record DateTimeEqualityJsonModel
{
    public DateTimeEqualityJsonModel(DateTimeEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public DateTimeEqualityJsonModel(
        EqualityOperator @operator,
        DateTimeReturning left,
        DateTimeReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public DateTimeReturning Left { get; }

    public DateTimeReturning Right { get; }
}

internal sealed class DateTimeEqualityConverter : JsonConverter<DateTimeEquality>
{
    public override DateTimeEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateTimeEqualityJsonModel equality =
            JsonSerializer.Deserialize<DateTimeEqualityJsonModel>(ref reader, options)!;

        return new DateTimeEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateTimeEqualityJsonModel(value), options);
    }
}
