using System.Text;
using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The JSON object of one record: its properties, the ones it requires and, for an
/// operator or a leaf, the <c>operator</c> and <c>type</c> it implies, which are read
/// and checked without a property of the record.
/// </summary>
internal sealed class RecordShape(
    string name,
    int kind,
    int type,
    string[] properties,
    ulong required
)
{
    /// <summary>The index <see cref="Match"/> gives the implied <c>operator</c>.</summary>
    public const int OperatorProperty = -1;

    /// <summary>The index <see cref="Match"/> gives the implied <c>type</c>.</summary>
    public const int TypeProperty = -2;

    public const int UnknownProperty = -3;

    private const int OperatorBit = 62;

    private const int TypeBit = 63;

    private readonly byte[][] _utf8 = [.. properties.Select(Encoding.UTF8.GetBytes)];

    /// <summary>The record type's name, for error messages.</summary>
    public string Name { get; } = name;

    /// <summary>
    /// The operator or leaf kind, <see cref="Kinds.Unknown"/> for any other record.
    /// </summary>
    public int Kind { get; } = kind;

    /// <summary>The declared type, <see cref="TypeIds.None"/> when it has none.</summary>
    public int Type { get; } = type;

    public IReadOnlyList<string> Properties { get; } = properties;

    /// <summary>The bits (see <see cref="Bit"/>) of the required properties.</summary>
    public ulong Required { get; } =
        required
        | (Kinds.IsOperator(kind) ? Bit(OperatorProperty) : 0)
        | (type == TypeIds.None ? 0 : Bit(TypeProperty));

    public static ulong Bit(int property)
    {
        return property switch
        {
            OperatorProperty => 1UL << OperatorBit,
            TypeProperty => 1UL << TypeBit,
            _ => 1UL << property,
        };
    }

    public string PropertyName(int property)
    {
        return property switch
        {
            OperatorProperty => "operator",
            TypeProperty => "type",
            _ => Properties[property],
        };
    }

    /// <summary>The index of the property name the reader is on.</summary>
    public int Match(ref Utf8JsonReader reader)
    {
        for (int i = 0; i < _utf8.Length; i++)
        {
            if (reader.ValueTextEquals(_utf8[i]))
            {
                return i;
            }
        }

        return Kinds.IsOperator(Kind) && reader.ValueTextEquals("operator"u8)
                ? OperatorProperty
            : (Type != TypeIds.None) && reader.ValueTextEquals("type"u8) ? TypeProperty
            : UnknownProperty;
    }
}
