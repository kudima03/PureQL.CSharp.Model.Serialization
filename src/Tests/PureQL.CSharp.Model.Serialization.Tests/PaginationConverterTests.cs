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

    // The converter itself performs no range validation on skip/take beyond
    // "present and non-null" - these tests pin that zero, negative, and
    // extreme long values all round trip unchanged, so a future rewrite that
    // adds such validation shows up as a deliberate, visible test change
    // rather than silently altering accepted input.
    [Fact]
    public void ReadZeroValues()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "skip": 0,
              "take": 0
            }
            """;

        Pagination value = JsonSerializer.Deserialize<Pagination>(input, _options)!;

        Assert.Equal(new Pagination(0, 0), value);
    }

    [Fact]
    public void WriteZeroValues()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "skip": 0,
              "take": 0
            }
            """;

        string output = JsonSerializer.Serialize(new Pagination(0, 0), _options);

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadNegativeValues()
    {
        const string input = /*lang=json,strict*/
            """
            {
              "skip": -1,
              "take": -1
            }
            """;

        Pagination value = JsonSerializer.Deserialize<Pagination>(input, _options)!;

        Assert.Equal(new Pagination(-1, -1), value);
    }

    [Fact]
    public void WriteNegativeValues()
    {
        const string expected = /*lang=json,strict*/
            """
            {
              "skip": -1,
              "take": -1
            }
            """;

        string output = JsonSerializer.Serialize(new Pagination(-1, -1), _options);

        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadBoundaryLongValues()
    {
        string input = /*lang=json,strict*/
            $$"""
            {
              "skip": {{long.MinValue}},
              "take": {{long.MaxValue}}
            }
            """;

        Pagination value = JsonSerializer.Deserialize<Pagination>(input, _options)!;

        Assert.Equal(new Pagination(long.MinValue, long.MaxValue), value);
    }

    [Fact]
    public void WriteBoundaryLongValues()
    {
        string expected = /*lang=json,strict*/
            $$"""
            {
              "skip": {{long.MinValue}},
              "take": {{long.MaxValue}}
            }
            """;

        string output = JsonSerializer.Serialize(
            new Pagination(long.MinValue, long.MaxValue),
            _options
        );

        Assert.Equal(expected, output);
    }
}
