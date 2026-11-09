using System.Reflection;
using System.Runtime.CompilerServices;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Keys;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Generator;

/// <summary>How a record property is read and written.</summary>
internal enum ValueKind
{
    Union,
    OptionalUnion,
    Record,
    Many,
    OptionalMany,
    Strings,
    OptionalStrings,
    String,
    OptionalString,
    Over,
    Direction,
    JoinKind,
    Literal,
    LiteralList,
    NullLiteral,
}

/// <param name="Name">The model property, <c>Values</c>.</param>
/// <param name="Json">The JSON property, the camelCase constructor parameter.</param>
/// <param name="Type">The value type, or the item type of a list.</param>
/// <param name="MinItems">The schema's <c>minItems</c> of a list.</param>
/// <param name="Literal">The <c>LiteralValues</c> member of a literal value.</param>
internal sealed record PropertyModel(
    string Name,
    string Json,
    ValueKind Kind,
    Type Type,
    int MinItems,
    string? Literal
)
{
    public bool IsRequired =>
        Kind
            is not (
                ValueKind.OptionalUnion
                or ValueKind.OptionalMany
                or ValueKind.OptionalStrings
                or ValueKind.OptionalString
                or ValueKind.Over
                or ValueKind.Direction
            );
}

/// <param name="Kind">The <c>Kinds</c> member: the operator, or the leaf kind.</param>
/// <param name="DeclaredType">The <c>TypeIds</c> member of the declared type.</param>
/// <param name="Family">The <c>Families</c> member of a comparison's operands.</param>
/// <param name="Arguments">The JSON names of the constructor's parameters, in order.</param>
/// <param name="NullableCause">
/// The condition under which a record declared nullable has a nullable type, as C#
/// over the locals of its reader, or <see langword="null"/> when it always has one.
/// </param>
internal sealed record RecordModel(
    Type Type,
    string Kind,
    string? DeclaredType,
    bool Nullable,
    string? Family,
    IReadOnlyList<PropertyModel> Properties,
    IReadOnlyList<string> Arguments,
    string? NullableCause
)
{
    public bool IsOperator => Kind is not ("Unknown" or "Field" or "Param" or "Key" or "Column" or "Literal");

    /// <summary>The <c>Signatures</c> constant of a case of a union.</summary>
    public string Signature =>
        IsOperator ? ModelCatalog.WithoutContext(Type.Name) : Type.Name;
}

internal sealed record UnionModel(Type Type, IReadOnlyList<Type> Cases);

/// <summary>
/// The model's records and unions, read by reflection from the referenced
/// <c>PureQL.CSharp.Model</c> assembly, with what the specification adds to them: the
/// operator a record's name starts with, its declared type, the <c>minItems</c> of its
/// lists, and what makes an operator declared nullable nullable.
/// </summary>
internal sealed class ModelCatalog
{
    /// <summary>The operators of the specification, in its order.</summary>
    public static readonly string[] Operators =
    [
        "add",
        "subtract",
        "multiply",
        "divide",
        "integerDivide",
        "modulo",
        "floor",
        "ceiling",
        "round",
        "concat",
        "dateAddDays",
        "dateDiffDays",
        "timeAddSeconds",
        "timeDiffSeconds",
        "datetimeAddSeconds",
        "datetimeDiffSeconds",
        "and",
        "or",
        "not",
        "equal",
        "notEqual",
        "in",
        "greaterThan",
        "lessThan",
        "greaterThanOrEqual",
        "lessThanOrEqual",
        "if",
        "coalesce",
        "count",
        "sum",
        "average",
        "min",
        "max",
        "any",
        "all",
    ];

    /// <summary>The operators chosen by the type family of an operand too.</summary>
    private static readonly string[] Comparisons =
    [
        "equal",
        "notEqual",
        "in",
        "greaterThan",
        "lessThan",
        "greaterThanOrEqual",
        "lessThanOrEqual",
    ];

    private static readonly string[] Families =
    [
        "Decimal",
        "String",
        "Boolean",
        "Date",
        "Time",
        "Datetime",
        "Uuid",
    ];

    private static readonly string[] Contexts = ["Row", "Projection", "Group", "Total"];

    /// <summary>The operands an operator declared nullable takes its type from.</summary>
    private static readonly string[] Operands =
    [
        "values",
        "operand",
        "left",
        "right",
        "then",
        "else",
        "selector",
    ];

    /// <summary>The schema's <c>minItems</c> of every list property.</summary>
    private static readonly Dictionary<string, int> MinItems = new Dictionary<
        string,
        int
    >(StringComparer.Ordinal)
    {
        ["aggregate"] = 1,
        ["conditions"] = 2,
        ["distinctBy"] = 1,
        ["groupBy"] = 1,
        ["joins"] = 1,
        ["orderBy"] = 1,
        ["select"] = 1,
        ["subqueries"] = 1,
        ["values"] = 2,
    };

