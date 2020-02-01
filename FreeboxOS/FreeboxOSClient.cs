using FreeboxOS.Resources;
using Nito.AsyncEx;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Zeroconf;

namespace FreeboxOS
{
    /// <summary>
    /// Freebox OS client
    /// </summary>
    public class FreeboxOSClient : IFreeboxOSClient
    {
        private const string PROTOCOL_NAME = "_fbx-api._tcp.local.";
        private const byte API_VERSION = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeboxOSClient"/> class
        /// </summary>
        /// <param name="rootCertificates">root certificates</param>
        public FreeboxOSClient(IRootCertificates rootCertificates)
        {
            RootCertificates = rootCertificates;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~FreeboxOSClient()
        {
            Dispose();
        }

        private IRootCertificates RootCertificates { get; }
        private AsyncLock Mutex { get; } = new AsyncLock();
        private string BaseURL { get; set; }

        private async Task InitAsync()
        {
            if (BaseURL != null)
            {
                return;
            }

            using (await Mutex.LockAsync())
            {
                var freeboxServer = (await ZeroconfResolver.ResolveAsync(PROTOCOL_NAME))?.FirstOrDefault();
                if (freeboxServer == null)
                {
                    throw new FreeboxOSException(Strings.FreeboxNotFound);
                }
                var properties = freeboxServer.Services.First().Value.Properties.First();
                BaseURL = $"https://{properties["api_domain"]}:{Convert.ToInt32(properties["https_port"])}{properties["api_base_url"]}v{API_VERSION}";
            }
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
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            if (chain.Build(certificate))
            {
                var chainRoot = chain.ChainElements.Cast<X509ChainElement>().Last().Certificate;
                return RootCertificates.Any(c => c.RawData.SequenceEqual(chainRoot.RawData));
            }
            return false;
        }

        /// <summary>
        /// API method call
        /// </summary>
        /// <typeparam name="T">result type</typeparam>
        /// <param name="apiUrl">api URL</param>
        /// <param name="method">method to call</param>
        /// <param name="parameters">method parameters</param>
        /// <returns>call result</returns>
        public async Task<T> GetAsync<T>(string apiUrl, string method, params object[] parameters)
        {
            await InitAsync();
            using var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = ValidateCertificate
            };
            using var httpClient = new HttpClient(httpClientHandler);
            var result = await JsonSerializer.DeserializeAsync<Result<T>>(
                await httpClient.GetStreamAsync($"{BaseURL}/{apiUrl}/{method}/{string.Join("/", parameters)}"));
            if (!result.Success)
            {
                throw new FreeboxOSException(result.ErrorCode, result.Message);
            }
            return result.Object;
        }

        /// <summary>
        /// Releases all resources used by the current <see cref="FreeboxOSClient"/> object
        /// </summary>
        public void Dispose()
        {
            RootCertificates.Dispose();
        }
    }
}
