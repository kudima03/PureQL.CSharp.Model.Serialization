using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record NullArrayScalarJsonModel
{
    public NullArrayScalarJsonModel(INullArrayScalar scalar)
        : this(new NullArrayType(), scalar.Value) { }

    [JsonConstructor]
    public NullArrayScalarJsonModel(NullArrayType type, IEnumerable<object?> value)
    {
        Type = type ?? throw new JsonException();
        if (value == null || value.Any(x => x != null))
        {
            throw new JsonException();
        }
        Value = value;
    }

    public NullArrayType Type { get; }

    public IEnumerable<object?> Value { get; }
}

internal sealed class NullArrayScalarConverter : JsonConverter<INullArrayScalar>
{
    public override INullArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        NullArrayScalarJsonModel model =
            JsonSerializer.Deserialize<NullArrayScalarJsonModel>(ref reader, options)!;

        return new NullArrayScalar(model.Value.Count());
    }

    public override void Write(
        Utf8JsonWriter writer,
        INullArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new NullArrayScalarJsonModel(value), options);
    }
}
