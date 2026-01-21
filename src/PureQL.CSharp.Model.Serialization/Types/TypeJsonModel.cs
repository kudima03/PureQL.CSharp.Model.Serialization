using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization.Types;

internal sealed record TypeJsonModel
{
    public TypeJsonModel(string name)
    {
        Name = name ?? throw new JsonException();
    }

    public string Name { get; }
}
