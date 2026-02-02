using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.String;
using PureQL.CSharp.Model.ArrayReturnings;

namespace PureQL.CSharp.Model.Serialization.Aggregates.String;

internal enum StringAggregateOperatorJsonModel
{
    None,
    min_string,
    max_string,
}

internal sealed record StringAggregateJsonModel
{
    public StringAggregateJsonModel(MinString minString)
        : this(StringAggregateOperatorJsonModel.min_string, minString.Argument) { }

    public StringAggregateJsonModel(MaxString maxString)
        : this(StringAggregateOperatorJsonModel.max_string, maxString.Argument) { }

    [JsonConstructor]
    public StringAggregateJsonModel(
        StringAggregateOperatorJsonModel @operator,
        StringArrayReturning arg
    )
    {
        Operator = @operator;
        Arg = arg ?? throw new JsonException();
    }

    public StringAggregateOperatorJsonModel Operator { get; }

    public StringArrayReturning Arg { get; }
}
