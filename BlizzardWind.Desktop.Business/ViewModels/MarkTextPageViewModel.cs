using BlizzardWind.Desktop.Business.Models;
using Microsoft.VisualBasic;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class MarkTextPageViewModel : MvxViewModel
    {
        public ObservableCollection<MarkTextImageModel> ImageCollection { get; set; }
        public ObservableCollection<MarkTextVersionModel> VersionCollection { get; set; }
        public ObservableCollection<MarkTextHeadlineModel> HeadlineCollection { get; set; }
    }

    public partial class MarkTextPageViewModel
    {
        public MarkTextPageViewModel()
        {
            ImageCollection = new ObservableCollection<MarkTextImageModel>();
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

        public void AddImages(string[]? fileNames)
        {
            if (fileNames == null || fileNames.Length < 1)
                return;
            foreach (string fileName in fileNames)
            {
                if (ImageCollection.Any(x => x.FilePath == fileName))
                    break;
                var model = new MarkTextImageModel()
                {
                    FileName = System.IO.Path.GetFileName(fileName),
                    Extension = System.IO.Path.GetExtension(fileName),
                    FilePath = fileName
                };
                ImageCollection.Add(model);
            }
        }
    }
}
