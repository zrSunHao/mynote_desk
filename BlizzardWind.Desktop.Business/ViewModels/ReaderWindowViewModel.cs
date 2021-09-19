﻿using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class ReaderWindowViewModel : MvxViewModel
    {
        private readonly ViewModelMediator _Mediator;
        private readonly IFileResourceService _FileResourceService;
        private Article _Note;

        public IMvxCommand LinkClickCommand => new MvxCommand<MarkElement>(OnLinkClick);

        public ObservableCollection<NoteStructureModel> HeadlineCollection { get; set; }
        public ObservableCollection<MarkElement> ElementCollection { get; set; }

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
            _Mediator.NoteChangedAction += NoteChanged;
        }

        public void OnWindowLoaded(Article note)
        {
            _Note = note;
            var parser = new MarkTextParser();
            List<MarkElement> elements = parser.GetMarkElements(_Note.Content);
            NoteUpdate(_Note, elements);
        }

        public Article GetCurrentNote()
        {
            return _Note;
        }

        private void OnLinkClick(MarkElement element)
        {
            if (LinkClickAction != null && element.KeyValue?.Value != null)
                LinkClickAction.Invoke(element.KeyValue.Value);
        }

        private void NoteChanged(Article note, List<MarkElement> elements)
        {
            if (_Note != null && _Note.Id != note.Id)
                return;
            NoteUpdate(note, elements);
        }

        private async void NoteUpdate(Article note, List<MarkElement> elements)
        {
            if (note == null || elements == null)
                return;
            ElementCollection.Clear();
            foreach (var item in elements)
            {
                if (item.Type == MarkType.txt || item.Type == MarkType.img)
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
    }
}
