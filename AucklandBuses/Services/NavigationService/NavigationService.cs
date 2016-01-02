namespace AucklandBuses.Services.NavigationService
{
    public class NavigationService : INavigationService
    {
        public Prism.Windows.Navigation.INavigationService _navigationService { get; private set; }

        public NavigationService(Prism.Windows.Navigation.INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool Navigate(Experiences experience, object param = null)
        {
            return _navigationService.Navigate(experience.ToString(), param);
        }

        public void ClearHistory()
        {
            _navigationService.ClearHistory();
        }

        public bool CanGoBack { get { return _navigationService.CanGoBack(); } }

        public void GoBack()
        {
            if (_navigationService.CanGoBack())
                _navigationService.GoBack();
        }
    }
}