    private static readonly Dictionary<Type, string> LiteralValues = new Dictionary<
        Type,
        string
    >
    {
        [typeof(long)] = "Integer",
        [typeof(decimal)] = "Decimal",
        [typeof(string)] = "String",
        [typeof(bool)] = "Boolean",
        [typeof(DateOnly)] = "Date",
        [typeof(TimeOnly)] = "Time",
        [typeof(DateTimeOffset)] = "Datetime",
        [typeof(Guid)] = "Uuid",
    };

    public ModelCatalog(Assembly model)
    {
        Type[] types =
        [
            .. model
                .GetExportedTypes()
                .Where(t => t.Namespace != typeof(IType).Namespace)
                .OrderBy(t => t.FullName, StringComparer.Ordinal),
        ];
        Unions = [.. types.Where(IsUnion).Select(t => new UnionModel(t, Cases(t)))];
        Records = [.. types.Where(t => t.IsClass && t.IsSealed).Select(Record)];
        if (types.Select(t => t.Name).Distinct(StringComparer.Ordinal).Count() != types.Length)
        {
            throw new InvalidOperationException("Model type names are not unique.");
        }
    }

    public IReadOnlyList<UnionModel> Unions { get; }

    public IReadOnlyList<RecordModel> Records { get; }

    public static bool IsUnion(Type type)
    {
        return type.IsValueType && typeof(IUnion).IsAssignableFrom(type);
    }

    public static string WithoutContext(string name)
    {
        foreach (string context in Contexts)
        {
            if (name.EndsWith(context, StringComparison.Ordinal))
            {
                return name[..^context.Length];
            }
        }

        throw new InvalidOperationException($"{name} names no context.");
    }

    public static string Pascal(string name)
    {
        return char.ToUpperInvariant(name[0]) + name[1..];
    }

    private static Type[] Cases(Type union)
    {
        return
        [
            .. union
                .GetConstructors()
                .Select(c => c.GetParameters())
                .Where(p => p.Length == 1)
                .Select(p => p[0].ParameterType),
        ];
    }

    private static RecordModel Record(Type type)
    {
        ConstructorInfo constructor = type.GetConstructors().Single();
        ParameterInfo[] parameters = constructor.GetParameters();
        PropertyInfo[] properties =
        [
            .. type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType != typeof(IType))
                .OrderBy(p => p.MetadataToken),
        ];
        bool literal = typeof(ILiteral).IsAssignableFrom(type);
        List<PropertyModel> models = [];
        foreach (PropertyInfo property in properties)
        {
            string json = char.ToLowerInvariant(property.Name[0]) + property.Name[1..];
            ParameterInfo parameter =
                parameters.SingleOrDefault(p => p.Name == json)
                ?? throw new InvalidOperationException(
                    $"{type.Name}.{property.Name} has no constructor parameter."
                );
            models.Add(Property(type, property, json, parameter.HasDefaultValue, literal));
        }

        if (literal && models.Count == 0)
        {
            models.Add(
                new PropertyModel("Value", "value", ValueKind.NullLiteral, typeof(void), 0, null)
            );
        }

