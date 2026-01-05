using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record BooleanEqualityJsonModel
{
    public BooleanEqualityJsonModel(BooleanEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public BooleanEqualityJsonModel(
        EqualityOperator @operator,
        BooleanReturning left,
        BooleanReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public BooleanReturning Left { get; }

    public BooleanReturning Right { get; }
}

internal sealed class BooleanEqualityConverter : JsonConverter<BooleanEquality>
{
    public override BooleanEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanEqualityJsonModel equality =
            JsonSerializer.Deserialize<BooleanEqualityJsonModel>(ref reader, options)!;

        return new BooleanEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new BooleanEqualityJsonModel(value), options);
    }
}
