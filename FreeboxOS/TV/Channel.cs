using System.Text.Json.Serialization;

namespace FreeboxOS.TV;

/// <summary>
/// Channel
/// </summary>
public class Channel : ChannelBase
{
    /// <summary>
    /// Gets or sets the name of the channel
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the logo URL
    /// </summary>
    [JsonPropertyName("logo_url")]
    public string LogoUrl { get; set; } = null!;
}
