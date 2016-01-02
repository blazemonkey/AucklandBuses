using AucklandBuses.Interfaces;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Navigation;
using Microsoft.Practices.Unity;
using AucklandBuses.Models;
using AucklandBuses.Services.RestService;
using AucklandBuses.Services.SqlLiteService;
using System.Collections.ObjectModel;
using AucklandBuses.Services.MessengerService;
using Prism.Commands;
using AucklandBuses.Services.NavigationService;
using AucklandBuses.Services.WebClientService;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace AucklandBuses.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        [Dependency]
        public ApiKeys ApiKeys { get; set; }

        [Dependency]
        public IRestService RestService { get; set; }

        [Dependency]
        public IMessengerService MessengerService { get; set; }

        [Dependency]
        public Prism.Windows.Navigation.INavigationService NavigationService { get; set; }

        [Dependency]
        public ISqlLiteService SqlLiteService { get; set; }

        [Dependency]
        public IWebClientService WebClientService { get; set; }

        private const double Total = 7;
        private string _loadingMessage;        
        private double _current;
        private string _progressText;
        private double _progressPercentage;

        private ObservableCollection<Agency> _agencies;
        private IEnumerable<Route> _routes;
        private ObservableCollection<Route> _filteredRoutes;
        private ObservableCollection<Stop> _stops;
        private Agency _selectedAgency;

        public string LoadingMessage
        {
            get { return _loadingMessage; }
            private set
            {
                _loadingMessage = value;
                OnPropertyChanged("LoadingMessage");
            }
        }

        public double Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                ProgressText = string.Format("{0}/{1}", _current, Total);
                ProgressPercentage = (_current / Total) * 100;

                OnPropertyChanged("Current");
            }
        }
        
        public string ProgressText
        {
            get { return _progressText; }
            private set
            {
                _progressText = value;
                OnPropertyChanged("ProgressText");
            }
        }

        public double ProgressPercentage
        {
            get { return _progressPercentage; }
            private set
            {
                _progressPercentage = value;
                OnPropertyChanged("ProgressPercentage");
            }
        }

        public ObservableCollection<Agency> Agencies
        {
            get { return _agencies; }
            private set
            {
                _agencies = value;
                OnPropertyChanged("Agencies");
            }
        }

        public IEnumerable<Route> Routes
        {
            get { return _routes; }
            private set
            {
                _routes = value;
                OnPropertyChanged("Routes");
            }
        }

        public ObservableCollection<Route> FilteredRoutes
        {
            get { return _filteredRoutes; }
            private set
            {
                _filteredRoutes = value;
                OnPropertyChanged("FilteredRoutes");
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

        public Agency SelectedAgency
        {
            get { return _selectedAgency; }
            set
            {   _selectedAgency = value;
                OnPropertyChanged("SelectedAgency");

                if (SelectedAgency.AgencyName == "All")
                {
                    //FilteredRoutes = new ObservableCollection<Route>(Routes.GroupBy(x => new { AgencyId = x.AgencyId,
                    //    ShortRouteId = x.RouteId.Substring(0, x.RouteId.IndexOf('-')) }).Select(x => x.First())
                    //    .OrderBy(x => x.AgencyId).ThenBy(x => x.RouteShortName));

                    FilteredRoutes = new ObservableCollection<Route>(Routes
                        .OrderBy(x => x.AgencyId).ThenBy(x => x.RouteShortName));
                }
                else
                {
                    //FilteredRoutes = new ObservableCollection<Route>(Routes.OrderBy(x => x.RouteShortName)
                    //    .GroupBy(x => x.RouteId.Substring(0, x.RouteId.IndexOf('-')))
                    //    .Select(x => x.First()).Where(x => x.AgencyId == SelectedAgency.AgencyId));

                    FilteredRoutes = new ObservableCollection<Route>(Routes.Where(x => x.AgencyId == SelectedAgency.AgencyId)
                        .OrderBy(x => x.AgencyId).ThenBy(x => x.RouteShortName));
                }
            }
        }

        public DelegateCommand<Route> TapRouteCommand { get; set; }
        public DelegateCommand<Stop> TapStopCommand { get; set; }

        public MainPageViewModel()
        {
            TapRouteCommand = new DelegateCommand<Route>(ExecuteTapRouteCommand);
            TapStopCommand = new DelegateCommand<Stop>(ExecuteTapStopCommand);

            LoadingMessage = "Updating Transport Information";
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (Routes != null && Routes.Any())
                return;

            MessengerService.Unregister<string>(this, "FilterRoutes", x => FilterRoutes(x));
            MessengerService.Register<string>(this, "FilterRoutes", x => FilterRoutes(x));           

            await RetrieveFromAT();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("api_key", ApiKeys.Keys.First(x => x.Id == "Auckland Transport").Value));
            //var getRoutes = await RestService.GetApi<RouteResponse>("http://api.at.govt.nz/v1/gtfs/", "routes", parameters);
            //var getAgencies = await RestService.GetApi<AgencyResponse>("http://api.at.govt.nz/v1/gtfs/", "agency", parameters);
            //var getStops = await RestService.GetApi<StopResponse>("http://api.at.govt.nz/v1/gtfs/", "stops", parameters);
            //var getTrips = await RestService.GetApi<TripResponse>("http://api.at.govt.nz/v1/gtfs/", "trips", parameters);
            //var getCalendars = await RestService.GetApi<CalendarResponse>("http://api.at.govt.nz/v1/gtfs/", "calendar", parameters);
            //await SqlLiteService.ClearRoutes();
            //await SqlLiteService.AddRoutes(getRoutes.Response);

            //await SqlLiteService.ClearAgencies();
            //await SqlLiteService.AddAgencies(getAgencies.Response);

            //await SqlLiteService.ClearStops();
            //await SqlLiteService.AddStops(getStops.Response);

            //await SqlLiteService.ClearTrips();
            //await SqlLiteService.AddTrips(getTrips.Response);

            //await SqlLiteService.ClearCalendars();
            //await SqlLiteService.AddCalendars(getCalendars.Response);

            Routes = await SqlLiteService.GetRoutes();
            var stops = await SqlLiteService.GetStops();
            Stops = new ObservableCollection<Stop>(stops.OrderBy(x => x.StopCode));

            var agencies = await SqlLiteService.GetAgencies();
            Agencies = new ObservableCollection<Agency>(agencies.OrderBy(x => x.AgencyName));
            Agencies.Insert(0, Agency.CreateAllInstance());
            SelectedAgency = Agencies.First();
            
            base.OnNavigatedTo(e, viewModelState);
        }

        private void FilterRoutes(string text)
        {

        }

        public void ExecuteTapRouteCommand(Route route)
        {
            NavigationParameters.Instance.SetParameters(route);
            NavigationService.Navigate(Experiences.Route.ToString(), null);
        }

        public void ExecuteTapStopCommand(Stop stop)
        {
            NavigationParameters.Instance.SetParameters(stop);
            NavigationService.Navigate(Experiences.Stop.ToString(), null);
        }

        private async Task RetrieveFromAT()
        {
            //await SqlLiteService.ClearAgencies();
            //var agencies = await WebClientService.DownloadFile<Agency, AgencyMap>("https://cdn01.at.govt.nz/data/agency.txt");
            //await SqlLiteService.AddAgencies(agencies);
            //Current = 1;
            
            //await SqlLiteService.ClearStops();
            //var stops = await WebClientService.DownloadFile<Stop, StopMap>("https://cdn01.at.govt.nz/data/stops.txt");
            //await SqlLiteService.AddStops(stops);
            //Current = 2;
            
            //await SqlLiteService.ClearRoutes();
            //var routes = await WebClientService.DownloadFile<Route, RouteMap>("https://cdn01.at.govt.nz/data/routes.txt");
            //await SqlLiteService.AddRoutes(routes);
            //Current = 3;

            //await SqlLiteService.ClearTrips();
            //var trips = await WebClientService.DownloadFile<Trip, TripMap>("https://cdn01.at.govt.nz/data/trips.txt");
            //await SqlLiteService.AddTrips(trips);
            //Current = 4;

            //await SqlLiteService.ClearCalendars();
            //var calendars = await WebClientService.DownloadFile<Calendar, CalendarMap>("https://cdn01.at.govt.nz/data/calendar.txt");
            //await SqlLiteService.AddCalendars(calendars);
            //Current = 5;

            //await SqlLiteService.ClearStopTimes();
            //var stopTimes = await WebClientService.DownloadFile<StopTime, StopTimeMap>("https://cdn01.at.govt.nz/data/stop_times.txt");
            //await SqlLiteService.AddStopTimes(stopTimes);
            //Current = 6;

            //await SqlLiteService.ClearCalendarDates();
            //var calendarDates = await WebClientService.DownloadFile<CalendarDate, CalendarDateMap>("https://cdn01.at.govt.nz/data/calendar_dates.txt");
            //await SqlLiteService.AddCalendarDates(calendarDates);
            //Current = 7;

            MessengerService.Send(false, "ShowContentDialog");
        }
    }
}
