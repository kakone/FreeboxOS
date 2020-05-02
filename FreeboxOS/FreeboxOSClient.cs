using System;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;
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
        public FreeboxOSClient(IHttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        private IHttpClient HttpClient { get; }
        private AsyncLock Mutex { get; } = new AsyncLock();
        private string? BaseURL { get; set; }

        private void SetBaseUrl(string apiDomain, int httpsPort, string apiBaseUrl)
        {
            BaseURL = $"https://{apiDomain}:{httpsPort}{apiBaseUrl}v{API_VERSION}";
        }

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
                    var version = await HttpClient.GetAsync<Version>("http://mafreebox.freebox.fr/api_version");
                    SetBaseUrl(version.ApiDomain, version.HttpsPort, version.ApiBaseUrl);
                }
                else
                {
                    var properties = freeboxServer.Services.First().Value.Properties.First();
                    SetBaseUrl(properties["api_domain"], Convert.ToInt32(properties["https_port"]), properties["api_base_url"]);
                }
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string apiUrl, string method, params object[] parameters) where T : class
        {
            await InitAsync();
            var result = await HttpClient.GetAsync<Result<T>>($"{BaseURL}/{apiUrl}/{method}/{string.Join("/", parameters)}");
            if (!result.Success)
            {
                throw new FreeboxOSException(result.ErrorCode, result.Message);
            }
            return result.Object;
        }
    }
}
