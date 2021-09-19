using BlizzardWind.App.Common.Consts;
using BlizzardWind.App.Common.Tools;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.Entities;
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
using System.Windows.Shapes;

namespace BlizzardWind.Desktop.App.Windows
{
    /// <summary>
    /// ReaderWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReaderWindow : Window
    {
        private readonly ReaderWindowViewModel VM;
        private Note _Note;

        public ReaderWindow(Note article)
        {
            InitializeComponent();
            _Note = article;

            VM = (ReaderWindowViewModel)DataContext;
            VM.LinkClickAction += LinkClick;
        }

        public void SetNote(Note note)
        {
            _Note = note;
            VM.OnWindowLoaded(_Note);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VM.OnWindowLoaded(_Note);
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
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

        private void Pint_Click(object sender, RoutedEventArgs e)
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

        private void SplitPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF(*.pdf;)|*.pdf;";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() != true)
                return;
            var fileName = dialog.FileName;
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists)
                PromptInformation(MesssageType.Error, "文件不存在");
            var name = file.Name.Replace(file.Extension, "");
            var outPath = System.IO.Path.Combine(file.DirectoryName, $"{name}_split{file.Extension}");
            try
            {
                PdfTool.Paginate(file.FullName, outPath);
            }
            catch (Exception ex)
            {
                PromptInformation(MesssageType.Error, ex.Message);
            }
        }
    }
}
