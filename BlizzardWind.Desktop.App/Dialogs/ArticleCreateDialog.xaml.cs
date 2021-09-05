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
    public partial class ArticleCreateDialog : Window
    {
        private readonly ArticleFamilyAndFolderModel _Model;
        private Guid? _FamilyId;

        public Guid? FolderId { get; set; }
        public string ArticleName { get; set; }

        public ArticleCreateDialog(ArticleFamilyAndFolderModel model)
        {
            InitializeComponent();
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
