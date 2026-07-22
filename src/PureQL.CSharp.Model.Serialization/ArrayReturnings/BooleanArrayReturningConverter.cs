using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachBooleanOperations;
using PureQL.CSharp.Model.EachComparisons;
using PureQL.CSharp.Model.EachEqualities;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class BooleanArrayReturningConverter
    : JsonConverter<BooleanArrayReturning>
{
    public override BooleanArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out BooleanField? field)
                ? new BooleanArrayReturning(field!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out IBooleanArrayScalar? scalar
            )
                ? new BooleanArrayReturning(new BooleanArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out BooleanArrayParameter? parameter
            )
                ? new BooleanArrayReturning(parameter!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachComparison? comparison
            )
                ? new BooleanArrayReturning(comparison!)
            : JsonExtensions.TryDeserialize(root, options, out EachEquality? equality)
                ? new BooleanArrayReturning(equality!)
            : JsonExtensions.TryDeserialize(root, options, out EachAndOperator? andOp)
                ? new BooleanArrayReturning(andOp!)
            : JsonExtensions.TryDeserialize(root, options, out EachOrOperator? orOp)
                ? new BooleanArrayReturning(orOp!)
            : JsonExtensions.TryDeserialize(root, options, out EachNotOperator? notOp)
                ? new BooleanArrayReturning(notOp!)
            : throw new JsonException(
                "Unable to determine BooleanArrayReturning type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanArrayReturning value,
        JsonSerializerOptions options
    )
    {
        value.Switch(
            scalar => JsonSerializer.Serialize<IBooleanArrayScalar>(writer, scalar, options),
            field => JsonSerializer.Serialize(writer, field, options),
            parameter => JsonSerializer.Serialize(writer, parameter, options),
            comparison => JsonSerializer.Serialize(writer, comparison, options),
            equality => JsonSerializer.Serialize(writer, equality, options),
            andOp => JsonSerializer.Serialize(writer, andOp, options),
            orOp => JsonSerializer.Serialize(writer, orOp, options),
            notOp => JsonSerializer.Serialize(writer, notOp, options)
        );
    }
}
