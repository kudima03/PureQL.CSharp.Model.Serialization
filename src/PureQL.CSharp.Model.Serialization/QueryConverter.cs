using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using PureQL.CSharp.Model.ArrayReturnings;
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
            query.Pagination,
            query.Distinct
        )
    { }

    [JsonConstructor]
    public QueryJsonModel(
        FromExpression from,
        IEnumerable<SelectExpression> select,
        OneOf<BooleanReturning, BooleanArrayReturning>? where,
        IEnumerable<Join>? join,
        IEnumerable<Field>? groupBy,
        BooleanReturning? having,
        IEnumerable<OrderByItem>? orderBy,
        Pagination? pagination,
        bool distinct = false
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
        Distinct = distinct;
    }

    public FromExpression From { get; }

    public IEnumerable<SelectExpression> Select { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OneOf<BooleanReturning, BooleanArrayReturning>? Where { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Join>? Join { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Field>? GroupBy { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BooleanReturning? Having { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<OrderByItem>? OrderBy { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Pagination? Pagination { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Distinct { get; }
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
            model.Pagination,
            model.Distinct
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
