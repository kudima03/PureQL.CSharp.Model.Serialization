using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Serialization;

internal sealed record OrderByItemJsonModel
{
    public OrderByItemJsonModel(OrderByItem item)
        : this(item.Field, item.Direction)
    { }

    [JsonConstructor]
    public OrderByItemJsonModel(Field field, SortDirection direction = SortDirection.Asc)
    {
        Field = field ?? throw new JsonException();
        Direction = direction;
    }

    public Field Field { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SortDirection Direction { get; }
}

internal sealed class OrderByItemConverter : JsonConverter<OrderByItem>
{
    public override OrderByItem Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        OrderByItemJsonModel model =
            JsonSerializer.Deserialize<OrderByItemJsonModel>(ref reader, options)
            ?? throw new JsonException();

        return new OrderByItem(model.Field, model.Direction);
    }

    public override void Write(
        Utf8JsonWriter writer,
        OrderByItem value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new OrderByItemJsonModel(value), options);
    }
}
