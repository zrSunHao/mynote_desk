using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.Models;
using BlizzardWind.Desktop.Business.ViewModels;
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
    /// ArticleFamilyPage.xaml 的交互逻辑
    /// </summary>
    public partial class ArticleFamilyPage : Page
    {
        private readonly ArticleFamilyPageViewModel VM;

        public ArticleFamilyPage()
        {
            InitializeComponent();
            VM = (ArticleFamilyPageViewModel)DataContext;
        }

        private void CraeteFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var options = new List<OptionIdItem>()
            {
                new OptionIdItem(){ Id = Guid.NewGuid(), Name="日常"},
                new OptionIdItem(){ Id = Guid.NewGuid(), Name="学习"},
                new OptionIdItem(){ Id = Guid.NewGuid(), Name="生活"},
                new OptionIdItem(){ Id = Guid.NewGuid(), Name="赚钱"},
                new OptionIdItem(){ Id = Guid.NewGuid(), Name="社交"},
            };
            FolderDialog dialog = new FolderDialog(options);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
            {
                VM.OnCreateFamilyClick(dialog.FolderName);
            }
        }

        private void CraeteFamilyButton_Click(object sender, RoutedEventArgs e)
        {
            FolderClassifyDialog dialog = new FolderClassifyDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
            { 
                VM.OnCreateFamilyClick(dialog.FamilyName);
            }
        }
    }
}
