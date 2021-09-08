using BlizzardWind.App.Common.Consts;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.ViewModels;
using Ookii.Dialogs.Wpf;
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

namespace BlizzardWind.Desktop.App.Windows
{
    /// <summary>
    /// LaunchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LaunchWindow : Window
    {
        private readonly LaunchWindowViewModel VM;

        public LaunchWindow()
        {
            InitializeComponent();

            VM = (LaunchWindowViewModel)DataContext;
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 从数据库中加载地址信息
            this.address_box.Text = await VM.GetBaseAddress();
        }

        private void SelectAddress_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() != true)
                return;
            this.address_box.Text = dialog.SelectedPath;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string address = this.address_box.Text;
            string password = this.password_box.Password.Trim();
            if(!System.IO.Directory.Exists(address))
            {
                this.PromptInformation(MesssageType.Error, "存储目录不存在");
                return;
            }
            if(string.IsNullOrEmpty(password) || password.Length < 4 || password.Length >12)
            {
                this.PromptInformation(MesssageType.Error, "密码校验未通过，请输入4 - 12位的密码");
                return;
            }
            try
            {
                await VM.LoginAsync(password, address);
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                this.PromptInformation(MesssageType.Error,ex.Message);
            }
            
        }
    }
}
