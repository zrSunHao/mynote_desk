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
        private List<MarkTextFileModel> _fileList = new List<MarkTextFileModel>();

        public IMvxCommand MainOperateCommand => new MvxCommand<int>(OnMainOperateClick);
        public IMvxCommand MainUploadCommand => new MvxCommand<int>(OnUploadOperateClick);
        public IMvxCommand FileOperateCommand => new MvxCommand<object[]>(OnFileOperateClick);

        public ObservableCollection<MarkTextFileModel> FileCollection { get; set; }
        public ObservableCollection<MarkTextHeadlineModel> HeadlineCollection { get; set; }

        public ObservableCollection<EditorOperateModel> MainOperateCollection { get; set; }
        public ObservableCollection<EditorOperateModel> UploadOperateCollection { get; set; }

        public Action<int> OnUploadFileClickAction { get; set; }

        private string coverPicture;
        public string CoverPicture
        {
            get => coverPicture;
            set => SetProperty(ref coverPicture, value);
        }

    }

    public partial class EditorWindowViewModel
    {
        public EditorWindowViewModel(IFileResourceService fileService)
        {
            _fileService = fileService;

            Initial();
        }

        public void Initial()
        {
            FileCollection = new ObservableCollection<MarkTextFileModel>();
            MainOperateCollection = new ObservableCollection<EditorOperateModel>()
            {
                new EditorOperateModel(){Name = "保存", Icon="\xe161", Type=EditorOperateType.Save},
                new EditorOperateModel(){Name = "云同步", Icon="\xe2c3", Type=EditorOperateType.CloudSync},
            };
            UploadOperateCollection = new ObservableCollection<EditorOperateModel>()
            {
                new EditorOperateModel(){Name = "添加封面", Icon="\xe3f4", Type=EditorOperateType.UploadCoverPicture},
                new EditorOperateModel(){Name = "上传图片", Icon="\xe161", Type=EditorOperateType.UploadImage},
                new EditorOperateModel(){Name = "上传Word", Icon="\xe161", Type=EditorOperateType.UploadWord},
                new EditorOperateModel(){Name = "上传Excel", Icon="\xe161", Type=EditorOperateType.UploadExcel},
                new EditorOperateModel(){Name = "上传PPT", Icon="\xe161", Type=EditorOperateType.UploadPPT},
                new EditorOperateModel(){Name = "上传文本文档", Icon="\xe161", Type=EditorOperateType.UploadTxt},
                new EditorOperateModel(){Name = "上传PDF", Icon="\xe161", Type=EditorOperateType.UploadPDF},
                new EditorOperateModel(){Name = "上传音频", Icon="\xe161", Type=EditorOperateType.UploadAudio},
                new EditorOperateModel(){Name = "上传视频", Icon="\xe161", Type=EditorOperateType.UploadVideo},
            };
            HeadlineCollection = new ObservableCollection<MarkTextHeadlineModel>()
            {
                new MarkTextHeadlineModel(){
                    Name = "大飒飒打撒打撒打撒",
                    Children = new List<MarkTextHeadlineModel>(){
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                    }
                },
                new MarkTextHeadlineModel(){
                    Name = "发生发射点发射点发生",
                    Children = new List<MarkTextHeadlineModel>(){
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                    }
                },
                new MarkTextHeadlineModel(){
                    Name = "发生发射点发射点发生",
                    Children = new List<MarkTextHeadlineModel>(){
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                    }
                },
            };
        }

        public async void OnWindowLoaded()
        {
            List<MarkTextFileModel> models = await _fileService.GetTextFilesAsync(MarkResourceType.image);
            foreach (MarkTextFileModel model in models)
            {
                FileCollection.Add(model);
            }
        }

        public async void OnAddFileClick(string[]? fileNames,int type)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            List<MarkTextFileModel> models = await _fileService
                .AddTextFileAsync(MarkResourceType.image, fileNames.ToList());
            foreach (MarkTextFileModel model in models)
            {
                FileCollection.Insert(0,model);
            }
        }

        public async void OnAddCoverPictureClick(string fileNames)
        {

        }

        private void OnMainOperateClick(int type)
        {
            Console.WriteLine(type);
        }

        private void OnUploadOperateClick(int type)
        {
            if(OnUploadFileClickAction != null)
            {
                OnUploadFileClickAction.Invoke(type);
            }
        }

        private void OnFileOperateClick(object[] args)
        {
            int type = int.Parse((string)args[0]);
            Guid id = (Guid)args[1];
            Console.WriteLine($"{type} ==> {id}");
        }
    }
}
