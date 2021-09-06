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
    /// FileEditDialog.xaml 的交互逻辑
    /// </summary>
    public partial class FileEditDialog : Window
    {
        public string FileName { get; set; }

        public FileEditDialog(string fileName)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(fileName))
                this.text_box.Text = fileName;
        }

        private void Yes_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            FileName = this.text_box.Text;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
