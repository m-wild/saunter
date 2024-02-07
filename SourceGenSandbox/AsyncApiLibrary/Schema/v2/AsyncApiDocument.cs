namespace AsyncApiLibrary.Schema.v2;

public class AsyncApiDocument : ICloneable
{
    /// <summary>
    /// Specifies the AsyncAPI Specification version being used.
    /// </summary>
    public string? AsyncApi { get; } = "2.6.0";

    /// <summary>
    /// Identifier of the application the AsyncAPI document is defining.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Provides metadata about the API. The metadata can be used by the clients if needed.
    /// </summary>
    public required Info Info { get; set; }

    /// <summary>
    /// Provides connection details of servers.
    /// </summary>
    public Dictionary<string, Server> Servers { get; set; } = [];

    /// <summary>
    /// A string representing the default content type to use when encoding/decoding a message's payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// </summary>
    public string? DefaultContentType { get; set; } = "application/json";

    /// <summary>
    /// The available channels and messages for the API.
    /// </summary>
    public Dictionary<string, ChannelItem> Channels { get; set; } = [];

    /// <summary>
    /// An element to hold various schemas for the specification.
    /// </summary>
    public Components Components { get; set; } = new();

    /// <summary>
    /// A list of tags used by the specification with additional metadata.
    /// Each tag name in the list MUST be unique.
    /// </summary>
    public HashSet<Tag> Tags { get; set; } = [];

    /// <summary>
    /// Additional external documentation.
    /// </summary>
    public ExternalDocumentation? ExternalDocs { get; set; }

    public AsyncApiDocument Clone()
    {
        AsyncApiDocument clone = new()
        {
            Id = Id,
            Info = Info,
            DefaultContentType = DefaultContentType,
            ExternalDocs = ExternalDocs,
            Channels = Channels.ToDictionary(c => c.Key, c => c.Value),
            Servers = Servers.ToDictionary(p => p.Key, p => p.Value),
            Components = Components.Clone()
        };

        if (Tags is not null)
        {
            clone.Tags = [.. Tags];
        }

        return clone;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}