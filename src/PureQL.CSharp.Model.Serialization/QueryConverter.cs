using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model.Fields;
using PureQL.CSharp.Model.Returnings;

namespace PureQL.CSharp.Model.Serialization;

internal sealed record QueryJsonModel
{
    public QueryJsonModel(Query query)
        : this(
            query.From,
            query.SelectExpressions,
            query.Where,
            query.Join,
            query.GroupBy,
            query.Having,
            query.OrderBy,
            query.Pagination
        )
    { }

    [JsonConstructor]
    public QueryJsonModel(
        FromExpression from,
        IEnumerable<SelectExpression> select,
        BooleanReturning? where,
        IEnumerable<Join>? join,
        IEnumerable<Field>? groupBy,
        BooleanReturning? having,
        IEnumerable<Field>? orderBy,
        Pagination? pagination
    )
    {
        From = from ?? throw new JsonException();
        Select = select ?? throw new JsonException();
        Where = where;
        Join = join;
        GroupBy = groupBy;
        Having = having;
        OrderBy = orderBy;
        Pagination = pagination;
    }

    public FromExpression From { get; }

    public IEnumerable<SelectExpression> Select { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BooleanReturning? Where { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Join>? Join { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Field>? GroupBy { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BooleanReturning? Having { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Field>? OrderBy { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Pagination? Pagination { get; }
}

internal sealed class QueryConverter : JsonConverter<Query>
{
    public override Query Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        QueryJsonModel model = JsonSerializer.Deserialize<QueryJsonModel>(
            ref reader,
            options
        )!;

        return new Query(
            model.From,
            model.Select,
            model.Where,
            model.Join,
            model.GroupBy,
            model.Having,
            model.OrderBy,
            model.Pagination
        );
    }

    public override void Write(
        Utf8JsonWriter writer,
        Query value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new QueryJsonModel(value), options);
    }
}
