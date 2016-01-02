using AucklandBuses.Interfaces;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Navigation;
using AucklandBuses.Models;
using AucklandBuses.Services.SqlLiteService;
using Microsoft.Practices.Unity;
using AucklandBuses.Services.NavigationService;
using System.Collections.ObjectModel;
using Prism.Commands;
using Windows.UI.Xaml.Controls;
using AucklandBuses.Services.MessengerService;
using AucklandBuses.Helpers;

namespace AucklandBuses.ViewModels
{
    public class RoutePageViewModel : ViewModelBase, IRoutePageViewModel
    {
        private bool _isLoadingTrips;
        private bool _hasTrips;
        private string _tripMessage;

        private Route _selectedRoute;
        private Trip _selectedTrip;
        private ObservableCollection<Stop> _stops;
        private ObservableCollection<Trip> _trips;
        private IEnumerable<StopTime> _stopTimes;
        private ObservableCollection<StopTime> _filteredStopTimes;
        private ObservableCollection<Calendar> _calendars;
        private string _mapKey;

        [Dependency]
        public IMessengerService MessengerService { get; set; }

        [Dependency]
        public ISqlLiteService SqlLiteService { get; set; }

        [Dependency]
        public ApiKeys ApiKeys { get; set; }

        public bool IsLoadingTrips
        {
            get { return _isLoadingTrips; }
            private set
            {
                _isLoadingTrips = value;
                OnPropertyChanged("IsLoadingTrips");
            }
        }

        public bool HasTrips
        {
            get { return _hasTrips; }
            private set
            {
                _hasTrips = value;
                OnPropertyChanged("HasTrips");
            }
        }

        public string TripMessage
        {
            get { return _tripMessage; }
            private set
            {
                _tripMessage = value;
                OnPropertyChanged("TripMessage");
            }
        }

        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            private set
            {
                _selectedRoute = value;
                OnPropertyChanged("SelectedRoute");
            }
        }

        public Trip SelectedTrip
        {
            get { return _selectedTrip; }
            set
            {
                _selectedTrip = value;
                OnPropertyChanged("SelectedTrip");

                var stopTimes = StopTimes.Where(x => x.TripId == SelectedTrip.TripId);
                FilteredStopTimes = new ObservableCollection<StopTime>(stopTimes);
                MessengerService.Send(stopTimes.Select(x => x.Stop), "DrawMapStops");
            }
        }

        public ObservableCollection<Stop> Stops
        {
            get { return _stops; }
            private set
            {
                _stops = value;
                OnPropertyChanged("Stops");
            }
        }

        public ObservableCollection<Trip> Trips
        {
            get { return _trips; }
            private set
            {
                _trips = value;
                OnPropertyChanged("Trips");
            }
        }

        public ObservableCollection<Calendar> Calendars
        {
            get { return _calendars; }
            private set
            {
                _calendars = value;
                OnPropertyChanged("Calendars");
            }
        }

        public IEnumerable<StopTime> StopTimes
        {
            get { return _stopTimes; }
            private set
            {
                _stopTimes = value;
                OnPropertyChanged("StopTimes");
            }
        }

        public ObservableCollection<StopTime> FilteredStopTimes
        {
            get { return _filteredStopTimes; }
            private set
            {
                _filteredStopTimes = value;
                OnPropertyChanged("FilteredStopTimes");
            }
        }

        public string MapKey
        {
            get { return _mapKey; }
            private set
            {
                _mapKey = value;
                OnPropertyChanged("MapKey");
            }
        }

        public DelegateCommand<StopTime> TapStopTimeCommand { get; set; }
        public DelegateCommand TapRefreshCommand { get; set; }
        public DelegateCommand<CalendarViewSelectedDatesChangedEventArgs> SelectedDatesChanged { get; set; }
        public DelegateCommand<CalendarViewDayItemChangingEventArgs> CalendarViewDayItemChanging { get; set; }

