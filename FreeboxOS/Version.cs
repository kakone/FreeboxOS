using System.Text.Json.Serialization;

namespace FreeboxOS
{
    /// <summary>
    /// Version
    /// </summary>
    public class Version
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        /// <summary>
        /// Gets or sets the device name
        /// </summary>
        [JsonPropertyName("device_name")]
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the box model
        /// </summary>
        [JsonPropertyName("box_model")]
        public string BoxModel { get; set; }

        /// <summary>
        /// Gets or sets the box model name
        /// </summary>
        [JsonPropertyName("box_model_name")]
        public string BoxModelName { get; set; }

        /// <summary>
        /// Gets or sets the API version
        /// </summary>
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the API base URL
        /// </summary>
        [JsonPropertyName("api_base_url")]
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the device type
        /// </summary>
        [JsonPropertyName("device_type")]
        public string DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the API domain
        /// </summary>
        [JsonPropertyName("api_domain")]
        public string ApiDomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the HTTPS is available
        /// </summary>
        [JsonPropertyName("https_available")]
        public bool HttpsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS port
        /// </summary>
        [JsonPropertyName("https_port")]
        public int HttpsPort { get; set; }
    }
}
