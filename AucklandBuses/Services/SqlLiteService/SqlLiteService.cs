using AucklandBuses.Models;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AucklandBuses.Services.SqlLiteService
{
    public class SqlLiteService : ISqlLiteService
    {
        private readonly SQLiteAsyncConnection _conn;
    
        public SqlLiteService()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "aucklandbuses.sqlite");
            _conn = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(
                new SQLitePlatformWinRT(), 
                new SQLiteConnectionString(path, false)));
        }

        public async Task InitDb()
        {
            await _conn.CreateTableAsync<Agency>();
            await _conn.CreateTableAsync<Calendar>();
            await _conn.CreateTableAsync<CalendarDate>();
            await _conn.CreateTableAsync<Route>();
            await _conn.CreateTableAsync<Stop>();
            await _conn.CreateTableAsync<StopTime>();
            await _conn.CreateTableAsync<Trip>();
        }

        public async Task<IEnumerable<Route>> GetRoutes()
        {
            return await _conn.Table<Route>().ToListAsync();
        }

        public async Task<Route> GetRoute(string id)
        {
            return await _conn.GetAsync<Route>(id);
        }

        public async Task<IEnumerable<Route>> GetRoutesByTripIds(IEnumerable<string> tripIds)
        {
            var ids = string.Join("','", tripIds);
            var routes = await _conn.QueryAsync<Route>("SELECT DISTINCT R.* " +
                                                       "FROM Trip AS T " + 
                                                       "JOIN Route AS R ON " + 
                                                       "T.RouteId = R.RouteId " +
                                                       "WHERE TripId IN ('" + ids + "')");

            return routes;
        }

        public Task<Route> GetOldestRoute(string id)
        {
            var routeShortId = id.Substring(0, id.IndexOf('-'));
            //var routes = await _conn.QueryAsync<Route>("SELECT * FROM Route WHERE RouteId LIKE '" + routeShortId + "-%'");
            return null;
        }

        public async Task ClearRoutes()
        {
            await _conn.DeleteAllAsync<Route>();
        }

        public async Task AddRoutes(IEnumerable<Route> routes)
        {
            await _conn.InsertAllAsync(routes);
        }

        public async Task<IEnumerable<Agency>> GetAgencies()
        {
            return await _conn.Table<Agency>().ToListAsync();
        }

        public async Task ClearAgencies()
        {
            await _conn.DeleteAllAsync<Agency>();
        }

        public async Task AddAgencies(IEnumerable<Agency> agencies)
        {
            await _conn.InsertAllAsync(agencies);
        }

        public async Task<IEnumerable<Stop>> GetStops()
        {
            return await _conn.Table<Stop>().ToListAsync();
        }

        public async Task<Stop> GetStop(string id)
        {
            return await _conn.GetAsync<Stop>(id);
        }

        public async Task<IEnumerable<Stop>> GetStopsByStopIds(IEnumerable<string> ids)
        {
            var stopIds = string.Join("','", ids);
            var stops = await _conn.QueryAsync<Stop>("SELECT * FROM Stop " +
                                                      "WHERE StopId IN ('" + stopIds + "')");

            return stops;
        }

        public async Task ClearStops()
        {
            await _conn.DeleteAllAsync<Stop>();
        }

        public async Task AddStops(IEnumerable<Stop> stops)
        {
            await _conn.InsertAllAsync(stops);
        }

        public async Task<IEnumerable<StopTime>> GetStopTimes()
        {
            return await _conn.Table<StopTime>().ToListAsync();
        }

        public async Task<IEnumerable<StopTime>> GetStopTimesByStopId(string stopId)
        {
            return await _conn.Table<StopTime>().Where(x => x.StopId == stopId).ToListAsync();
        }

        public async Task<IEnumerable<StopTime>> GetStopTimesByTripId(string tripId)
        {
            return await _conn.Table<StopTime>().Where(x => x.TripId == tripId).ToListAsync();
        }

        public async Task<IEnumerable<StopTime>> GetStopTimesByTripIds(IEnumerable<string> tripIds)
        {
            var ids = string.Join("','", tripIds);
            var stopTimes = await _conn.QueryAsync<StopTime>("SELECT * " +
                                                       "FROM StopTime " +
                                                       "WHERE TripId IN ('" + ids + "')");
            var stops = await GetStopsByStopIds(stopTimes.Select(x => x.StopId));
            foreach (var stopTime in stopTimes)
            {
                stopTime.Stop = stops.First(x => x.StopId == stopTime.StopId);
            }

            return stopTimes;
        }

        public async Task ClearStopTimes()
        {
            await _conn.DeleteAllAsync<StopTime>();
        }

        public async Task AddStopTimes(IEnumerable<StopTime> stopTimes)
        {
            await _conn.InsertAllAsync(stopTimes);
        }

        public async Task<IEnumerable<Trip>> GetTrips()
        {
            return await _conn.Table<Trip>().ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsByRouteId(string routeId)
        {
            return await _conn.Table<Trip>().Where(x => x.RouteId == routeId).ToListAsync();
        }

        public async Task ClearTrips()
        {
            await _conn.DeleteAllAsync<Trip>();
        }

        public async Task AddTrips(IEnumerable<Trip> trips)
        {
            await _conn.InsertAllAsync(trips);
        }

        public async Task<IEnumerable<Calendar>> GetCalendars()
        {
            return await _conn.Table<Calendar>().ToListAsync();
        }

        public async Task<Calendar> GetCalendar(string id)
        {
            return await _conn.GetAsync<Calendar>(id);
        }

        public async Task<IEnumerable<Calendar>> GetCalendarByServiceIds(IEnumerable<string> serviceIds)
        {
            var calendars = new List<Calendar>();
            foreach (var id in serviceIds)
            {
                var calendar = await _conn.Table<Calendar>().Where(x => x.ServiceId == id).FirstOrDefaultAsync();
                calendars.Add(calendar);
            }

            return calendars;
        }

        public async Task ClearCalendars()
        {
            await _conn.DeleteAllAsync<Calendar>();
        }

        public async Task AddCalendars(IEnumerable<Calendar> calendars)
        {
            await _conn.InsertAllAsync(calendars);
        }

        public async Task<IEnumerable<CalendarDate>> GetCalendarDates()
        {
            return await _conn.Table<CalendarDate>().ToListAsync();
        }

        public async Task<IEnumerable<CalendarDate>> GetCalendarDatesByServiceIds(IEnumerable<string> serviceIds)
        {
            var calendarDates = new List<CalendarDate>();
            foreach (var id in serviceIds)
            {
                var calendarDate = await _conn.Table<CalendarDate>().Where(x => x.ServiceId == id).FirstOrDefaultAsync();
                calendarDates.Add(calendarDate);
            }

            return calendarDates;
        }

        public async Task ClearCalendarDates()
        {
            await _conn.DeleteAllAsync<CalendarDate>();
        }

        public async Task AddCalendarDates(IEnumerable<CalendarDate> calendarDates)
        {
            await _conn.InsertAllAsync(calendarDates);
        }
    }
}
