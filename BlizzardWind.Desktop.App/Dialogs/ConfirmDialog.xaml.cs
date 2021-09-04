using BlizzardWind.App.Common.Consts;
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
using System.Windows.Shapes;

namespace BlizzardWind.Desktop.App.Dialogs
{
    /// <summary>
    /// ConfirmDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog(int type,string message)
        {
            InitializeComponent();

            var pack = "pack://application:,,,/Assets/Images/Icons/";
            var defaultUrl = new Uri($"{pack}info.png");
            switch (type)
            {
                case MesssageType.Info:
                    defaultUrl = new Uri($"{pack}success.png");
                    break;
                case MesssageType.Success:
                    defaultUrl = new Uri($"{pack}info.png");
                    break;
                case MesssageType.Warn:
                    defaultUrl = new Uri($"{pack}warn.png");
                    break;
                case MesssageType.Error:
                    defaultUrl = new Uri($"{pack}error.png");
                    break;
                default:
                    defaultUrl = new Uri($"{pack}info.png");
                    break;
            }
            this.icon_Img.Source = new BitmapImage(defaultUrl);
            this.msg_Box.Text = message;
        }

        private void Yes_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
