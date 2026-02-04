using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record TimeArrayEqualityJsonModel
{
    public TimeArrayEqualityJsonModel(TimeArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public TimeArrayEqualityJsonModel(
        EqualityOperator @operator,
        TimeArrayReturning left,
        TimeArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public TimeArrayReturning Left { get; }

    public TimeArrayReturning Right { get; }
}

internal sealed class TimeArrayEqualityConverter : JsonConverter<TimeArrayEquality>
{
    public override TimeArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<TimeArrayEqualityJsonModel>(ref reader, options)!;

        return new TimeArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeArrayEqualityJsonModel(value), options);
    }
}
