using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record DateEqualityJsonModel
{
    public DateEqualityJsonModel(DateEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public DateEqualityJsonModel(
        EqualityOperator @operator,
        DateReturning left,
        DateReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public DateReturning Left { get; }

    public DateReturning Right { get; }
}

public sealed class DateEqualityConverter : JsonConverter<DateEquality>
{
    public override DateEquality? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateEqualityJsonModel equality =
            JsonSerializer.Deserialize<DateEqualityJsonModel>(ref reader, options)!;

        return new DateEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateEqualityJsonModel(value), options);
    }
}
