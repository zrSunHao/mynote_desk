using BlizzardWind.Desktop.Controls.RouteEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlizzardWind.Desktop.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.frmMain.AddHandler(MyPageRoute.MyPageRouteChangedEvent, new RoutedEventHandler(MyPageRouteChangedHandler));
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MyPageRouteChangedHandler(object sender, RoutedEventArgs e)
        {
            var myRoute = e.OriginalSource as MyPageRoute;
            if (myRoute != null && !string.IsNullOrEmpty(myRoute.Route))
                this.frmMain.Source = new Uri($"pack://application:,,,/Pages/{myRoute.Route}.xaml");
            //需调用ViewModel绑定路由，否则菜单无法正常使用
        }
    }
}
