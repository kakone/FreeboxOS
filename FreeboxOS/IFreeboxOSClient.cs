namespace FreeboxOS;

/// <summary>
/// Freebox OS client interface
/// </summary>
public interface IFreeboxOSClient
{
    /// <summary>
    /// Gets the Freebox URL
    /// </summary>
    string? URL { get; }

    /// <summary>
    /// Creates an HTTP message handler to call the Freebox OS API
    /// </summary>
    HttpMessageHandler CreateHttpMessageHandler();

    /// <summary>
    /// Searches the Freebox
    /// </summary>
    /// <returns>the Freebox URL</returns>
    Task<string> InitAsync();

    /// <summary>
    /// API method call
    /// </summary>
    /// <typeparam name="T">result type</typeparam>
    /// <param name="apiUrl">api URL</param>
    /// <param name="method">method to call</param>
    /// <param name="parameters">method parameters</param>
    /// <returns>call result</returns>
    Task<T> GetAsync<T>(string apiUrl, string method, params object[] parameters) where T : class;
}
