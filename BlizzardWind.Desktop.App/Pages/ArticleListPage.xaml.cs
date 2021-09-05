using BlizzardWind.Desktop.App.Dialogs;
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
    /// ArticleListPage.xaml 的交互逻辑
    /// </summary>
    public partial class ArticleListPage : Page
    {
        private readonly ArticleListPageViewModel VM;

        public ArticleListPage()
        {
            InitializeComponent();
            VM = (ArticleListPageViewModel)DataContext;
        }

        private async void CreateArticle_Button_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null)
                return;
            var model = VM.GetFamilyOptions();
            var dialog = new ArticleCreateDialog(model);
            dialog.ShowDialog();
            if(dialog.DialogResult == true && dialog.FolderId.HasValue && !string.IsNullOrEmpty(dialog.ArticleName))
            {
                await VM.CreateArticle(dialog.FolderId.Value, dialog.ArticleName);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(VM != null)
                await VM.PageLoad();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && VM != null)
                VM.SerachFolder();
        }
    }
}
