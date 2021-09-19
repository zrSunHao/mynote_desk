using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class EditorPageViewModel : MvxViewModel
    {
        private readonly IFileResourceService _FileService;
        private readonly INoteService _NoteService;
        private readonly ViewModelMediator _Mediator;
        private List<MarkNoteFileModel> _FileList = new List<MarkNoteFileModel>();
        private Note _Note;

        public IMvxCommand MainOperateCommand => new MvxCommand<int>(OnMainOperateClick);
        public IMvxCommand MainUploadCommand => new MvxCommand<int>(OnUploadOperateClick);
        public IMvxCommand FileIdCopyCommand => new MvxCommand<MarkNoteFileModel>(OnFileIdCopyClick);
        public IMvxCommand FileRenameCommand => new MvxCommand<MarkNoteFileModel>(OnFileRenameClick);
        public IMvxCommand FileReplaceCommand => new MvxCommand<MarkNoteFileModel>(OnFileReplaceClick);
        public IMvxCommand FileExportCommand => new MvxCommand<MarkNoteFileModel>(OnFileExportClick);
        public IMvxCommand FileDeleteCommand => new MvxCommand<MarkNoteFileModel>(OnFileDeleteClick);

        public ObservableCollection<MarkNoteFileModel> FileCollection { get; set; }
        public ObservableCollection<OptionTypeItem> EditorFileTypeCollection { get; set; }
        public ObservableCollection<EditorOperateModel> MainOperateCollection { get; set; }
        public ObservableCollection<EditorOperateModel> UploadOperateCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<string, int, bool> UploadFileAction { get; set; }
        public Action<string> FileIdCopyAction { get; set; }
        public Action<string, MarkNoteFileModel> FileReplaceAction { get; set; }
        public Action<MarkNoteFileModel> FileExportAction { get; set; }
        public Action<MarkNoteFileModel> FileRenameAction { get; set; }

        private int _fileFilterType;
        public int FileFilterType
        {
            get => _fileFilterType;
            set => SetProperty(ref _fileFilterType, value);
        }

        private string _fileFilterName;
        public string FileFilterName
        {
            get => _fileFilterName;
            set => SetProperty(ref _fileFilterName, value);
        }

        private int _fileCount;
        public int FileCount
        {
            get => _fileCount;
            set => SetProperty(ref _fileCount, value);
        }

        private string _document;
        public string Document
        {
            get => _document;
            set => SetProperty(ref _document, value);
        }

    }

    public partial class EditorPageViewModel
    {
        public EditorPageViewModel(IFileResourceService fileService,
            INoteService noteService, ViewModelMediator mediator)
        {
            _FileService = fileService;
            _NoteService = noteService;
            _Mediator = mediator;
            Initial();
        }

        public void Initial()
        {
            FileCollection = new ObservableCollection<MarkNoteFileModel>();
            MainOperateCollection = new ObservableCollection<EditorOperateModel>()
            {
                new EditorOperateModel(){Name = "保存",  Type=EditorOperateType.Save},
                new EditorOperateModel(){Name = "云同步",  Type=EditorOperateType.CloudSync},
            };
            UploadOperateCollection = new ObservableCollection<EditorOperateModel>()
            {
                new EditorOperateModel(){Name = "图片",  Type=EditorOperateType.UploadImage},
                new EditorOperateModel(){Name = "office文件",  Type=EditorOperateType.UploadOfficeFile},
                new EditorOperateModel(){Name = "文本文档",  Type=EditorOperateType.UploadTxt},
                new EditorOperateModel(){Name = "PDF",  Type=EditorOperateType.UploadPDF},
                new EditorOperateModel(){Name = "音频",  Type=EditorOperateType.UploadAudio},
                new EditorOperateModel(){Name = "视频",  Type=EditorOperateType.UploadVideo},
            };
            EditorFileTypeCollection = new ObservableCollection<OptionTypeItem>()
            {
                new OptionTypeItem(){Name = "全部",Type = -1 },
                new OptionTypeItem(){Name = "封面",Type = EditorOperateType.UploadCoverPicture },
                new OptionTypeItem(){Name = "图片",Type = EditorOperateType.UploadImage },
                new OptionTypeItem(){Name = "office文件",Type = EditorOperateType.UploadOfficeFile },
                new OptionTypeItem(){Name = "文本文件",Type = EditorOperateType.UploadTxt },
                new OptionTypeItem(){Name = "PDF",Type = EditorOperateType.UploadPDF },
                new OptionTypeItem(){Name = "音频",Type = EditorOperateType.UploadAudio },
                new OptionTypeItem(){Name = "视频",Type = EditorOperateType.UploadVideo },
            };

            FileFilterType = -1;
        }

        public void WindowLoaded()
        {
            _Note = _Mediator.GetNote();
            LoadNoteAsync(_Note.Id);
        }

        public async void AddFileClick(string[]? fileNames, int type)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            List<MarkNoteFileModel> models = await _FileService
                .AddNoteFileAsync(type, fileNames.ToList(), _Note.Id);
            _FileList.InsertRange(0, models);
            foreach (MarkNoteFileModel model in models)
            {
                FileCollection.Insert(0, model);
            }
            FileCount = _FileList.Count;
            if (type == EditorOperateType.UploadCoverPicture)
            {
                _Note.CoverPictureId = models[0].Id;
                await _NoteService.UpdateAsync(_Note);
            }
        }

        public void FileFilter()
        {
            FileCollection.Clear();
            List<MarkNoteFileModel> list = _FileList;
            if (!string.IsNullOrEmpty(FileFilterName))
                list = list.Where(x => x.FileName.Contains(FileFilterName)).ToList();
            if (FileFilterType != -1)
                list = list.Where(x => x.Type == FileFilterType).ToList();
            foreach (MarkNoteFileModel model in list)
                FileCollection.Add(model);
        }

        public async void TextChange(string value)
        {
            var parser = new MarkTextParser();
            List<MarkElement> elements = parser.GetMarkElements(Document);
            _Note.Content = Document;
            _Note.Title = elements.FirstOrDefault(x => x.Type == MarkType.h1)?.Content;
            _Note.Keys = elements.FirstOrDefault(x => x.Type == MarkType.key)?.Content;
            await _NoteService.UpdateAsync(_Note);
            _Mediator.NoteChangedNotify(_Note, elements);
        }

        public async void FileReplace(MarkNoteFileModel model, string fileName)
        {
            await _FileService.RelaceAsync(model, fileName);
            var index = FileCollection.IndexOf(model);
            if (index >= 0)
            {
                FileCollection.Remove(model);
                FileCollection.Insert(index, model);
            }
        }

        public async void FileRename(MarkNoteFileModel model)
        {
            await _FileService.RenameAsync(model.Id, model.FileName);
            var index = FileCollection.IndexOf(model);
            if (index >= 0)
            {
                FileCollection.Remove(model);
                FileCollection.Insert(index, model);
            }
        }

        private void OnMainOperateClick(int type)
        {
            Console.WriteLine(type);
        }

        private void OnUploadOperateClick(int type)
        {
            string filter = GetFileFilter(type);
            bool multiselect = true;
            if (type == EditorOperateType.UploadCoverPicture)
                multiselect = false;
            if (UploadFileAction != null)
            {
                UploadFileAction.Invoke(filter, type, multiselect);
            }
        }

        private void OnFileIdCopyClick(MarkNoteFileModel model)
        {
            string msg = "暂不支持该文件类型的显示";
            switch (model.Type)
            {
                case EditorOperateType.UploadCoverPicture:
                case EditorOperateType.UploadImage:
                    msg = $"#img] <{model.FileName}>({model.Id.ToString()})";
                    break;
                case EditorOperateType.UploadTxt:
                    msg = $"#txt] <{model.FileName}>({model.Id.ToString()})";
                    break;
                default:
                    break;
            }
            if (FileIdCopyAction != null)
                FileIdCopyAction.Invoke(msg);
        }

        private void OnFileReplaceClick(MarkNoteFileModel model)
        {
            string filter = GetFileFilter(model.Type);
            if (FileReplaceAction != null)
                FileReplaceAction.Invoke(filter, model);
        }

        private void OnFileExportClick(MarkNoteFileModel model)
        {
            if (FileExportAction != null)
                FileExportAction.Invoke(model);
        }

        private void OnFileRenameClick(MarkNoteFileModel model)
        {
            if (FileRenameAction != null)
                FileRenameAction.Invoke(model);
        }

        private async void OnFileDeleteClick(MarkNoteFileModel model)
        {
            await _FileService.DeleteAsync(model.Id);
            _FileList.Remove(model);
            FileCollection.Remove(model);
        }

        private async void LoadNoteAsync(Guid noteId)
        {
            _Note = await _NoteService.GetAsync(noteId);
            if (_Note == null)
                throw new Exception("文章数据为空！");
            Document = _Note.Content;
            List<MarkNoteFileModel> models = await _FileService.GetNoteFilesAsync(noteId);
            _FileList.AddRange(models);
            foreach (MarkNoteFileModel model in models)
            {
                FileCollection.Add(model);
            }
            FileCount = _FileList.Count;
            if (_Note.CoverPictureId.HasValue)
            {
                var model = _FileList.FirstOrDefault(x => x.Id == _Note.CoverPictureId.Value);
            }
        }

        private string GetFileFilter(int type)
        {
            string filter = "图像文件|*.jpg;*.jpeg;*.gif;*.png;";
            switch (type)
            {
                case EditorOperateType.UploadCoverPicture:
                    filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;)|*.jpg;*.jpeg;*.gif;*.png;";
                    break;
                case EditorOperateType.UploadImage:
                    filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;)|*.jpg;*.jpeg;*.gif;*.png;";
                    break;
                case EditorOperateType.UploadOfficeFile:
                    filter = "office文件(word,excel.ppt)|*.docx;*.doc;*.xlsx;*.xls;*.pptx;*.ppt;";
                    break;
                case EditorOperateType.UploadTxt:
                    filter = "文本文件(*.txt;*.cs;)| *.txt;*.cs;";
                    break;
                case EditorOperateType.UploadPDF:
                    filter = "PDF(*.pdf;)|*.pdf;";
                    break;
                case EditorOperateType.UploadAudio:
                    filter = "音频文件(*.mp3;*.flac;)|*.mp3;*.flac;";
                    break;
                case EditorOperateType.UploadVideo:
                    filter = "图像文件(*.mp4;*.flv;)|*.mp4;*.flv;";
                    break;
            }
            return filter;
        }
    }
}
