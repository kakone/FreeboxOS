using System.Text.Json.Serialization;

namespace FreeboxOS.TV;

/// <summary>
/// Channels package
/// </summary>
public class Package
{
    /// <summary>
    /// Gets or sets the package identifier
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the package
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the state of the package
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = null!;
}
