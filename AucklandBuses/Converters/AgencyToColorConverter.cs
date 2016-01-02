using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace AucklandBuses.Converters
{
    public class AgencyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var defaultColor = new SolidColorBrush(Colors.Black);

            if (value == null || value.GetType() != typeof(string))
                return defaultColor;

            var agencyId = value.ToString();

            switch (agencyId)
            {
                case "NZBGW":
                    return new SolidColorBrush(Color.FromArgb(255, 10, 133, 78));
                case "HE":
                    return new SolidColorBrush(Color.FromArgb(255, 74, 30, 45));
                case "RTH":
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                case "FGL":
                    return new SolidColorBrush(Color.FromArgb(255, 255, 92, 15));
                case "BAYES":
                    return new SolidColorBrush(Color.FromArgb(255, 0, 120, 159));
                case "NZBML":
                    return new SolidColorBrush(Color.FromArgb(255, 50, 63, 141));
                case "NZBWP":
                    return new SolidColorBrush(Color.FromArgb(255, 176, 175, 171));
                case "UE":
                    return new SolidColorBrush(Color.FromArgb(255, 138, 129, 186));
                case "BTL":
                    return new SolidColorBrush(Color.FromArgb(255, 221, 200, 143));
                case "NZBNS":
                    return new SolidColorBrush(Color.FromArgb(255, 99, 179, 252));
                default:
                    return defaultColor;
            }            
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
