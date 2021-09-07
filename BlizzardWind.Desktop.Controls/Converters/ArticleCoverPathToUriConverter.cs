using BlizzardWind.App.Common.Tools;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class ArticleCoverPathToUriConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return GetDefaultCover();
            try
            {
                string path = (string)values[0];
                Guid key = (Guid)values[1];
                if (!File.Exists(path) || key == Guid.Empty)
                    return GetDefaultCover();

                string keyStr = FileEncryptTool.GuidToKey(key);
                byte[] buffer = FileEncryptTool.GetDecryptFileBytes(path, keyStr);
                if (buffer == null || buffer.Length < 1)
                    return GetDefaultCover();

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    var bitmap = new BitmapImage();
                    bitmap.DecodePixelHeight = 128;
                    bitmap.DecodePixelWidth = 128;
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return GetDefaultCover();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage GetDefaultCover()
        {
            var pack = "pack://application:,,,/Assets/Images/Icons/";
            var defaultUrl = new Uri($"{pack}note.png");
            return new BitmapImage(defaultUrl);
        }
    }
}
