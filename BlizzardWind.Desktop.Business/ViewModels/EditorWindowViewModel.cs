using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class EditorWindowViewModel : MvxViewModel
    {
        private readonly IFileResourceService _fileService;
        private readonly IArticleService _articleService;
        private readonly ViewModelMediator _Mediator;
        private List<MarkTextFileModel> _fileList = new List<MarkTextFileModel>();
        private Article _Article;

        public IMvxCommand MainOperateCommand => new MvxCommand<int>(OnMainOperateClick);
        public IMvxCommand MainUploadCommand => new MvxCommand<int>(OnUploadOperateClick);
        public IMvxCommand FileOperateCommand => new MvxCommand<object[]>(OnFileOperateClick);

        public ObservableCollection<MarkTextFileModel> FileCollection { get; set; }
        public ObservableCollection<ArticleStructureModel> ArticleStructureCollection { get; set; }
        public ObservableCollection<OptionTypeItem> EditorFileTypeCollection { get; set; }

        public ObservableCollection<EditorOperateModel> MainOperateCollection { get; set; }
        public ObservableCollection<EditorOperateModel> UploadOperateCollection { get; set; }


        public Action<string, int, bool> OnUploadFileClickAction { get; set; }

        private string coverPicture;
        public string CoverPicture
        {
            get => coverPicture;
            set => SetProperty(ref coverPicture, value);
        }

        private int fileFilterType;
        public int FileFilterType
        {
            get => fileFilterType;
            set => SetProperty(ref fileFilterType, value);
        }

        private string fileFilterName;
        public string FileFilterName
        {
            get => fileFilterName;
            set => SetProperty(ref fileFilterName, value);
        }

        private int fileCount;
        public int FileCount
        {
            get => fileCount;
            set => SetProperty(ref fileCount, value);
        }

        private string document;
        public string Document
        {
            get => document;
            set => SetProperty(ref document, value);
        }

    }

    public partial class EditorWindowViewModel
    {
        public EditorWindowViewModel(IFileResourceService fileService, 
            IArticleService articleService, ViewModelMediator mediator)
        {
            _fileService = fileService;
            _articleService = articleService;
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

        public async void OnAddFileClick(string[]? fileNames, int type)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            List<MarkTextFileModel> models = await _fileService
                .AddArticleFileAsync(type, fileNames.ToList(), _Article.Id);
            _fileList.InsertRange(0, models);
            foreach (MarkTextFileModel model in models)
            {
                FileCollection.Insert(0, model);
            }
            FileCount = _fileList.Count;
            if (type == EditorOperateType.UploadCoverPicture)
            {
                CoverPicture = models[0].FilePath;
                _Article.CoverPictureId = models[0].ID;
                await _articleService.UpdateAsync(_Article);
            }
        }

        public void OnFileFilter()
        {
            FileCollection.Clear();
            List<MarkTextFileModel> list = _fileList;
            if (!string.IsNullOrEmpty(FileFilterName))
                list = list.Where(x => x.FileName.Contains(FileFilterName)).ToList();
            if (FileFilterType != -1)
                list = list.Where(x => x.Type == FileFilterType).ToList();
            foreach (MarkTextFileModel model in list)
                FileCollection.Add(model);
        }

        public async void OnTextChange(string value)
        {
            var parser = new MarkTextParser();
            var elements = parser.GetMarkElements(Document);
            _Article.Content = Document;
            _Article.Title = elements.FirstOrDefault(x => x.Type == MarkType.h1)?.Content;
            _Article.Keys = elements.FirstOrDefault(x => x.Type == MarkType.key)?.Content;
            await _articleService.UpdateAsync(_Article);

            _Mediator.ArticleChangedNotify(_Article);
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

        private void OnMainOperateClick(int type)
        {
            Console.WriteLine(type);
        }

        private void OnUploadOperateClick(int type)
        {
            string filter = "图像文件|*.jpg;*.jpeg;*.gif;*.png;";
            bool multiselect = true;
            switch (type)
            {
                case EditorOperateType.UploadCoverPicture:
                    filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;)|*.jpg;*.jpeg;*.gif;*.png;";
                    multiselect = false;
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
            if (OnUploadFileClickAction != null)
            {
                OnUploadFileClickAction.Invoke(filter, type, multiselect);
            }
        }

        private void OnFileOperateClick(object[] args)
        {
            int type = int.Parse((string)args[0]);
            Guid id = (Guid)args[1];
            Console.WriteLine($"{type} ==> {id}");
        }

        private async void LoadArticleAsync(Guid articleID)
        {
            _Article = await _articleService.GetAsync(articleID);
            if (_Article == null)
                throw new Exception("文章数据为空！");
            Document = _Article.Content;
            List<MarkTextFileModel> models = await _fileService.GetArticleFilesAsync(articleID);
            _fileList.AddRange(models);
            foreach (MarkTextFileModel model in models)
            {
                FileCollection.Add(model);
            }
            FileCount = _fileList.Count;
            if (_Article.CoverPictureId.HasValue)
                CoverPicture = _fileList.FirstOrDefault(x => x.ID == _Article.CoverPictureId.Value)?.FilePath;
        }
    }
}
