using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
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
    public partial class ArticleListPageViewModel : MvxViewModel
    {
        private readonly IArticleService _articleService;
        private readonly IFamilyService _familyService;

        public IMvxCommand ContextMenuCommand => new MvxCommand<object>(OnContextMenuClick);

        public ObservableCollection<int> ArticleCollection { get; set; }

        public ObservableCollection<ArticleFamilyModel> FamilyCollection { get; set; }
    }

    public partial class ArticleListPageViewModel
    {
        public ArticleListPageViewModel(IArticleService articleService, IFamilyService familyService)
        {
            _articleService = articleService;
            _familyService = familyService;
            ArticleCollection = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, };
            FamilyCollection = new ObservableCollection<ArticleFamilyModel>()
            {
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 1",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 2",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 3",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 4",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 5"},
                    }
                },
                new ArticleFamilyModel()
                {
                    ID = Guid.NewGuid(),
                    Name = "大类 - 5",
                    FoldersCollection = new ObservableCollection<ArticleFolder>()
                    {
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 1"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 2"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 3"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 4"},
                        new ArticleFolder(){ ID = Guid.NewGuid(),Name="文件夹 - 5"},
                    }
                },
            };
        }

        private void OnContextMenuClick(object type)
        {
            Console.WriteLine(type.ToString());
        }
    }
}
