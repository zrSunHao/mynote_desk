using BlizzardWind.App.Common.MarkText;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public  class ImageWidthConverter : IValueConverter
    {
        private const int SmallWidth = 220;
        private const int MedieWidth = 330;
        private const int LargeWidth = 440;
        private const int ExtraWidth = 660;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return ExtraWidth;
            string ornaments = string.Empty;
            ornaments = value as string;
            if (string.IsNullOrWhiteSpace(ornaments))
                return ExtraWidth;

            if (ornaments.Contains(MarkOrnamentConsts.SmallWidth))
                return SmallWidth;
            else if (ornaments.Contains(MarkOrnamentConsts.MediumWidth))
                return MedieWidth;
            else if (ornaments.Contains(MarkOrnamentConsts.LargeWidth))
                return LargeWidth;
            else
                return ExtraWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
