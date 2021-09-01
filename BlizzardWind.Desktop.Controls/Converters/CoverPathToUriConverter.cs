using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class CoverPathToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pack = "pack://application:,,,/Assets/Images/Icons/";
            var defaultUrl = new Uri($"{pack}note.png");
            if (value == null)
                return defaultUrl;
            var path = value.ToString();
            if(string.IsNullOrEmpty(path) || !File.Exists(path))
                return defaultUrl;
            return new Uri(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
