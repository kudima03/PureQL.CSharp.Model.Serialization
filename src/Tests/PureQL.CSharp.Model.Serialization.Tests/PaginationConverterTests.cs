using System.Text.Json;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization.Tests;

public sealed record PaginationConverterTests
{
    private readonly JsonSerializerOptions _options;

    public PaginationConverterTests()
    {
        _options = new JsonSerializerOptions()
        {
            NewLine = "\n",
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        foreach (JsonConverter converter in new PureQLConverters())
        {
            _options.Converters.Add(converter);
        }
    }

    [Fact]
    public void Read()
    {
        long skip = Random.Shared.NextInt64();
        long take = Random.Shared.NextInt64();
        string input = /*lang=json,strict*/
            $$"""
            {
              "skip": {{skip}},
              "take": {{take}}
            }
            """;

        Pagination value = JsonSerializer.Deserialize<Pagination>(input, _options)!;

        Assert.Equal(new Pagination(skip, take), value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("{}")]
    [InlineData("{")]
    [InlineData("}")]
    [InlineData( /*lang=json,strict*/
        """{"skip":1}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"take":1}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"skip":1,"take":null}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"skip":null,"take":1}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"skip":"","take":1}"""
    )]
    [InlineData( /*lang=json,strict*/
        """{"skip":1,"take":""}"""
    )]
    public void ThrowsExceptionOnBadFormat(string input)
    {
        _ = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Pagination>(input, _options)
        );
    }

    [Fact]
    public void Write()
    {
        long skip = Random.Shared.NextInt64();
        long take = Random.Shared.NextInt64();
        string expected = /*lang=json,strict*/
            $$"""
            {
              "skip": {{skip}},
              "take": {{take}}
            }
            """;

        string output = JsonSerializer.Serialize(new Pagination(skip, take), _options);

        Assert.Equal(expected, output);
    }
}
