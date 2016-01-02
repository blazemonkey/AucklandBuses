using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AucklandBuses.Converters
{
    public class DateFormatConverter : IValueConverter
    {
        // This converts the value object to the string to display.
        // This will work with most simple types.
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            if (value == null)
                return "--:--";

            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                var datetime = (DateTime)value;
                var nzTime = TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));
                return string.Format(formatString, nzTime);
            }

            // If the format string is null or empty, simply
            // call ToString() on the value.
            return value.ToString();
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
