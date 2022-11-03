using Microsoft.Extensions.DependencyInjection;

namespace FreeboxOS;

/// <summary>
/// To add the Freebox OS services to a <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceManager
{
    /// <summary>
    /// Adds the Freebox OS services to the specified <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">the <see cref="IServiceCollection"/> to add the services to</param>
    /// <returns>a reference to the <paramref name="services"/> instance</returns>
    public static IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<IRootCertificates, RootCertificates>();
        services.AddScoped<IFreeboxOSClient, FreeboxOSClient>();
        services.AddScoped<ITVApi, TVApi>();
        return services;
    }
}
