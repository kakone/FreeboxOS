using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeboxOS
{
    /// <summary>
    /// HTTP client
    /// </summary>
    public class HttpClient : IHttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class
        /// </summary>
        /// <param name="rootCertificates">root certificates</param>
        public HttpClient(IRootCertificates rootCertificates)
        {
            RootCertificates = rootCertificates;
        }

        private IRootCertificates RootCertificates { get; }

        private bool ValidateCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if ((sslPolicyErrors & SslPolicyErrors.None) > 0)
            {
                return true;
            }

            if ((sslPolicyErrors & (SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateNotAvailable)) > 0)
            {
                return false;
            }

            foreach (var rootCertificate in RootCertificates)
            {
                chain.ChainPolicy.ExtraStore.Add(rootCertificate);
            }
            chain.ChainPolicy.VerificationFlags |= X509VerificationFlags.AllowUnknownCertificateAuthority;
            if (chain.Build(certificate))
            {
                var chainRoot = chain.ChainElements.Cast<X509ChainElement>().Last().Certificate;
                return RootCertificates.Any(c => c.RawData.SequenceEqual(chainRoot.RawData));
            }
            return false;
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string requestUri)
        {
            using var httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ValidateCertificate
            };

            using var httpClient = new System.Net.Http.HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            return await JsonSerializer.DeserializeAsync<T>(await httpClient.GetStreamAsync(requestUri));
        }
    }
}
