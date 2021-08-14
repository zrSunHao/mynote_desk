using BlizzardWind.Desktop.Business.Models;
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
    }

    public partial class MarkTextPageViewModel
    {
        public MarkTextPageViewModel()
        {
            ImageCollection = new ObservableCollection<MarkTextImageModel>();
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
