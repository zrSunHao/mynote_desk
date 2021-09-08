using BlizzardWind.App.Common.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BlizzardWind.Desktop.Controls.Converters
{
    public class TxtPathToContentConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultContent = string.Empty;
            if (values == null || values.Length < 2)
                return defaultContent;
            try
            {
                var path = values[0].ToString();
                var key = (Guid)values[1];
                if (string.IsNullOrEmpty(path) || !File.Exists(path) || key == Guid.Empty)
                    return defaultContent;

                var extension = Path.GetExtension(path);
                var list = new List<string>() { ".txt", ".cs" };
                if (!list.Any(x => list.Contains(extension)))
                    return defaultContent;

                string keyStr = FileEncryptTool.GuidToKey(key);
                byte[] buffer = FileEncryptTool.GetDecryptFileBytes(path, keyStr);
                if (buffer == null || buffer.Length < 1)
                    return defaultContent;
                return Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return defaultContent;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
