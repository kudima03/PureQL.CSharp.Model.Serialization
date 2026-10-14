using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal static class FieldRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Field(new TypeBoolean(), (s, f) => new FieldBoolean(s, f));
        registry.Field(
            new TypeBooleanNullable(),
            (s, f) => new FieldBooleanNullable(s, f)
        );
        registry.Field(new TypeDate(), (s, f) => new FieldDate(s, f));
        registry.Field(new TypeDateNullable(), (s, f) => new FieldDateNullable(s, f));
        registry.Field(new TypeDatetime(), (s, f) => new FieldDatetime(s, f));
        registry.Field(
            new TypeDatetimeNullable(),
            (s, f) => new FieldDatetimeNullable(s, f)
        );
        registry.Field(new TypeDecimal(), (s, f) => new FieldDecimal(s, f));
        registry.Field(
            new TypeDecimalNullable(),
            (s, f) => new FieldDecimalNullable(s, f)
        );
        registry.Field(new TypeInteger(), (s, f) => new FieldInteger(s, f));
        registry.Field(
            new TypeIntegerNullable(),
            (s, f) => new FieldIntegerNullable(s, f)
        );
        registry.Field(new TypeString(), (s, f) => new FieldString(s, f));
        registry.Field(new TypeStringNullable(), (s, f) => new FieldStringNullable(s, f));
        registry.Field(new TypeTime(), (s, f) => new FieldTime(s, f));
        registry.Field(new TypeTimeNullable(), (s, f) => new FieldTimeNullable(s, f));
        registry.Field(new TypeUuid(), (s, f) => new FieldUuid(s, f));
        registry.Field(new TypeUuidNullable(), (s, f) => new FieldUuidNullable(s, f));
    }
}
