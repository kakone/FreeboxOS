using FreeboxOS;
using Microsoft.Extensions.DependencyInjection;

namespace Test.Android;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        var testButton = FindViewById<Button>(Resource.Id.testButton);
        testButton!.Click += TestButton_ClickAsync;
    }

    private async void TestButton_ClickAsync(object? sender, EventArgs e)
    {
        var services = new ServiceCollection().AddFreeboxOSAPI().BuildServiceProvider();
        var tvApi = services.GetService<ITVApi>()!;
        var channels = await tvApi.GetChannelsAsync();
        Toast.MakeText(ApplicationContext, $"{channels.Count()} channels", ToastLength.Long)!.Show();
    }
}
