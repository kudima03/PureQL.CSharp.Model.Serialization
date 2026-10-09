using System.Text.Json;

namespace PureQL.CSharp.Model.Serialization;

/// <summary>How one model type is read from and written to JSON.</summary>
internal abstract class ModelDescriptor(ModelRegistry registry, Type type)
{
    public Type Type { get; } = type;

    protected ModelRegistry Registry { get; } = registry;

    /// <summary>
    /// The signatures (see <see cref="Discriminators"/>) of the JSON nodes this type
    /// accepts, used by the unions that contain it.
    /// </summary>
    public abstract IReadOnlyCollection<string> Signatures { get; }

    public abstract object Read(JsonElement element, string path);

    public abstract void Write(Utf8JsonWriter writer, object value);

    public virtual void Seal() { }
}
