using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.BooleanOperations;

internal sealed record NotOperatorJsonModel
{
    public NotOperatorJsonModel(NotOperator @operator)
        : this(BooleanOperator.Not, @operator.Condition) { }

    [JsonConstructor]
    public NotOperatorJsonModel(BooleanOperator @operator, BooleanReturning condition)
    {
        Operator =
            @operator == BooleanOperator.None ? throw new JsonException() : @operator;
        Condition = condition ?? throw new JsonException();
    }

    public BooleanOperator Operator { get; }

    public BooleanReturning Condition { get; }
}

public sealed class NotOperatorConverter : JsonConverter<NotOperator>
{
    public override NotOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NotOperatorJsonModel booleanOperator =
            JsonSerializer.Deserialize<NotOperatorJsonModel>(ref reader, options)!;

        return booleanOperator.Operator != BooleanOperator.Not
            ? throw new JsonException()
            : new NotOperator(booleanOperator.Condition);
    }

    public override void Write(
        Utf8JsonWriter writer,
        NotOperator value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NotOperatorJsonModel(value), options);
    }
}
