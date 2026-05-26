using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.Aggregates.Numeric;

internal enum CountOperatorJsonModel
{
    None,
    count,
}

internal sealed record CountJsonModel
{
    public CountJsonModel(Count count)
        : this(CountOperatorJsonModel.count, count.Argument) { }

    [JsonConstructor]
    public CountJsonModel(CountOperatorJsonModel @operator, ArrayReturning arg)
    {
        Operator =
            @operator == CountOperatorJsonModel.None ? throw new JsonException() : @operator;
        Arg = arg ?? throw new JsonException();
    }

    public CountOperatorJsonModel Operator { get; }

    public ArrayReturning Arg { get; }
}

internal sealed class CountConverter : JsonConverter<Count>
{
    public override Count Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        CountJsonModel model =
            JsonSerializer.Deserialize<CountJsonModel>(ref reader, options)!;

        return model.Operator != CountOperatorJsonModel.count
            ? throw new JsonException()
            : new Count(model.Arg);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Count value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new CountJsonModel(value), options);
    }
}
