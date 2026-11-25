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
    ManyOfMany,
    Strings,
    OptionalStrings,
    String,
    OptionalString,
    Over,
    Direction,
    JoinKind,
    Form,
    Mode,
    Flag,
    Literal,
    LiteralList,
    NullLiteral,
}

/// <summary>What chooses the case of an operator beyond its declared type.</summary>
internal enum Choice
{
    None,

    /// <summary>The family of the type <c>left</c> declares.</summary>
    Left,

    /// <summary>The family of the type <c>operand</c> declares.</summary>
    Operand,

    /// <summary>Whether <c>digits</c> is present.</summary>
    Digits,
}

/// <param name="Name">The model property, <c>Values</c>.</param>
/// <param name="Json">The JSON property, the camelCase constructor parameter.</param>
/// <param name="Type">The value type, or the item type of a list.</param>
/// <param name="MinItems">The schema's <c>minItems</c> of a list.</param>
/// <param name="MaxItems">The schema's <c>maxItems</c> of a list, 0 for none.</param>
/// <param name="Literal">The <c>LiteralValues</c> member of a literal value.</param>
/// <param name="Check">The <c>Checks</c> member that checks the value, if any.</param>
internal sealed record PropertyModel(
    string Name,
    string Json,
    ValueKind Kind,
    Type Type,
    int MinItems,
    int MaxItems,
    string? Literal,
    string? Check
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
                or ValueKind.Mode
                or ValueKind.Flag
            );
}

/// <param name="Kind">The <c>Kinds</c> member: the operator, or the leaf kind.</param>
/// <param name="DeclaredType">The <c>TypeIds</c> member of the declared type.</param>
/// <param name="Family">
/// The <c>Families</c> member of what else chooses the operator (see
/// <see cref="Choice"/>): the family of an operand, or whether <c>digits</c> is present.
/// </param>
/// <param name="Arguments">The JSON names of the constructor's parameters, in order.</param>
/// <param name="NullableCause">
/// The condition under which a record declared nullable has a nullable type, as C#
/// over the locals of its reader, or <see langword="null"/> when it always has one.
/// </param>
/// <param name="DecimalCause">
/// The condition under which a record declared <c>decimal</c> (or <c>decimal?</c>),
/// of an operator that also has an <c>integer</c> record, has a <c>decimal</c> type:
/// one of its number operands declares one. <see langword="null"/> when its operands
/// are always decimals.
/// </param>
/// <param name="AtLeastOne">
/// Whether the record has only optional properties, of which the schema requires one
/// (<c>minProperties: 1</c>).
/// </param>
/// <param name="Check">The <c>Checks</c> member that checks the record, if any.</param>
internal sealed record RecordModel(
    Type Type,
    string Kind,
    string? DeclaredType,
    bool Nullable,
    string? Family,
    IReadOnlyList<PropertyModel> Properties,
    IReadOnlyList<string> Arguments,
    string? NullableCause,
    string? DecimalCause,
    bool AtLeastOne,
    string? Check
)
{
    /// <summary>
    /// The condition a non-null <c>coalesce</c> needs: exactly one non-null operand, as
    /// one after it would never be reached (<c>maxContains: 1</c>).
    /// </summary>
    public string? NonNullCause =>
        (Kind == "Coalesce") && !Nullable ? "Nullability.OneNonNull(values!)" : null;

    public bool IsOperator => Kind is not ("Unknown" or "Field" or "Param" or "Key" or "Column" or "Literal");

    /// <summary>The <c>Signatures</c> constant of a case of a union.</summary>
    public string Signature =>
        IsOperator ? ModelCatalog.WithoutContext(Type.Name) : Type.Name;
}

/// <param name="Records">The cases that are records.</param>
/// <param name="Count">Whether a bare number (<c>long</c>) is a case.</param>
/// <param name="All">Whether the constant <c>"all"</c> is a case.</param>
/// <param name="Check">The <c>Checks</c> member that checks the value, if any.</param>
internal sealed record UnionModel(
    Type Type,
    IReadOnlyList<Type> Records,
    bool Count,
    bool All,
    string? Check
);

