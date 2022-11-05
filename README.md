# Freebox OS TV API

## Usage

Add the Freebox OS services with the `AddFreeboxOSAPI` extension method :
```csharp
services.AddFreeboxOSAPI();
```
Call the `IFreeboxOSClient.InitAsync` method to search the Freebox :
```csharp
await services.GetRequiredService<IFreeboxOSClient>().InitAsync();
```
Then, use the `ITVApi` interface :
```csharp
var tv = services.GetService<ITVApi>();
var packages = await tv.GetPackagesAsync();
var numberedChannels = await tv.GetChannelsAsync(packages.First());
```

You can look at the [test sample](Test/Program.cs).

## Download
[![NuGet](https://img.shields.io/nuget/v/FreeboxOS.svg)](https://www.nuget.org/packages/FreeboxOS)
