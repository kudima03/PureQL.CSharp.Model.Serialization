using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates.Time;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class TimeReturningConverter : JsonConverter<TimeReturning>
{
    public override TimeReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out TimeParameter? parameter)
                ? new TimeReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out ITimeScalar? scalar)
                ? new TimeReturning(new TimeScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out TimeAggregate? aggregate
            )
                ? new TimeReturning(aggregate!)
            : throw new JsonException("Unable to determine TimeReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            parameter => JsonSerializer.Serialize(writer, parameter, options),
            scalar => JsonSerializer.Serialize<ITimeScalar>(writer, scalar, options),
            aggregate => JsonSerializer.Serialize(writer, aggregate, options)
        );
    }
}
