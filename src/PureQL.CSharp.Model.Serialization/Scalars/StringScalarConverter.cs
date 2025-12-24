using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record StringScalarJsonModel
{
    public StringScalarJsonModel(IStringScalar scalar)
        : this(new StringType(), scalar.Value) { }

    [JsonConstructor]
    public StringScalarJsonModel(StringType type, string value)
    {
        Type = type ?? throw new JsonException();
        Value = value ?? throw new JsonException();
    }

    public StringType Type { get; }

    public string Value { get; }
}

public sealed class StringScalarConverter : JsonConverter<IStringScalar>
{
    public override IStringScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StringScalarJsonModel model = JsonSerializer.Deserialize<StringScalarJsonModel>(
            ref reader,
            options
        )!;

        return new StringScalar(model.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IStringScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StringScalarJsonModel(value), options);
    }
}
