using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record NumberArrayEqualityJsonModel
{
    public NumberArrayEqualityJsonModel(NumberArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public NumberArrayEqualityJsonModel(
        EqualityOperator @operator,
        NumberArrayReturning left,
        NumberArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public NumberArrayReturning Left { get; }

    public NumberArrayReturning Right { get; }
}

internal sealed class NumberArrayEqualityConverter : JsonConverter<NumberArrayEquality>
{
    public override NumberArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<NumberArrayEqualityJsonModel>(
                ref reader,
                options
            )!;

        return new NumberArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new NumberArrayEqualityJsonModel(value),
            options
        );
    }
}
