using BlizzardWind.Desktop.Business.Models;
using BlizzardWind.Desktop.Business.ViewModels;
using Microsoft.Win32;
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
    /// EditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditorWindow : Window
    {
        private readonly EditorWindowViewModel VM;

        public EditorWindow()
        {
            InitializeComponent();
            VM = (EditorWindowViewModel)DataContext;
            VM.OnUploadFileClickAction += SelectFileButton_Click;
        }

        private void SelectFileButton_Click(string filter, int type,bool multiselect = true)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = multiselect;
            if (dialog.ShowDialog() != true)
                return;
            VM.OnAddFileClick(dialog.FileNames, type);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
                VM.OnWindowLoaded();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM != null)
                VM.OnFileFilter();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && VM != null)
                VM.OnFileFilter();
        }

        //private void Rectangle_Drop(object sender, DragEventArgs e)
        //{
        //    if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        //        return;
        //    string[]? fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
        //    if (fileNames == null || fileNames.Length < 1)
        //        return;
        //}
    }
}
