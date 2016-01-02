using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AucklandBuses.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.GetType() != typeof(bool))
                return Visibility.Collapsed;

            var boolean = System.Convert.ToBoolean(value.ToString());

            if (parameter != null)
                boolean = !boolean;

            return boolean == true ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
