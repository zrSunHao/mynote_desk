using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Consts;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Helpers;
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

        public ObservableCollection<OperateModel> MainOperateCollection { get; set; }
        public ObservableCollection<OperateModel> UploadOperateCollection { get; set; }
        public ObservableCollection<MarkNoteFileModel> FileCollection { get; set; }
        public ObservableCollection<OptionTypeItem> EditorFileTypeCollection { get; set; }
        public EditorFilesInfo FilesInfo { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<Note> NoteReaderWindowAction { get; set; }
        public Action<string, int, bool> UploadFileAction { get; set; }
        public Action<string> FileIdCopyAction { get; set; }
        public Action<string, MarkNoteFileModel> FileReplaceAction { get; set; }
        public Action<MarkNoteFileModel> FileExportAction { get; set; }
        public Action<MarkNoteFileModel> FileRenameAction { get; set; }

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
            MainOperateCollection = OperateHelper.GetEditorMainOperate();
            UploadOperateCollection = OperateHelper.GetEditorFileUploadOperate();
            EditorFileTypeCollection = OptionHelper.GetEditorFileTypes();

            FilesInfo = new EditorFilesInfo();
            FilesInfo.FilterType = -1;
        }

        public void WindowLoaded()
        {
            // 笔记信息来自中介者
            _Note = _Mediator.GetNote();
            LoadNoteAsync(_Note.Id);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fileNames">文件路径名</param>
        /// <param name="type">文件类型</param>
        public async void AddFileClick(string[]? fileNames, int type)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            List<MarkNoteFileModel> models = await _FileService
                .AddNoteFileAsync(type, fileNames.ToList(), _Note.Id);
            _FileList.InsertRange(0, models);

            foreach (MarkNoteFileModel model in models)
                FileCollection.Insert(0, model);
            FilesInfo.Count = _FileList.Count;

            if (type == MarkResourceType.Cover)
            {
                _Note.CoverPictureId = models[0].Id;
                await _NoteService.UpdateAsync(_Note);
            }
        }

        /// <summary>
        /// 文件筛选
        /// </summary>
        public void FileFilter()
        {
            FileCollection.Clear();
            List<MarkNoteFileModel> list = _FileList;
            if (!string.IsNullOrEmpty(FilesInfo.FilterName))
                list = list.Where(x => x.FileName.Contains(FilesInfo.FilterName)).ToList();
            if (FilesInfo.FilterType != -1)
                list = list.Where(x => x.Type == FilesInfo.FilterType).ToList();
            foreach (MarkNoteFileModel model in list)
                FileCollection.Add(model);
        }

        /// <summary>
        /// 文本改变
        /// </summary>
        /// <param name="value"></param>
        public async void TextChange(string value)
        {
            var parser = new MarkTextParser();
            List<MarkElement> elements = parser.GetMarkElements(Document);
            _Note.Content = Document;
            _Note.Title = elements.FirstOrDefault(x => x.Type == MarkNoteElementType.h1)?.Content;
            _Note.Keys = elements.FirstOrDefault(x => x.Type == MarkNoteElementType.key)?.Content;
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
            switch (type)
            {
                case EditorOperateType.See:
                    if (NoteReaderWindowAction != null)
                        NoteReaderWindowAction.Invoke(_Note);
                    break;
                default:
                    break;
            }
        }

        private void OnUploadOperateClick(int type)
        {
            string filter = MarkNoteHelper.GetWinFileFilter(type);
            bool multiselect = true;
            if (type == MarkResourceType.Cover)
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
                case MarkResourceType.Cover:
                case MarkResourceType.Image:
                    msg = $"#img] <{model.FileName}>({model.Id.ToString()})";
                    break;
                case MarkResourceType.Txt:
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
            string filter = MarkNoteHelper.GetWinFileFilter(model.Type);
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
            FilesInfo.Count = _FileList.Count;
            if (_Note.CoverPictureId.HasValue)
            {
                var model = _FileList.FirstOrDefault(x => x.Id == _Note.CoverPictureId.Value);
            }
        }
    }

    public class EditorFilesInfo : MvxViewModel
    {
        private int _filterType;
        public int FilterType
        {
            get => _filterType;
            set => SetProperty(ref _filterType, value);
        }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set => SetProperty(ref _filterName, value);
        }

        private int _count;
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
    }
}
