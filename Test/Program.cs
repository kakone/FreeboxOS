#pragma warning disable IDE1006
using System;
using System.Linq;
using System.Threading.Tasks;
using FreeboxOS;
using Microsoft.Extensions.DependencyInjection;

namespace Test
{
    class Program
    {
        static async Task Main()
        {
            using var services = new ServiceCollection().AddFreeboxOSAPI().BuildServiceProvider();
            var tv = services.GetService<ITVApi>();
            Console.WriteLine($"TV enabled: {await tv.IsEnabledAsync()}");
            var channels = await tv.GetChannelsAsync();
            Console.WriteLine($"Channels number: {channels.Count()}");
            var packages = await tv.GetPackagesAsync();
            Console.WriteLine($"Packages: {string.Join(", ", packages.Select(p => p.Name))}");
            var package = packages.First();
            Console.WriteLine($"Channels number for {package.Name}: {(await tv.GetChannelsAsync(package)).Count()}");
            var now = DateTimeOffset.Now;
            var epg = (await tv.GetEpgAsync(now)).First();
            var program = epg.Value[now];
            Console.WriteLine($"Current program on {channels.First(c => c.Id == epg.Key).Name}: {program.Title} " +
                $"({program.StartDate.TimeOfDay:hh\\:mm} - {program.EndDate.TimeOfDay:hh\\:mm})");
        }
    }
}
#pragma warning restore IDE1006
