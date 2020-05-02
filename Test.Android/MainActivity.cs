using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using FreeboxOS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Test.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var testButton = FindViewById<Button>(Resource.Id.testButton);
            testButton.Click += TestButton_ClickAsync;
        }

        private async void TestButton_ClickAsync(object sender, EventArgs e)
        {
            Toast.MakeText(ApplicationContext, $"{(await GetTVApi().GetChannelsAsync()).Count()} channels", ToastLength.Long).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private ITVApi GetTVApi()
        {
            var services = new ServiceCollection().AddFreeboxOSAPI();
            services.Replace(new ServiceDescriptor(typeof(IHttpClient), typeof(HttpClient), ServiceLifetime.Scoped));
            return services.BuildServiceProvider().GetService<ITVApi>();
        }
    }
}
