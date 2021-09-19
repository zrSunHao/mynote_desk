using BlizzardWind.App.Common.Consts;
using BlizzardWind.App.Common.MarkText;
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
    public partial class NoteFamilyPageViewModel : MvxViewModel
    {
        private readonly IFolderService _FolderService;
        private readonly IFamilyService _FamilyService;
        private readonly INoteService _NoteService;
        private readonly IFileResourceService _FileService;

        public IMvxCommand SelectedFamilyCommand => new MvxCommand<NoteFamily>(OnSelectedClick);
        public IMvxCommand SearchCommand => new MvxCommand(OnSearchClick);
        public IMvxCommand EditFamilyCommand => new MvxCommand<NoteFamily>(OnEditFamilyClick);
        public IMvxCommand DeleteFamilyCommand => new MvxCommand<NoteFamily>(OnDeleteFamilyClick);
        public IMvxCommand CreateFolderCommand => new MvxCommand(OnCreateFolderClick);
        public IMvxCommand EdiFolderCommand => new MvxCommand<NoteFolder>(OnEditFolderClick);
        public IMvxCommand DeleteFolderCommand => new MvxCommand<NoteFolder>(OnDeleteFolderClick);
        public IMvxCommand FolderUploadCoverCommand => new MvxCommand<NoteFolder>(OnFolderUploadCoverClick);

        public ObservableCollection<NoteFamily> FamilyCollection { get; set; }
        public ObservableCollection<NoteFolder> FolderCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<string> ConfirmDialogAction { get; set; }
        public Action<NoteFamily> FamilyEditDialogAction { get; set; }
        public Action<int, string, NoteFolder> FolderUploadCoverDialogAction { get; set; }

        public Action<List<OptionIdItem>> FolderCreateDialogAction { get; set; }
        public Action<int, string, NoteFamily> FamilyDeleteDialogAction { get; set; }
        public Action<int, string, NoteFolder> FolderDeleteDialogAction { get; set; }
        public Action<NoteFolder, List<OptionIdItem>> FolderEditDialogAction { get; set; }

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
    }

    public partial class NoteFamilyPageViewModel
    {
        public NoteFamilyPageViewModel(IFolderService folderService,
            IFamilyService familyService,
            INoteService noteService,
            IFileResourceService fileService)
        {
            _FolderService = folderService;
            _FamilyService = familyService;
            _NoteService = noteService;
            _FileService = fileService;

            FamilyCollection = new ObservableCollection<NoteFamily>();
            FolderCollection = new ObservableCollection<NoteFolder>();
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
            var family = new NoteFamily()
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deleted = false
            };
            await _FamilyService.AddAsync(family);

            var list = new List<NoteFamily>();
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

        public async Task<bool> EditFamily(NoteFamily family)
        {
            await _FamilyService.UpdateAsync(family);

            var list = new List<NoteFamily>();
            foreach (var item in FamilyCollection)
            {
                if (item.Id == family.Id)
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

        public async Task<bool> DeleteFamily(NoteFamily family)
        {
            await _FamilyService.DeleteAsync(family.Id);

            FamilyCollection.Remove(family);
            if (family.Id == FamilyId)
            {
                if (FamilyCollection.Any())
                    await LoadFolderAsync(FamilyCollection[0].Id);
                else
                    FamilyId = Guid.Empty;
            }
            return true;
        }

        public async Task<bool> CreateFolder(Guid familyId, string name)
        {
            if (string.IsNullOrEmpty(name) || familyId == Guid.Empty)
                return false;
            var folder = new NoteFolder()
            {
                Id = Guid.NewGuid(),
                Name = name,
                FamilyId = familyId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deleted = false
            };
            await _FolderService.AddAsync(folder);

            if (familyId != FamilyId || (!string.IsNullOrEmpty(FolderName) && !name.Contains(FolderName)))
                return true;
            var list = new List<NoteFolder>();
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

        public async Task<bool> EditFolder(NoteFolder folder)
        {
            await _FolderService.UpdateAsync(folder);

            var list = new List<NoteFolder>();
            foreach (var item in FolderCollection)
            {
                if (item.FamilyId == FamilyId)
                    list.Add(item);
            }
            if (string.IsNullOrEmpty(FolderName))
                list = list.OrderBy(x => x.Name).ToList();
            else
                list = list.Where(x => x.Name.Contains(FolderName)).OrderBy(x => x.Name).ToList();
            FolderCollection.Clear();
            foreach (var item in list)
            {
                FolderCollection.Add(item);
            }
            return true;
        }

        public async Task<bool> DeleteFolder(NoteFolder folder)
        {
            await _FolderService.DeleteAsync(folder.Id);
            await LoadFolderAsync(FamilyId);
            return true;
        }

        public async Task<bool> FolderUploadCover(NoteFolder folder)
        {
            await _FolderService.UpdateAsync(folder);

            if (!folder.CoverPictureId.HasValue)
                return false;
            var index = FolderCollection.IndexOf(folder);
            FolderCollection.Remove(folder);
            FolderCollection.Insert(index, folder);
            return true;
        }

        public async Task<MarkNoteFileModel?> AddFile(string[]? fileNames, int type, Guid noteId)
        {
            if (fileNames == null || fileNames.Length < 1)
                return null;
            List<MarkNoteFileModel> models = await _FileService
                .AddNoteFileAsync(type, fileNames.ToList(), noteId);
            if (models.Any())
                return models[0];
            return null;
        }


        private void OnEditFamilyClick(NoteFamily family)
        {
            if (FamilyEditDialogAction != null)
                FamilyEditDialogAction.Invoke(family);
        }

        private async void OnDeleteFamilyClick(NoteFamily family)
        {
            var count = await _NoteService.GetFamilyCountAsync(family.Id);
            if (count <= 0)
            {
                await DeleteFamily(family);
                return;
            }
            var msg = $"【{family.Name}】大类下共有【{count}】篇文章，确定要删除吗？";
            if (FamilyDeleteDialogAction != null)
                FamilyDeleteDialogAction.Invoke(MesssageType.Warn, msg, family);
        }

        private void OnCreateFolderClick()
        {
            if (FolderCreateDialogAction == null)
                return;
            var options = this.GetFamilyOptions();
            FolderCreateDialogAction.Invoke(options);
        }

        private void OnEditFolderClick(NoteFolder folder)
        {
            if (FolderEditDialogAction == null)
                return;
            var options = this.GetFamilyOptions();
            FolderEditDialogAction.Invoke(folder, options);
        }

        private async void OnDeleteFolderClick(NoteFolder folder)
        {
            var count = await _NoteService.GetFolderCountAsync(folder.Id);
            if (count <= 0)
            {
                await DeleteFolder(folder);
                return;
            }
            var msg = $"【{folder.Name}】文件夹下共有【{count}】篇文章，确定要删除吗？";
            if (FolderDeleteDialogAction != null)
                FolderDeleteDialogAction.Invoke(MesssageType.Warn, msg, folder);
        }

        private async void OnSelectedClick(NoteFamily family)
        {
            await LoadFolderAsync(family.Id);
        }

        private async void OnSearchClick()
        {
            await LoadFolderAsync(FamilyId);
        }

        private void OnFolderUploadCoverClick(NoteFolder folder)
        {
            if (FolderUploadCoverDialogAction == null)
                return;
            string filter = "图像文件|*.jpg;*.jpeg;*.gif;*.png;";
            FolderUploadCoverDialogAction.Invoke(MarkResourceType.Cover, filter, folder);
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
            var list = await _FolderService.GetListAsync(familyId, FolderName);
            foreach (var folder in list)
            {
                if (folder.CoverPictureId.HasValue)
                {
                    var file = await _FileService.GetByIdAsync(folder.CoverPictureId.Value);
                    if (file != null)
                    {
                        folder.SetCoverPicturePath(file.FilePath);
                        folder.SetCoverPictureKey(file.SecretKey);
                    }
                }
                FolderCollection.Add(folder);
            }
            return true;
        }
    }
}
