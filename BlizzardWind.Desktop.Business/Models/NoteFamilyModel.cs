using BlizzardWind.Desktop.Business.Entities;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.Models
{
    public class NoteFamilyModel : MvxViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsFamily => true;

        public string DisplayName => GetDisplayName();

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }


        public ObservableCollection<NoteFolder> Children { get; set; }

        public void AddFoldersRange(IEnumerable<NoteFolder> folders)
        {
            foreach (var item in folders)
            {
                Children.Add(item);
            }
        }

        public void OnClick()
        {
            IsExpanded = !IsExpanded;
        }

        private string GetDisplayName()
        {
            int count = Children == null ? 0 : Children.Count;
            return $"{Name} - [ {count} ]";
        }
    }

    public class NoteFamilyAndFolderModel
    {
        public List<NoteFamily> Familys { get; set; }

        public List<NoteFolder> Folders { get; set; }
    }
}
