using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;
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
using System.Windows.Shapes;

namespace BlizzardWind.Desktop.App.Dialogs
{
    /// <summary>
    /// ArticleCreateDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ArticleDialog : Window
    {
        private readonly ArticleFamilyAndFolderModel _Model;
        private Guid? _FamilyId;

        public Guid? FolderId { get; set; }
        public string ArticleName { get; set; }

        public ArticleDialog(ArticleFamilyAndFolderModel model, Guid? folderId,string title)
        {
            InitializeComponent();
            if(!string.IsNullOrEmpty(title))
                this.text_box.Text = title;
            _Model = model;
            if (_Model?.Familys!= null)
            {
                this.FamilyComboBox.ItemsSource = model.Familys
                    .Select(x => new OptionIdItem() 
                    { 
                        Id = x.Id,
                        Name = x.Name 
                    });
            }
            if (_Model?.Familys != null && _Model?.Folders != null && folderId != null)
            {
                this.text_box.IsEnabled = false;
                var familyId = model.Folders.FirstOrDefault(x => x.Id == folderId)?.FamilyId;
                if (familyId == null)
                    return;
                var family = _Model.Familys.FirstOrDefault(x => x.Id == familyId);
                if (family == null)
                    return;
                var familyIndex = _Model.Familys.IndexOf(family);
                this.FamilyComboBox.SelectedIndex = familyIndex;

                var folders = _Model.Folders.Where(x => x.FamilyId == _FamilyId)
                    .Select(x => new OptionIdItem()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
                this.FolderComboBox.ItemsSource = folders;
                var folder = folders.FirstOrDefault(x => x.Id == folderId);
                if (folder == null)
                    return;
                var foldersIndex = folders.IndexOf(folder);
                this.FolderComboBox.SelectedIndex = foldersIndex;
            }
        }

        private void FamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _FamilyId = (this.FamilyComboBox.SelectedItem as OptionIdItem)?.Id;
            if (_FamilyId.HasValue && _Model?.Folders != null)
            {
                this.FolderComboBox.ItemsSource = _Model.Folders.Where(x=>x.FamilyId == _FamilyId)
                    .Select(x => new OptionIdItem() 
                    { 
                        Id = x.Id, 
                        Name = x.Name 
                    });
            }
        }

        private void Yes_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ArticleName = this.text_box.Text;
            FolderId = (this.FolderComboBox.SelectedItem as OptionIdItem)?.Id;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
