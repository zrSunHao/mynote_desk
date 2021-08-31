using BlizzardWind.Desktop.App.Windows;
using BlizzardWind.Desktop.Business.Models;
using BlizzardWind.Desktop.Business.ViewModels;
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
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly HomePageViewModel VM;

        public HomePage()
        {
            InitializeComponent();
            VM = (HomePageViewModel)DataContext;
            VM.OnAppAction = (app) => this.OnAppClick(app);
        }

        private void OnAppClick(HomeAppModel app)
        {
            switch (app.Type)
            {
                case HomeAppType.EDITOR:
                    LaunchEditorWindow();
                    break;
                default:
                    break;
            }
        }

        private void LaunchEditorWindow()
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.GetType() == typeof(EditorWindow))
                    //return;
                    item.Close();
            }
            EditorWindow editerWindow = new EditorWindow();
            editerWindow.Show();
        }
    }
}
