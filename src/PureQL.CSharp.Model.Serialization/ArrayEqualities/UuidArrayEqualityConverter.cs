using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record UuidArrayEqualityJsonModel
{
    public UuidArrayEqualityJsonModel(UuidArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public UuidArrayEqualityJsonModel(
        EqualityOperator @operator,
        UuidArrayReturning left,
        UuidArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public UuidArrayReturning Left { get; }

    public UuidArrayReturning Right { get; }
}

internal sealed class UuidArrayEqualityConverter : JsonConverter<UuidArrayEquality>
{
    public override UuidArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        UuidArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<UuidArrayEqualityJsonModel>(ref reader, options)!;

        return new UuidArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        UuidArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new UuidArrayEqualityJsonModel(value), options);
    }
}
