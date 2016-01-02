using AucklandBuses.Models;
using AucklandBuses.Services.MessengerService;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AucklandBuses.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoutePage : Page
    {
        private CalendarView _calendarView;
        private MapControl _mapControl;

        [Dependency]
        public IMessengerService MessengerService { get; set; }

        public RoutePage()
        {
            this.InitializeComponent();
            MessengerService = App.Container.Resolve<MessengerService>();
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
            _mapControl = map;
            MessengerService.Register<IEnumerable<Stop>>(this, "DrawMapStops", DrawMapStops);
            MessengerService.Register<Stop>(this, "CenterMapStop", CenterMapStop);
        }

        private void CalendarView_Loaded(object sender, RoutedEventArgs e)
        {
            _calendarView = (CalendarView)sender;
            MessengerService.Register<IEnumerable<Calendar>>(this, "SetCalendar", SetCalendar);
        }

        private void SetCalendar(IEnumerable<Calendar> calendars)
        {
            var cal = calendars.ToList();

            if (!cal.Any())
                return;

            _calendarView.MinDate = DateTime.ParseExact(cal.First().StartDate, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture);
            _calendarView.MaxDate = DateTime.ParseExact(cal.First().EndDate, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture);
        }

        private void DrawMapStops(IEnumerable<Stop> stops)
        {
            var geopoints = new List<Geopoint>();
            _mapControl.Children.Clear();

            var stopsToDraw = stops.ToList();
            foreach (var stop in stopsToDraw)
            {
                var centerPoint = DrawPointOnMap(stop);
                geopoints.Add(centerPoint);
            }

            SetCenterOfPoints(geopoints);
        }

        private void CenterMapStop(Stop stop)
        {
            var geopoints = new List<Geopoint>();

            var basicGeoposition = new BasicGeoposition();
            basicGeoposition.Latitude = stop.StopLat;
            basicGeoposition.Longitude = stop.StopLon;
            var geopoint = new Geopoint(basicGeoposition);

            geopoints.Add(geopoint);
            SetCenterOfPoints(geopoints);
        }

        private Geopoint DrawPointOnMap(Stop stop)
        {
            var center = new BasicGeoposition();
            center.Latitude = stop.StopLat;
            center.Longitude = stop.StopLon;
            var centerPoint = new Geopoint(center);
            
            var text = new TextBlock
            {
                FontWeight = FontWeights.Light,
                FontSize = 14,
                Text = stop.StopCode.ToString(),
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
            _mapControl.Children.Add(grid);

            return centerPoint;
        }

        private void SetCenterOfPoints(IEnumerable<Geopoint> positions)
        {
            var mapWidth = _mapControl.ActualWidth;
            var mapHeight = _mapControl.ActualHeight;
            if (mapWidth == 0 || mapHeight == 0)
                return;

            if (positions.Count() == 0)
                return;

            if (positions.Count() == 1)
            {
                var singleGeoposition = new BasicGeoposition();
                singleGeoposition.Latitude = positions.First().Position.Latitude;
                singleGeoposition.Longitude = positions.First().Position.Longitude;
                _mapControl.Center = new Geopoint(singleGeoposition);
                _mapControl.ZoomLevel = 16;
                return;
            }

            var maxLatitude = positions.Max(x => x.Position.Latitude);
            var minLatitude = positions.Min(x => x.Position.Latitude);

            var maxLongitude = positions.Max(x => x.Position.Longitude);
            var minLongitude = positions.Min(x => x.Position.Longitude);

            var centerLatitude = ((maxLatitude - minLatitude) / 2) + minLatitude;
            var centerLongitude = ((maxLongitude - minLongitude) / 2) + minLongitude;

            var nw = new BasicGeoposition()
            {
                Latitude = maxLatitude,
                Longitude = minLongitude
            };

            var se = new BasicGeoposition()
            {
                Latitude = minLatitude,
                Longitude = maxLongitude
            };

            var buffer = 1;
            //best zoom level based on map width
            var zoomWidth = Math.Log(360.0 / 256.0 * (mapWidth - 2 * buffer) / (maxLongitude - minLongitude)) / Math.Log(2);
            //best zoom level based on map height
            var zoomHeight = Math.Log(180.0 / 256.0 * (mapHeight - 2 * buffer) / (maxLatitude - minLatitude)) / Math.Log(2);
            var zoom = (zoomWidth + zoomHeight) / 2;
            _mapControl.ZoomLevel = zoom - 0.8;

            //var box = new GeoboundingBox(nw, se);
            var geoposition = new BasicGeoposition();
            geoposition.Latitude = ((maxLatitude - minLatitude) / 2) + minLatitude;
            geoposition.Longitude = ((maxLongitude - minLongitude) / 2) + minLongitude;
            _mapControl.Center = new Geopoint(geoposition);
        }
    }
}
