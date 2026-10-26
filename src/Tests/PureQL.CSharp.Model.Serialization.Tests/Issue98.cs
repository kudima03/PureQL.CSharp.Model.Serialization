namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// The converters accept a node declared nullable whose operands give it a non-null
/// type, although the schema rejects it. The tests of that rule are skipped until the
/// issue is fixed.
/// </summary>
internal static class Issue98
{
    public const string Skip =
        "Converters accept a nullable declaration over non-null operands: "
        + "https://github.com/kudima03/PureQL.CSharp.Model.Serialization/issues/98";
}
