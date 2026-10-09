using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal static class ParameterRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((ParamBoolean x) => new ParamAsBooleanNullable(x)),
            UnionCase.Of((ParamBooleanNullable x) => new ParamAsBooleanNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamDate x) => new ParamAsDateNullable(x)),
            UnionCase.Of((ParamDateNullable x) => new ParamAsDateNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamDatetime x) => new ParamAsDatetimeNullable(x)),
            UnionCase.Of((ParamDatetimeNullable x) => new ParamAsDatetimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamDecimal x) => new ParamAsDecimal(x)),
            UnionCase.Of((ParamInteger x) => new ParamAsDecimal(x))
        );
        registry.Union(
            UnionCase.Of((ParamDecimal x) => new ParamAsDecimalNullable(x)),
            UnionCase.Of((ParamDecimalNullable x) => new ParamAsDecimalNullable(x)),
            UnionCase.Of((ParamInteger x) => new ParamAsDecimalNullable(x)),
            UnionCase.Of((ParamIntegerNullable x) => new ParamAsDecimalNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamInteger x) => new ParamAsIntegerNullable(x)),
            UnionCase.Of((ParamIntegerNullable x) => new ParamAsIntegerNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamString x) => new ParamAsStringNullable(x)),
            UnionCase.Of((ParamStringNullable x) => new ParamAsStringNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamTime x) => new ParamAsTimeNullable(x)),
            UnionCase.Of((ParamTimeNullable x) => new ParamAsTimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((ParamUuid x) => new ParamAsUuidNullable(x)),
            UnionCase.Of((ParamUuidNullable x) => new ParamAsUuidNullable(x))
        );
        registry.Param(new TypeBoolean(), p => new ParamBoolean(p));
        registry.Param(new TypeBooleanNullable(), p => new ParamBooleanNullable(p));
        registry.Param(new TypeDate(), p => new ParamDate(p));
        registry.Param(new TypeDateNullable(), p => new ParamDateNullable(p));
        registry.Param(new TypeDatetime(), p => new ParamDatetime(p));
        registry.Param(new TypeDatetimeNullable(), p => new ParamDatetimeNullable(p));
        registry.Param(new TypeDecimal(), p => new ParamDecimal(p));
        registry.Param(new TypeDecimalNullable(), p => new ParamDecimalNullable(p));
        registry.Param(new TypeInteger(), p => new ParamInteger(p));
        registry.Param(new TypeIntegerNullable(), p => new ParamIntegerNullable(p));
        registry.Param(new TypeString(), p => new ParamString(p));
        registry.Param(new TypeStringNullable(), p => new ParamStringNullable(p));
        registry.Param(new TypeTime(), p => new ParamTime(p));
        registry.Param(new TypeTimeNullable(), p => new ParamTimeNullable(p));
        registry.Param(new TypeUuid(), p => new ParamUuid(p));
        registry.Param(new TypeUuidNullable(), p => new ParamUuidNullable(p));
    }
}
