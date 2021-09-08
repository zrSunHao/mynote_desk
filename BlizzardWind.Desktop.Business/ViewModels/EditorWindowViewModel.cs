using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class EditorWindowViewModel : MvxViewModel
    {
        private readonly IFileResourceService _FileService;
        private readonly IArticleService _ArticleService;
        private readonly ViewModelMediator _Mediator;
        private List<MarkTextFileModel> _FileList = new List<MarkTextFileModel>();
        private Article _Article;

        public IMvxCommand MainOperateCommand => new MvxCommand<int>(OnMainOperateClick);
        public IMvxCommand MainUploadCommand => new MvxCommand<int>(OnUploadOperateClick);
        public IMvxCommand FileIdCopyCommand => new MvxCommand<MarkTextFileModel>(OnFileIdCopyClick);
        public IMvxCommand FileRenameCommand => new MvxCommand<MarkTextFileModel>(OnFileRenameClick);
        public IMvxCommand FileReplaceCommand => new MvxCommand<MarkTextFileModel>(OnFileReplaceClick);
        public IMvxCommand FileExportCommand => new MvxCommand<MarkTextFileModel>(OnFileExportClick);
        public IMvxCommand FileDeleteCommand => new MvxCommand<MarkTextFileModel>(OnFileDeleteClick);

        public ObservableCollection<MarkTextFileModel> FileCollection { get; set; }
        public ObservableCollection<ArticleStructureModel> ArticleStructureCollection { get; set; }
        public ObservableCollection<OptionTypeItem> EditorFileTypeCollection { get; set; }
        public ObservableCollection<EditorOperateModel> MainOperateCollection { get; set; }
        public ObservableCollection<EditorOperateModel> UploadOperateCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<string, int, bool> UploadFileAction { get; set; }
        public Action<string> FileIdCopyAction { get; set; }
        public Action<string, MarkTextFileModel> FileReplaceAction { get; set; }
        public Action<MarkTextFileModel> FileExportAction { get; set; }
        public Action<MarkTextFileModel> FileRenameAction { get; set; }

        private string _coverPicturePath;
        public string CoverPicturePath
        {
            get => _coverPicturePath;
            set => SetProperty(ref _coverPicturePath, value);
        }

        private Guid _coverPictureKey;
        public Guid CoverPictureKey
        {
            get => _coverPictureKey;
            set => SetProperty(ref _coverPictureKey, value);
        }

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

    public partial class EditorWindowViewModel
    {
        public EditorWindowViewModel(IFileResourceService fileService,
            IArticleService articleService, ViewModelMediator mediator)
        {
            _FileService = fileService;
            _ArticleService = articleService;
            _Mediator = mediator;
            Initial();
        }

        public void Initial()
        {
            FileCollection = new ObservableCollection<MarkTextFileModel>();
            MainOperateCollection = new ObservableCollection<EditorOperateModel>()
            {
                new EditorOperateModel(){Name = "保存",  Type=EditorOperateType.Save},
                new EditorOperateModel(){Name = "云同步",  Type=EditorOperateType.CloudSync},
            };
            UploadOperateCollection = new ObservableCollection<EditorOperateModel>()
            {
                new EditorOperateModel(){Name = "封面",  Type=EditorOperateType.UploadCoverPicture},
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
            ArticleStructureCollection = new ObservableCollection<ArticleStructureModel>();

            FileFilterType = -1;
        }

        public void WindowLoaded(Article article)
        {
            _Article = article;
            LoadArticleAsync(_Article.Id);
        }

        public async void AddFileClick(string[]? fileNames, int type)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            List<MarkTextFileModel> models = await _FileService
                .AddArticleFileAsync(type, fileNames.ToList(), _Article.Id);
            _FileList.InsertRange(0, models);
            foreach (MarkTextFileModel model in models)
            {
                FileCollection.Insert(0, model);
            }
            FileCount = _FileList.Count;
            if (type == EditorOperateType.UploadCoverPicture)
            {
                CoverPicturePath = models[0].FilePath;
                CoverPictureKey = models[0].SecretKey;
                _Article.CoverPictureId = models[0].Id;
                await _ArticleService.UpdateAsync(_Article);
            }
        }

        public void FileFilter()
        {
            FileCollection.Clear();
            List<MarkTextFileModel> list = _FileList;
            if (!string.IsNullOrEmpty(FileFilterName))
                list = list.Where(x => x.FileName.Contains(FileFilterName)).ToList();
            if (FileFilterType != -1)
                list = list.Where(x => x.Type == FileFilterType).ToList();
            foreach (MarkTextFileModel model in list)
                FileCollection.Add(model);
        }

        public async void TextChange(string value)
        {
            var parser = new MarkTextParser();
            List<MarkElement> elements = parser.GetMarkElements(Document);
            _Article.Content = Document;
            _Article.Title = elements.FirstOrDefault(x => x.Type == MarkType.h1)?.Content;
            _Article.Keys = elements.FirstOrDefault(x => x.Type == MarkType.key)?.Content;
            await _ArticleService.UpdateAsync(_Article);
            _Mediator.ArticleChangedNotify(_Article, elements);

            ArticleStructureCollection.Clear();
            ArticleStructureModel? topComponent = null;
            bool hasTitle_2 = false;
            foreach (var el in elements)
            {
                if (el.Level == MarkTypeLevel.Skip)
                {
                    hasTitle_2 = false;
                    continue;
                }
                ArticleStructureModel component = new()
                {
                    Name = el.ShortContent,
                    Type = el.RowType,
                    TypeName = el.TypeName,
                    Level = el.Level,
                };
                if (component.Level == MarkTypeLevel.Title_1)
                {
                    if (topComponent != null)
                        ArticleStructureCollection.Add(topComponent);
                    topComponent = component;
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkTypeLevel.Single)
                {
                    if (topComponent != null)
                        ArticleStructureCollection.Add(topComponent);
                    topComponent = null;
                    ArticleStructureCollection.Add(component);
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkTypeLevel.Title_2)
                {
                    if (topComponent == null)
                        topComponent = component;
                    else
                        topComponent.AddChildren(component);
                    hasTitle_2 = true;
                    continue;
                }
                if (component.Level == MarkTypeLevel.Leaf)
                {
                    if (topComponent == null)
                        ArticleStructureCollection.Add(component);
                    else if (!hasTitle_2)
                        topComponent.AddChildren(component);
                    else if (!topComponent.Children.Any())
                        topComponent.AddChildren(component);
                    else
                    {
                        int index = topComponent.Children.Count - 1;
                        topComponent.Children[index].AddChildren(component);
                    }
                }
            }
            if (topComponent != null)
            {
                ArticleStructureCollection.Add(topComponent);
            }
        }

        public async void FileReplace(MarkTextFileModel model, string fileName)
        {
            await _FileService.RelaceAsync(model, fileName);
            var index = FileCollection.IndexOf(model);
            if (index >= 0)
            {
                FileCollection.Remove(model);
                FileCollection.Insert(index, model);
            }
        }

        public async void FileRename(MarkTextFileModel model)
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

        private void OnFileIdCopyClick(MarkTextFileModel model)
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

        private void OnFileReplaceClick(MarkTextFileModel model)
        {
            string filter = GetFileFilter(model.Type);
            if (FileReplaceAction != null)
                FileReplaceAction.Invoke(filter, model);
        }

        private void OnFileExportClick(MarkTextFileModel model)
        {
            if (FileExportAction != null)
                FileExportAction.Invoke(model);
        }

        private void OnFileRenameClick(MarkTextFileModel model)
        {
            if (FileRenameAction != null)
                FileRenameAction.Invoke(model);
        }

        private async void OnFileDeleteClick(MarkTextFileModel model)
        {
            await _FileService.DeleteAsync(model.Id);
            _FileList.Remove(model);
            FileCollection.Remove(model);
        }

        private async void LoadArticleAsync(Guid articleId)
        {
            _Article = await _ArticleService.GetAsync(articleId);
            if (_Article == null)
                throw new Exception("文章数据为空！");
            Document = _Article.Content;
            List<MarkTextFileModel> models = await _FileService.GetArticleFilesAsync(articleId);
            _FileList.AddRange(models);
            foreach (MarkTextFileModel model in models)
            {
                FileCollection.Add(model);
            }
            FileCount = _FileList.Count;
            if (_Article.CoverPictureId.HasValue)
            {
                var model = _FileList.FirstOrDefault(x => x.Id == _Article.CoverPictureId.Value);
                if (model != null)
                {
                    CoverPicturePath = model.FilePath;
                    CoverPictureKey = model.SecretKey;
                }
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
