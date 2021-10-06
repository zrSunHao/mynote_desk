using BlizzardWind.App.Common.MarkText;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class ImageStretchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Stretch.None;
            if (value as string == MarkOrnamentConsts.ImgUniform)
                return Stretch.Uniform;
            else 
                return Stretch.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
