using System;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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

        /// <summary>
        /// Creates an HTTP client handler instance
        /// </summary>
        /// <returns>a new HTTP client handler instance</returns>
        protected override Task<HttpClientHandler> CreateHttpClientHandlerAsync()
        {
            return Task.FromResult(new HttpClientHandler() { ServerCertificateCustomValidationCallback = ValidateCertificate });
        }
    }
}
