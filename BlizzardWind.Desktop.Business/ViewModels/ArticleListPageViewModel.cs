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
        private readonly IArticleService _ArticleService;
        private readonly IFamilyService _FamilyService;
        private readonly IFolderService _FolderService;

        private List<ArticleFamily> _Familys = new();
        private List<ArticleFolder> _Folders = new();
        private ArticleFamilyModel? _CurrentFamily = null;
        private ArticleFolder? _CurrentFolder = null;
        private int _NewArticleCount = 0;
        private int _ArticleTotal = 0;
        private string _OrigionRightMsg = "";

        public IMvxCommand FamilyClickCommand => new MvxCommand<ArticleFamilyModel>(OnFamilyClick);
        public IMvxCommand FolderClickCommand => new MvxCommand<ArticleFolder>(OnFolderClick);
        public IMvxCommand SearchAticleClickCommand => new MvxCommand(OnSearchAticleClick);
        public IMvxCommand ResetSearchClickCommand => new MvxCommand(OnResetSearchClick);

        public List<OptionValueItem> ArticleSortColumns { get; set; }
        public ObservableCollection<Article> ArticleCollection { get; set; }
        public ObservableCollection<ArticleFamilyModel> FamilyCollection { get; set; }

        private Guid _familyId;
        public Guid FamilyId
        {
            get => _familyId;
            set => SetProperty(ref _familyId, value);
        }

        private Guid _folderId;
        public Guid FolderId
        {
            get => _folderId;
            set => SetProperty(ref _folderId, value);
        }

        private string _searchFolderName;
        public string SearchFolderName
        {
            get => _searchFolderName;
            set => SetProperty(ref _searchFolderName, value);
        }

        private string _searchArticleTitle;
        public string SearchArticleTitle
        {
            get => _searchArticleTitle;
            set => SetProperty(ref _searchArticleTitle, value);
        }

        private string _searchArticleKey;
        public string SearchArticleKey
        {
            get => _searchArticleKey;
            set => SetProperty(ref _searchArticleKey, value);
        }

        private string _searchSortColumn;
        public string SearchSortColumn
        {
            get => _searchSortColumn;
            set => SetProperty(ref _searchSortColumn, value);
        }

        private string _leftMsg;
        public string LeftMsg
        {
            get => _leftMsg;
            set => SetProperty(ref _leftMsg, value);
        }

        private string _rightMsg;
        public string RightMsg
        {
            get => _rightMsg;
            set => SetProperty(ref _rightMsg, value);
        }
    }

    public partial class ArticleListPageViewModel
    {
        public ArticleListPageViewModel(IArticleService articleService,
            IFamilyService familyService, IFolderService folderService)
        {
            _ArticleService = articleService;
            _FamilyService = familyService;
            _FolderService = folderService;

            ArticleCollection = new ObservableCollection<Article>();
            FamilyCollection = new ObservableCollection<ArticleFamilyModel>();
            ArticleSortColumns = new List<OptionValueItem>()
            {
                new OptionValueItem(){ Name = "时间倒序",Value = ArticleColumnConsts.UpdatedAt},
                new OptionValueItem(){ Name = "标题正序",Value = ArticleColumnConsts.Title},
            };
            InitialDefaultValue();
        }

        public async Task<bool> PageLoad()
        {
            _Familys = await _FamilyService.GetListAsync();
            _Folders = await _FolderService.GetAllListAsync();
            LoadFamilys();
            await LoadArticles();
            return true;
        }

        public void SerachFolder()
        {
            LoadFamilys();
        }

        public async Task<bool> CreateArticle(Guid folderId, string name)
        {
            var article = new Article()
            {
                Id = Guid.NewGuid(),
                FolderId = folderId,
                State = -1,
                Keys = name,
                Title = name,
                Content = "#h1] 文章总标题\r\n",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deleted = false
            };
            await _ArticleService.AddAsync(article);
            _ArticleTotal++;
            if (folderId == FolderId || FolderId == Guid.Empty)
            {
                _NewArticleCount++;
                ArticleCollection.Insert(0, article);
                RightMsg = $"新添加 {_NewArticleCount}  |  当前显示 - {ArticleCollection.Count}   | {_OrigionRightMsg}  |  总共 - {_ArticleTotal}";
            }
            else
            {
                RightMsg = $"当前显示 - {ArticleCollection.Count}  |  {_OrigionRightMsg} | 总共 - {_ArticleTotal}";
            }

            return true;
        }

        public ArticleFamilyAndFolderModel GetFamilyOptions()
        {
            return new ArticleFamilyAndFolderModel()
            {
                Familys = _Familys,
                Folders = _Folders,
            };
        }

        private async void OnFamilyClick(ArticleFamilyModel family)
        {
            family.OnClick();
            _CurrentFamily = family;
            if (_CurrentFolder == null)
                FolderId = Guid.Empty;
            else if (_CurrentFolder.FamilyId != family.Id)
            {
                _CurrentFolder = null;
                FolderId = Guid.Empty;
                await LoadArticles();
                ShowLeftMsg();
            }
        }

        private async void OnFolderClick(ArticleFolder folder)
        {
            if (FolderId == folder.Id)
                return;
            FolderId = folder.Id;
            _CurrentFolder = folder;
            await LoadArticles();
            ShowLeftMsg();
        }

        private async void OnSearchAticleClick()
        {
            await LoadArticles();
        }

        private async void OnResetSearchClick()
        {
            InitialDefaultValue();
            LoadFamilys();
            await LoadArticles();
        }

        private void InitialDefaultValue()
        {
            LeftMsg = string.Empty;
            RightMsg = string.Empty;
            FamilyId = Guid.Empty;
            FolderId = Guid.Empty;
            SearchSortColumn = ArticleColumnConsts.UpdatedAt;
            SearchFolderName = string.Empty;
            SearchArticleTitle = string.Empty;
            SearchArticleKey = string.Empty;

            _ArticleTotal = 0;
            _OrigionRightMsg = "";
            _CurrentFamily = null;
            _CurrentFolder = null;
        }

        private void LoadFamilys()
        {
            FamilyId = Guid.Empty;
            FolderId = Guid.Empty;
            FamilyCollection.Clear();
            if (!_Familys.Any())
            {
                ShowLeftMsg();
                return;
            }

            IEnumerable<ArticleFamily> familys = new List<ArticleFamily>();
            IEnumerable<ArticleFolder> folders = new List<ArticleFolder>();
            if (!string.IsNullOrWhiteSpace(SearchFolderName))
            {
                folders = _Folders.Where(x => x.Name.Contains(SearchFolderName)).OrderBy(x => x.Name);
                var familyIds = folders.Select(x => x.FamilyId);
                if (!familyIds.Any())
                    return;
                familys = _Familys.Where(x => familyIds.Contains(x.Id)).OrderBy(x => x.Name);
            }
            else
            {
                familys = _Familys;
                folders = _Folders;
            }

            foreach (var item in familys)
            {
                var family = new ArticleFamilyModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    FoldersCollection = new ObservableCollection<ArticleFolder>(),
                };
                var familyFolders = folders.Where(x => x.FamilyId == item.Id);
                family.AddFoldersRange(familyFolders);
                FamilyCollection.Add(family);
            }

            ShowLeftMsg();
        }

        private async Task<bool> LoadArticles()
        {
            _NewArticleCount = 0;
            _ArticleTotal = 0;
            ArticleCollection.Clear();
            var result = await _ArticleService.GetListAsync(FolderId, SearchSortColumn, SearchArticleTitle, SearchArticleKey);
            _ArticleTotal = result.Total;
            foreach (var item in result.Items)
            {
                ArticleCollection.Add(item);
            }

            _OrigionRightMsg = $"符合筛选条件 - {result.FilterTotal}";
            RightMsg = $"当前显示 - {result.Items.Count}  |  {_OrigionRightMsg}  |  总共 - {_ArticleTotal}";
            return true;
        }

        private void ShowLeftMsg()
        {
            int currentFolderTotal = FamilyCollection.Sum(x => x.FoldersCollection.Count);
            var familyMsg = "  |  全部大类";
            if (_CurrentFamily != null && _CurrentFolder != null)
                familyMsg = $"  |  {_CurrentFamily.Name}  ==>  {_CurrentFolder.Name}";
            LeftMsg = $"大类 - {FamilyCollection.Count} / {_Familys.Count}  |  文件夹 - {currentFolderTotal} / {_Folders.Count}{familyMsg}";
        }
    }


}
