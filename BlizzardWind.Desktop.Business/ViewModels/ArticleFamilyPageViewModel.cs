using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using BlizzardWind.Desktop.Business.Services;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class ArticleFamilyPageViewModel : MvxViewModel
    {
        private readonly IFolderService _folderService;
        private readonly IFamilyService _familyService;

        public IMvxCommand SelectedFamilyCommand => new MvxCommand<ArticleFamilyModel>(OnSelectedClick);
        public IMvxCommand SearchCommand => new MvxCommand(OnSearchClick);

        public ObservableCollection<ArticleFamilyModel> FamilyCollection { get; set; }
        public ObservableCollection<ArticleFolder> FolderCollection { get; set; }

        private string folderName;
        public string FolderName
        {
            get => folderName;
            set => SetProperty(ref folderName, value);
        }

        private Guid familyID;
        public Guid FamilyID
        {
            get => familyID;
            set => SetProperty(ref familyID, value);
        }
    }

    public partial class ArticleFamilyPageViewModel
    {
        public ArticleFamilyPageViewModel(IFolderService folderService, IFamilyService familyService)
        {
            _folderService = folderService;
            _familyService = familyService;

            FamilyCollection = new ObservableCollection<ArticleFamilyModel>()
            {
                new ArticleFamilyModel() 
                { 
                    ID = Guid.NewGuid(),
                    Name = "大类 - 1",
                    FoldersCollection = new ObservableCollection<ArticleFolder>() 
                    { 
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="1 - 文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="1 - 文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="1 - 文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="1 - 文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="1 - 文件夹 - 5"},
                    } 
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 2",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="2 - 文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="2 - 文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="2 - 文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="2 - 文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="2 - 文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 3",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="3 - 文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="3 - 文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="3 - 文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="3 - 文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="3 - 文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 4",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="4 - 文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="4 - 文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="4 - 文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="4 - 文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="4 - 文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 5",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="5 - 文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="5 - 文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="5 - 文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="5 - 文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="5 - 文件夹 - 5"},
                    }
                },
            };
            FolderCollection = new ObservableCollection<ArticleFolder>();
            if (FamilyCollection.Any())
            {
                FamilyID = FamilyCollection[0].ID;
                if (FamilyCollection[0].FoldersCollection != null)
                    foreach (var item in FamilyCollection[0].FoldersCollection)
                        FolderCollection.Add(item);
            }
        }

        public async void OnCreateFamilyClick(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            Console.WriteLine(name);
        }

        public void OnCreateFolderClick()
        {
            Console.WriteLine();
        }

        private void OnSelectedClick(ArticleFamilyModel family)
        {
            if (family == null || family.FoldersCollection == null)
                return;
            FamilyID = family.ID;
            FolderCollection.Clear();
            foreach (var item in family.FoldersCollection)
            {
                FolderCollection.Add(item);
            }
        }

        private void OnSearchClick()
        {
            Console.WriteLine();
        }

        
    }
}
