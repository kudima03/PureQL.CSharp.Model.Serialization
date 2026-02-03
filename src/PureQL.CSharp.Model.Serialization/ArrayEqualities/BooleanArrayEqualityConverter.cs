using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record BooleanArrayEqualityJsonModel
{
    public BooleanArrayEqualityJsonModel(BooleanArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public BooleanArrayEqualityJsonModel(
        EqualityOperator @operator,
        BooleanArrayReturning left,
        BooleanArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public BooleanArrayReturning Left { get; }

    public BooleanArrayReturning Right { get; }
}

internal sealed class BooleanArrayEqualityConverter : JsonConverter<BooleanArrayEquality>
{
    public override BooleanArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        BooleanArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<BooleanArrayEqualityJsonModel>(
                ref reader,
                options
            )!;

        return new BooleanArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new BooleanArrayEqualityJsonModel(value),
            options
        );
    }
}
