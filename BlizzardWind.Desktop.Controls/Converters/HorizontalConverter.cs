using BlizzardWind.App.Common.MarkText;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class HorizontalConverter : IValueConverter
    {
        private const HorizontalAlignment Left = HorizontalAlignment.Left;
        private const HorizontalAlignment Center = HorizontalAlignment.Center;
        private const HorizontalAlignment Right = HorizontalAlignment.Right;
        private const HorizontalAlignment Stretch = HorizontalAlignment.Stretch;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Left;
            string ornaments = string.Empty;
            ornaments = value as string;
            if (string.IsNullOrWhiteSpace(ornaments))
                return Left;

            if (ornaments.Contains(MarkOrnamentConsts.HorizontalCenter))
                return Center;
            else if (ornaments.Contains(MarkOrnamentConsts.HorizontalRight))
                return Right;
            else if (ornaments.Contains(MarkOrnamentConsts.HorizontalStretch))
                return Stretch;
            else
                return Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
