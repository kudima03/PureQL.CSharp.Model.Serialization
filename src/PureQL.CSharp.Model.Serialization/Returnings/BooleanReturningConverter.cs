using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.BooleanOperations;
using PureQL.CSharp.Model.Equalities;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Parameters;
using PureQL.CSharp.Model.Returnings;
using PureQL.CSharp.Model.Scalars;

namespace PureQL.CSharp.Model.Serialization.Returnings;

public sealed class BooleanReturningConverter : JsonConverter<BooleanReturning>
{
    public override BooleanReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out BooleanField? boolean)
                ? new BooleanReturning(boolean!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out BooleanParameter? parameter
            )
                ? new BooleanReturning(parameter!)
            : JsonExtensions.TryDeserialize(root, options, out IBooleanScalar? scalar)
                ? new BooleanReturning(new BooleanScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(root, options, out Equality? equality)
                ? new BooleanReturning(equality!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out BooleanOperator? booleanOperator
            )
                ? new BooleanReturning(booleanOperator!)
            : throw new JsonException("Unable to determine BooleanReturning type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        BooleanReturning value,
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
            JsonSerializer.Serialize<IBooleanScalar>(writer, value.AsT2, options);
        }
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(writer, value.AsT3, options);
        }
        else if (value.IsT4)
        {
            JsonSerializer.Serialize(writer, value.AsT4, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
