using System.Text.Json.Serialization;

namespace FreeboxOS.TV;

/// <summary>
/// Channel
/// </summary>
public class NumberedChannel : ChannelBase
{
    /// <summary>
    /// Gets or sets the number of the channel
    /// </summary>
    [JsonPropertyName("number")]
    public int Number { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the channel is published 
    /// </summary>
    [JsonPropertyName("pub_service")]
    public bool PubService { get; set; }

    /// <summary>
    /// Gets or sets the streams
    /// </summary>
    [JsonPropertyName("streams")]
    public IEnumerable<Stream> Streams { get; set; } = null!;
}
