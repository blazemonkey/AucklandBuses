using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using System;
using CsvHelper.Configuration;

namespace AucklandBuses.Models
{
    public class Route : ITransit
    {
        [PrimaryKey]
        [JsonProperty(PropertyName = "route_id")]
        public string RouteId { get; set; }
        [JsonProperty(PropertyName = "agency_id")]
        public string AgencyId { get; set; }
        [JsonProperty(PropertyName = "route_short_name")]
        public string RouteShortName { get; set; }
        [JsonProperty(PropertyName = "route_long_name")]
        public string RouteLongName { get; set; }
        [JsonProperty(PropertyName = "route_desc")]
        public string RouteDesc { get; set; }
        [JsonProperty(PropertyName = "route_type")]
        public int RouteType { get; set; }
        [JsonProperty(PropertyName = "route_url")]
        public string RouteUrl { get; set; }
        [JsonProperty(PropertyName = "route_color")]
        public string RouteColor { get; set; }
        [JsonProperty(PropertyName = "route_text_color")]
        public string RouteTextColor { get; set; }
    }

    public class RouteResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "response")]
        public List<Route> Response { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }

    public sealed class RouteMap : CsvClassMap<Route>
    {
        public RouteMap()
        {
            Map(m => m.RouteId).Index(0);
            Map(m => m.AgencyId).Index(1);
            Map(m => m.RouteShortName).Index(2);
            Map(m => m.RouteLongName).Index(3);
            Map(m => m.RouteType).Index(4);
            Map(m => m.RouteColor).Index(5);
            Map(m => m.RouteTextColor).Index(6);
        }
    }

}
