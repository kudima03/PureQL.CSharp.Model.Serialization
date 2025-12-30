using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record NumberEqualityJsonModel
{
    public NumberEqualityJsonModel(NumberEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public NumberEqualityJsonModel(
        EqualityOperator @operator,
        NumberReturning left,
        NumberReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public NumberReturning Left { get; }

    public NumberReturning Right { get; }
}

public sealed class NumberEqualityConverter : JsonConverter<NumberEquality>
{
    public override NumberEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NumberEqualityJsonModel equality =
            JsonSerializer.Deserialize<NumberEqualityJsonModel>(ref reader, options)!;

        return new NumberEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NumberEqualityJsonModel(value), options);
    }
}
