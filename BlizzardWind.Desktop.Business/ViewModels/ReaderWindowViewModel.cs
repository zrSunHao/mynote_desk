using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Consts;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Helpers;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class ReaderWindowViewModel : MvxViewModel
    {
        private readonly ViewModelMediator _Mediator;
        private readonly IFileResourceService _FileResourceService;
        private Note _Note;

        public IMvxCommand LinkClickCommand => new MvxCommand<MarkStandardBlock>(OnLinkClick);
        public IMvxCommand OperateCommand => new MvxCommand<int>(OnOperateClick);

        public ObservableCollection<MarkStandardBlock> BlocksCollection { get; set; }
        public ObservableCollection<NoteStructureModel> NoteStructureCollection { get; set; }
        public ObservableCollection<OperateModel> OperateCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<string> LinkClickAction { get; set; }
        public Action<int> OperateAction { get; set; }
    }

    public partial class ReaderWindowViewModel
    {
        public ReaderWindowViewModel(ViewModelMediator mediator, IFileResourceService fileResourceService)
        {
            _Mediator = mediator;
            _FileResourceService = fileResourceService;
            BlocksCollection = new ObservableCollection<MarkStandardBlock>();
            NoteStructureCollection = new ObservableCollection<NoteStructureModel>();
            OperateCollection = OperateHelper.GetReaderOperate();
            _Mediator.NoteChangedAction += NoteChanged;
        }

        public void OnWindowLoaded(Note note)
        {
            _Note = note;
            var parser = new MarkTextParser();
            List<MarkStandardBlock> blocks = parser.GetMarkBlocks(_Note.Content);
            NoteUpdate(_Note, blocks);
        }

        public Note GetCurrentNote()
        {
            return _Note;
        }

        private void OnOperateClick(int type)
        {
            if (OperateAction != null)
            {
                OperateAction.Invoke(type);
            }
        }

        private void OnLinkClick(MarkStandardBlock block)
        {
            if (LinkClickAction != null && block.Map?.Value != null)
                LinkClickAction.Invoke(block.Map.Value);
        }

        private void NoteChanged(Note note, List<MarkStandardBlock> blocks)
        {
            if (_Note != null && _Note.Id != note.Id)
                return;
            NoteUpdate(note, blocks);
        }

        private async void NoteUpdate(Note note, List<MarkStandardBlock> blocks)
        {
            if (note == null || blocks == null)
                return;
            NoteStructureUpdate(blocks);
            BlocksCollection.Clear();
            foreach (var item in blocks)
            {
                if (item.Type == MarkNoteElementType.txt || item.Type == MarkNoteElementType.img)
                {
                    item.Text = item.Map?.Name;
                    Guid fileId = Guid.Empty;
                    try
                    {
                        string idStr = item.Map?.Value == null ? "" : item.Map.Value;
                        fileId = Guid.Parse(idStr);
                    }
                    catch (Exception) { }
                    if (fileId != Guid.Empty)
                    {
                        var file = await _FileResourceService.GetByIdAsync(fileId);
                        if (file != null)
                        {
                            item.SetFilePath(file.FilePath);
                            item.SetFileKey(file.SecretKey);
                        }
                    }
                }
                BlocksCollection.Add(item);
            }
        }

        private void NoteStructureUpdate(List<MarkStandardBlock> blocks)
        {
            NoteStructureCollection.Clear();
            NoteStructureModel? topComponent = null;
            bool hasTitle_2 = false;
            foreach (var el in blocks)
            {
                if (el.Level == MarkElementLevel.Skip)
                {
                    hasTitle_2 = false;
                    continue;
                }
                NoteStructureModel component = new()
                {
                    Name = el.BriefText,
                    Type = el.Type,
                    TypeName = el.TypeName,
                    Level = el.Level,
                    Children = new List<NoteStructureModel>()
                };
                if (component.Level == MarkElementLevel.Title_1)
                {
                    if (topComponent != null)
                        NoteStructureCollection.Add(topComponent);
                    topComponent = component;
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkElementLevel.Single)
                {
                    if (topComponent != null)
                        NoteStructureCollection.Add(topComponent);
                    topComponent = null;
                    NoteStructureCollection.Add(component);
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkElementLevel.Title_2)
                {
                    if (topComponent == null)
                        topComponent = component;
                    else
                        topComponent.AddChildren(component);
                    hasTitle_2 = true;
                    continue;
                }
                if (component.Level == MarkElementLevel.Leaf)
                {
                    if (topComponent == null)
                        NoteStructureCollection.Add(component);
                    else if (!hasTitle_2)
                        topComponent.AddChildren(component);
                    else if (!topComponent.Children.Any())
                        topComponent.AddChildren(component);
                    else
                    {
                        int index = topComponent.Children.Count - 1;
                        topComponent.Children[index].AddChildren(component);
                    }
                }
            }
            if (topComponent != null)
            {
                NoteStructureCollection.Add(topComponent);
            }
        }
    }
}
