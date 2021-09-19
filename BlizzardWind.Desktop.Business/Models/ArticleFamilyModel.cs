using BlizzardWind.Desktop.Business.Entities;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.Models
{
    public class ArticleFamilyModel : MvxViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string DisplayName => GetDisplayName();

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }


        public ObservableCollection<ArticleFolder> FoldersCollection { get; set; }

        public void AddFoldersRange(IEnumerable<ArticleFolder> folders)
        {
            foreach (var item in folders)
            {
                FoldersCollection.Add(item);
            }
        }

        public void OnClick()
        {
            IsExpanded = !IsExpanded;
        }

        private string GetDisplayName()
        {
            int count = FoldersCollection == null ? 0 : FoldersCollection.Count;
            return $"{Name} - [ {count} ]";
        }
    }

    public class ArticleFamilyAndFolderModel
    {
        public List<ArticleFamily> Familys { get; set; }

        public List<ArticleFolder> Folders { get; set; }
    }
}
