using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization.Arithmetics;

internal enum ArithmeticOperator
{
    None,
    Add,
    Subtract,
    Multiply,
    Divide,
}

internal sealed record ArithmeticOperatorJsonModel
{
    public ArithmeticOperatorJsonModel(Add add)
        : this(ArithmeticOperator.Add, add.Arguments) { }

    public ArithmeticOperatorJsonModel(Subtract subtract)
        : this(ArithmeticOperator.Subtract, subtract.Arguments) { }

    public ArithmeticOperatorJsonModel(Multiply multiply)
        : this(ArithmeticOperator.Multiply, multiply.Arguments) { }

    public ArithmeticOperatorJsonModel(Divide divide)
        : this(ArithmeticOperator.Divide, divide.Arguments) { }

    [JsonConstructor]
    public ArithmeticOperatorJsonModel(
        ArithmeticOperator @operator,
        IEnumerable<NumberReturning> arguments
    )
    {
        Operator = @operator;
        Arguments = arguments ?? throw new JsonException();
    }

    public ArithmeticOperator Operator { get; }

    public IEnumerable<NumberReturning> Arguments { get; }
}
