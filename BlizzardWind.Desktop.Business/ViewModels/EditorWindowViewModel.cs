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

        public IMvxCommand ImageCopyCommand => new MvxCommand<string>(OnImageCopyClick);
        public IMvxCommand ImageReplaceCommand => new MvxCommand<string>(OnImageReplaceClick);
        public IMvxCommand ImageDeleteCommand => new MvxCommand<string>(OnImageDeleteClick);

        public ObservableCollection<MarkTextFileModel> ImageCollection { get; set; }
        public ObservableCollection<MarkTextVersionModel> VersionCollection { get; set; }
        public ObservableCollection<MarkTextHeadlineModel> HeadlineCollection { get; set; }
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
            ImageCollection = new ObservableCollection<MarkTextFileModel>();
            VersionCollection = new ObservableCollection<MarkTextVersionModel>()
            {
                new MarkTextVersionModel(){Name = "版本1",Time = DateTime.Now},
                new MarkTextVersionModel(){Name = "版本2",Time = DateTime.Now},
                new MarkTextVersionModel(){Name = "版本3",Time = DateTime.Now},
                new MarkTextVersionModel(){Name = "版本4",Time = DateTime.Now},
                new MarkTextVersionModel(){Name = "版本5",Time = DateTime.Now},
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
                ImageCollection.Add(model);
            }
        }

        public async void OnAddImagesClick(string[]? fileNames)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            List<MarkTextFileModel> models = await _fileService
                .AddTextFileAsync(MarkResourceType.image, fileNames.ToList());
            foreach (MarkTextFileModel model in models)
            {
                ImageCollection.Add(model);
            }
        }

        private void OnImageCopyClick(string path)
        {
            Console.WriteLine(path);
        }

        private void OnImageReplaceClick(string path)
        {
            Console.WriteLine(path);
        }

        private void OnImageDeleteClick(string path)
        {
            Console.WriteLine(path);
        }
    }
}
