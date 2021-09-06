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
        private readonly IFileResourceService _FileService;
        private readonly ViewModelMediator _Mediator;

        private List<ArticleFamily> _Familys = new();
        private List<ArticleFolder> _Folders = new();
        private ArticleFamilyModel? _CurrentFamily = null;
        private ArticleFolder? _CurrentFolder = null;
        private int _NewArticleCount = 0;
        private int _ArticleTotal = 0;
        private int _FilterTotal = 0;

        public IMvxCommand FamilyClickCommand => new MvxCommand<ArticleFamilyModel>(OnFamilyClick);
        public IMvxCommand FolderClickCommand => new MvxCommand<ArticleFolder>(OnFolderClick);
        public IMvxCommand SearchAticleClickCommand => new MvxCommand(OnSearchAticleClick);
        public IMvxCommand ResetSearchClickCommand => new MvxCommand(OnResetSearchClick);
        public IMvxCommand ArticleSeeCommand => new MvxCommand<Article>(OnArticleSeeClick);
        public IMvxCommand ArticleMoveCommand => new MvxCommand<Article>(OnArticleMoveClick);
        public IMvxCommand ArticleEditCommand => new MvxCommand<Article>(OnArticleEditClick);
        public IMvxCommand ArticleUploadCoverCommand => new MvxCommand<Article>(OnArticleUploadCoverClick);
        public IMvxCommand ArticleDeleteCommand => new MvxCommand<Article>(OnArticleDeleteClick);

        public List<OptionValueItem> ArticleSortColumns { get; set; }
        public ObservableCollection<Article> ArticleCollection { get; set; }
        public ObservableCollection<ArticleFamilyModel> FamilyCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        //public Action<Article> ArticleSeeDialogAction { get; set; }
        public Action<Article> ArticleEditDialogAction { get; set; }
        public Action<Article> ArticleMoveDialogAction { get; set; }
        public Action<int,string,Article> ArticleUploadCoverDialogAction { get; set; }

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
            IFamilyService familyService, IFolderService folderService,
            IFileResourceService fileService, ViewModelMediator mediator)
        {
            _ArticleService = articleService;
            _FamilyService = familyService;
            _FolderService = folderService;
            _FileService = fileService;
            _Mediator = mediator;

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
            }
            ShowRightMsg();
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

        public async Task<bool> ArticleMove(Article article)
        {
            await _ArticleService.UpdateAsync(article);

            if (FolderId != Guid.Empty && article.FolderId != FolderId)
            {
                ArticleCollection.Remove(article);
                _ArticleTotal--;
                _FilterTotal--;
                ShowRightMsg();
            }
            //if (SearchSortColumn == ArticleColumnConsts.UpdatedAt)
            //{
            //    ArticleCollection.Remove(article);
            //    ArticleCollection.Insert(0, article);
            //}
            //else
            //{
            //    var list = ArticleCollection.OrderBy(x => x.Title).ToList();
            //    var index = list.IndexOf(article);
            //    ArticleCollection.Remove(article);
            //    if(ArticleCollection.Count < index)
            //        ArticleCollection.Add(article);
            //    else
            //        ArticleCollection.Insert(index, article);
            //}
            
            return true;
        }

        public async Task<bool> ArticleUploadCover(Article article)
        {
            await _ArticleService.UpdateAsync(article);

            if (!article.CoverPictureId.HasValue)
                return false;
            var coverPath = await _FileService.GetPathByIdAsync(article.CoverPictureId.Value);
            article.SetCoverPicturePath(coverPath);
            var index = ArticleCollection.IndexOf(article);
            ArticleCollection.Remove(article);
            ArticleCollection.Insert(index, article);
            return true;
        }

        public async Task<Guid> AddFile(string[]? fileNames, int type,Guid articleId)
        {
            if (fileNames == null || fileNames.Length < 1)
                return Guid.Empty;
            List<MarkTextFileModel> models = await _FileService
                .AddArticleFileAsync(type, fileNames.ToList(), articleId);
            if (models.Any())
                return models[0].ID;
            return Guid.Empty;
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

        private void OnArticleSeeClick(Article article)
        {
            _Mediator.SetShowArticle(article);
            _Mediator.RouteRedirect(PageNameConsts.MarkTextPage);
        }

        private void OnArticleMoveClick(Article article)
        {
            if (ArticleMoveDialogAction == null)
                return;
            ArticleMoveDialogAction.Invoke(article);
        }

        private void OnArticleEditClick(Article article)
        {
            if (ArticleEditDialogAction == null)
                return;
            ArticleEditDialogAction.Invoke(article);
        }

        private void OnArticleUploadCoverClick(Article article)
        {
            if (ArticleUploadCoverDialogAction == null)
                return;
            string filter = "图像文件|*.jpg;*.jpeg;*.gif;*.png;";
            ArticleUploadCoverDialogAction.Invoke(EditorOperateType.UploadCoverPicture, filter, article);
        }

        private async void OnArticleDeleteClick(Article article)
        {
            await _ArticleService.DeleteAsync(article.Id);
            ArticleCollection.Remove(article);
            _ArticleTotal--;
            _FilterTotal--;
            ShowRightMsg();
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
            _FilterTotal = 0;
            _NewArticleCount = 0;
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
            _FilterTotal = 0;
            _NewArticleCount = 0;
            _ArticleTotal = 0;
            ArticleCollection.Clear();

            var result = await _ArticleService.GetListAsync(FolderId, SearchSortColumn, SearchArticleTitle, SearchArticleKey);
            foreach (var item in result.Items)
            {
                if(item.CoverPictureId.HasValue)
                {
                    var coverPath = await _FileService.GetPathByIdAsync(item.CoverPictureId.Value);
                    item.SetCoverPicturePath(coverPath);
                }
                ArticleCollection.Add(item);
            }

            _ArticleTotal = result.Total;
            _FilterTotal = result.FilterTotal;
            ShowRightMsg();
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

        private void ShowRightMsg()
        {
            var newMsg = "";
            if (_NewArticleCount > 0)
                newMsg = $"新添加 {_NewArticleCount}  |  ";
            RightMsg = $"{newMsg}当前显示 - {ArticleCollection.Count}   | 符合筛选条件 - {_FilterTotal}  |  总共 - {_ArticleTotal}";
        }
    }


}
