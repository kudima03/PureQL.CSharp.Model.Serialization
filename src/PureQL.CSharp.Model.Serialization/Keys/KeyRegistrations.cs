using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Keys;

internal static class KeyRegistrations
{
    public static void Register(ModelRegistry registry)
    {
        registry.Union(
            UnionCase.Of((KeyBoolean x) => new KeyAsBooleanNullable(x)),
            UnionCase.Of((KeyBooleanNullable x) => new KeyAsBooleanNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyDate x) => new KeyAsDateNullable(x)),
            UnionCase.Of((KeyDateNullable x) => new KeyAsDateNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyDatetime x) => new KeyAsDatetimeNullable(x)),
            UnionCase.Of((KeyDatetimeNullable x) => new KeyAsDatetimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyDecimal x) => new KeyAsDecimal(x)),
            UnionCase.Of((KeyInteger x) => new KeyAsDecimal(x))
        );
        registry.Union(
            UnionCase.Of((KeyDecimal x) => new KeyAsDecimalNullable(x)),
            UnionCase.Of((KeyDecimalNullable x) => new KeyAsDecimalNullable(x)),
            UnionCase.Of((KeyInteger x) => new KeyAsDecimalNullable(x)),
            UnionCase.Of((KeyIntegerNullable x) => new KeyAsDecimalNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyInteger x) => new KeyAsIntegerNullable(x)),
            UnionCase.Of((KeyIntegerNullable x) => new KeyAsIntegerNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyString x) => new KeyAsStringNullable(x)),
            UnionCase.Of((KeyStringNullable x) => new KeyAsStringNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyTime x) => new KeyAsTimeNullable(x)),
            UnionCase.Of((KeyTimeNullable x) => new KeyAsTimeNullable(x))
        );
        registry.Union(
            UnionCase.Of((KeyUuid x) => new KeyAsUuidNullable(x)),
            UnionCase.Of((KeyUuidNullable x) => new KeyAsUuidNullable(x))
        );
        registry.Key(new TypeBoolean(), k => new KeyBoolean(k));
        registry.Key(new TypeBooleanNullable(), k => new KeyBooleanNullable(k));
        registry.Key(new TypeDate(), k => new KeyDate(k));
        registry.Key(new TypeDateNullable(), k => new KeyDateNullable(k));
        registry.Key(new TypeDatetime(), k => new KeyDatetime(k));
        registry.Key(new TypeDatetimeNullable(), k => new KeyDatetimeNullable(k));
        registry.Key(new TypeDecimal(), k => new KeyDecimal(k));
        registry.Key(new TypeDecimalNullable(), k => new KeyDecimalNullable(k));
        registry.Key(new TypeInteger(), k => new KeyInteger(k));
        registry.Key(new TypeIntegerNullable(), k => new KeyIntegerNullable(k));
        registry.Key(new TypeString(), k => new KeyString(k));
        registry.Key(new TypeStringNullable(), k => new KeyStringNullable(k));
        registry.Key(new TypeTime(), k => new KeyTime(k));
        registry.Key(new TypeTimeNullable(), k => new KeyTimeNullable(k));
        registry.Key(new TypeUuid(), k => new KeyUuid(k));
        registry.Key(new TypeUuidNullable(), k => new KeyUuidNullable(k));
    }
}
