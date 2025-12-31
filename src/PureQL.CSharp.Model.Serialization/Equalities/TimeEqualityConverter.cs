using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record TimeEqualityJsonModel
{
    public TimeEqualityJsonModel(TimeEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public TimeEqualityJsonModel(
        EqualityOperator @operator,
        TimeReturning left,
        TimeReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public TimeReturning Left { get; }

    public TimeReturning Right { get; }
}

public sealed class TimeEqualityConverter : JsonConverter<TimeEquality>
{
    public override TimeEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TimeEqualityJsonModel equality =
            JsonSerializer.Deserialize<TimeEqualityJsonModel>(ref reader, options)!;

        return new TimeEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TimeEqualityJsonModel(value), options);
    }
}
