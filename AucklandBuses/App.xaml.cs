using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Prism.Windows;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using AucklandBuses.Services.JsonService;
using AucklandBuses.Services.RestService;
using AucklandBuses.Models;
using AucklandBuses.Services.FileReaderService;
using AucklandBuses.Services.SqlLiteService;
using AucklandBuses.Services.MessengerService;
using AucklandBuses.Services.WebClientService;

namespace AucklandBuses
{
    public enum Experiences { Main, Route, Stop }
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : PrismApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>        

        public static readonly UnityContainer Container = new UnityContainer();

        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance(NavigationService);
            Container.RegisterType<IFileReaderService, FileReaderService>();
            Container.RegisterType<IJsonService, JsonService>();
            Container.RegisterType<IMessengerService, MessengerService>();
            Container.RegisterType<IRestService, RestService>();
            Container.RegisterType<ISqlLiteService, SqlLiteService>();
            Container.RegisterType<IWebClientService, WebClientService>();

            var apiKeys = await Container.Resolve<IFileReaderService>().ReadFile("keys.txt", "Configurations");
            var keys = Container.Resolve<IJsonService>().Deserialize<ApiKeys>(apiKeys);
            Container.RegisterInstance(keys);

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("key", keys.Keys.First(x => x.Id == "Transit Feeds").Value));
            parameters.Add(new KeyValuePair<string, string>("feed", "auckland-transport/124"));
            //await Container.Resolve<IRestService>().GetApi<FeedVersion>("http://api.transitfeeds.com/v1/", "getFeedVersions", parameters);

            await Container.Resolve<ISqlLiteService>().InitDb();
        }

        protected override object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(Experiences.Main.ToString(), null);
            return Task.FromResult<object>(null);
        }
    }
}
