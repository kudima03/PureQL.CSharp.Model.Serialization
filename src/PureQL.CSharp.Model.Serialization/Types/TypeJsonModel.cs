namespace PureQL.CSharp.Model.Serialization.Types;

internal sealed record TypeJsonModel
{
    public TypeJsonModel(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
