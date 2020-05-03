using System.IO;
using System.Threading.Tasks;

namespace FreeboxOS
{
    /// <summary>
    /// HTTP client
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation
        /// </summary>
        /// <param name="requestUri">the Uri the request is sent to</param>
        /// <returns>the task object representing the asynchronous operation</returns>
        Task<Stream> GetStreamAsync(string requestUri);

        /// <summary>
        /// Sends a GET request to the specified Uri and converts the response body to the specified type
        /// </summary>
        /// <typeparam name="T">result type</typeparam>
        /// <param name="requestUri">the Uri the request is sent to</param>
        /// <returns>the task object representing the asynchronous operation</returns>
        Task<T> GetAsync<T>(string requestUri);
    }
}
