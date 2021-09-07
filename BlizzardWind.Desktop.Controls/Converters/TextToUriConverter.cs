using BlizzardWind.App.Common.MarkText;
using BlizzardWind.App.Common.Tools;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class TextToUriConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var pack = "pack://application:,,,/Assets/Images/Icons/";
            var defaultUrl = new Uri($"{pack}file.png");
            if (values.Length < 3)
                return new BitmapImage(defaultUrl);
            int type = (int)values[0];
            string path = (string)values[1];

            switch (type)
            {
                case MarkResourceType.Cover:
                case MarkResourceType.Image:
                    defaultUrl = new Uri($"{pack}image.png");
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

            try
            {
                Guid key = (Guid)values[2];
                if (type != MarkResourceType.Cover && type != MarkResourceType.Image)
                    return new BitmapImage(defaultUrl);
                else if (!File.Exists(path) || key == Guid.Empty)
                    return new BitmapImage(defaultUrl);
                else
                {
                    string keyStr = FileEncryptTool.GuidToKey(key);
                    byte[] buffer = FileEncryptTool.GetDecryptFileBytes(path, keyStr);
                    if (buffer == null || buffer.Length < 1)
                        return new BitmapImage(defaultUrl);

                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        return bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BitmapImage(defaultUrl);
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
