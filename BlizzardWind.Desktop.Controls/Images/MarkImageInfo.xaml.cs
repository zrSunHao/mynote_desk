using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlizzardWind.Desktop.Controls.Images
{
    /// <summary>
    /// MarkImageInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MarkImageInfo : UserControl
    {
        public MarkImageInfo()
        {
            InitializeComponent();
        }

        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }
        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(MarkImageInfo));

        public Uri ImageUrl
        {
            get => (Uri)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }
        // Using a DependencyProperty as the backing store for ImageUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(Uri), typeof(MarkImageInfo));

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        // Using a DependencyProperty as the backing store for IconFontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(MarkImageInfo));
    }
}
