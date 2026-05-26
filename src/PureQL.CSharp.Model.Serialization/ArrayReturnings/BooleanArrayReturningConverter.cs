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
        if (value.TryPickT0(out BooleanArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize<IBooleanArrayScalar>(writer, scalar, options);
        }
        else if (value.TryPickT1(out BooleanField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out BooleanArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT3(out EachComparison? comparison, out _))
        {
            JsonSerializer.Serialize(writer, comparison, options);
        }
        else if (value.TryPickT4(out EachEquality? equality, out _))
        {
            JsonSerializer.Serialize(writer, equality, options);
        }
        else if (value.TryPickT5(out EachAndOperator? andOp, out _))
        {
            JsonSerializer.Serialize(writer, andOp, options);
        }
        else if (value.TryPickT6(out EachOrOperator? orOp, out _))
        {
            JsonSerializer.Serialize(writer, orOp, options);
        }
        else if (value.TryPickT7(out EachNotOperator? notOp, out _))
        {
            JsonSerializer.Serialize(writer, notOp, options);
        }
        else
        {
            throw new JsonException(
                "Unable to determine BooleanArrayReturning type."
            );
        }
    }
}
