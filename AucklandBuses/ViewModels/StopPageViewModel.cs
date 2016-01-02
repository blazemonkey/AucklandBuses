using AucklandBuses.Interfaces;
using AucklandBuses.Models;
using AucklandBuses.Services.NavigationService;
using AucklandBuses.Services.RestService;
using AucklandBuses.Services.SqlLiteService;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AucklandBuses.ViewModels
{
    public class StopPageViewModel : ViewModelBase, IStopPageViewModel
    {
        private bool _isLoadingRoutes;
        private bool _hasRoutes;
        private string _routeMessage;
        private bool _isLoadingMovements;
        private bool _hasMovements;
        private string _movementMessage;
        private DateTime _refreshTime;
        private Stop _selectedStop;
        private ObservableCollection<Route> _routes;
        private ObservableCollection<Movement> _movements;
        private string _mapKey;

        [Dependency]
        public IRestService RestService { get; set; }

        [Dependency]
        public Prism.Windows.Navigation.INavigationService NavigationService { get; set; }

        [Dependency]
        public ISqlLiteService SqlLiteService { get; set; }

        [Dependency]
        public ApiKeys ApiKeys { get; set; }

        public bool IsLoadingRoutes
        {
            get { return _isLoadingRoutes; }
            private set
            {
                _isLoadingRoutes = value;
                OnPropertyChanged("IsLoadingRoutes");
            }
        }

        public bool HasRoutes
        {
            get { return _hasRoutes; }
            private set
            {
                _hasRoutes = value;
                OnPropertyChanged("HasRoutes");
            }
        }

        public string RouteMessage
        {
            get { return _routeMessage; }
            private set
            {
                _routeMessage = value;
                OnPropertyChanged("RouteMessage");
            }
        }

        public bool IsLoadingMovements
        {
            get { return _isLoadingMovements; }
            private set
            {
                _isLoadingMovements = value;
                OnPropertyChanged("IsLoadingMovements");
            }
        }

        public bool HasMovements
        {
            get { return _hasMovements; }
            private  set
            {
                _hasMovements = value;
                OnPropertyChanged("HasMovements");
            }
        }

        public string MovementMessage
        {
            get { return _movementMessage; }
            private set
            {
                _movementMessage = value;
                OnPropertyChanged("MovementMessage");
            }
        }

        public DateTime RefreshTime
        {
            get { return _refreshTime; }
            private set
            {
                _refreshTime = value;
                OnPropertyChanged("RefreshTime");
            }
        }

        public Stop SelectedStop
        {
            get { return _selectedStop; }
            private set
            {
                _selectedStop = value;
                OnPropertyChanged("SelectedStop");
            }
        }

        public ObservableCollection<Route> Routes
        {
            get { return _routes; }
            private set
            {
                _routes = value;
                OnPropertyChanged("Routes");
            }
        }

        public ObservableCollection<Movement> Movements
        {
            get { return _movements; }
            private set
            {
                _movements = value;
                OnPropertyChanged("Movements");
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

        public DelegateCommand<Route> TapRouteCommand { get; set; }
        public DelegateCommand TapRefreshCommand { get; set; }

        public StopPageViewModel()
        {
            TapRouteCommand = new DelegateCommand<Route>(ExecuteTapRouteCommand);
            TapRefreshCommand = new DelegateCommand(ExecuteTapRefreshCommand);
            HasRoutes = true;
            HasMovements = true;
        }

        public async void ExecuteTapRefreshCommand()
        {
            var datetime = DateTime.UtcNow;
            RefreshTime = TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));

            var movements = await GetLiveTimes(SelectedStop.StopCode);
            if (movements != null)
                Movements = new ObservableCollection<Movement>(movements.OrderBy(x => x.ActualArrivalTime));
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            MapKey = ApiKeys.Keys.First(x => x.Id == "Bing Maps").Value;
            IsLoadingRoutes = true;
            IsLoadingMovements = true;

            var stop = (Stop)NavigationParameters.Instance.GetParameters();
            SelectedStop = stop;

            var datetime = DateTime.UtcNow;
            RefreshTime = TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));

            var movements = await GetLiveTimes(SelectedStop.StopCode);
            if (movements != null)
                Movements = new ObservableCollection<Movement>(movements.OrderBy(x => x.ActualArrivalTime));

            await GetRoutes(SelectedStop.StopId);

            base.OnNavigatedTo(e, viewModelState);
        }

        private async Task GetRoutes(string stopId)
        {
            try
            {
                var stopTimes = await SqlLiteService.GetStopTimesByStopId(stopId);
                var tripIds = stopTimes.Select(x => x.TripId).Distinct().ToList();
                var routes = await SqlLiteService.GetRoutesByTripIds(tripIds);

                Routes = new ObservableCollection<Route>(routes);

                if (!Routes.Any())
                    RouteMessage = "There are no Routes that pass through this Stop";
            }
            catch (Exception)
            {
                RouteMessage = "Error loading Routes for Stop";
                HasRoutes = false;
            }
            finally
            {
                IsLoadingRoutes = false;
            }
        }

        private async Task<IEnumerable<Movement>> GetLiveTimes(int stopCode)
        {
            try
            {               
                var response = await RestService.GetApi<MovementResponse>("http://api.maxx.co.nz/RealTime/v2/Departures/Stop/", stopCode.ToString());
                if (response.Error != null)
                {
                    var code = response.Error.Code;

                    switch (code)
                    {
                        case "SM101":
                            {
                                MovementMessage = "No data available for this Stop";
                                HasMovements = false;
                            }
                            break;
                    }
                }

                if (response.Movements == null || !response.Movements.Any())
                {
                    MovementMessage = "No data available for this Stop";
                    HasMovements = false;
                }
                
                return response.Movements;
            }
            catch (Exception)
            {                
                MovementMessage = "Cannot retrieve Live Data from Auckland Transport servers at this time";
                HasMovements = false;
                return null;
            }
            finally
            {
                IsLoadingMovements = false;
            }
        }

        public void ExecuteTapRouteCommand(Route route)
        {
            NavigationParameters.Instance.SetParameters(route);
            NavigationService.Navigate(Experiences.Route.ToString(), null);
        }
    }
}
