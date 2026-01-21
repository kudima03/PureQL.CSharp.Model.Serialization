using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization;

internal sealed record PaginationJsonModel
{
    public PaginationJsonModel(Pagination pagination)
        : this(pagination.Skip, pagination.Take) { }

    [JsonConstructor]
    public PaginationJsonModel(long? skip, long? take)
    {
        Skip = skip ?? throw new JsonException();
        Take = take ?? throw new JsonException();
    }

    public long? Skip { get; }

    public long? Take { get; }
}

internal sealed class PaginationConverter : JsonConverter<Pagination>
{
    public override Pagination Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        PaginationJsonModel model = JsonSerializer.Deserialize<PaginationJsonModel>(
            ref reader,
            options
        )!;

        return new Pagination(model.Skip!.Value, model.Take!.Value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Pagination value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new PaginationJsonModel(value), options);
    }
}
