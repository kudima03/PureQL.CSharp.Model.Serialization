using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record UuidEqualityJsonModel
{
    public UuidEqualityJsonModel(UuidEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public UuidEqualityJsonModel(
        EqualityOperator @operator,
        UuidReturning left,
        UuidReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public UuidReturning Left { get; }

    public UuidReturning Right { get; }
}

public sealed class UuidEqualityConverter : JsonConverter<UuidEquality>
{
    public override UuidEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidEqualityJsonModel equality =
            JsonSerializer.Deserialize<UuidEqualityJsonModel>(ref reader, options)!;

        return new UuidEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidEqualityJsonModel(value), options);
    }
}
