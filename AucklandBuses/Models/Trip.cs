using CsvHelper.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AucklandBuses.Models
{
    public class Trip : ITransit
    {
        [JsonProperty(PropertyName = "route_id")]
        public string RouteId { get; set; }
        [JsonProperty(PropertyName = "service_id")]
        public string ServiceId { get; set; }
        [JsonProperty(PropertyName = "trip_id")]
        public string TripId { get; set; }
        [JsonProperty(PropertyName = "trip_headsign")]
        public string TripHeadsign { get; set; }
        [JsonProperty(PropertyName = "direction_id")]
        public int DirectionId { get; set; }
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }
        [JsonProperty(PropertyName = "shape_id")]
        public string ShapeId { get; set; }
        [JsonProperty(PropertyName = "trip_short_name")]
        public string TripShortName { get; set; }
        [JsonProperty(PropertyName = "trip_type")]
        public string TripType { get; set; }
        public string TripStartEndTime { get; set; }
    }

    public class TripResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "response")]
        public List<Trip> Response { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }

    public sealed class TripMap : CsvClassMap<Trip>
    {
        public TripMap()
        {
            Map(m => m.RouteId).Index(0);
            Map(m => m.ServiceId).Index(1);
            Map(m => m.TripId).Index(2);
            Map(m => m.TripHeadsign).Index(3);
            Map(m => m.DirectionId).Index(4);
            Map(m => m.BlockId).Index(5);
            Map(m => m.ShapeId).Index(6);
        }
    }   
}
