using System.Collections;
using System.Reflection;
using OneOf;
using PureQL.CSharp.Model.Parameters;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Builds instances of every model type by reflection: one per case of a union, and
/// for a record one with every optional value set (and non-default <c>over</c> /
/// <c>direction</c>) and one with none.
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

    private static object Create(Type type, bool full)
    {
        if (IsUnion(type, out Type[] cases))
        {
            return Wrap(type, cases[0], Create(cases[0], false));
        }

        ConstructorInfo constructor = type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .First();
        return constructor.Invoke([
            .. constructor.GetParameters().Select(p => Value(p, full)),
        ]);
    }

    private static object? Value(ParameterInfo parameter, bool full)
    {
        Type type = parameter.ParameterType;
        return !type.IsValueType && parameter.HasDefaultValue && !full
            ? null
            : Value(Nullable.GetUnderlyingType(type) ?? type, full);
    }

    private static object Value(Type type, bool full)
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
            items.SetValue(Value(item, false), 0);
            items.SetValue(Value(item, false), 1);
            return items;
        }

        return type switch
        {
            _ when type == typeof(string) => "name",
            _ when type == typeof(long) => -42L,
            _ when type == typeof(int) => 3,
            _ when type == typeof(decimal) => 12.50m,
            _ when type == typeof(bool) => full,
            _ when type == typeof(DateOnly) => new DateOnly(2024, 2, 29),
            _ when type == typeof(TimeOnly) => new TimeOnly(23, 59, 59).Add(
                TimeSpan.FromTicks(1234567)
            ),
            _ when type == typeof(DateTimeOffset) => new DateTimeOffset(
                2024,
                1,
                31,
                18,
                30,
                0,
                TimeSpan.FromMinutes(-330)
            ),
            _ when type == typeof(Guid) => Guid.Parse(
                "3f2a6c1e-8b4d-4e2a-9c1f-1a2b3c4d5e6f"
            ),
            _ when type == typeof(AggregateOver) => full
                ? AggregateOver.All
                : AggregateOver.Group,
            _ when type == typeof(SortDirection) => full
                ? SortDirection.Desc
                : SortDirection.Asc,
            _ when type == typeof(JoinType) => JoinType.Right,
            _ => Create(type, full),
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
