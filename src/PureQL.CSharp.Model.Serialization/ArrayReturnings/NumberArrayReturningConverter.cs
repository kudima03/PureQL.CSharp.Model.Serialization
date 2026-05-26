using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.ArrayParameters;
using PureQL.CSharp.Model.ArrayReturnings;
using PureQL.CSharp.Model.ArrayScalars;
using PureQL.CSharp.Model.EachArithmetics;
using PureQL.CSharp.Model.EachDateArithmetics;
using PureQL.CSharp.Model.EachDateTimeArithmetics;
using PureQL.CSharp.Model.EachTimeArithmetics;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization.ArrayReturnings;

internal sealed class NumberArrayReturningConverter : JsonConverter<NumberArrayReturning>
{
    public override NumberArrayReturning Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out NumberField? field)
                ? new NumberArrayReturning(field!)
            : JsonExtensions.TryDeserialize(root, options, out INumberArrayScalar? scalar)
                ? new NumberArrayReturning(new NumberArrayScalar(scalar!.Value))
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out NumberArrayParameter? parameter
            )
                ? new NumberArrayReturning(parameter!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachArithmetic? arithmetic
            )
                ? new NumberArrayReturning(arithmetic!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateDiffDays? dateDiff
            )
                ? new NumberArrayReturning(dateDiff!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachDateTimeDiffSeconds? dateTimeDiff
            )
                ? new NumberArrayReturning(dateTimeDiff!)
            : JsonExtensions.TryDeserialize(
                root,
                options,
                out EachTimeDiffSeconds? timeDiff
            )
                ? new NumberArrayReturning(timeDiff!)
            : throw new JsonException(
                "Unable to determine NumberArrayReturning type."
            );
    }

    public override void Write(
        Utf8JsonWriter writer,
        NumberArrayReturning value,
        JsonSerializerOptions options
    )
    {
        if (value.TryPickT0(out NumberArrayParameter? parameter, out _))
        {
            JsonSerializer.Serialize(writer, parameter, options);
        }
        else if (value.TryPickT1(out NumberField? field, out _))
        {
            JsonSerializer.Serialize(writer, field, options);
        }
        else if (value.TryPickT2(out NumberArrayScalar? scalar, out _))
        {
            JsonSerializer.Serialize<INumberArrayScalar>(writer, scalar, options);
        }
        else if (value.TryPickT3(out EachArithmetic? arithmetic, out _))
        {
            JsonSerializer.Serialize(writer, arithmetic, options);
        }
        else if (value.TryPickT4(out EachDateDiffDays? dateDiff, out _))
        {
            JsonSerializer.Serialize(writer, dateDiff, options);
        }
        else if (value.TryPickT5(out EachDateTimeDiffSeconds? dateTimeDiff, out _))
        {
            JsonSerializer.Serialize(writer, dateTimeDiff, options);
        }
        else if (value.TryPickT6(out EachTimeDiffSeconds? timeDiff, out _))
        {
            JsonSerializer.Serialize(writer, timeDiff, options);
        }
        else
        {
            throw new JsonException(
                "Unable to determine NumberArrayReturning type."
            );
        }
    }
}
