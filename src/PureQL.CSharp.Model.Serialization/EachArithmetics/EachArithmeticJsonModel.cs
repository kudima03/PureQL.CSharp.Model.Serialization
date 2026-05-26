using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.EachArithmetics;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.EachArithmetics;

internal enum EachArithmeticOperatorName
{
    None,
    eachAdd,
    eachSubtract,
    eachMultiply,
    eachDivide,
}

internal sealed record EachArithmeticOperatorJsonModel
{
    public EachArithmeticOperatorJsonModel(EachAdd add)
        : this(EachArithmeticOperatorName.eachAdd, add.Values) { }

    public EachArithmeticOperatorJsonModel(EachSubtract subtract)
        : this(EachArithmeticOperatorName.eachSubtract, subtract.Values) { }

    public EachArithmeticOperatorJsonModel(EachMultiply multiply)
        : this(EachArithmeticOperatorName.eachMultiply, multiply.Values) { }

    public EachArithmeticOperatorJsonModel(EachDivide divide)
        : this(EachArithmeticOperatorName.eachDivide, divide.Values) { }

    [JsonConstructor]
    public EachArithmeticOperatorJsonModel(
        EachArithmeticOperatorName @operator,
        IEnumerable<OneOf<NumberReturning, NumberArrayReturning>> values
    )
    {
        Operator =
            @operator == EachArithmeticOperatorName.None
                ? throw new JsonException()
                : @operator;
        Values = values ?? throw new JsonException();
    }

    public EachArithmeticOperatorName Operator { get; }

    public IEnumerable<OneOf<NumberReturning, NumberArrayReturning>> Values { get; }
}
