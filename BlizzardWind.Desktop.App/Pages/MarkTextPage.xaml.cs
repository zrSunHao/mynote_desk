using BlizzardWind.App.Common.Consts;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace BlizzardWind.Desktop.App.Pages
{
    /// <summary>
    /// MarkTextPage.xaml 的交互逻辑
    /// </summary>
    public partial class MarkTextPage : Page
    {
        private readonly MarkTextPageViewModel VM;

        public MarkTextPage()
        {
            InitializeComponent();
            VM = (MarkTextPageViewModel)DataContext;
            VM.LinkClickAction += LinkClick;
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            VM.OnPageLoaded();
        }

        private void LinkClick(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = link,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                PromptInformation(MesssageType.Error, $"链接打开失败：{ex.Message}");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dictionaries = this.Resources.MergedDictionaries;
            for (int i = 0; i < dictionaries.Count; i++)
            {
                foreach (var item in dictionaries[i].Keys)
                {
                    string? c = item.ToString();
                    if (c == "Text.Font.Primary")
                    {
                        dictionaries[i][item] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                        break;
                    }
                }
            }

            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {

                    printDialog.PrintVisual(article_grid, "My Print Job");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            for (int i = 0; i < dictionaries.Count; i++)
            {
                foreach (var item in dictionaries[i].Keys)
                {
                    string? c = item.ToString();
                    if (c == "Text.Font.Primary")
                    {
                        dictionaries[i][item] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0C0C0"));
                        break;
                    }
                }
            }
        }
    }
}
