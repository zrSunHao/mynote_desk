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
    /// MarkFileInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MarkFileInfo : UserControl
    {
        public MarkFileInfo()
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
            DependencyProperty.Register("FileName", typeof(string), typeof(MarkFileInfo));

        public Guid ID
        {
            get { return (Guid)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(Guid), typeof(MarkFileInfo));

        public Uri ImageUrl
        {
            get => (Uri)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }
        // Using a DependencyProperty as the backing store for ImageUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(Uri), typeof(MarkFileInfo));

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        // Using a DependencyProperty as the backing store for IconFontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(MarkFileInfo));



        public ICommand UserOperate
        {
            get { return (ICommand)GetValue(UserOperateProperty); }
            set { SetValue(UserOperateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserCopy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserOperateProperty =
            DependencyProperty.Register("UserOperate", typeof(ICommand), typeof(MarkFileInfo));

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var ele = sender as MenuItem;
            if (ele != null && UserOperate != null)
            {
                UserOperate.Execute(new object[] { ele.Tag, ID });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ele = sender as Button;
            if(ele != null && UserOperate != null)
            {
                UserOperate.Execute(new object[] { ele.Tag, ID });
            }
        }
    }

    public class BindingProxy : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
