﻿using System;
using System.Linq;
using FreeboxOS;
using Microsoft.Extensions.DependencyInjection;

using var services = new ServiceCollection().AddFreeboxOSAPI().BuildServiceProvider();
var freeboxOSClient = services.GetRequiredService<IFreeboxOSClient>();
Console.WriteLine($"Freebox URL: {await freeboxOSClient.InitAsync()}");
var tv = services.GetRequiredService<ITVApi>();
Console.WriteLine($"TV enabled: {await tv.IsEnabledAsync()}");
var channels = await tv.GetChannelsAsync();
Console.WriteLine($"Channels number: {channels.Count()}");
var packages = await tv.GetPackagesAsync();
Console.WriteLine($"Packages: {string.Join(", ", packages.Select(p => p.Name))}");
var package = packages.First();
var numberedChannels = await tv.GetChannelsAsync(package);
Console.WriteLine($"Channels number for {package.Name}: {numberedChannels.Count()}");
var now = DateTimeOffset.Now;
var epg = (await tv.GetEpgAsync(now)).First();
var program = epg.Value[now];
if (program != null)
{
    Console.WriteLine($"Current program on {channels.First(c => c.Id == epg.Key).Name}: {program.Title} " +
        $"({program.StartDate.TimeOfDay:hh\\:mm} - {program.EndDate.TimeOfDay:hh\\:mm})");
}
