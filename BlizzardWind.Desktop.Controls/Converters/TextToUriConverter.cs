using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class TextToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultUrl = new Uri("pack://application:,,,/Assets/Images/User.jpeg"); 
            if (value == null)
                return defaultUrl;
            var path = value.ToString();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return defaultUrl;
            return new Uri(path);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
