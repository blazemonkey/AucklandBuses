using CsvHelper.Configuration;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace AucklandBuses.Models
{
    public class Stop : ITransit
    {
        [PrimaryKey]
        [JsonProperty(PropertyName = "stop_id")]
        public string StopId { get; set; }
        [JsonProperty(PropertyName = "stop_name")]
        public string StopName { get; set; }
        [JsonProperty(PropertyName = "stop_desc")]
        public string StopDesc { get; set; }
        [JsonProperty(PropertyName = "stop_lat")]
        public double StopLat { get; set; }
        [JsonProperty(PropertyName = "stop_lon")]
        public double StopLon { get; set; }
        [JsonProperty(PropertyName = "zone_id")]
        public string ZoneId { get; set; }
        [JsonProperty(PropertyName = "stop_url")]
        public string StopUrl { get; set; }
        [JsonProperty(PropertyName = "stop_code")]
        public int StopCode { get; set; }
        [JsonProperty(PropertyName = "stop_street")]
        public string StopStreet { get; set; }
        [JsonProperty(PropertyName = "stop_city")]
        public string StopCity { get; set; }
        [JsonProperty(PropertyName = "stop_region")]
        public string StopRegion { get; set; }
        [JsonProperty(PropertyName = "stop_postcode")]
        public string StopPostcode { get; set; }
        [JsonProperty(PropertyName = "stop_country")]
        public string StopCountry { get; set; }
        [JsonProperty(PropertyName = "location_type")]
        public int LocationType { get; set; }
        [JsonProperty(PropertyName = "parent_station")]
        public string ParentStation { get; set; }
        [JsonProperty(PropertyName = "stop_timezone")]
        public string StopTimezone { get; set; }
        [JsonProperty(PropertyName = "wheelchair_boarding")]
        public string WheelchairBoarding { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }
        [JsonProperty(PropertyName = "the_geom")]
        public string TheGeom { get; set; }
    }

    public class StopResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "response")]
        public List<Stop> Response { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }

    public sealed class StopMap : CsvClassMap<Stop>
    {
        public StopMap()
        {
            Map(m => m.StopId).Index(0);
            Map(m => m.StopCode).Index(1);
            Map(m => m.StopName).Index(2);
            Map(m => m.StopDesc).Index(3);
            Map(m => m.StopLat).Index(4);
            Map(m => m.StopLon).Index(5);
            Map(m => m.LocationType).Index(6);
            Map(m => m.ParentStation).Index(7);
        }
    }   
}
