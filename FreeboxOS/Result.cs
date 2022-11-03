using System.Text.Json.Serialization;

namespace FreeboxOS;

/// <summary>
/// Web service return result class
/// </summary>
internal class Result<T> where T : class
{
    /// <summary>
    /// Gets or sets a value indicating whether the request was successful
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the error code
    /// </summary>
    [JsonPropertyName("error_code")]
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the error message
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the result of the request
    /// </summary>
    [JsonPropertyName("result")]
    public T Object { get; set; } = null!;
}
