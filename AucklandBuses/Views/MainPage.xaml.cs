using AucklandBuses.Services.MessengerService;
using Microsoft.Practices.Unity;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AucklandBuses.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool _oneTime;

        [Dependency]
        public IMessengerService MessengerService { get; set; }

        public MainPage()
        {
            InitializeComponent();
            MessengerService = App.Container.Resolve<MessengerService>();
            MessengerService.Register<bool>(this, "ShowContentDialog", ShowContentDialog);
            NavigationCacheMode = NavigationCacheMode.Enabled;            
        }        

        private void RouteSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                MessengerService.Send(sender.Text, "FilterRoutes");
            }
        }

        private void StopSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                
            }
        }        

        private async void ShowContentDialog(bool show)
        {
            if (show)
                await ContentDialog.ShowAsync();
            else
                ContentDialog.Hide();
        }

        private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_oneTime)
                return;

            _oneTime = true;

            //var contentDialog = (ContentDialog)sender;
            //await contentDialog.ShowAsync();
        }
    }
}
