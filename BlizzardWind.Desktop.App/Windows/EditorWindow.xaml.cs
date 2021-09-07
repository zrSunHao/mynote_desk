using BlizzardWind.App.Common.Consts;
using BlizzardWind.App.Common.Tools;
using BlizzardWind.Desktop.App.Dialogs;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;
using BlizzardWind.Desktop.Business.ViewModels;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

namespace BlizzardWind.Desktop.App.Windows
{
    /// <summary>
    /// EditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditorWindow : Window
    {
        static Subject<string> MySubject = new Subject<string>();

        private readonly EditorWindowViewModel VM;
        private readonly Article _Article;

        public EditorWindow(Article article)
        {
            InitializeComponent();

            _Article = article;
            VM = (EditorWindowViewModel)DataContext;
            VM.PromptInformationAction += PromptInformation;
            VM.UploadFileAction += UploadFile;

            VM.FileIdCopyAction += FileIdCopy;
            VM.FileReplaceAction += FileReplace;
            VM.FileExportAction += FileExport;
            VM.FileRenameAction += FileRename;

            MySubject.Throttle(TimeSpan.FromSeconds(1))
                .Subscribe((s) =>
                {
                    System.Windows.Application.Current.Dispatcher
                    .Invoke((Action)(() =>
                    {
                        VM.TextChange(s);
                    }));
                });
        }

        private void PromptInformation(int type, string msg)
        {
            var dialog = new ConfirmDialog(type, msg);
            dialog.ShowDialog();
        }

        private void FileIdCopy(string msg)
        {
            Clipboard.SetText(msg);
        }

        private void FileReplace(string filter, MarkTextFileModel model)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() != true)
                return;
            if (dialog.FileNames.Any())
                VM.FileReplace(model, dialog.FileNames[0]);
        }

        private void FileExport(MarkTextFileModel model)
        {
            if (!System.IO.File.Exists(model.FilePath))
            {
                PromptInformation(MesssageType.Error, $"本地未存储[{model.FileName}{model.Extension}]文件！");
                return;
            }
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() != true)
                return;

            if (System.IO.Directory.Exists(dialog.SelectedPath))
            {
                string savePath = System.IO.Path.Combine(dialog.SelectedPath, $"{model.FileName}{model.Extension}");
                if (System.IO.File.Exists(savePath))
                    PromptInformation(MesssageType.Error, $"文件[{model.FileName}{model.Extension}]已存在！");
                else
                {
                    string keyStr = FileEncryptTool.GuidToKey(model.SecretKey);
                    byte[] buffer = FileEncryptTool.GetDecryptFileBytes(model.FilePath, keyStr);
                    if (buffer == null || buffer.Length < 1)
                        PromptInformation(MesssageType.Error, $"文件[{model.FileName}{model.Extension}]解密失败！");
                    else
                        System.IO.File.WriteAllBytes(savePath, buffer);
                }
                    
            }

        }

        private void FileRename(MarkTextFileModel model)
        {
            FileEditDialog dialog = new FileEditDialog(model.FileName);
            dialog.ShowDialog();
            if (dialog.DialogResult == true && VM != null)
            {
                model.FileName = dialog.FileName;
                VM.FileRename(model);
            }
        }

        private void UploadFile(string filter, int type, bool multiselect = true)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = multiselect;
            if (dialog.ShowDialog() != true)
                return;
            VM.AddFileClick(dialog.FileNames, type);
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
                VM.WindowLoaded(_Article);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM != null)
                VM.FileFilter();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && VM != null)
                VM.FileFilter();
        }

        private void EditerBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MySubject.OnNext("");
        }

        //private void Rectangle_Drop(object sender, DragEventArgs e)
        //{
        //    if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        //        return;
        //    string[]? fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
        //    if (fileNames == null || fileNames.Length < 1)
        //        return;
        //}
    }
}
