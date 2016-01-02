using AucklandBuses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Services.SqlLiteService
{
    public interface ISqlLiteService
    {
        Task InitDb();
        Task<IEnumerable<Route>> GetRoutes();
        Task ClearRoutes();
        Task AddRoutes(IEnumerable<Route> routes);
        Task<Route> GetRoute(string id);
        Task<IEnumerable<Route>> GetRoutesByTripIds(IEnumerable<string> tripIds);
        Task<Route> GetOldestRoute(string id);
        Task<IEnumerable<Agency>> GetAgencies();
        Task ClearAgencies();
        Task AddAgencies(IEnumerable<Agency> agencies);
        Task<IEnumerable<Stop>> GetStops();
        Task<Stop> GetStop(string id);
        Task<IEnumerable<Stop>> GetStopsByStopIds(IEnumerable<string> ids);
        Task ClearStops();
        Task AddStops(IEnumerable<Stop> stops);
        Task<IEnumerable<StopTime>> GetStopTimes();
        Task<IEnumerable<StopTime>> GetStopTimesByStopId(string stopId);        
        Task<IEnumerable<StopTime>> GetStopTimesByTripId(string tripId);
        Task<IEnumerable<StopTime>> GetStopTimesByTripIds(IEnumerable<string> tripIds);
        Task ClearStopTimes();
        Task AddStopTimes(IEnumerable<StopTime> stopTimes);
        Task<IEnumerable<Trip>> GetTrips();
        Task<IEnumerable<Trip>> GetTripsByRouteId(string routeId);
        Task ClearTrips();
        Task AddTrips(IEnumerable<Trip> trips);
        Task<IEnumerable<Calendar>> GetCalendars();
        Task<Calendar> GetCalendar(string id);
        Task<IEnumerable<Calendar>> GetCalendarByServiceIds(IEnumerable<string> serviceIds);
        Task ClearCalendars();
        Task AddCalendars(IEnumerable<Calendar> calendars);
        Task<IEnumerable<CalendarDate>> GetCalendarDates();
        Task<IEnumerable<CalendarDate>> GetCalendarDatesByServiceIds(IEnumerable<string> serviceIds);
        Task ClearCalendarDates();
        Task AddCalendarDates(IEnumerable<CalendarDate> calendarDates);
    }
}
