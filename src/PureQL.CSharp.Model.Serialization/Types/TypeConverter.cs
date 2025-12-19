using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Types;

public sealed class TypeConverter<T> : JsonConverter<T>
    where T : IType, new()
{
    public override T? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        TypeJsonModel typeModel =
            JsonSerializer.Deserialize<TypeJsonModel>(ref reader, options)
            ?? throw new JsonException();

        T type = new T();

        return typeModel.Name != type.Name ? throw new JsonException() : type;
    }

    public override void Write(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new TypeJsonModel(value.Name), options);
    }
}
