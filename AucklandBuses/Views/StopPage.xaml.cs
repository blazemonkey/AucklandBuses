using AucklandBuses.Models;
using AucklandBuses.Services.NavigationService;
using AucklandBuses.UserControls;
using AucklandBuses.ViewModels;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AucklandBuses.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StopPage : Page
    {
        private Stop _selectedStop;

        public StopPage()
        {
            this.InitializeComponent();
        }

        private void WindowsMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            var map = (MapControl)sender;
            SetMapControl(map);
        }

        private void PhoneMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            var map = (MapControl)sender;
            SetMapControl(map);
        }

        private void SetMapControl(MapControl map)
        { 
            var center = new BasicGeoposition();
            center.Latitude = _selectedStop.StopLat;
            center.Longitude = _selectedStop.StopLon;
            var centerPoint = new Geopoint(center);
            map.Center = centerPoint;

            var text = new TextBlock
            {
                FontWeight = FontWeights.Light,
                FontSize = 14,
                Text = _selectedStop.StopCode.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var grid = new Grid
            {
                Height = 20,
                Width = 40,
                Margin = new Thickness(10),
                Background = new SolidColorBrush(Colors.Black)                                
            };

            grid.Children.Add(text);
            MapControl.SetLocation(grid, centerPoint);
            MapControl.SetNormalizedAnchorPoint(grid, new Point(0.5, 0.5));
            map.Children.Add(grid);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var stop = (Stop)NavigationParameters.Instance.GetParameters();
            _selectedStop = stop;
            base.OnNavigatedTo(e);
        }
    }
}
