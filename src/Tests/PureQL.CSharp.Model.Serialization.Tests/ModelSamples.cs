using System.Collections;
using System.Reflection;
using OneOf;
using PureQL.CSharp.Model.Parameters;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Builds instances of every model type by reflection: one per case of a union, and
/// for a record one with every optional value set (and non-default <c>over</c> /
/// <c>direction</c>) and one with none. The items of a list differ, as list literals
/// and <c>distinctBy</c> repeat no value, and the operands of an operator give it the
/// type it declares: a nullable operator gets nullable operands and a non-null one
/// non-null operands (a non-null <c>coalesce</c> one of each).
/// </summary>
internal static class ModelSamples
{
    public static IReadOnlyList<Type> Types { get; } =
    [
        .. typeof(PureQLQuery)
            .Assembly.GetExportedTypes()
            .Where(t =>
                t.IsClass && !t.IsAbstract && (t.Namespace != "PureQL.CSharp.Model.Types")
            )
            .OrderBy(t => t.FullName, StringComparer.Ordinal),
    ];

    public static IEnumerable<object> Of(Type type)
    {
        if (IsUnion(type, out Type[] cases))
        {
            foreach (Type caseType in cases)
            {
                yield return Wrap(type, caseType, Create(caseType, false));
            }

            yield break;
        }

        yield return Create(type, true);
        yield return Create(type, false);
    }

    private static object Create(Type type, bool full, bool? nullable = null)
    {
        if (IsUnion(type, out Type[] cases))
        {
            bool wanted = nullable ?? IsNullable(type);
            Type pick = cases.FirstOrDefault(c => IsNullable(c) == wanted) ?? cases[0];
            return Wrap(type, pick, Create(pick, false));
        }

        ConstructorInfo constructor = Constructor(type);
        bool operands = IsNullable(type);
        bool coalesce =
            type.Name.StartsWith("Coalesce", StringComparison.Ordinal) && !operands;
        return constructor.Invoke([
            .. constructor
                .GetParameters()
                .Select(p => Value(p, full, operands, coalesce)),
        ]);
    }

    /// <summary>
    /// Whether a type is a record declared nullable that takes an operand, so that its
    /// exact type follows from its operands.
    /// </summary>
    public static bool IsNullableOperator(Type type)
    {
        return IsNullable(type)
            && !IsUnion(type, out _)
            && Constructor(type).GetParameters().Any(p => IsOperand(p.ParameterType));
    }

    /// <summary>
    /// A record declared nullable built over non-null operands only and with no optional
    /// value set, so that nothing but its declaration makes it nullable.
    /// </summary>
    public static object OverNonNull(Type type)
    {
        ConstructorInfo constructor = Constructor(type);
        return constructor.Invoke([
            .. constructor.GetParameters().Select(p => Value(p, false, false, false)),
        ]);
    }

    private static ConstructorInfo Constructor(Type type)
    {
        return type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .First();
    }

    private static bool IsOperand(Type type)
    {
        Type item =
            type.IsGenericType
            && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                ? type.GetGenericArguments()[0]
                : type;
        return IsUnion(item, out _);
    }

    private static bool IsNullable(Type type)
    {
        return type.Name.Contains("Nullable", StringComparison.Ordinal);
    }

    private static object? Value(
        ParameterInfo parameter,
        bool full,
        bool nullable,
        bool coalesce
    )
    {
        Type type = parameter.ParameterType;
        return !type.IsValueType && parameter.HasDefaultValue && !full
            ? null
            : Value(
                Nullable.GetUnderlyingType(type) ?? type,
                full,
                0,
                nullable,
                coalesce
            );
    }

    private static object Value(
        Type type,
        bool full,
        int variant = 0,
        bool nullable = false,
        bool coalesce = false
    )
    {
        if (type == typeof(OneOf<long, ParamInteger>))
        {
            return full
                ? (OneOf<long, ParamInteger>)new ParamInteger("page")
                : (OneOf<long, ParamInteger>)20L;
        }

        if (
            type.IsGenericType
            && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        )
        {
            Type item = type.GetGenericArguments()[0];
            Array items = Array.CreateInstance(item, 2);
            for (int i = 0; i < 2; i++)
            {
                // A non-null coalesce has exactly one non-null operand.
                bool wanted = nullable || (coalesce && (i == 0));
                items.SetValue(Value(item, false, i, wanted), i);
            }

            return items;
        }

        return type switch
        {
            _ when type == typeof(string) => $"name{variant}",
            _ when type == typeof(long) => -42L + variant,
            _ when type == typeof(int) => 3,
            _ when type == typeof(decimal) => 12.50m + variant,
            _ when type == typeof(bool) => full ^ (variant == 1),
            _ when type == typeof(DateOnly) => new DateOnly(2024, 2, 29).AddDays(variant),
            _ when type == typeof(TimeOnly) => new TimeOnly(23, 59, 59).Add(
                TimeSpan.FromTicks((1234560 + variant) * 10)
            ),
            _ when type == typeof(DateTimeOffset) => new DateTimeOffset(
                2024,
                1,
                31,
                18,
                30,
                variant,
                TimeSpan.Zero
            ),
            _ when type == typeof(Guid) => Guid.Parse(
                $"3f2a6c1e-8b4d-4e2a-9c1f-1a2b3c4d5e6{variant}"
            ),
            _ when type == typeof(AggregateOver) => full
                ? AggregateOver.All
                : AggregateOver.Group,
            _ when type == typeof(SortDirection) => full
                ? SortDirection.Desc
                : SortDirection.Asc,
            _ when type == typeof(JoinKind) => JoinKind.Right,
            _ => Create(type, full, nullable),
        };
    }

    private static object Wrap(Type union, Type caseType, object value)
    {
        return union.GetConstructor([caseType])!.Invoke([value]);
    }

    private static bool IsUnion(Type type, out Type[] cases)
    {
        Type? baseType = type.BaseType;
        bool isUnion =
            baseType is { IsGenericType: true }
            && baseType
                .GetGenericTypeDefinition()
                .Name.StartsWith("OneOfBase", StringComparison.Ordinal);
        cases = isUnion ? baseType!.GetGenericArguments() : [];
        return isUnion;
    }
}

/// <summary>
/// Compares two model graphs by runtime type and value, including union cases.
/// </summary>
internal static class ModelAssert
{
    public static void Equivalent(object? expected, object? actual, string path = "$")
    {
        if (expected is null || actual is null)
        {
            Assert.True(expected is null && actual is null, $"{path}: null mismatch");
            return;
        }

        Assert.True(
            expected.GetType() == actual.GetType(),
            $"{path}: expected {expected.GetType().Name}, got {actual.GetType().Name}"
        );
        Type type = expected.GetType();
        if (expected is IOneOf union)
        {
            Equivalent(union.Value, ((IOneOf)actual).Value, $"{path}<{type.Name}>");
        }
        else if (
            type.IsPrimitive
            || type.IsEnum
            || type.IsValueType
            || expected is string
        )
        {
            Assert.True(Equals(expected, actual), $"{path}: {expected} != {actual}");
        }
        else if (expected is IEnumerable items)
        {
            object[] left = [.. items.Cast<object>()];
            object[] right = [.. ((IEnumerable)actual).Cast<object>()];
            Assert.True(left.Length == right.Length, $"{path}: length mismatch");
            for (int i = 0; i < left.Length; i++)
            {
                Equivalent(left[i], right[i], $"{path}[{i}]");
            }
        }
        else
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                Equivalent(
                    property.GetValue(expected),
                    property.GetValue(actual),
                    $"{path}.{property.Name}"
                );
            }
        }
    }
}
