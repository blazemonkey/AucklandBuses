using CsvHelper.Configuration;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Models
{
    public class CalendarDate : ITransit
    {
        [PrimaryKey]
        [AutoIncrement]
        public int CalendarDateId { get; set; }
        public string ServiceId { get; set; }
        public string Date { get; set; }
        public int ExceptionType { get; set; }
    }

    public sealed class CalendarDateMap : CsvClassMap<CalendarDate>
    {
        public CalendarDateMap()
        {
            Map(m => m.ServiceId).Index(0);
            Map(m => m.Date).Index(1);
            Map(m => m.ExceptionType).Index(2);
        }
    }
}
