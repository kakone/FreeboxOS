using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Java.Security.Cert;
using Xamarin.Android.Net;

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
        protected override async Task<HttpClientHandler> CreateHttpClientHandlerAsync()
        {
            using var certificateFactory = CertificateFactory.GetInstance("X.509");
            var rootCertificates = new List<Certificate>();
            foreach (var rootCertificate in RootCertificates)
            {
                using var stream = new MemoryStream(rootCertificate.GetRawCertData());
                rootCertificates.Add(await certificateFactory.GenerateCertificateAsync(stream));
            }
            return new AndroidClientHandler() { TrustedCerts = rootCertificates };
        }
    }
}
