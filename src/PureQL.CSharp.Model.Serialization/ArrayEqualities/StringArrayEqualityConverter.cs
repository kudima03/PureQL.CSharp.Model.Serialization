using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayEqualities;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.Serialization.Equalities;

namespace PureQL.CSharp.Model.Serialization.ArrayEqualities;

internal sealed record StringArrayEqualityJsonModel
{
    public StringArrayEqualityJsonModel(StringArrayEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public StringArrayEqualityJsonModel(
        EqualityOperator @operator,
        StringArrayReturning left,
        StringArrayReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public StringArrayReturning Left { get; }

    public StringArrayReturning Right { get; }
}

internal sealed class StringArrayEqualityConverter : JsonConverter<StringArrayEquality>
{
    public override StringArrayEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringArrayEqualityJsonModel equality =
            JsonSerializer.Deserialize<StringArrayEqualityJsonModel>(
                ref reader,
                options
            )!;

        return new StringArrayEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringArrayEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new StringArrayEqualityJsonModel(value),
            options
        );
    }
}
