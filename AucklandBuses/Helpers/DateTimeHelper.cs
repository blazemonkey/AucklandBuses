using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime ParseWithTwentyFourHourTime(string dateTime)
        {
            var hour = dateTime.Substring(0, 2);
            
            if (int.Parse(hour) >= 24)
                dateTime = dateTime.Replace(hour + ":", "00:");

            return DateTime.Parse(dateTime);
        }
    }
}
