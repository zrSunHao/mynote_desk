using BlizzardWind.App.Common.MarkText;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class TextToUriConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var pack = "pack://application:,,,/Assets/Images/Icons/";
            var defaultUrl = new Uri($"{pack}file.png");
            if (values.Length!=2)
                return defaultUrl;
            int type = (int)values[0];
            string path = (string)values[1];

            switch (type)
            {
                case MarkResourceType.Cover:
                case MarkResourceType.Image:
                    if (string.IsNullOrEmpty(path) || !File.Exists(path))
                        defaultUrl = new Uri($"{pack}image.png");
                    else
                        defaultUrl = new Uri(path);
                    break;
                case MarkResourceType.OfficeFile:
                    defaultUrl = new Uri($"{pack}office.png");
                    break;
                case MarkResourceType.Txt:
                    defaultUrl = new Uri($"{pack}txt.png");
                    break;
                case MarkResourceType.PDF:
                    defaultUrl = new Uri($"{pack}pdf.png");
                    break;
                case MarkResourceType.Audio:
                    defaultUrl = new Uri($"{pack}audio.png");
                    break;
                case MarkResourceType.Video:
                    defaultUrl = new Uri($"{pack}video.png");
                    break;
            }
            return defaultUrl;

        }

        public object Convert(object[] values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    var defaultText = "";
        //    if (value == null)
        //        return defaultText;
        //    var uri = value as Uri;
        //    if (uri == null)
        //        return defaultText;
        //    return uri.ToString();
        //}

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
