using FreeboxOS.TV;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeboxOS
{
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

        /// <summary>
        /// Gets a value indicating whether the TV service is enabled or not
        /// </summary>
        /// <returns>true if the TV service is enabled, false otherwise</returns>
        public async Task<bool> IsEnabledAsync()
        {
            return (await GetAsync<Status>("status")).Enabled;
        }

        /// <summary>
        /// Gets the channels
        /// </summary>
        /// <returns>the channels list</returns>
        public async Task<IEnumerable<Channel>> GetChannelsAsync()
        {
            return (await GetAsync<IDictionary<string, Channel>>("channels")).Select(c => c.Value);
        }

        /// <summary>
        /// Gets the packages
        /// </summary>
        /// <returns>the packages list</returns>
        public Task<IEnumerable<Package>> GetPackagesAsync()
        {
            return GetAsync<IEnumerable<Package>>("bouquets");
        }

        /// <summary>
        /// Gets the channels of a package 
        /// </summary>
        /// <param name="package">package</param>
        /// <returns>channels list</returns>
        public Task<IEnumerable<NumberedChannel>> GetChannelsAsync(Package package)
        {
            return GetAsync<IEnumerable<NumberedChannel>>("bouquets", package.Id, "channels");
        }

        /// <summary>
        /// Gets the electronic program guide
        /// </summary>
        /// <param name="dateTimeOffset">hour</param>
        /// <returns>the electronic program guide for the given hour</returns>
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
}
