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
    /// FolderClassifyDialog.xaml 的交互逻辑
    /// </summary>
    public partial class FolderClassifyDialog : Window
    {
        public string FamilyName { get; set; }

        public FolderClassifyDialog(string familyName)
        {
            InitializeComponent();
            if(!string.IsNullOrEmpty(familyName))
                this.text_box.Text =  familyName;
        }

        private void Yes_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            FamilyName = this.text_box.Text;
        }
    }
}
