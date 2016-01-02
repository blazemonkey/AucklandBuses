using CsvHelper.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Models
{
    public class Calendar : ITransit
    {
        [JsonProperty(PropertyName = "service_id")]
        public string ServiceId { get; set; }
        [JsonProperty(PropertyName = "monday")]
        public int Monday { get; set; }
        [JsonProperty(PropertyName = "tuesday")]
        public int Tuesday { get; set; }
        [JsonProperty(PropertyName = "wednesday")]
        public int Wednesday { get; set; }
        [JsonProperty(PropertyName = "thursday")]
        public int Thursday { get; set; }
        [JsonProperty(PropertyName = "friday")]
        public int Friday { get; set; }
        [JsonProperty(PropertyName = "saturday")]
        public int Saturday { get; set; }
        [JsonProperty(PropertyName = "sunday")]
        public int Sunday { get; set; }
        [JsonProperty(PropertyName = "start_date")]
        public string StartDate { get; set; }
        [JsonProperty(PropertyName = "end_date")]
        public string EndDate { get; set; }
    }

    public class CalendarResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "response")]
        public List<Calendar> Response { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }

    public sealed class CalendarMap : CsvClassMap<Calendar>
    {
        public CalendarMap()
        {
            Map(m => m.ServiceId).Index(0);
            Map(m => m.Monday).Index(1);
            Map(m => m.Tuesday).Index(2);
            Map(m => m.Wednesday).Index(3);
            Map(m => m.Thursday).Index(4);
            Map(m => m.Friday).Index(5);
            Map(m => m.Saturday).Index(6);
            Map(m => m.Sunday).Index(7);
            Map(m => m.StartDate).Index(8);
            Map(m => m.EndDate).Index(9);
        }
    }
}