/// <summary>
/// The model's records and unions, read by reflection from the referenced
/// <c>PureQL.CSharp.Model</c> assembly, with what the specification adds to them: the
/// operator a record's name starts with, its declared type and what else chooses it,
/// the <c>minItems</c> of its lists, what makes an operator declared nullable nullable,
/// and the rules of the schema on values that the model's types do not express.
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
        "truncate",
        "abs",
        "sign",
        "power",
        "sqrt",
        "exp",
        "ln",
        "log10",
        "log",
        "sin",
        "cos",
        "tan",
        "asin",
        "acos",
        "atan",
        "atan2",
        "pi",
        "concat",
        "length",
        "upper",
        "lower",
        "foldCase",
        "normalize",
        "removeDiacritics",
        "trim",
        "trimStart",
        "trimEnd",
        "substring",
        "replace",
        "position",
        "splitPart",
        "padStart",
        "padEnd",
        "repeat",
        "reverse",
        "concatWith",
        "matchesRegex",
        "extractRegex",
        "replaceRegex",
        "dateAddDays",
        "dateDiffDays",
        "timeAddSeconds",
        "timeDiffSeconds",
        "datetimeAddSeconds",
        "datetimeDiffSeconds",
        "dateAddMonths",
        "datetimeAddMonths",
        "dateDiffMonths",
        "datetimeDiffMonths",
        "dateFromParts",
        "timeFromParts",
        "datetimeFromParts",
        "year",
        "quarter",
        "month",
        "day",
        "dayOfWeek",
        "dayOfYear",
        "week",
        "weekYear",
        "hour",
        "minute",
        "second",
        "microsecond",
        "startOfYear",
        "startOfQuarter",
        "startOfMonth",
        "startOfWeek",
        "startOfDay",
        "startOfHour",
        "startOfMinute",
        "startOfSecond",
        "asString",
        "asDate",
        "asTime",
        "asDatetime",
        "asInteger",
        "asBoolean",
        "asUuid",
        "asDecimal",
        "asNullable",
        "tryAsInteger",
        "tryAsDecimal",
        "tryAsBoolean",
        "tryAsDate",
        "tryAsTime",
        "tryAsDatetime",
        "tryAsUuid",
        "and",
        "or",
        "not",
        "equal",
        "notEqual",
        "greaterThan",
        "lessThan",
        "greaterThanOrEqual",
        "lessThanOrEqual",
        "contains",
        "startsWith",
        "endsWith",
        "in",
        "if",
        "coalesce",
        "nullIfEqual",
        "greatest",
        "least",
        "count",
        "countDistinct",
        "sum",
        "average",
        "min",
        "max",
        "first",
        "last",
        "median",
        "percentileContinuous",
        "percentileDiscrete",
        "mode",
        "variancePopulation",
        "standardDeviationPopulation",
        "varianceSample",
        "standardDeviationSample",
        "covariancePopulation",
        "covarianceSample",
        "correlation",
        "regressionSlope",
        "regressionIntercept",
        "regressionRSquared",
        "concatAll",
        "any",
        "all",
        "rowNumber",
        "rank",
        "denseRank",
        "percentRank",
        "cumulativeDistribution",
        "ntile",
        "previous",
        "next",
    ];


    /// <summary>The operators chosen by the type family of <c>left</c> too.</summary>
    private static readonly string[] ByLeft =
    [
        "equal",
        "notEqual",
        "greaterThan",
        "lessThan",
        "greaterThanOrEqual",
        "lessThanOrEqual",
    ];

    /// <summary>
    /// The operators whose name has no declared type after the family of an operand.
    /// </summary>
    private static readonly string[] Conditions = [.. ByLeft, "in"];

    /// <summary>
    /// The families that choose an operator: the type an operand declares, or whether
    /// <c>digits</c> is present.
    /// </summary>
    public static readonly string[] Families =
    [
        "Integer",
        "Decimal",
        "String",
        "Boolean",
        "Date",
        "Time",
        "Datetime",
        "Uuid",
        "WithDigits",
        "WithoutDigits",
    ];

    private static readonly string[] Contexts = ["Row", "Projection", "Group", "Total"];

    /// <summary>The schema's <c>minItems</c> of every list property.</summary>
    private static readonly Dictionary<string, int> MinItems = new Dictionary<
        string,
        int
    >(StringComparer.Ordinal)
    {
        ["aggregate"] = 1,
        ["conditions"] = 2,
        ["distinctBy"] = 1,
        ["except"] = 2,
        ["groupBy"] = 1,
        ["intersect"] = 2,
        ["joins"] = 1,
        ["orderBy"] = 1,
        ["partitionBy"] = 1,
        ["removables"] = 1,
        ["rows"] = 1,
        ["select"] = 1,
        ["subqueries"] = 1,
        ["union"] = 2,
        ["unionAll"] = 2,
        ["value"] = 1,
        ["values"] = 2,
    };

    /// <summary>
    /// The schema's <c>maxItems</c>: a range window has exactly one sort key, which the
    /// distances are measured on.
    /// </summary>
    private static readonly Dictionary<string, int> MaxItems = new Dictionary<
        string,
        int
    >(StringComparer.Ordinal)
    {
        ["WindowWithoutFrameWithRangeInteger.orderBy"] = 1,
        ["WindowWithoutFrameWithRangeDecimal.orderBy"] = 1,
        ["WindowWithoutFrameWithRangeDate.orderBy"] = 1,
        ["WindowWithoutFrameWithRangeDatetime.orderBy"] = 1,
    };

    /// <summary>
    /// The records with optional properties only, of which the schema requires one, and
    /// those of them whose two sides cannot both be <c>"all"</c>.
    /// </summary>
    private static readonly Dictionary<string, string?> AtLeastOne = new Dictionary<
        string,
        string?
    >(StringComparer.Ordinal)
    {
        [nameof(Pagination)] = null,
        [nameof(Frame)] = "Sides",
        [nameof(RangeInteger)] = "Sides",
        [nameof(RangeDecimal)] = "Sides",
        [nameof(RangeDate)] = "Sides",
        [nameof(RangeDatetime)] = "Sides",
    };

    /// <summary>
    /// The unions of a literal or a parameter, or of a bare number, whose literal value
    /// the schema restricts further.
    /// </summary>
    private static readonly Dictionary<string, string> UnionChecks = new Dictionary<
        string,
        string
    >(StringComparer.Ordinal)
    {
        [nameof(Offset)] = "Offset",
        [nameof(Buckets)] = "Positive",
        [nameof(RegexGroup)] = "Positive",
        [nameof(Fraction)] = "Fraction",
        [nameof(RegexPattern)] = "Regex",
        [nameof(DistanceDecimal)] = "Distance",
        [nameof(DistanceDatetimeSecondsSeconds)] = "Distance",
    };

    /// <summary>The properties whose literal value the schema restricts.</summary>
    private static readonly Dictionary<string, string> PropertyChecks = new Dictionary<
        string,
        string
    >(StringComparer.Ordinal)
    {
        ["zone"] = "Zone",
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
        if (types.Select(t => t.Name).Distinct(StringComparer.Ordinal).Count() != types.Length)
        {
            throw new InvalidOperationException("Model type names are not unique.");
        }

        Unions = [.. types.Where(IsUnion).Select(Union)];
        List<RecordModel> records =
        [
            .. types.Where(t => t.IsClass && t.IsSealed && (t != typeof(All))).Select(Record),
        ];
        records = [.. records.Select(r => r with { Family = FamilyOf(r, records) })];
        Records =
        [
            .. records.Select(r =>
                r with
                {
                    NullableCause = NullableCause(r, records),
                    DecimalCause = DecimalCause(r, records),
                }
            ),
        ];
        SortedDictionary<string, Choice> choices = [with(StringComparer.Ordinal)];
        foreach (IGrouping<string, RecordModel> kind in Records.Where(r => r.IsOperator).GroupBy(r => r.Kind))
        {
            choices[kind.Key] = ChoiceOf(kind);
        }

        Choices = choices;

        IntegerFamilies =
        [
            .. Records
                .Where(r => r.Family == "Integer")
                .Select(r => r.Kind)
                .Distinct()
                .Order(StringComparer.Ordinal),
        ];
        if (AtLeastOne.Keys.Any(name => !Records.Any(r => r.Type.Name == name)))
        {
            throw new InvalidOperationException("A record of AtLeastOne is not in the model.");
        }

        if (UnionChecks.Keys.Any(name => !Unions.Any(u => u.Type.Name == name)))
        {
            throw new InvalidOperationException("A union of UnionChecks is not in the model.");
        }
    }

    public IReadOnlyList<UnionModel> Unions { get; }

    public IReadOnlyList<RecordModel> Records { get; }

    /// <summary>What chooses each operator beyond its declared type, by kind.</summary>
    public IReadOnlyDictionary<string, Choice> Choices { get; }

    /// <summary>
    /// The operators that have a case for an <c>integer</c> operand of its own; for
    /// any other, an <c>integer</c> operand is of the decimal family.
    /// </summary>
    public IReadOnlyList<string> IntegerFamilies { get; }

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

    public static string Camel(string name)
    {
        return char.ToLowerInvariant(name[0]) + name[1..];
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

    private static UnionModel Union(Type type)
    {
        Type[] cases = Cases(type);
        Type[] records = [.. cases.Where(c => (c != typeof(long)) && (c != typeof(All)))];
        return records.Any(c => !c.IsClass || !c.IsSealed)
            ? throw new InvalidOperationException($"{type.Name} has a case that is not a record.")
            : new UnionModel(
            type,
            records,
            cases.Contains(typeof(long)),
            cases.Contains(typeof(All)),
            UnionChecks.GetValueOrDefault(type.Name)
        );
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
            string json = Camel(property.Name);
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
                new PropertyModel("Value", "value", ValueKind.NullLiteral, typeof(void), 0, 0, null, null)
            );
        }

        bool atLeastOne = AtLeastOne.TryGetValue(type.Name, out string? check);
        bool optionalOnly =
            (Kind(type) == "Unknown") && (models.Count > 0) && models.All(p => !p.IsRequired);
        if (atLeastOne != optionalOnly)
        {
            throw new InvalidOperationException(
                $"{type.Name} has optional properties only: list it in AtLeastOne."
            );
        }

        IType? declared = DeclaredType(type);
        return new RecordModel(
            type,
            Kind(type),
            declared is null ? null : TypeId(declared),
            declared?.Nullable ?? false,
            null,
            models,
            [.. parameters.Select(p => p.Name!)],
            null,
            null,
            atLeastOne,
            check
        );
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
        Type? item = ItemType(type);
        Type? nested = item is null ? null : ItemType(item);
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
            _ when (type == typeof(NormalizationForm)) && !optional => (ValueKind.Form, type),
            _ when (type == typeof(RoundingMode)) && optional => (ValueKind.Mode, type),
            _ when (type == typeof(bool)) && optional => (ValueKind.Flag, type),
            _ when (nullable is not null) && IsUnion(nullable) => (
                ValueKind.OptionalUnion,
                nullable
            ),
            _ when IsUnion(type) && !optional => (ValueKind.Union, type),
            _ when item == typeof(string) => (
                optional ? ValueKind.OptionalStrings : ValueKind.Strings,
                item
            ),
            _ when (nested is not null) && !optional && nested.IsClass => (
                ValueKind.ManyOfMany,
                nested
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
                or ValueKind.ManyOfMany
                or ValueKind.Strings
                or ValueKind.OptionalStrings
                or ValueKind.LiteralList;
        int minItems =
            !list ? 0
            : MinItems.TryGetValue(json, out int min) ? min
            : throw new InvalidOperationException(
                $"{record.Name}.{property.Name} has no known minItems."
            );
        int maxItems = MaxItems.GetValueOrDefault($"{record.Name}.{json}");
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
            maxItems,
            kind is ValueKind.Literal or ValueKind.LiteralList
                ? LiteralValues[valueType]
                : null,
            PropertyChecks.GetValueOrDefault(json)
        );
    }

    private static Type? ItemType(Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ? type.GetGenericArguments()[0]
            : null;
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
    /// the next word of the name, after which the name holds only the family of an
    /// operand and the declared type.
    /// </summary>
    private static string Operator(Type type)
    {
        string name = WithoutContext(type.Name);
        return Operators
                .Where(op =>
                    name.StartsWith(Pascal(op), StringComparison.Ordinal)
                    && ((name.Length == op.Length) || char.IsUpper(name[op.Length]))
                    && (Rest(type, op) is not null)
                )
                .MaxBy(op => op.Length)
            ?? throw new InvalidOperationException($"{type.Name} names no operator.");
    }

    /// <summary>
    /// The family in the name of an operator record after its operator, <c>""</c> for
    /// none, or <see langword="null"/> when the rest of the name is not a family
    /// followed by the declared type (comparisons do not name their boolean type).
    /// </summary>
    private static string? Rest(Type type, string op)
    {
        string rest = WithoutContext(type.Name)[op.Length..];
        IType declared = DeclaredType(type)
            ?? throw new InvalidOperationException($"{type.Name} declares no type.");
        string typeId = TypeId(declared);
        if (!Conditions.Contains(op))
        {
            if (!rest.EndsWith(typeId, StringComparison.Ordinal))
            {
                return rest.Length == 0 ? "" : null;
            }

            rest = rest[..^typeId.Length];
        }

        return (rest.Length == 0) || Families.Contains(rest) ? rest : null;
    }

    /// <summary>
    /// The family that chooses an operator record besides its declared type: that of an
    /// operand, named after the operator, or, for an operator split by the presence of
    /// <c>digits</c>, whether the record has it.
    /// </summary>
    private static string? FamilyOf(RecordModel record, IReadOnlyList<RecordModel> records)
    {
        if (!record.IsOperator)
        {
            return null;
        }

        string family = Rest(record.Type, Camel(record.Kind))!;
        bool byDigits = records.Any(r =>
            (r.Kind == record.Kind)
            && (Rest(r.Type, Camel(r.Kind)) is "WithDigits" or "WithoutDigits")
        );
        return byDigits
                ? record.Properties.Any(p => (p.Json == "digits") && p.IsRequired)
                    ? "WithDigits"
                    : "WithoutDigits"
            : family.Length == 0 ? null
            : family;
    }

    private static Choice ChoiceOf(IEnumerable<RecordModel> kind)
    {
        RecordModel[] records = [.. kind];
        string op = Camel(records[0].Kind);
        Choice choice =
            records.All(r => r.Family is null) ? Choice.None
            : records.Any(r => r.Family is "WithDigits" or "WithoutDigits") ? Choice.Digits
            : ByLeft.Contains(op) ? Choice.Left
            : Choice.Operand;
        string property = choice == Choice.Left ? "left" : "operand";
        return choice == Choice.None ? choice
            : records.Any(r => r.Family is null)
                ? throw new InvalidOperationException($"Some {op} records name no family.")
            : (choice != Choice.Digits) && records.Any(r => r.Properties.All(p => p.Json != property))
                ? throw new InvalidOperationException($"Some {op} records have no {property}.")
            : choice;
    }

    /// <summary>
    /// What makes a record declared <c>decimal</c> a decimal rather than an integer: one
    /// of its operands that are typed as numbers, of either type, declares a decimal (as
    /// for <c>add</c>, <c>if</c>, <c>greatest</c>). Only records of an operator that also
    /// has an <c>integer</c> record for the family in the context need one, and only
    /// where an operand admits an integer.
    /// </summary>
    private static string? DecimalCause(RecordModel record, IReadOnlyList<RecordModel> records)
    {
        if (!record.IsOperator || (record.DeclaredType is not ("Decimal" or "DecimalNullable")))
        {
            return null;
        }

        string integer = record.DeclaredType.Replace("Decimal", "Integer", StringComparison.Ordinal);
        bool hasInteger = records.Any(r =>
            (r.Kind == record.Kind)
            && (r.Family == record.Family)
            && (r.DeclaredType == integer)
            && (r.Type.Namespace == record.Type.Namespace)
        );
        if (!hasInteger)
        {
            return null;
        }

        List<string> causes = [];
        foreach (PropertyModel property in record.Properties)
        {
            if (
                (property.Json is "predicate" or "default" or "condition")
                || !IsOperand(property.Type)
                || !Cases(property.Type).Any(c => DeclaredType(c)?.Name == "decimal")
            )
            {
                continue;
            }

            if (!Cases(property.Type).Any(c => DeclaredType(c)?.Name == "integer"))
            {
                // The operand's type makes the record a decimal.
                return null;
            }

            string local = Emitter.Local(property.Json);
            causes.Add(
                property.Kind is ValueKind.Union or ValueKind.OptionalUnion
                    ? $"Declarations.IsDecimal({local})"
                : property.Kind == ValueKind.Many ? $"Declarations.AnyDecimal({local}!)"
                : throw new InvalidOperationException(
                    $"{record.Type.Name}.{property.Name} is not an operand."
                )
            );
        }

        return causes.Count > 0
            ? string.Join(" || ", causes)
            : throw new InvalidOperationException(
                $"{record.Type.Name} is declared decimal without a number operand."
            );
    }

    /// <summary>Whether every case of an operand union declares a nullable type.</summary>
    private static bool IsAlwaysNullable(Type operand)
    {
        return IsUnion(operand)
            && Cases(operand).All(c => DeclaredType(c)?.Nullable ?? false);
    }

    /// <summary>Whether a union is a position of expressions, an operand.</summary>
    private static bool IsOperand(Type type)
    {
        return IsUnion(type)
            && type.Namespace!.EndsWith("Expressions", StringComparison.Ordinal);
    }

    /// <summary>
    /// What makes a record declared nullable nullable: one of its operands that is (all
    /// of them for <c>coalesce</c>), a <c>predicate</c> of an aggregate, or no
    /// <c>default</c> of <c>previous</c> / <c>next</c>. Only records of an operator that
    /// also has a non-null record for the type in the context need one: the others are
    /// always nullable (<c>min</c> of a total query, <c>tryAsInteger</c>).
    /// </summary>
    private static string? NullableCause(RecordModel record, IReadOnlyList<RecordModel> records)
    {
        if (!record.Nullable || !record.IsOperator)
        {
            return null;
        }

        string nonNull = record.DeclaredType![..^"Nullable".Length];
        bool hasNonNull = records.Any(r =>
            (r.Kind == record.Kind)
            && (r.Family == record.Family)
            && (r.DeclaredType == nonNull)
            && (r.Type.Namespace == record.Type.Namespace)
        );
        if (!hasNonNull)
        {
            return null;
        }

        string op = Camel(record.Kind);
        List<string> causes = [];
        foreach (PropertyModel property in record.Properties)
        {
            string local = Emitter.Local(property.Json);
            if (property.Json == "predicate")
            {
                causes.Add($"{local}.HasValue");
            }
            else if (property.Json == "default")
            {
                causes.Add($"!{local}.HasValue");
            }
            else if (!IsOperand(property.Type))
            {
                continue;
            }
            else if ((property.Kind == ValueKind.Union) && IsAlwaysNullable(property.Type))
            {
                // The operand's type gives the record its nullable type.
                return null;
            }
            else
            {
                causes.Add(
                    property.Kind is ValueKind.Union or ValueKind.OptionalUnion
                        ? $"Nullability.Of({local})"
                    : (property.Kind == ValueKind.Many) && (op == "coalesce")
                        ? $"Nullability.All({local}!)"
                    : property.Kind == ValueKind.Many ? $"Nullability.Any({local}!)"
                    : throw new InvalidOperationException(
                        $"{record.Type.Name}.{property.Name} is not an operand."
                    )
                );
            }
        }

        return causes.Count > 0
            ? string.Join(" || ", causes)
            : throw new InvalidOperationException(
                $"{record.Type.Name} is declared nullable without an operand."
            );
    }
}
