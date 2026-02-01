using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.ArrayTypes;

namespace PureQL.CSharp.Model.Serialization.ArrayScalars;

internal sealed record StringArrayScalarJsonModel
{
    public StringArrayScalarJsonModel(IStringArrayScalar scalar)
        : this(new StringArrayType(), scalar.Value) { }

    [JsonConstructor]
    public StringArrayScalarJsonModel(StringArrayType type, IEnumerable<string> value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public StringArrayType Type { get; }

    public IEnumerable<string> Value { get; }
}

internal sealed class StringArrayScalarConverter : JsonConverter<IStringArrayScalar>
{
    public override IStringArrayScalar Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringArrayScalarJsonModel model =
            JsonSerializer.Deserialize<StringArrayScalarJsonModel>(ref reader, options)!;

        return new StringArrayScalar(model.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IStringArrayScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StringArrayScalarJsonModel(value), options);
    }
}
