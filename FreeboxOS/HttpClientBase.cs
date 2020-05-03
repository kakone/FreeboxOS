using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeboxOS
{
    /// <summary>
    /// HTTP client base class
    /// </summary>
    public abstract class HttpClientBase : IHttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientBase"/> class
        /// </summary>
        /// <param name="rootCertificates">root certificates</param>
        public HttpClientBase(IRootCertificates rootCertificates)
        {
            RootCertificates = rootCertificates;
        }

        /// <summary>
        /// Gets the root certificates
        /// </summary>
        protected IRootCertificates RootCertificates { get; }

        /// <summary>
        /// Creates an HTTP client handler instance
        /// </summary>
        /// <returns>a new HTTP client handler instance</returns>
        protected virtual Task<HttpClientHandler> CreateHttpClientHandlerAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual async Task<Stream> GetStreamAsync(string requestUri)
        {
            using var httpClientHandler = await CreateHttpClientHandlerAsync();
            using var httpClient = new System.Net.Http.HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            return await httpClient.GetStreamAsync(requestUri);
        }

        /// <inheritdoc/>
        public virtual async Task<T> GetAsync<T>(string requestUri)
        {
            return await JsonSerializer.DeserializeAsync<T>(await GetStreamAsync(requestUri));
        }
    }
}
