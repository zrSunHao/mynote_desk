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

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class ReaderWindowViewModel : MvxViewModel
    {
        private readonly ViewModelMediator _Mediator;
        private readonly IFileResourceService _FileResourceService;
        private Note _Note;

        public IMvxCommand LinkClickCommand => new MvxCommand<MarkElement>(OnLinkClick);

        public ObservableCollection<MarkElement> ElementCollection { get; set; }
        public ObservableCollection<NoteStructureModel> NoteStructureCollection { get; set; }

        public Action<int, string> PromptInformationAction { get; set; }
        public Action<string> LinkClickAction { get; set; }
    }

    public partial class ReaderWindowViewModel
    {
        public ReaderWindowViewModel(ViewModelMediator mediator, IFileResourceService fileResourceService)
        {
            _Mediator = mediator;
            _FileResourceService = fileResourceService;
            ElementCollection = new ObservableCollection<MarkElement>();
            NoteStructureCollection = new ObservableCollection<NoteStructureModel>();
            _Mediator.NoteChangedAction += NoteChanged;
        }

        public void OnWindowLoaded(Note note)
        {
            _Note = note;
            var parser = new MarkTextParser();
            List<MarkElement> elements = parser.GetMarkElements(_Note.Content);
            NoteUpdate(_Note, elements);
        }

        public Note GetCurrentNote()
        {
            return _Note;
        }

        private void OnLinkClick(MarkElement element)
        {
            if (LinkClickAction != null && element.KeyValue?.Value != null)
                LinkClickAction.Invoke(element.KeyValue.Value);
        }

        private void NoteChanged(Note note, List<MarkElement> elements)
        {
            if (_Note != null && _Note.Id != note.Id)
                return;
            NoteUpdate(note, elements);
        }

        private async void NoteUpdate(Note note, List<MarkElement> elements)
        {
            if (note == null || elements == null)
                return;
            NoteStructureUpdate(elements);
            ElementCollection.Clear();
            foreach (var item in elements)
            {
                if (item.Type == MarkNoteElementType.txt || item.Type == MarkNoteElementType.img)
                {
                    item.Content = item.KeyValue?.Name;
                    Guid fileId = Guid.Empty;
                    try
                    {
                        string idStr = item.KeyValue?.Value == null ? "" : item.KeyValue.Value;
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
                ElementCollection.Add(item);
            }
        }

        private void NoteStructureUpdate(List<MarkElement> elements)
        {
            NoteStructureCollection.Clear();
            NoteStructureModel? topComponent = null;
            bool hasTitle_2 = false;
            foreach (var el in elements)
            {
                if (el.Level == MarkTypeLevel.Skip)
                {
                    hasTitle_2 = false;
                    continue;
                }
                NoteStructureModel component = new()
                {
                    Name = el.ShortContent,
                    Type = el.RowType,
                    TypeName = el.TypeName,
                    Level = el.Level,
                };
                if (component.Level == MarkTypeLevel.Title_1)
                {
                    if (topComponent != null)
                        NoteStructureCollection.Add(topComponent);
                    topComponent = component;
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkTypeLevel.Single)
                {
                    if (topComponent != null)
                        NoteStructureCollection.Add(topComponent);
                    topComponent = null;
                    NoteStructureCollection.Add(component);
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkTypeLevel.Title_2)
                {
                    if (topComponent == null)
                        topComponent = component;
                    else
                        topComponent.AddChildren(component);
                    hasTitle_2 = true;
                    continue;
                }
                if (component.Level == MarkTypeLevel.Leaf)
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