        IType? declared = DeclaredType(type);
        string kind = Kind(type);
        string? family = Family(type, kind);
        RecordModel record = new RecordModel(
            type,
            kind,
            declared is null ? null : TypeId(declared),
            declared?.Nullable ?? false,
            family,
            models,
            [.. parameters.Select(p => p.Name!)],
            null
        );
        return record with { NullableCause = NullableCause(record) };
    }

    private static PropertyModel Property(
        Type record,
        PropertyInfo property,
        string json,
        bool optional,
        bool literal
    )
    {
        Type type = property.PropertyType;
        Type? item =
            type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                ? type.GetGenericArguments()[0]
                : null;
        Type? nullable = Nullable.GetUnderlyingType(type);
        bool literalValue = literal && (json == "value");
        (ValueKind kind, Type valueType) = type switch
        {
            _ when literalValue && LiteralValues.ContainsKey(type) => (
                ValueKind.Literal,
                type
            ),
            _ when literalValue && (item is not null) && LiteralValues.ContainsKey(item) => (
                ValueKind.LiteralList,
                item
            ),
            _ when type == typeof(string) => (
                optional ? ValueKind.OptionalString : ValueKind.String,
                type
            ),
            _ when type == typeof(AggregateOver) => (ValueKind.Over, type),
            _ when type == typeof(SortDirection) => (ValueKind.Direction, type),
            _ when type == typeof(JoinKind) => (ValueKind.JoinKind, type),
            _ when (nullable is not null) && IsUnion(nullable) => (
                ValueKind.OptionalUnion,
                nullable
            ),
            _ when IsUnion(type) && !optional => (ValueKind.Union, type),
            _ when item == typeof(string) => (
                optional ? ValueKind.OptionalStrings : ValueKind.Strings,
                item
            ),
            _ when (item is not null) && (IsUnion(item) || item.IsClass) => (
                optional ? ValueKind.OptionalMany : ValueKind.Many,
                item
            ),
            _ when type.IsClass && type.IsSealed && !optional => (ValueKind.Record, type),
            _ => throw new InvalidOperationException(
                $"{record.Name}.{property.Name} has an unknown kind of value."
            ),
        };
        bool list =
            kind
            is ValueKind.Many
                or ValueKind.OptionalMany
                or ValueKind.Strings
                or ValueKind.OptionalStrings;
        int minItems =
            !list ? 0
            : MinItems.TryGetValue(json, out int min) ? min
            : throw new InvalidOperationException(
                $"{record.Name}.{property.Name} has no known minItems."
            );
        return kind is ValueKind.Strings or ValueKind.OptionalStrings && (minItems != 1)
            ? throw new InvalidOperationException(
                $"{record.Name}.{property.Name} is a list of names with minItems {minItems}."
            )
            : new PropertyModel(
            property.Name,
            json,
            kind,
            valueType,
            minItems,
            kind is ValueKind.Literal or ValueKind.LiteralList
                ? LiteralValues[valueType]
                : null
        );
    }

    private static IType? DeclaredType(Type type)
    {
        PropertyInfo? property = type.GetProperty("Type");
        return property?.PropertyType == typeof(IType)
            ? (IType?)property.GetValue(RuntimeHelpers.GetUninitializedObject(type))
            : null;
    }

    private static string TypeId(IType type)
    {
        return Pascal(type.Name) + (type.Nullable ? "Nullable" : "");
    }

    private static string Kind(Type type)
    {
        return type switch
        {
            _ when typeof(IOperator).IsAssignableFrom(type) => Pascal(Operator(type)),
            _ when typeof(IField).IsAssignableFrom(type) => "Field",
            _ when typeof(IParameter).IsAssignableFrom(type) => "Param",
            _ when typeof(IKey).IsAssignableFrom(type) => "Key",
            _ when typeof(ISubqueryColumn).IsAssignableFrom(type) => "Column",
            _ when typeof(ILiteral).IsAssignableFrom(type) => "Literal",
            _ => "Unknown",
        };
    }

    /// <summary>
    /// The operator a record's name starts with: the longest operator name followed by
    /// the next word of the name.
    /// </summary>
    private static string Operator(Type type)
    {
        string name = WithoutContext(type.Name);
        return Operators
                .Where(op =>
                    name.StartsWith(Pascal(op), StringComparison.Ordinal)
                    && (
                        (name.Length == op.Length) || char.IsUpper(name[op.Length])
                    )
                )
                .MaxBy(op => op.Length)
            ?? throw new InvalidOperationException($"{type.Name} names no operator.");
    }

    private static string? Family(Type type, string kind)
    {
        if (!Comparisons.Contains(char.ToLowerInvariant(kind[0]) + kind[1..]))
        {
            return null;
        }

        string family = WithoutContext(type.Name)[kind.Length..];
        return Families.Contains(family)
            ? family
            : throw new InvalidOperationException($"{type.Name} names no operand family.");
    }

    /// <summary>Whether every case of an operand union declares a nullable type.</summary>
    private static bool IsAlwaysNullable(Type operand)
    {
        return IsUnion(operand)
            && Cases(operand).All(c => DeclaredType(c)?.Nullable ?? false);
    }

    private static string? NullableCause(RecordModel record)
    {
        string op = char.ToLowerInvariant(record.Kind[0]) + record.Kind[1..];
        bool total = record.Type.Namespace!.EndsWith(".TotalExpressions", StringComparison.Ordinal);
        if (
            !record.Nullable
            || !record.IsOperator
            || (total && op is "average" or "min" or "max")
        )
        {
            return null;
        }

        List<string> causes = [];
        foreach (PropertyModel property in record.Properties)
        {
            string local = Emitter.Local(property.Json);
            if (Operands.Contains(property.Json) && IsAlwaysNullable(property.Type))
            {
                // The operand's type gives the record its nullable type.
                return null;
            }

            if (Operands.Contains(property.Json))
            {
                causes.Add(
                    property.Kind == ValueKind.Union ? $"Nullability.Of({local})"
                    : property.Kind != ValueKind.Many
                        ? throw new InvalidOperationException(
                            $"{record.Type.Name}.{property.Name} is not an operand."
                        )
                    : op == "coalesce" ? $"Nullability.All({local}!)"
                    : $"Nullability.Any({local}!)"
                );
            }
            else if (property.Json == "predicate")
            {
                causes.Add($"{local}.HasValue");
            }
        }

        return causes.Count > 0
            ? string.Join(" || ", causes)
            : throw new InvalidOperationException(
                $"{record.Type.Name} is declared nullable without an operand."
            );
    }
}
