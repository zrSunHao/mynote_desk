using System;
using System.Globalization;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class FamilyPageWidthToColumsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)value;
            double count = width / 160;
            if (count < 1)
                return 1;
            else
                return System.Convert.ToInt32(count); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
