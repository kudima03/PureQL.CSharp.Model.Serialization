using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Reads what the specification dispatches on. Operator nodes are told apart by
/// <c>operator</c> (and <c>round</c> by the presence of <c>digits</c>), leaves by their
/// shape and declared <c>type</c>, and operands by the family <c>probe.*</c> reports.
/// </summary>
internal static class Discriminators
{
    private static readonly Dictionary<string, TypeFamily> OperatorFamilies =
        new Dictionary<string, TypeFamily>(StringComparer.Ordinal)
        {
            ["add"] = TypeFamily.Decimal,
            ["subtract"] = TypeFamily.Decimal,
            ["multiply"] = TypeFamily.Decimal,
            ["divide"] = TypeFamily.Decimal,
            ["integerDivide"] = TypeFamily.Decimal,
            ["modulo"] = TypeFamily.Decimal,
            ["floor"] = TypeFamily.Decimal,
            ["ceiling"] = TypeFamily.Decimal,
            ["round"] = TypeFamily.Decimal,
            ["dateDiffDays"] = TypeFamily.Decimal,
            ["timeDiffSeconds"] = TypeFamily.Decimal,
            ["datetimeDiffSeconds"] = TypeFamily.Decimal,
            ["count"] = TypeFamily.Decimal,
            ["sum"] = TypeFamily.Decimal,
            ["and"] = TypeFamily.Boolean,
            ["or"] = TypeFamily.Boolean,
            ["not"] = TypeFamily.Boolean,
            ["equal"] = TypeFamily.Boolean,
            ["notEqual"] = TypeFamily.Boolean,
            ["in"] = TypeFamily.Boolean,
            ["greaterThan"] = TypeFamily.Boolean,
            ["lessThan"] = TypeFamily.Boolean,
            ["greaterThanOrEqual"] = TypeFamily.Boolean,
            ["lessThanOrEqual"] = TypeFamily.Boolean,
            ["any"] = TypeFamily.Boolean,
            ["all"] = TypeFamily.Boolean,
            ["concat"] = TypeFamily.String,
            ["dateAddDays"] = TypeFamily.Date,
            ["timeAddSeconds"] = TypeFamily.Time,
            ["datetimeAddSeconds"] = TypeFamily.Datetime,
        };

    private static readonly Dictionary<string, TypeFamily> TypeFamilies = new Dictionary<
        string,
        TypeFamily
    >(StringComparer.Ordinal)
    {
        ["integer"] = TypeFamily.Decimal,
        ["decimal"] = TypeFamily.Decimal,
        ["string"] = TypeFamily.String,
        ["boolean"] = TypeFamily.Boolean,
        ["date"] = TypeFamily.Date,
        ["time"] = TypeFamily.Time,
        ["datetime"] = TypeFamily.Datetime,
        ["uuid"] = TypeFamily.Uuid,
    };

    /// <summary>The property that tells each kind of leaf apart, in order.</summary>
    private static readonly (string Property, string Kind)[] LeafKinds =
    [
        ("source", "field"),
        ("param_name", "param"),
        ("key", "key"),
        ("subquery", "column"),
        ("expression", "item"),
        ("value", "literal"),
    ];

    public static string Operator(string name, bool digits)
    {
        return digits ? $"{name}(digits)" : name;
    }

    public static string Leaf(string kind, string typeName, bool nullable)
    {
        return nullable ? $"{kind} {typeName}?" : $"{kind} {typeName}";
    }

    public static string Of(JsonElement element, string path)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw JsonErrors.At(path, $"Expected an object, found {element.ValueKind}");
        }

        if (element.TryGetProperty("operator", out JsonElement name))
        {
            return name.ValueKind != JsonValueKind.String
                ? throw JsonErrors.At($"{path}.operator", "Expected a string")
                : Operator(name.GetString()!, element.TryGetProperty("digits", out _));
        }

        string kind =
            LeafKind(element)
            ?? throw JsonErrors.At(path, "Expected an operator node or a reference");

        if (!element.TryGetProperty("type", out JsonElement type))
        {
            throw JsonErrors.At(path, "Missing property 'type'");
        }

        (string typeName, bool nullable) = Type(type, $"{path}.type");
        return Leaf(kind, typeName, nullable);
    }

    public static (string Name, bool Nullable) Type(JsonElement type, string path)
    {
        if (type.ValueKind != JsonValueKind.Object)
        {
            throw JsonErrors.At(path, $"Expected a type object, found {type.ValueKind}");
        }

        string? name = null;
        bool nullable = false;
        foreach (JsonProperty property in type.EnumerateObject())
        {
            JsonValueKind kind = property.Value.ValueKind;
            bool isName = property.NameEquals("name") && (kind == JsonValueKind.String);
            bool isNullable =
                property.NameEquals("nullable")
                && kind is JsonValueKind.True or JsonValueKind.False;
            if (!isName && !isNullable)
            {
                throw JsonErrors.At(path, $"Unexpected type property '{property.Name}'");
            }

            if (isName)
            {
                name = property.Value.GetString();
            }
            else
            {
                nullable = property.Value.GetBoolean();
            }
        }

        return name is null
            ? throw JsonErrors.At(path, "Missing type name")
            : (name, nullable);
    }

    /// <summary>
    /// The family of an operand as <c>probe.*</c> reads it, without validating it:
    /// a leaf's <c>type.name</c>, a fixed-type operator, or the operand that carries the
    /// type (<c>if.then</c>, <c>coalesce.values[0]</c>, an aggregate's <c>selector</c>).
    /// </summary>
    public static TypeFamily? Probe(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        if (!element.TryGetProperty("operator", out JsonElement name))
        {
            return
                element.TryGetProperty("type", out JsonElement type)
                && type.ValueKind == JsonValueKind.Object
                && type.TryGetProperty("name", out JsonElement typeName)
                && typeName.ValueKind == JsonValueKind.String
                && TypeFamilies.TryGetValue(typeName.GetString()!, out TypeFamily family)
                ? family
                : null;
        }

        if (name.ValueKind != JsonValueKind.String)
        {
            return null;
        }

        string op = name.GetString()!;
        return OperatorFamilies.TryGetValue(op, out TypeFamily fixedFamily)
            ? fixedFamily
            : SpineFamily(element, op);
    }

    private static TypeFamily? SpineFamily(JsonElement element, string op)
    {
        return op switch
        {
            "if" => ProbeProperty(element, "then"),
            "coalesce" => ProbeFirst(element, "values"),
            "min" or "max" or "average" => ProbeProperty(element, "selector"),
            _ => null,
        };
    }

    private static TypeFamily? ProbeProperty(JsonElement element, string name)
    {
        return element.TryGetProperty(name, out JsonElement operand)
            ? Probe(operand)
            : null;
    }

    private static TypeFamily? ProbeFirst(JsonElement element, string name)
    {
        return
            element.TryGetProperty(name, out JsonElement operands)
            && operands.ValueKind == JsonValueKind.Array
            && operands.GetArrayLength() > 0
            ? Probe(operands[0])
            : null;
    }

    private static string? LeafKind(JsonElement element)
    {
        foreach ((string property, string kind) in LeafKinds)
        {
            if (element.TryGetProperty(property, out _))
            {
                return kind;
            }
        }

        return null;
    }
}
