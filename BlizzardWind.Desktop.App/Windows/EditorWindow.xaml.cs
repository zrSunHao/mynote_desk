using BlizzardWind.App.Common.Consts;
using BlizzardWind.App.Common.Tools;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;
using BlizzardWind.Desktop.Business.ViewModels;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        public EditorWindow(Article article)
        {
            InitializeComponent();
        }

        



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        
    }
}
