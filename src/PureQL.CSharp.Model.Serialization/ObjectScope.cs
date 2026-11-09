namespace PureQL.CSharp.Model.Serialization;

/// <summary>The state of one JSON object being read as a record.</summary>
internal struct ObjectScope(RecordShape shape)
{
    public RecordShape Shape { get; } = shape;

    /// <summary>The bits of the properties read so far.</summary>
    public ulong Seen { get; set; }

    /// <summary>Whether the path ends with the property being read.</summary>
    public bool InProperty { get; set; }
}
