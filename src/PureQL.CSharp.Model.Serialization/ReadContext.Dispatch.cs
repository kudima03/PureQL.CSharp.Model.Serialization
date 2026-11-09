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
    /// literal), the <c>type</c> it declares and, for a comparison, the family of the
    /// type <c>left</c> (<c>operand</c> for <c>in</c>) declares.
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
                left = Family(ref probe, !isOperator || IsFamilyOf(kind, false));
            }
            else if (probe.ValueTextEquals("operand"u8))
            {
                operand = Family(ref probe, !isOperator || IsFamilyOf(kind, true));
            }
            else
            {
                if (!isOperator && (kind == Kinds.Unknown))
                {
                    kind = LeafKind(ref probe);
                }

                probe.Skip();
            }

            if (IsComplete(isOperator, kind, type, kind == Kinds.In ? operand : left))
            {
                break;
            }
        }

        return Complete(union, isOperator, kind, type, left, operand);
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
                : $"{union} does not accept '{text}' of {TypeIds.Name(family)}"
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
                probe.ValueTextEquals("groupBy"u8) ? QueryClauses.GroupBy
                : probe.ValueTextEquals("aggregate"u8) ? QueryClauses.Aggregate
                : probe.ValueTextEquals("pagination"u8) ? QueryClauses.Pagination
                : probe.ValueTextEquals("orderBy"u8) ? QueryClauses.OrderBy
                : QueryClauses.None;
            probe.Skip();
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

            probe.Skip();
        }

        return false;
    }

    /// <summary>
    /// Whether the family of <c>operand</c> (else of <c>left</c>) chooses the variant of
    /// the operator.
    /// </summary>
    private static bool IsFamilyOf(int kind, bool operand)
    {
        return Kinds.IsComparison(kind) && (operand ? kind == Kinds.In : kind != Kinds.In);
    }

    private static bool IsComplete(bool isOperator, int kind, int type, int family)
    {
        return (type != TypeIds.None)
            && (
                isOperator
                    ? !Kinds.IsComparison(kind) || (family >= 0)
                    : kind != Kinds.Unknown
            );
    }

    private int Complete(
        string union,
        bool isOperator,
        int kind,
        int type,
        int left,
        int operand
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

        if (!isOperator || !Kinds.IsComparison(kind))
        {
            return Serialization.Signature.Of(kind, type);
        }

        string name = kind == Kinds.In ? "operand" : "left";
        int family = kind == Kinds.In ? operand : left;
        if (family < 0)
        {
            throw Error($"Missing property '{name}'");
        }

        if (family == Families.None)
        {
            Push(name);
            throw Error($"Cannot determine the type for {union}");
        }

        return Serialization.Signature.Of(kind, type, family);
    }

    /// <summary>
    /// The family of the type an operand declares (<see cref="Families.None"/> when it
    /// declares none of them), read when <paramref name="wanted"/> or when the operator
    /// is not known yet. Nothing else is validated: the case reads the operand in full.
    /// The reader is left on the operand's last token.
    /// </summary>
    private static int Family(ref Utf8JsonReader probe, bool wanted)
    {
        _ = probe.Read();
        int family = Families.None;
        if (wanted && (probe.TokenType == JsonTokenType.StartObject))
        {
            Utf8JsonReader operand = probe;
            family = DeclaredFamily(ref operand);
        }

        probe.Skip();
        return family;
    }

    private static int DeclaredFamily(ref Utf8JsonReader operand)
    {
        while (operand.Read() && (operand.TokenType == JsonTokenType.PropertyName))
        {
            if (!operand.ValueTextEquals("type"u8))
            {
                operand.Skip();
                continue;
            }

            if (!operand.Read() || (operand.TokenType != JsonTokenType.StartObject))
            {
                return Families.None;
            }

            while (operand.Read() && (operand.TokenType == JsonTokenType.PropertyName))
            {
                if (!operand.ValueTextEquals("name"u8))
                {
                    operand.Skip();
                    continue;
                }

                return operand.Read() && (operand.TokenType == JsonTokenType.String)
                    ? Families.Of(TypeIds.Find(ref operand))
                    : Families.None;
            }

            return Families.None;
        }

        return Families.None;
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
