using FreeboxOS.TV;

namespace FreeboxOS;

/// <summary>
/// Interface for TV API
/// </summary>
public interface ITVApi
{
    /// <summary>
    /// Gets a value indicating whether the TV service is enabled or not
    /// </summary>
    /// <returns>true if the TV service is enabled, false otherwise</returns>
    Task<bool> IsEnabledAsync();

    /// <summary>
    /// Gets the channels
    /// </summary>
    /// <returns>the channels list</returns>
    Task<IEnumerable<Channel>> GetChannelsAsync();

    /// <summary>
    /// Gets the packages
    /// </summary>
    /// <returns>the packages list</returns>
    Task<IEnumerable<Package>> GetPackagesAsync();

    /// <summary>
    /// Gets the channels of a package 
    /// </summary>
    /// <param name="package">package</param>
    /// <returns>channels list</returns>
    Task<IEnumerable<NumberedChannel>> GetChannelsAsync(Package package);

    /// <summary>
    /// Gets the electronic program guide
    /// </summary>
    /// <param name="dateTimeOffset">hour</param>
    /// <returns>the electronic program guide for the given hour</returns>
    Task<Epg> GetEpgAsync(DateTimeOffset dateTimeOffset);
}
