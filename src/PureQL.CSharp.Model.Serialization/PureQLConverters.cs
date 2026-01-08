using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Serialization.BooleanOperations;
using PureQL.CSharp.Model.Serialization.Comparisons;
using PureQL.CSharp.Model.Serialization.Equalities;
using PureQL.CSharp.Model.Serialization.Fields;
using PureQL.CSharp.Model.Serialization.Parameters;
using PureQL.CSharp.Model.Serialization.Returnings;
using PureQL.CSharp.Model.Serialization.Scalars;
using PureQL.CSharp.Model.Serialization.Types;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization;

public sealed record PureQLConverters : IEnumerable<JsonConverter>
{
    public IEnumerator<JsonConverter> GetEnumerator()
    {
        yield return new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);
        yield return new AndOperatorConverter();
        yield return new BooleanOperatorConverter();
        yield return new NotOperatorConverter();
        yield return new OrOperatorConverter();
        yield return new BooleanEqualityConverter();
        yield return new DateEqualityConverter();
        yield return new DateTimeEqualityConverter();
        yield return new EqualityConverter();
        yield return new NumberEqualityConverter();
        yield return new StringEqualityConverter();
        yield return new TimeEqualityConverter();
        yield return new UuidEqualityConverter();
        yield return new BooleanFieldConverter();
        yield return new DateFieldConverter();
        yield return new DateTimeFieldConverter();
        yield return new FieldConverter();
        yield return new NumberFieldConverter();
        yield return new StringFieldConverter();
        yield return new TimeFieldConverter();
        yield return new UuidFieldConverter();
        yield return new BooleanParameterConverter();
        yield return new DateParameterConverter();
        yield return new DateTimeParameterConverter();
        yield return new NullParameterConverter();
        yield return new NumberParameterConverter();
        yield return new StringParameterConverter();
        yield return new TimeParameterConverter();
        yield return new UuidParameterConverter();
        yield return new BooleanReturningConverter();
        yield return new DateReturningConverter();
        yield return new DateTimeReturningConverter();
        yield return new NumberReturningConverter();
        yield return new StringReturningConverter();
        yield return new TimeReturningConverter();
        yield return new UuidReturningConverter();
        yield return new BooleanScalarConverter();
        yield return new DateScalarConverter();
        yield return new DateTimeScalarConverter();
        yield return new NullScalarConverter();
        yield return new NumberScalarConverter();
        yield return new StringScalarConverter();
        yield return new TimeScalarConverter();
        yield return new UuidScalarConverter();
        yield return new DateComparisonConverter();
        yield return new DateTimeComparisonConverter();
        yield return new NumberComparisonConverter();
        yield return new TypeConverter<BooleanType>();
        yield return new TypeConverter<DateType>();
        yield return new TypeConverter<DateTimeType>();
        yield return new TypeConverter<NullType>();
        yield return new TypeConverter<NumberType>();
        yield return new TypeConverter<StringType>();
        yield return new TypeConverter<TimeType>();
        yield return new TypeConverter<UuidType>();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
