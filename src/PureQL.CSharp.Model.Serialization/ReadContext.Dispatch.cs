using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// What the unions read ahead to choose their case. A copy of the reader looks at the
/// node's properties without consuming them, and stops as soon as the case is known,
/// so the node itself is then read once, by the case.
/// </summary>
internal sealed partial class ReadContext
{
    /// <summary>The name of the last operator that is none of the specification's.</summary>
    private string? UnknownOperator { get; set; }

    /// <summary>
    /// The <see cref="Serialization.Signature"/> of the node the reader is on: its
    /// <c>operator</c> or, for a leaf, its shape (<c>source</c> for a field,
    /// <c>param</c>, <c>key</c>, <c>subquery</c> for a list column, <c>value</c> for a
    /// literal), the <c>type</c> it declares and, for an operator chosen by more (see
    /// <see cref="Kinds.ChoiceOf"/>), the family of the type <c>left</c> or
    /// <c>operand</c> declares, or whether <c>digits</c> is present.
    /// </summary>
    /// <param name="reader">The reader, on the node; it is not moved.</param>
    /// <param name="union">The union choosing a case, for error messages.</param>
    public int Signature(ref Utf8JsonReader reader, string union)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw Error($"Expected an object, found {Describe(reader.TokenType)}");
        }

        Utf8JsonReader probe = reader;
        int kind = Kinds.Unknown;
        bool isOperator = false;
        int type = TypeIds.None;
        int left = -1;
        int operand = -1;
        bool digits = false;
        while (Read(ref probe) == JsonTokenType.PropertyName)
        {
            if (probe.ValueTextEquals("operator"u8))
            {
                Push("operator");
                if (Read(ref probe) != JsonTokenType.String)
                {
                    throw Error("Expected a string");
                }

                kind = Kinds.Operator(ref probe);
                isOperator = true;
                UnknownOperator = kind == Kinds.Unknown ? probe.GetString() : null;
                Pop();
            }
            else if (probe.ValueTextEquals("type"u8))
            {
                Push("type");
                _ = Read(ref probe);
                type = ReadType(ref probe);
                Pop();
            }
            else if (probe.ValueTextEquals("left"u8))
            {
                left = Declared(ref probe, !isOperator || (Kinds.ChoiceOf(kind) == Choice.Left));
            }
            else if (probe.ValueTextEquals("operand"u8))
            {
                operand = Declared(
                    ref probe,
                    !isOperator || (Kinds.ChoiceOf(kind) == Choice.Operand)
                );
            }
            else
            {
                if (!isOperator && (kind == Kinds.Unknown))
                {
                    kind = LeafKind(ref probe);
                }

                digits |= probe.ValueTextEquals("digits"u8);
                Skip(ref probe);
            }

            if (IsComplete(isOperator, kind, type, left, operand))
            {
                break;
            }
        }

        return Complete(union, isOperator, kind, type, left, operand, digits);
    }

    /// <summary>A union that has no case for the signature.</summary>
    public JsonException NotAccepted(string union, int signature)
    {
        int kind = signature >> 16;
        int type = (signature >> 8) & 0xFF;
        int family = signature & 0xFF;
        string text =
            $"{(kind == Kinds.Unknown ? UnknownOperator : Kinds.Name(kind))} "
            + DescribeType(type);
        return Error(
            family == Families.None
                ? $"{union} does not accept '{text}'"
                : $"{union} does not accept '{text}' {Families.Describe(family)}"
        );
    }

    /// <summary>
    /// The clauses of the query the reader is on that choose its variant, as the
    /// schema's dependencies between them do.
    /// </summary>
    public QueryClauses Clauses(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw Error($"Expected an object, found {Describe(reader.TokenType)}");
        }

        Utf8JsonReader probe = reader;
        QueryClauses clauses = QueryClauses.None;
        while (Read(ref probe) == JsonTokenType.PropertyName)
        {
            clauses |=
                probe.ValueTextEquals("unionAll"u8) ? QueryClauses.UnionAll
                : probe.ValueTextEquals("union"u8) ? QueryClauses.Union
                : probe.ValueTextEquals("intersect"u8) ? QueryClauses.Intersect
                : probe.ValueTextEquals("except"u8) ? QueryClauses.Except
                : probe.ValueTextEquals("groupBy"u8) ? QueryClauses.GroupBy
                : probe.ValueTextEquals("aggregate"u8) ? QueryClauses.Aggregate
                : probe.ValueTextEquals("pagination"u8) ? QueryClauses.Pagination
                : probe.ValueTextEquals("orderBy"u8) ? QueryClauses.OrderBy
                : QueryClauses.None;
            Skip(ref probe);
        }

        return clauses;
    }

    /// <summary>Whether the object the reader is on has the property.</summary>
    public bool Has(ref Utf8JsonReader reader, ReadOnlySpan<byte> name)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw Error($"Expected an object, found {Describe(reader.TokenType)}");
        }

        Utf8JsonReader probe = reader;
        while (Read(ref probe) == JsonTokenType.PropertyName)
        {
            if (probe.ValueTextEquals(name))
            {
                return true;
            }

            Skip(ref probe);
        }

        return false;
    }

    /// <summary>
    /// The type the first sort key of the window the reader is on declares, which
    /// chooses the distances of a <c>range</c>, or <see cref="TypeIds.None"/>.
    /// </summary>
    public int RangeKey(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw Error($"Expected an object, found {Describe(reader.TokenType)}");
        }

        Utf8JsonReader probe = reader;
        while (Read(ref probe) == JsonTokenType.PropertyName)
        {
            if (!probe.ValueTextEquals("orderBy"u8))
            {
                Skip(ref probe);
                continue;
            }

            Utf8JsonReader keys = probe;
            if (!keys.Read() || (keys.TokenType != JsonTokenType.StartArray))
            {
                return TypeIds.None;
            }

            if (!keys.Read() || (keys.TokenType != JsonTokenType.StartObject))
            {
                return TypeIds.None;
            }

            while (keys.Read() && (keys.TokenType == JsonTokenType.PropertyName))
            {
                if (keys.ValueTextEquals("expression"u8))
                {
                    return keys.Read() && (keys.TokenType == JsonTokenType.StartObject)
                        ? DeclaredName(ref keys)
                        : TypeIds.None;
                }

                if (!keys.TrySkip())
                {
                    return TypeIds.None;
                }
            }

            return TypeIds.None;
        }

        return TypeIds.None;
    }

    private static bool IsComplete(bool isOperator, int kind, int type, int left, int operand)
    {
        return (type != TypeIds.None)
            && (
                isOperator
                    ? Kinds.ChoiceOf(kind) switch
                    {
                        Choice.Left => left >= 0,
                        Choice.Operand => operand >= 0,
                        // Only the end of the node tells that digits are absent.
                        Choice.Digits => false,
                        Choice.None => true,
                        _ => true,
                    }
                    : kind != Kinds.Unknown
            );
    }

    private int Complete(
        string union,
        bool isOperator,
        int kind,
        int type,
        int left,
        int operand,
        bool digits
    )
    {
        if (!isOperator && (kind == Kinds.Unknown))
        {
            throw Error("Expected an operator node or a reference");
        }

        if (type == TypeIds.None)
        {
            throw Error("Missing property 'type'");
        }

        Choice choice = isOperator ? Kinds.ChoiceOf(kind) : Choice.None;
        if (choice == Choice.None)
        {
            return Serialization.Signature.Of(kind, type);
        }

        if (choice == Choice.Digits)
        {
            return Serialization.Signature.Of(
                kind,
                type,
                digits ? Families.WithDigits : Families.WithoutDigits
            );
        }

        string name = choice == Choice.Operand ? "operand" : "left";
        int declared = choice == Choice.Operand ? operand : left;
        if (declared < 0)
        {
            throw Error($"Missing property '{name}'");
        }

        int family = Families.Of(kind, declared);
        if (family == Families.None)
        {
            Push(name);
            throw Error($"Cannot determine the type for {union}");
        }

        return Serialization.Signature.Of(kind, type, family);
    }

    /// <summary>
    /// The name id of the type an operand declares (<see cref="TypeIds.None"/> when it
    /// declares none), read when <paramref name="wanted"/> or when the operator is not
    /// known yet. Nothing else is validated: the case reads the operand in full. The
    /// reader is left on the operand's last token.
    /// </summary>
    private int Declared(ref Utf8JsonReader probe, bool wanted)
    {
        _ = Read(ref probe);
        int declared = TypeIds.None;
        if (wanted && (probe.TokenType == JsonTokenType.StartObject))
        {
            Utf8JsonReader operand = probe;
            declared = DeclaredName(ref operand);
        }

        Skip(ref probe);
        return declared;
    }

    /// <summary>
    /// The name id of the type the node declares, read on a copy of the reader. A value
    /// cut short gives <see cref="TypeIds.None"/>: the caller reports it when it skips
    /// the node.
    /// </summary>
    private static int DeclaredName(ref Utf8JsonReader operand)
    {
        while (operand.Read() && (operand.TokenType == JsonTokenType.PropertyName))
        {
            if (!operand.ValueTextEquals("type"u8))
            {
                if (!operand.TrySkip())
                {
                    return TypeIds.None;
                }

                continue;
            }

            if (!operand.Read() || (operand.TokenType != JsonTokenType.StartObject))
            {
                return TypeIds.None;
            }

            while (operand.Read() && (operand.TokenType == JsonTokenType.PropertyName))
            {
                if (!operand.ValueTextEquals("name"u8))
                {
                    if (!operand.TrySkip())
                    {
                        return TypeIds.None;
                    }

                    continue;
                }

                return operand.Read() && (operand.TokenType == JsonTokenType.String)
                    ? TypeIds.Find(ref operand)
                    : TypeIds.None;
            }

            return TypeIds.None;
        }

        return TypeIds.None;
    }

    private static int LeafKind(ref Utf8JsonReader probe)
    {
        return probe.ValueTextEquals("source"u8) ? Kinds.Field
            : probe.ValueTextEquals("param"u8) ? Kinds.Param
            : probe.ValueTextEquals("key"u8) ? Kinds.Key
            : probe.ValueTextEquals("subquery"u8) ? Kinds.Column
            : probe.ValueTextEquals("value"u8) ? Kinds.Literal
            : Kinds.Unknown;
    }
}
