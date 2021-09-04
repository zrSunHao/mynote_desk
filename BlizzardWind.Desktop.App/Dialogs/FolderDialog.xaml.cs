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
    /// FolderDialog.xaml 的交互逻辑
    /// </summary>
    public partial class FolderDialog : Window
    {
        public Guid? FamilyId { get; set; }

        public string FolderName { get; set; }

        public FolderDialog(List<OptionIdItem> options, Guid? familyId ,string folderName)
        {
            InitializeComponent();
            if (options != null)
                this.comboBox.ItemsSource = options;
            FamilyId = familyId;
            if (familyId.HasValue && options != null)
            {
                var item = options.FirstOrDefault(x=>x.Id ==  familyId.Value);
                if (item != null)
                    this.comboBox.SelectedIndex = options.IndexOf(item);
            }
            if (!string.IsNullOrEmpty(folderName))
                this.text_box.Text = folderName;
        }

        private void Yes_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            FolderName = this.text_box.Text;
            FamilyId = (this.comboBox.SelectedItem as OptionIdItem)?.Id;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
