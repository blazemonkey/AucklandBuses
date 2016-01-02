using CsvHelper.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AucklandBuses.Models
{
    public class Agency : ITransit
    {
        [JsonProperty(PropertyName = "agency_id")]
        public string AgencyId { get; set; }
        [JsonProperty(PropertyName = "agency_name")]
        public string AgencyName { get; set; }
        [JsonProperty(PropertyName = "agency_url")]
        public string AgencyUrl { get; set; }
        [JsonProperty(PropertyName = "agency_timezone")]
        public string AgencyTimezone { get; set; }
        [JsonProperty(PropertyName = "agency_lang")]
        public string AgencyLang { get; set; }
        [JsonProperty(PropertyName = "agency_phone")]
        public string AgencyPhone { get; set; }
        [JsonProperty(PropertyName = "agency_fare_url")]
        public string AgencyFareUrl { get; set; }

        public static Agency CreateAllInstance()
        {
            return new Agency() { AgencyId = "ALL", AgencyName = "All" };
        }
    }

    public class AgencyResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "response")]
        public List<Agency> Response { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }

    public sealed class AgencyMap : CsvClassMap<Agency>
    {
        public AgencyMap()
        {
            Map(m => m.AgencyId).Index(0);
            Map(m => m.AgencyName).Index(1);
            Map(m => m.AgencyUrl).Index(2);
            Map(m => m.AgencyTimezone).Index(3);
            Map(m => m.AgencyPhone).Index(4);
            Map(m => m.AgencyLang).Index(5);
        }
    }    
}
