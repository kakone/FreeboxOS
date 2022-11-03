using FreeboxOS.TV;
using Microsoft.Extensions.Caching.Memory;

namespace FreeboxOS;

/// <summary>
/// TV API
/// </summary>
public class TVApi : Api, ITVApi
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TVApi"/> class
    /// </summary>
    /// <param name="freeboxOSClient">Freebox OS client</param>
    public TVApi(IFreeboxOSClient freeboxOSClient) : base(freeboxOSClient)
    {
    }

    private MemoryCache EpgCache { get; } = new MemoryCache(new MemoryCacheOptions());

    /// <inheritdoc/>
    public async Task<bool> IsEnabledAsync()
    {
        return (await GetAsync<Status>("status")).Enabled;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Channel>> GetChannelsAsync()
    {
        return (await GetAsync<IDictionary<string, Channel>>("channels")).Select(c => c.Value);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Package>> GetPackagesAsync()
    {
        return GetAsync<IEnumerable<Package>>("bouquets");
    }

    /// <inheritdoc/>
    public Task<IEnumerable<NumberedChannel>> GetChannelsAsync(Package package)
    {
        return GetAsync<IEnumerable<NumberedChannel>>("bouquets", package.Id, "channels");
    }

    /// <inheritdoc/>
    public async Task<Epg> GetEpgAsync(DateTimeOffset dateTimeOffset)
    {
        dateTimeOffset = new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, 0, 0, dateTimeOffset.Offset);
        if (EpgCache.TryGetValue(dateTimeOffset, out Epg epg))
        {
            return epg;
        }

        epg = await GetAsync<Epg>("epg/by_time", dateTimeOffset.ToUnixTimeSeconds());
        EpgCache.Set(dateTimeOffset, epg, dateTimeOffset.AddMinutes(65));
        return epg;
    }
}
