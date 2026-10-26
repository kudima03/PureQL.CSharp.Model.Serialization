using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// Reads what the specification dispatches on. Every node declares its <c>type</c>, so
/// operator nodes are told apart by <c>operator</c> and the declared type, leaves by
/// their shape and declared type (select items and group keys by the type of their
/// expression), and operands by the family of the type they declare.
/// </summary>
internal static class Discriminators
{
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
        ("param", "param"),
        ("key", "key"),
        ("subquery", "column"),
        ("expression", "item"),
        ("value", "literal"),
    ];

    /// <summary>
    /// The properties that hold the operands an operator takes its nullability from:
    /// not a <c>condition</c>, <c>digits</c> or <c>predicate</c>.
    /// </summary>
    private static readonly string[] OperandProperties =
    [
        "values",
        "operand",
        "left",
        "right",
        "then",
        "else",
        "selector",
    ];

    public static string Operator(string name, string typeName, bool nullable)
    {
        return nullable ? $"{name} {typeName}?" : $"{name} {typeName}";
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
            if (name.ValueKind != JsonValueKind.String)
            {
                throw JsonErrors.At($"{path}.operator", "Expected a string");
            }

            (string operatorType, bool operatorNullable) = DeclaredType(element, path);
            return Operator(name.GetString()!, operatorType, operatorNullable);
        }

        string kind =
            LeafKind(element)
            ?? throw JsonErrors.At(path, "Expected an operator node or a reference");

        // A select item or group key has no type of its own: it has the type its
        // expression declares.
        if (kind == "item")
        {
            if (
                !element.TryGetProperty("expression", out JsonElement expression)
                || (expression.ValueKind != JsonValueKind.Object)
            )
            {
                throw JsonErrors.At(path, "Missing property 'expression'");
            }

            (string itemType, bool itemNullable) = DeclaredType(
                expression,
                $"{path}.expression"
            );
            return Leaf(kind, itemType, itemNullable);
        }

        (string typeName, bool nullable) = DeclaredType(element, path);
        return Leaf(kind, typeName, nullable);
    }

    private static (string Name, bool Nullable) DeclaredType(
        JsonElement element,
        string path
    )
    {
        return element.ValueKind != JsonValueKind.Object
                ? throw JsonErrors.At(
                    path,
                    $"Expected an object, found {element.ValueKind}"
                )
            : element.TryGetProperty("type", out JsonElement type)
                ? Type(type, $"{path}.type")
            : throw JsonErrors.At(path, "Missing property 'type'");
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
                continue;
            }

            // A non-null type is written without 'nullable': one spelling per type.
            nullable = property.Value.GetBoolean()
                ? true
                : throw JsonErrors.At(
                    path,
                    "A non-null type is written without 'nullable'"
                );
        }

        return name is null
            ? throw JsonErrors.At(path, "Missing type name")
            : (name, nullable);
    }

    /// <summary>The operands of an operator node that has been read.</summary>
    public static IEnumerable<JsonElement> Operands(JsonElement element)
    {
        foreach (string name in OperandProperties)
        {
            if (!element.TryGetProperty(name, out JsonElement operand))
            {
                continue;
            }

            if (operand.ValueKind != JsonValueKind.Array)
            {
                yield return operand;
                continue;
            }

            foreach (JsonElement item in operand.EnumerateArray())
            {
                yield return item;
            }
        }
    }

    /// <summary>Whether an operand that has been read declares a nullable type.</summary>
    public static bool IsNullable(JsonElement operand)
    {
        return operand.TryGetProperty("type", out JsonElement type)
            && type.TryGetProperty("nullable", out _);
    }

    /// <summary>
    /// The family of an operand: that of the type it declares. Nothing else is
    /// validated, the case that is chosen reads the operand in full.
    /// </summary>
    public static TypeFamily? Probe(JsonElement element)
    {
        return
            element.ValueKind == JsonValueKind.Object
            && element.TryGetProperty("type", out JsonElement type)
            && type.ValueKind == JsonValueKind.Object
            && type.TryGetProperty("name", out JsonElement typeName)
            && typeName.ValueKind == JsonValueKind.String
            && TypeFamilies.TryGetValue(typeName.GetString()!, out TypeFamily family)
            ? family
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
