using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Equalities;

internal sealed record StringEqualityJsonModel
{
    public StringEqualityJsonModel(StringEquality equality)
        : this(EqualityOperator.Equal, equality.Left, equality.Right) { }

    [JsonConstructor]
    public StringEqualityJsonModel(
        EqualityOperator @operator,
        StringReturning left,
        StringReturning right
    )
    {
        Operator =
            @operator == EqualityOperator.None ? throw new JsonException() : @operator;
        Left = left ?? throw new JsonException();
        Right = right ?? throw new JsonException();
    }

    public EqualityOperator Operator { get; }

    public StringReturning Left { get; }

    public StringReturning Right { get; }
}

internal sealed class StringEqualityConverter : JsonConverter<StringEquality>
{
    public override StringEquality Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringEqualityJsonModel equality =
            JsonSerializer.Deserialize<StringEqualityJsonModel>(ref reader, options)!;

        return new StringEquality(equality.Left, equality.Right);
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringEquality value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StringEqualityJsonModel(value), options);
    }
}
