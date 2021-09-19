using BlizzardWind.App.Common.Consts;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.App.Windows;
using BlizzardWind.Desktop.Business.Entities;
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
            VM.PromptInformationAction += PromptInformation;
            VM.ArticleReaderWindowAction += ArticleReaderWindow;
            VM.ArticleMoveDialogAction += ArticleMoveDialog;
            VM.ArticleUploadCoverDialogAction += ArticleUploadCoverDialog;
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
        }

        private async void CreateArticle_Button_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null)
                return;
            var model = VM.GetFamilyOptions();
            var dialog = new ArticleDialog(model,null,string.Empty);
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

        private void ArticleReaderWindow(Article article)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.GetType() == typeof(EditorWindow))
                {
                    PromptInformation(MesssageType.Error, "存在正在编辑的文章");
                    return;
                }
            }
            EditorWindow editerWindow = new EditorWindow(article);
            editerWindow.Show();
        }

        private async void ArticleMoveDialog(Article article)
        {
            if (VM == null)
                return;
            var model = VM.GetFamilyOptions();
            var dialog = new ArticleDialog(model, article.FolderId,article.Title);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && dialog.FolderId.HasValue && !string.IsNullOrEmpty(dialog.ArticleName))
            {
                article.FolderId = dialog.FolderId.Value;
                article.Title = dialog.ArticleName;
                await VM.ArticleMove(article);
            }
        }

        private async void ArticleUploadCoverDialog(int type, string filter, Article article)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() != true)
                return;
            var imgId = await VM.AddFile(dialog.FileNames, type, article.Id);
            if (imgId == Guid.Empty)
                return;
            article.CoverPictureId = imgId;
            await VM.ArticleUploadCover(article);
        }
    }
}