        public RoutePageViewModel()
        {
            TapStopTimeCommand = new DelegateCommand<StopTime>(ExecuteTapStopTimeCommand);
            TapRefreshCommand = new DelegateCommand(ExecuteTapRefreshCommand);
            SelectedDatesChanged = new DelegateCommand<CalendarViewSelectedDatesChangedEventArgs>(ExecuteSelectedDatesChanged);
            CalendarViewDayItemChanging = new DelegateCommand<CalendarViewDayItemChangingEventArgs>(ExecuteCalendarViewDayItemChanging);
        }
        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {            
            MapKey = ApiKeys.Keys.First(x => x.Id == "Bing Maps").Value;

            var route = (Route)NavigationParameters.Instance.GetParameters();
            SelectedRoute = route;

            IsLoadingTrips = true;
            var trips = await GetTrips(SelectedRoute.RouteId);
            Trips = new ObservableCollection<Trip>(trips.OrderBy(x => x.TripStartEndTime));
            SelectedTrip = Trips.First();

            var calendars = await GetCalendars(trips, SelectedRoute.RouteId);
            Calendars = new ObservableCollection<Calendar>(calendars);            
           
            var calendarDates = await GetCalendarDates(trips, SelectedRoute.RouteId);

            MessengerService.Send(calendars, "SetCalendar");
            base.OnNavigatedTo(e, viewModelState);
        }

        private async Task<IEnumerable<Trip>> GetTrips(string routeId)
        {
            var trips = await SqlLiteService.GetTripsByRouteId(routeId);
            StopTimes = await SqlLiteService.GetStopTimesByTripIds(trips.Select(x => x.TripId));

            foreach (var trip in trips)
            {
                trip.TripStartEndTime = DateTimeHelper.ParseWithTwentyFourHourTime(StopTimes.First(x => x.TripId == trip.TripId).ArrivalTime).ToString("HH:mm tt") + " - "
                    + DateTimeHelper.ParseWithTwentyFourHourTime(StopTimes.Last(x => x.TripId == trip.TripId).ArrivalTime).ToString("HH:mm tt");
            }

            if (!trips.Any())
            {
                HasTrips = false;
                TripMessage = "No Trips found for this Route";
            }
            else
            {
                HasTrips = true;
            }

            IsLoadingTrips = false;
            return trips;
        } 

        private async Task<IEnumerable<Calendar>> GetCalendars(IEnumerable<Trip> trips, string routeId)
        {            
            var calendars = await SqlLiteService.GetCalendarByServiceIds(trips.Select(x => x.ServiceId));
            
            return calendars;
        }

        private async Task<IEnumerable<CalendarDate>> GetCalendarDates(IEnumerable<Trip> trips, string routeId)
        {
            var calendarDates = await SqlLiteService.GetCalendarDatesByServiceIds(trips.Select(x => x.ServiceId));

            return calendarDates;
        }

        public void ExecuteSelectedDatesChanged(CalendarViewSelectedDatesChangedEventArgs args)
        {
           
        }

        public void ExecuteCalendarViewDayItemChanging(CalendarViewDayItemChangingEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.Item.Date.ToString());

            if (Calendars == null)
                return;

            args.Item.IsBlackout = false;

            if ((Calendars.Any(x => DateTime.ParseExact(x.StartDate, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture).Date.CompareTo(args.Item.Date.Date) < 0)) ||
                    (Calendars.Any(x => DateTime.ParseExact(x.EndDate, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture).Date.CompareTo(args.Item.Date.Date) > 0)))
            {
                args.Item.IsBlackout = true;
            }
        }

        public void ExecuteTapStopTimeCommand(StopTime stopTime)
        {
            MessengerService.Send(stopTime.Stop, "CenterMapStop");
        }

        public void ExecuteTapRefreshCommand()
        {
            var stopTimes = StopTimes.Where(x => x.TripId == SelectedTrip.TripId);
            FilteredStopTimes = new ObservableCollection<StopTime>(stopTimes);
            MessengerService.Send(stopTimes.Select(x => x.Stop), "DrawMapStops");
        }
    }
}
