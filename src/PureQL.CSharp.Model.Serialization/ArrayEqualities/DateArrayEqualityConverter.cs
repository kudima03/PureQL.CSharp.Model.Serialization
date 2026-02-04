using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record DateArrayEqualityJsonModel
{
    public DateArrayEqualityJsonModel(DateArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public DateArrayEqualityJsonModel(
        EqualityOperator @operator,
        DateArrayReturning left,
        DateArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public DateArrayReturning Left { get; }

    public DateArrayReturning Right { get; }
}

internal sealed class DateArrayEqualityConverter : JsonConverter<DateArrayEquality>
{
    public override DateArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        DateArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<DateArrayEqualityJsonModel>(ref reader, options)!;

        return new DateArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new DateArrayEqualityJsonModel(value), options);
    }
}
