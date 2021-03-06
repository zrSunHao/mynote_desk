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
    public partial class NoteListPage : Page
    {
        private readonly NoteListPageViewModel VM;

        public NoteListPage()
        {
            InitializeComponent();
            VM = (NoteListPageViewModel)DataContext;
            VM.PromptInformationAction += PromptInformation;
            VM.NoteReaderWindowAction += NoteReaderWindow;
            VM.NoteMoveDialogAction += NoteMoveDialog;
            VM.NoteUploadCoverDialogAction += NoteUploadCoverDialog;
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
        }

        private async void CreateNote_Button_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null)
                return;
            var model = VM.GetFamilyOptions();
            var dialog = new ArticleDialog(model,null,string.Empty);
            dialog.ShowDialog();
            if(dialog.DialogResult == true && dialog.FolderId.HasValue && !string.IsNullOrEmpty(dialog.ArticleName))
            {
                await VM.CreateNote(dialog.FolderId.Value, dialog.ArticleName);
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

        private void NoteReaderWindow(Note article)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.GetType() == typeof(ReaderWindow))
                {
                    var window  = item as ReaderWindow;
                    if (window != null)
                        window.SetNote(article);
                    return;
                }
            }
            ReaderWindow reader = new ReaderWindow(article);
            reader.Show();
        }

        private async void NoteMoveDialog(Note article)
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
                await VM.NoteMove(article);
            }
        }

        private async void NoteUploadCoverDialog(int type, string filter, Note article)
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
            await VM.NoteUploadCover(article);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(VM != null)
                VM.TreeNodeClick(e.NewValue);
        }
    }
}
