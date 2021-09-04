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
        private readonly IFolderService _FolderService;
        private readonly IFamilyService _FamilyService;

        public IMvxCommand SelectedFamilyCommand => new MvxCommand<ArticleFamily>(OnSelectedClick);
        public IMvxCommand SearchCommand => new MvxCommand(OnSearchClick);
        public IMvxCommand EditFamilyCommand => new MvxCommand<ArticleFamily>(OnEditFamilyClick);
        public IMvxCommand RemoveFamilyCommand => new MvxCommand<ArticleFamily>(OnRemoveFamilyClick);
        public IMvxCommand CreateFolderCommand => new MvxCommand(OnCreateFolderClick);
        public IMvxCommand EdiFolderCommand => new MvxCommand<ArticleFolder>(OnEditFolderClick);
        public IMvxCommand RemoveFolderCommand => new MvxCommand<ArticleFolder>(OnRemoveFolderClick);

        public ObservableCollection<ArticleFamily> FamilyCollection { get; set; }
        public ObservableCollection<ArticleFolder> FolderCollection { get; set; }

        public Action<string> ConfirmDialogAction { get; set; }
        public Action<ArticleFamily> FamilyEditDialogAction { get; set; }
        public Action<List<OptionIdItem>> FolderCreateDialogAction { get; set; }
        public Action<ArticleFolder,List<OptionIdItem>> FolderEditDialogAction { get; set; }

        private Guid _familyId;
        public Guid FamilyId
        {
            get => _familyId;
            set => SetProperty(ref _familyId, value);
        }

        private string _folderName;
        public string FolderName
        {
            get => _folderName;
            set => SetProperty(ref _folderName, value);
        }

        private int _pageIndex;
        public int PageIndex
        {
            get => _pageIndex;
            set => SetProperty(ref _pageIndex, value);
        }

        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        private int _total;
        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
    }

    public partial class ArticleFamilyPageViewModel
    {
        public ArticleFamilyPageViewModel(IFolderService folderService, IFamilyService familyService)
        {
            _FolderService = folderService;
            _FamilyService = familyService;

            FamilyCollection = new ObservableCollection<ArticleFamily>();
            FolderCollection = new ObservableCollection<ArticleFolder>();
            PageIndex = 0;
            PageSize = 20;
        }

        public async Task<bool> PageLoad()
        {
            var familys = await _FamilyService.GetListAsync();
            foreach (var family in familys)
                FamilyCollection.Add(family);
            if (familys.Any())
                await LoadFolderAsync(familys[0].Id);
            return true;
        }

        public async Task<bool> CreateFamily(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            var family = new ArticleFamily()
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deleted = false
            };
            await _FamilyService.AddAsync(family);

            var list = new List<ArticleFamily>();
            list.AddRange(FamilyCollection);
            list.Add(family);
            list = list.OrderBy(x => x.Name).ToList();
            FamilyCollection.Clear();
            foreach (var item in list)
            {
                FamilyCollection.Add(item);
            }
            return true;
        }

        public async Task<bool> EditFamily(ArticleFamily family)
        {
            await _FamilyService.UpdateAsync(family);

            var list = new List<ArticleFamily>();
            foreach (var item in FamilyCollection)
            {
                if(item.Id == family.Id)
                    item.Name = family.Name;
                list.Add(item);
            }
            list = list.OrderBy(x => x.Name).ToList();
            FamilyCollection.Clear();
            foreach (var item in list)
            {
                FamilyCollection.Add(item);
            }
            return true;
        }

        public async Task<bool> CreateFolder(Guid familyId, string name)
        {
            if (string.IsNullOrEmpty(name) || familyId == Guid.Empty)
                return false;
            var folder = new ArticleFolder()
            {
                Id = Guid.NewGuid(),
                Name = name,
                FamilyId = familyId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deleted = false
            };
            await _FolderService.AddAsync(folder);

            if (familyId != FamilyId || !name.Contains(FolderName))
                return true;
            var list = new List<ArticleFolder>();
            list.AddRange(FolderCollection);
            list.Add(folder);
            list = list.OrderBy(x => x.Name).ToList();
            FolderCollection.Clear();
            foreach (var item in list)
            {
                FolderCollection.Add(item);
            }
            return true;
        }

        public async Task<bool> EditFolder(ArticleFolder folder)
        {
            await _FolderService.UpdateAsync(folder);

            var list = new List<ArticleFolder>();
            foreach (var item in FolderCollection)
            {
                if (item.FamilyId == FamilyId)
                    list.Add(item);
            }
            list = list.OrderBy(x => x.Name).ToList();
            FolderCollection.Clear();
            foreach (var item in list)
            {
                FolderCollection.Add(item);
            }
            return true;
        }



        private void OnEditFamilyClick(ArticleFamily family)
        {
            if (FamilyEditDialogAction != null)
                FamilyEditDialogAction.Invoke(family);
        }

        private void OnRemoveFamilyClick(ArticleFamily family)
        {
            Console.WriteLine();
        }

        private void OnCreateFolderClick()
        {
            if (FolderCreateDialogAction == null)
                return;
            var options = this.GetFamilyOptions();
            FolderCreateDialogAction.Invoke(options);
        }

        private void OnEditFolderClick(ArticleFolder folder)
        {
            if (FolderEditDialogAction == null)
                return;
            var options = this.GetFamilyOptions();
            FolderEditDialogAction.Invoke(folder, options);
        }

        private void OnRemoveFolderClick(ArticleFolder folder)
        {
            Console.WriteLine();
        }

        private async void OnSelectedClick(ArticleFamily family)
        {
            await LoadFolderAsync(family.Id);
        }

        private async void OnSearchClick()
        {
            await LoadFolderAsync(FamilyId);
        }

        private List<OptionIdItem> GetFamilyOptions()
        {
            return FamilyCollection.
                Select(x => new OptionIdItem()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        private async Task<bool> LoadFolderAsync(Guid familyId)
        {
            FamilyId = familyId;
            FolderCollection.Clear();
            var result = await _FolderService.GetListAsync(familyId, PageIndex, PageSize, FolderName);
            Total = result.Total;
            FamilyId = familyId;
            foreach (var folder in result.Items)
                FolderCollection.Add(folder);
            return true;
        }
    }
}
