using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Parameters;

internal static class ParameterRegistrations
{
    public static void Register(ModelRegistry registry)
    {
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
