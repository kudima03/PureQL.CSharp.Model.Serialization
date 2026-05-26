using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.EachArithmetics;

namespace PureQL.CSharp.Model.Serialization.EachArithmetics;

internal sealed class EachAddConverter : JsonConverter<EachAdd>
{
    public override EachAdd Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachArithmeticOperatorJsonModel model =
            JsonSerializer.Deserialize<EachArithmeticOperatorJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator != EachArithmeticOperatorName.eachAdd
            ? throw new JsonException()
            : new EachAdd(model.Values);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachAdd value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachArithmeticOperatorJsonModel(value),
            options
        );
    }
}

internal sealed class EachSubtractConverter : JsonConverter<EachSubtract>
{
    public override EachSubtract Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachArithmeticOperatorJsonModel model =
            JsonSerializer.Deserialize<EachArithmeticOperatorJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator != EachArithmeticOperatorName.eachSubtract
            ? throw new JsonException()
            : new EachSubtract(model.Values);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachSubtract value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachArithmeticOperatorJsonModel(value),
            options
        );
    }
}

internal sealed class EachMultiplyConverter : JsonConverter<EachMultiply>
{
    public override EachMultiply Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachArithmeticOperatorJsonModel model =
            JsonSerializer.Deserialize<EachArithmeticOperatorJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator != EachArithmeticOperatorName.eachMultiply
            ? throw new JsonException()
            : new EachMultiply(model.Values);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachMultiply value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachArithmeticOperatorJsonModel(value),
            options
        );
    }
}

internal sealed class EachDivideConverter : JsonConverter<EachDivide>
{
    public override EachDivide Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        EachArithmeticOperatorJsonModel model =
            JsonSerializer.Deserialize<EachArithmeticOperatorJsonModel>(
                ref reader,
                options
            )!;

        return model.Operator != EachArithmeticOperatorName.eachDivide
            ? throw new JsonException()
            : new EachDivide(model.Values);
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachDivide value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new EachArithmeticOperatorJsonModel(value),
            options
        );
    }
}

internal sealed class EachArithmeticConverter : JsonConverter<EachArithmetic>
{
    public override EachArithmetic Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        return JsonExtensions.TryDeserialize(root, options, out EachAdd? add)
                ? new EachArithmetic(add!)
            : JsonExtensions.TryDeserialize(root, options, out EachSubtract? subtract)
                ? new EachArithmetic(subtract!)
            : JsonExtensions.TryDeserialize(root, options, out EachMultiply? multiply)
                ? new EachArithmetic(multiply!)
            : JsonExtensions.TryDeserialize(root, options, out EachDivide? divide)
                ? new EachArithmetic(divide!)
            : throw new JsonException("Unable to determine EachArithmetic type.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        EachArithmetic value,
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
            JsonSerializer.Serialize(writer, value.AsT2, options);
        }
        else if (value.IsT3)
        {
            JsonSerializer.Serialize(writer, value.AsT3, options);
        }
        else
        {
            throw new JsonException("Unable to determine EachArithmetic type.");
        }
    }
}
