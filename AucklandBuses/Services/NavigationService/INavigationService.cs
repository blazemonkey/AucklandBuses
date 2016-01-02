namespace AucklandBuses.Services.NavigationService
{
    

    public interface INavigationService
    {
        bool Navigate(Experiences experience, object param = null);
        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
