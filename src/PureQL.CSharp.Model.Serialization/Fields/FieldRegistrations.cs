using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Fields;

internal static class FieldRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((FieldBoolean x) => new FieldAsBooleanNullable(x)),
            UnionCase.Of((FieldBooleanNullable x) => new FieldAsBooleanNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldDate x) => new FieldAsDateNullable(x)),
            UnionCase.Of((FieldDateNullable x) => new FieldAsDateNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldDatetime x) => new FieldAsDatetimeNullable(x)),
            UnionCase.Of((FieldDatetimeNullable x) => new FieldAsDatetimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldDecimal x) => new FieldAsDecimal(x)),
            UnionCase.Of((FieldInteger x) => new FieldAsDecimal(x))
        );
        registry.Union(
            UnionCase.Of((FieldDecimal x) => new FieldAsDecimalNullable(x)),
            UnionCase.Of((FieldDecimalNullable x) => new FieldAsDecimalNullable(x)),
            UnionCase.Of((FieldInteger x) => new FieldAsDecimalNullable(x)),
            UnionCase.Of((FieldIntegerNullable x) => new FieldAsDecimalNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldInteger x) => new FieldAsIntegerNullable(x)),
            UnionCase.Of((FieldIntegerNullable x) => new FieldAsIntegerNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldString x) => new FieldAsStringNullable(x)),
            UnionCase.Of((FieldStringNullable x) => new FieldAsStringNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldTime x) => new FieldAsTimeNullable(x)),
            UnionCase.Of((FieldTimeNullable x) => new FieldAsTimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((FieldUuid x) => new FieldAsUuidNullable(x)),
            UnionCase.Of((FieldUuidNullable x) => new FieldAsUuidNullable(x))
        );
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
