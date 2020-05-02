using System.Text.Json.Serialization;

namespace FreeboxOS.TV
{
    /// <summary>
    /// Channel base class
    /// </summary>
    public abstract class ChannelBase
    {
        /// <summary>
        /// Gets or sets the channel identifier
        /// </summary>
        [JsonPropertyName("uuid")]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating wether the channel is available or not
        /// </summary>
        [JsonPropertyName("available")]
        public bool Available { get; set; } = true;
    }
}
