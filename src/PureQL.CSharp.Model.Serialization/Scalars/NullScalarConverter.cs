using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Scalars;
using PureQL.CSharp.Model.Types;

namespace PureQL.CSharp.Model.Serialization.Scalars;

internal sealed record NullScalarJsonModel
{
    public NullScalarJsonModel(NullType? type)
    {
        Type = type ?? throw new JsonException();
    }

    public NullType? Type { get; }
}

public sealed class NullScalarConverter : JsonConverter<INullScalar>
{
    public override INullScalar? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        _ = JsonSerializer.Deserialize<NullScalarJsonModel>(ref reader, options)!;

        return new NullScalar();
    }

    public override void Write(
        Utf8JsonWriter writer,
        INullScalar value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new NullScalarJsonModel(new NullType()),
            options
        );
    }
}
