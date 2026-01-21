using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Arithmetics;

namespace PureQL.CSharp.Model.Serialization.Arithmetics;

internal sealed class ArithmeticConverter : JsonConverter<Arithmetic>
{
    public override Arithmetic Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out Add? add)
                ? new Arithmetic(add!)
            : JsonExtensions.TryDeserialize(root, options, out Subtract? subtract)
                ? new Arithmetic(subtract!)
            : JsonExtensions.TryDeserialize(root, options, out Multiply? multiply)
                ? new Arithmetic(multiply!)
            : JsonExtensions.TryDeserialize(root, options, out Divide? divide)
                ? new Arithmetic(divide!)
            : throw new JsonException("Unable to determine Arithmetic type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Arithmetic value,
        JsonSerializerOptions options
    )
    {
        if (value.IsT0)
        {
            JsonSerializer.Serialize(writer, value.AsT0, options);
        }
        else if (value.IsT1)
        {
            JsonSerializer.Serialize(writer, value.AsT1, options);
        }
        else if (value.IsT2)
        {
            JsonSerializer.Serialize(writer, value.AsT2, options);
        }
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(writer, value.AsT3, options);
        }
        else
        {
            throw new JsonException("Unable to determine Arithmetic type.");
        }
    }
}
