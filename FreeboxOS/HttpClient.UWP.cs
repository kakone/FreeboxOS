using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Web.Http.Filters;

namespace FreeboxOS
{
    /// <summary>
    /// HTTP client
    /// </summary>
    public class HttpClient : HttpClientBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class
        /// </summary>
        /// <param name="rootCertificates">root certificates</param>
        public HttpClient(IRootCertificates rootCertificates) : base(rootCertificates)
        {
        }

        /// <inheritdoc/>
        public override async Task<T> GetAsync<T>(string requestUri)
        {
            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;
            using var httpClient = new Windows.Web.Http.HttpClient(filter);
            var response = await httpClient.GetAsync(new Uri(requestUri));
            var transportInformation = response.RequestMessage.TransportInformation;
            if (transportInformation.ServerCertificateErrorSeverity == SocketSslErrorSeverity.Fatal)
            {
                throw new SecurityException();
            }
            var rootCertificate = transportInformation.ServerIntermediateCertificates.LastOrDefault()?.GetCertificateBlob().ToArray();
            if (rootCertificate == null || !RootCertificates.Any(c => c.RawData != rootCertificate))
            {
                throw new SecurityException();
            }
            return await JsonSerializer.DeserializeAsync<T>((await response.Content.ReadAsInputStreamAsync()).AsStreamForRead());
        }
    }
}
