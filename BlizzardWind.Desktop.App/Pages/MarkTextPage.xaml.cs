﻿using BlizzardWind.Desktop.Business.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        }

        
    }
}