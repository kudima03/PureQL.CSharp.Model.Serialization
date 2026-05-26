using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Aggregates;
using PureQL.CSharp.Model.Aggregates.Numeric;
using PureQL.CSharp.Model.Arithmetics;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

internal sealed class NumberReturningConverter : JsonConverter<NumberReturning>
{
    public override NumberReturning Read(
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
                out NumberParameter? parameter
            )
                ? new NumberReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out INumberScalar? scalar)
                ? new NumberReturning(new NumberScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out Arithmetic? arithmetic
            )
                ? new NumberReturning(arithmetic!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberAggregate? numberAgg
            )
                ? new NumberReturning(numberAgg!)
            : JsonExtensions.TryDeserialize(root, options, out Count? count)
                ? new NumberReturning(count!)
            : throw new JsonException("Unable to determine NumberReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out NumberParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out NumberScalar? scalar, out _))
        {
            JsonSerializer.Serialize<INumberScalar>(writer, scalar, options);
        }
        else if (value.TryPickT2(out Arithmetic? arithmetic, out _))
        {
            JsonSerializer.Serialize(writer, arithmetic, options);
        }
        else if (value.TryPickT3(out NumberAggregate? numberAgg, out _))
        {
            JsonSerializer.Serialize(writer, numberAgg, options);
        }
        else if (value.TryPickT4(out Count? count, out _))
        {
            JsonSerializer.Serialize(writer, count, options);
        }
        else
        {
            throw new JsonException("Unable to determine NumberReturning type.");
        }
    }
}
