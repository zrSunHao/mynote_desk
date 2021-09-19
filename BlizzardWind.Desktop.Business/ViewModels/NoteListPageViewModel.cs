using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class NoteListPageViewModel : MvxViewModel
    {
        private readonly INoteService _NoteService;
        private readonly IFamilyService _FamilyService;
        private readonly IFolderService _FolderService;
        private readonly IFileResourceService _FileService;
        private readonly ViewModelMediator _Mediator;

        private List<NoteFamily> _Familys = new();
        private List<NoteFolder> _Folders = new();
        private NoteFamilyModel? _CurrentFamily = null;
        private NoteFolder? _CurrentFolder = null;
        private int _NewNoteCount = 0;
        private int _NoteTotal = 0;
        private int _FilterTotal = 0;

        public IMvxCommand FamilyClickCommand => new MvxCommand<NoteFamilyModel>(OnFamilyClick);
        public IMvxCommand FolderClickCommand => new MvxCommand<NoteFolder>(OnFolderClick);
        public IMvxCommand SearchNoteClickCommand => new MvxCommand(OnSearchNoteClick);
        public IMvxCommand ResetSearchClickCommand => new MvxCommand(OnResetSearchClick);
        public IMvxCommand NoteSeeCommand => new MvxCommand<Note>(OnNoteSeeClick);
        public IMvxCommand NoteMoveCommand => new MvxCommand<Note>(OnNoteMoveClick);
        public IMvxCommand NoteEditCommand => new MvxCommand<Note>(OnNoteEditClick);
        public IMvxCommand NoteUploadCoverCommand => new MvxCommand<Note>(OnNoteUploadCoverClick);
        public IMvxCommand NoteDeleteCommand => new MvxCommand<Note>(OnNoteDeleteClick);

        public List<OptionValueItem> NoteSortColumns { get; set; }
        public ObservableCollection<Note> NoteCollection { get; set; }
        public ObservableCollection<NoteFamilyModel> FamilyCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<Note> NoteReaderWindowAction { get; set; }
        public Action<Note> NoteMoveDialogAction { get; set; }
        public Action<int, string, Note> NoteUploadCoverDialogAction { get; set; }

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

        private string _searchNoteTitle;
        public string SearchNoteTitle
        {
            get => _searchNoteTitle;
            set => SetProperty(ref _searchNoteTitle, value);
        }

        private string _searchNoteKey;
        public string SearchNoteKey
        {
            get => _searchNoteKey;
            set => SetProperty(ref _searchNoteKey, value);
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

    public partial class NoteListPageViewModel
    {
        public NoteListPageViewModel(INoteService noteService,
            IFamilyService familyService, IFolderService folderService,
            IFileResourceService fileService, ViewModelMediator mediator)
        {
            _NoteService = noteService;
            _FamilyService = familyService;
            _FolderService = folderService;
            _FileService = fileService;
            _Mediator = mediator;

            NoteCollection = new ObservableCollection<Note>();
            FamilyCollection = new ObservableCollection<NoteFamilyModel>();
            NoteSortColumns = new List<OptionValueItem>()
            {
                new OptionValueItem(){ Name = "时间倒序",Value = NoteColumnConsts.UpdatedAt},
                new OptionValueItem(){ Name = "标题正序",Value = NoteColumnConsts.Title},
            };
            InitialDefaultValue();
        }

        public async Task<bool> PageLoad()
        {
            _Familys = await _FamilyService.GetListAsync();
            _Folders = await _FolderService.GetAllListAsync();
            LoadFamilys();
            await LoadNotes();
            return true;
        }

        public void SerachFolder()
        {
            LoadFamilys();
        }

        public async Task<bool> CreateNote(Guid folderId, string name)
        {
            var note = new Note()
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
            await _NoteService.AddAsync(note);
            _NoteTotal++;
            if (folderId == FolderId || FolderId == Guid.Empty)
            {
                _NewNoteCount++;
                NoteCollection.Insert(0, note);
            }
            ShowRightMsg();
            return true;
        }

        public NoteFamilyAndFolderModel GetFamilyOptions()
        {
            return new NoteFamilyAndFolderModel()
            {
                Familys = _Familys,
                Folders = _Folders,
            };
        }

        public async Task<bool> NoteMove(Note note)
        {
            await _NoteService.UpdateAsync(note);

            if (FolderId != Guid.Empty && note.FolderId != FolderId)
            {
                NoteCollection.Remove(note);
                _NoteTotal--;
                _FilterTotal--;
                ShowRightMsg();
            }
            return true;
        }

        public async Task<bool> NoteUploadCover(Note note)
        {
            await _NoteService.UpdateAsync(note);

            if (!note.CoverPictureId.HasValue)
                return false;
            var file = await _FileService.GetByIdAsync(note.CoverPictureId.Value);
            if (file != null)
            {
                note.SetCoverPicturePath(file.FilePath);
                note.SetCoverPictureKey(file.SecretKey);
            }
            var index = NoteCollection.IndexOf(note);
            NoteCollection.Remove(note);
            NoteCollection.Insert(index, note);
            return true;
        }

        public async Task<Guid> AddFile(string[]? fileNames, int type, Guid noteId)
        {
            if (fileNames == null || fileNames.Length < 1)
                return Guid.Empty;
            List<MarkNoteFileModel> models = await _FileService
                .AddNoteFileAsync(type, fileNames.ToList(), noteId);
            if (models.Any())
                return models[0].Id;
            return Guid.Empty;
        }



        private async void OnFamilyClick(NoteFamilyModel family)
        {
            family.OnClick();
            _CurrentFamily = family;
            if (_CurrentFolder == null)
                FolderId = Guid.Empty;
            else if (_CurrentFolder.FamilyId != family.Id)
            {
                _CurrentFolder = null;
                FolderId = Guid.Empty;
                await LoadNotes();
                ShowLeftMsg();
            }
        }

        private async void OnFolderClick(NoteFolder folder)
        {
            if (FolderId == folder.Id)
                return;
            FolderId = folder.Id;
            _CurrentFolder = folder;
            await LoadNotes();
            ShowLeftMsg();
        }

        private async void OnSearchNoteClick()
        {
            await LoadNotes();
        }

        private async void OnResetSearchClick()
        {
            InitialDefaultValue();
            LoadFamilys();
            await LoadNotes();
        }

        private void OnNoteSeeClick(Note note)
        {
            if (NoteReaderWindowAction == null)
                return;
            NoteReaderWindowAction.Invoke(note);
        }

        private void OnNoteMoveClick(Note note)
        {
            if (NoteMoveDialogAction == null)
                return;
            NoteMoveDialogAction.Invoke(note);
        }

        private void OnNoteEditClick(Note note)
        {
            _Mediator.SetShowNote(note);
            _Mediator.RouteRedirect(PageNameConsts.EditorPage);
        }

        private void OnNoteUploadCoverClick(Note note)
        {
            if (NoteUploadCoverDialogAction == null)
                return;
            string filter = "图像文件|*.jpg;*.jpeg;*.gif;*.png;";
            NoteUploadCoverDialogAction.Invoke(EditorOperateType.UploadCoverPicture, filter, note);
        }

        private async void OnNoteDeleteClick(Note note)
        {
            await _NoteService.DeleteAsync(note.Id);
            NoteCollection.Remove(note);
            _NoteTotal--;
            _FilterTotal--;
            ShowRightMsg();
        }



        private void InitialDefaultValue()
        {
            LeftMsg = string.Empty;
            RightMsg = string.Empty;
            FamilyId = Guid.Empty;
            FolderId = Guid.Empty;
            SearchSortColumn = NoteColumnConsts.UpdatedAt;
            SearchFolderName = string.Empty;
            SearchNoteTitle = string.Empty;
            SearchNoteKey = string.Empty;

            _NoteTotal = 0;
            _FilterTotal = 0;
            _NewNoteCount = 0;
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

            IEnumerable<NoteFamily> familys = new List<NoteFamily>();
            IEnumerable<NoteFolder> folders = new List<NoteFolder>();
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
                var family = new NoteFamilyModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    FoldersCollection = new ObservableCollection<NoteFolder>(),
                };
                var familyFolders = folders.Where(x => x.FamilyId == item.Id);
                family.AddFoldersRange(familyFolders);
                FamilyCollection.Add(family);
            }

            ShowLeftMsg();
        }

        private async Task<bool> LoadNotes()
        {
            _FilterTotal = 0;
            _NewNoteCount = 0;
            _NoteTotal = 0;
            NoteCollection.Clear();

            var result = await _NoteService.GetListAsync(FolderId, SearchSortColumn, SearchNoteTitle, SearchNoteKey);
            foreach (var item in result.Items)
            {
                if (item.CoverPictureId.HasValue)
                {
                    var file = await _FileService.GetByIdAsync(item.CoverPictureId.Value);
                    if (file != null)
                    {
                        item.SetCoverPicturePath(file.FilePath);
                        item.SetCoverPictureKey(file.SecretKey);
                    }
                }
                NoteCollection.Add(item);
            }

            _NoteTotal = result.Total;
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
            if (_NewNoteCount > 0)
                newMsg = $"新添加 {_NewNoteCount}  |  ";
            RightMsg = $"{newMsg}当前显示 - {NoteCollection.Count}   | 符合筛选条件 - {_FilterTotal}  |  总共 - {_NoteTotal}";
        }
    }


}
