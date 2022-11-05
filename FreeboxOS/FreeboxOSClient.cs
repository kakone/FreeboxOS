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
        HttpMessageHandler = CreateHttpMessageHandler();
    }

    private IRootCertificates RootCertificates { get; }
    private string? BaseURL { get; set; }
    private HttpMessageHandler HttpMessageHandler { get; }

    /// <inheritdoc/>
    public string? URL { get; private set; }

    /// <inheritdoc/>
    public HttpMessageHandler CreateHttpMessageHandler()
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
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
            }
        };
    }

    private void SetBaseUrl(string apiDomain, int httpsPort, string apiBaseUrl)
    {
        URL = $"https://{apiDomain}:{httpsPort}";
        BaseURL = $"{URL}{apiBaseUrl}v{API_VERSION}";
    }

    private async Task<T> GetAsync<T>(string requestUri) where T : class
    {
        using var httpClient = new HttpClient(HttpMessageHandler, false);
        return (await httpClient.GetFromJsonAsync<T>(requestUri))!;
    }

    /// <inheritdoc/>
    public async Task<string> InitAsync()
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
            var properties = freeboxServer.Services.Values.First().Properties[0];
            SetBaseUrl(properties["api_domain"], Convert.ToInt32(properties["https_port"]), properties["api_base_url"]);
        }
        return URL!;
    }

    /// <inheritdoc/>
    public async Task<T> GetAsync<T>(string apiUrl, string method, params object[] parameters) where T : class
    {
        if (BaseURL == null)
        {
            throw new Exception($"You must call {nameof(IFreeboxOSClient)}.{nameof(InitAsync)} method first to find the Freebox");
        }
        var result = await GetAsync<Result<T>>($"{BaseURL}/{apiUrl}/{method}/{string.Join("/", parameters)}");
        if (!result.Success)
        {
            throw new FreeboxOSException(result.ErrorCode, result.Message);
        }
        return result.Object;
    }
}
