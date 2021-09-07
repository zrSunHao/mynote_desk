using BlizzardWind.App.Common.Consts;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.Entities;
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
            VM.PromptInformationAction += PromptInformation;
            VM.FamilyEditDialogAction += EditFamilyDialog;
            VM.FamilyDeleteDialogAction += DeleteFamilyDialog;
            VM.FolderCreateDialogAction += CraeteFolderDialog;
            VM.FolderEditDialogAction += EditFolderDialog;
            VM.FolderDeleteDialogAction += DeleteFolderDialog;
            VM.FolderUploadCoverDialogAction += FolderUploadCoverDialog;
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
        }

        private async void EditFamilyDialog(ArticleFamily family)
        {
            FolderClassifyDialog dialog = new FolderClassifyDialog(family.Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
            {
                family.Name = dialog.FamilyName;
                await VM.EditFamily(family);
            }   
        }

        private async void DeleteFamilyDialog(int type,string msg,ArticleFamily family)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
            {
                await VM.DeleteFamily(family);
            }
        }

        private async void CraeteFolderDialog(List<OptionIdItem> options)
        {
            FolderDialog dialog = new FolderDialog(options, null, "");
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null && dialog.FamilyId.HasValue)
            {
                await VM.CreateFolder(dialog.FamilyId.Value, dialog.FolderName);
            }
        }

        private async void EditFolderDialog(ArticleFolder folder, List<OptionIdItem> options)
        {
            FolderDialog dialog = new FolderDialog(options, folder.FamilyId, folder.Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null && dialog.FamilyId.HasValue)
            {
                folder.Name = dialog.FolderName;
                folder.FamilyId = dialog.FamilyId.Value;
                await VM.EditFolder(folder);
            }
        }

        private async void DeleteFolderDialog(int type, string msg, ArticleFolder folder)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
            {
                await VM.DeleteFolder(folder);
            }
        }

        private async void FolderUploadCoverDialog(int type, string filter, ArticleFolder folder)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() != true)
                return;
            try
            {
                var file = await VM.AddFile(dialog.FileNames, type, folder.Id);
                if (file != null)
                {
                    folder.SetCoverPicturePath(file.FilePath);
                    folder.SetCoverPictureKey(file.SecretKey);
                    folder.CoverPictureId = file.Id;
                }
                await VM.FolderUploadCover(folder);
            }catch(Exception ex)
            {
                PromptInformation(MesssageType.Error, ex.Message);
            }
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
                await VM.PageLoad();
        }

        private async void CraeteFamilyButton_Click(object sender, RoutedEventArgs e)
        {
            FolderClassifyDialog dialog = new FolderClassifyDialog("");
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
                await VM.CreateFamily(dialog.FamilyName);
        }
    }
}
