using System.Text.Json.Serialization;

namespace FreeboxOS.TV;

/// <summary>
/// TV service status
/// </summary>
public class Status
{
    /// <summary>
    /// Gets or sets a value indicating if the TV service is enabled or not
    /// </summary>
    [JsonPropertyName("tv_enabled")]
    public bool Enabled { get; set; }
}
