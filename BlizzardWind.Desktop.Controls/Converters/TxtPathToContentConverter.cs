using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class TxtPathToContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultContent = string.IsNullOrEmpty;
            if (value == null)
                return defaultContent;
            var path = value.ToString();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return defaultContent;
            var extension = Path.GetExtension(path);
            var list = new List<string>() { ".txt", ".cs" };
            if(!list.Any(x=>list.Contains(extension)))
                return defaultContent;
            try
            {
                return File.ReadAllText(path);
            }catch (Exception) { }
            return defaultContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
