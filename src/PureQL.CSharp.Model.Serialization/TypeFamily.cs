namespace PureQL.CSharp.Model.Serialization;

/// <summary>
/// The families that <c>probe.*</c> in the specification tells apart: <c>integer</c>
/// belongs to the decimal family.
/// </summary>
internal enum TypeFamily
{
    Decimal,
    String,
    Boolean,
    Date,
    Time,
    Datetime,
    Uuid,
}
