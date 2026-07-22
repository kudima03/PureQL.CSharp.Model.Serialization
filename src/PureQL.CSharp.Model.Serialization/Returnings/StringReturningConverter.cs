using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.String;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class StringReturningConverter : JsonConverter<StringReturning>
{
    public override StringReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(
                root,
                options,
                out StringParameter? parameter
            )
                ? new StringReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out IStringScalar? scalar)
                ? new StringReturning(new StringScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out StringAggregate? aggregate
            )
                ? new StringReturning(aggregate!)
            : throw new JsonException("Unable to determine StringReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        StringReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            parameter => JsonSerializer.Serialize(writer, parameter, options),
            scalar => JsonSerializer.Serialize<IStringScalar>(writer, scalar, options),
            aggregate => JsonSerializer.Serialize(writer, aggregate, options)
        );
    }
}
