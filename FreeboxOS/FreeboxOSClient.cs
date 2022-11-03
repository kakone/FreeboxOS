using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using Zeroconf;

namespace FreeboxOS;

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

    private IRootCertificates RootCertificates { get; }
    private SemaphoreSlim Lock { get; } = new SemaphoreSlim(1, 1);
    private string? URL { get; set; }
    private string? BaseURL { get; set; }

    private void SetBaseUrl(string apiDomain, int httpsPort, string apiBaseUrl)
    {
        URL = $"https://{apiDomain}:{httpsPort}";
        BaseURL = $"{URL}{apiBaseUrl}v{API_VERSION}";
    }

    private async Task<T> GetAsync<T>(string requestUri) where T : class
    {
        using var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
        {
            if (chain == null || cert == null)
            {
                return true;
            }
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
            foreach (var rootCertificate in RootCertificates)
            {
                chain.ChainPolicy.CustomTrustStore.Add(rootCertificate);
            }
            return chain.Build(cert);
        };
        using var httpClient = new HttpClient(httpClientHandler);
        return (await httpClient.GetFromJsonAsync<T>(requestUri))!;
    }

    private async ValueTask InitAsync()
    {
        if (URL != null)
        {
            return;
        }

        await Lock.WaitAsync();
        try
        {
            IZeroconfHost? freeboxServer;
            try
            {
                freeboxServer = (await ZeroconfResolver.ResolveAsync(PROTOCOL_NAME))?.FirstOrDefault();
            }
            catch (NetworkInformationException)
            {
                freeboxServer = null;
            }
            if (freeboxServer == null)
            {
                var version = await GetAsync<Version>("https://mafreebox.freebox.fr/api_version");
                SetBaseUrl(version.ApiDomain, version.HttpsPort, version.ApiBaseUrl);
            }
            else
            {
                var properties = freeboxServer.Services.First().Value.Properties.First();
                SetBaseUrl(properties["api_domain"], Convert.ToInt32(properties["https_port"]), properties["api_base_url"]);
            }
        }
        finally
        {
            Lock.Release();
        }
    }

    /// <inheritdoc/>
    public async ValueTask<string> GetUrlAsync()
    {
        await InitAsync();
        return URL!;
    }

    /// <inheritdoc/>
    public async Task<T> GetAsync<T>(string apiUrl, string method, params object[] parameters) where T : class
    {
        await InitAsync();
        var result = await GetAsync<Result<T>>($"{BaseURL}/{apiUrl}/{method}/{string.Join("/", parameters)}");
        if (!result.Success)
        {
            throw new FreeboxOSException(result.ErrorCode, result.Message);
        }
        return result.Object;
    }
}
