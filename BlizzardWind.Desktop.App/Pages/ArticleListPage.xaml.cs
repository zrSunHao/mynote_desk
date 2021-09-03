﻿using BlizzardWind.Desktop.App.Dialogs;
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

namespace BlizzardWind.Desktop.App.Pages
{
    /// <summary>
    /// ArticleListPage.xaml 的交互逻辑
    /// </summary>
    public partial class ArticleListPage : Page
    {
        public ArticleListPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderClassifyDialog dialog = new FolderClassifyDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                var z = dialog.Message;
            }
        }
    }
}
