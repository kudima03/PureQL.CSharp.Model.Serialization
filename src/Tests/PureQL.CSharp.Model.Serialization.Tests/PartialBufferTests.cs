using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// Reads from a stream or a pipe, as <c>DeserializeAsync</c> and ASP.NET Core model
/// binding do: the serializer gives a converter its whole value, but on a reader whose
/// block is not final, as more of the stream may follow.
/// </summary>
public sealed record PartialBufferTests
{
    /// <summary>
    /// A query whose unions dispatch on every kind of look-ahead: grouped and paginated
    /// query variants, a <c>from</c> subquery, an <c>in</c> subquery, operators and the
    /// operand family of comparisons.
    /// </summary>
    private const string Pipeline = "samples/valid/057_subquery_full_pipeline.json";

    [Theory]
    [MemberData(nameof(ValidFiles))]
    public async Task ReadsAQueryFromAStreamInChunks(string name)
    {
        string json = SpecificationFiles.Read(name);

        // Followed by more JSON, the first query is read before the stream ends.
        PureQLQuery[] queries = (
            await JsonSerializer.DeserializeAsync<PureQLQuery[]>(
                new ChunkedStream(Encoding.UTF8.GetBytes($"[{json},\n{json}]")),
                SmallBuffer
            )
        )!;

        Assert.Equal(2, queries.Length);
        Assert.All(
            queries,
            query =>
                Assert.Equal(
                    JsonSemantics.Canonical(json),
                    JsonSemantics.Canonical(PureQLJson.Serialize(query))
                )
        );
    }

    [Theory]
    [MemberData(nameof(ValidFiles))]
    public async Task ReadsAQueryFromAPipe(string name)
    {
        // The serializer's pipe reader fails on a comment cut across blocks.
        string json = JsonNode
            .Parse(
                SpecificationFiles.Read(name),
                documentOptions: new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip,
                    MaxDepth = 256,
                }
            )!
            .ToJsonString();
        PipeReader pipe = PipeReader.Create(
            new ChunkedStream(Encoding.UTF8.GetBytes(json)),
            new StreamPipeReaderOptions(bufferSize: 16, minimumReadSize: 1)
        );

        PureQLQuery query = await JsonSerializer.DeserializeAsync<PureQLQuery>(
            pipe,
            PureQLJson.Options
        );

        Assert.Equal(
            JsonSemantics.Canonical(json),
            JsonSemantics.Canonical(PureQLJson.Serialize(query))
        );
    }

    [Theory]
    [MemberData(nameof(ModelTypes))]
    public async Task ReadsEveryVariantFromAStreamInChunks(string typeName)
    {
        Type type = ModelSamples.Types.Single(t => t.FullName == typeName);
        foreach (object value in ModelSamples.Of(type))
        {
            string json = JsonSerializer.Serialize(value, type, PureQLJson.Options);

            // Followed by more JSON, the first value is read before the stream ends.
            Array back = (Array)(
                await JsonSerializer.DeserializeAsync(
                    new ChunkedStream(Encoding.UTF8.GetBytes($"[{json},{json}]")),
                    type.MakeArrayType(),
                    SmallBuffer
                )
            )!;

            Assert.Equal(2, back.Length);
            ModelAssert.Equivalent(value, back.GetValue(0));
            ModelAssert.Equivalent(value, back.GetValue(1));
        }
    }

    [Fact]
    public void ReadsAWholeValueFromANonFinalBlock()
    {
        byte[] json = Encoding.UTF8.GetBytes(SpecificationFiles.Read(Pipeline));

        PureQLQuery query = ReadPartial(json, json.Length);

        Assert.Equal(
            JsonSemantics.Canonical(Encoding.UTF8.GetString(json)),
            JsonSemantics.Canonical(PureQLJson.Serialize(query))
        );
    }

    [Fact]
    public void RejectsATruncatedValueOfANonFinalBlock()
    {
        byte[] json = Encoding.UTF8.GetBytes(SpecificationFiles.Read(Pipeline).Trim());

        for (int length = 1; length < json.Length; length++)
        {
            JsonException error = Assert.ThrowsAny<JsonException>(
                () => ReadPartial(json, length)
            );
            Assert.NotNull(error.Message);
        }
    }

    public static TheoryData<string> ValidFiles()
    {
        return SpecificationFiles.Data(SpecificationFiles.Valid());
    }

    public static TheoryData<string> ModelTypes()
    {
        return SpecificationFiles.Data(ModelSamples.Types.Select(t => t.FullName!));
    }

    /// <summary>
    /// The converters with a buffer smaller than any query, so the serializer reads the
    /// stream in many blocks and none of them is final before the value is read.
    /// </summary>
    private static JsonSerializerOptions SmallBuffer { get; } =
        new JsonSerializerOptions(PureQLJson.Options) { DefaultBufferSize = 16 };

    /// <summary>
    /// Calls the converter of <see cref="PureQLQuery"/> as the serializer does, on a
    /// reader over the first <paramref name="length"/> bytes whose block is not final.
    /// </summary>
    private static PureQLQuery ReadPartial(byte[] json, int length)
    {
        JsonConverter<PureQLQuery> converter = (JsonConverter<PureQLQuery>)
            PureQLJson.Options.GetConverter(typeof(PureQLQuery));
        Utf8JsonReader reader = new Utf8JsonReader(
            json.AsSpan(0, length),
            isFinalBlock: false,
            new JsonReaderState(new JsonReaderOptions { MaxDepth = 256 })
        );
        return reader.Read()
            ? converter.Read(ref reader, typeof(PureQLQuery), PureQLJson.Options)
            : throw new JsonException("No value");
    }

    /// <summary>A stream that returns 1 to 16 bytes per read.</summary>
    private sealed class ChunkedStream(byte[] bytes) : Stream
    {
        private readonly byte[] _bytes = bytes;

        private int _position;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Read(buffer.AsSpan(offset, count));
        }

        public override int Read(Span<byte> buffer)
        {
            int size = Math.Min(
                Math.Min(buffer.Length, _bytes.Length - _position),
                1 + (_position % 16)
            );
            _bytes.AsSpan(_position, size).CopyTo(buffer);
            _position += size;
            return size;
        }

        public override ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default
        )
        {
            return ValueTask.FromResult(Read(buffer.Span));
        }

        public override Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Read(buffer.AsSpan(offset, count)));
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
