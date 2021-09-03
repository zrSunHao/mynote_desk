using BlizzardWind.Desktop.Business.Entities;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.Models
{
    public class ArticleFamilyModel : ArticleFamily
    {
        public bool? IsDeleted {  get; set; }

        public int FolderCount => FoldersCollection == null ? 0 : FoldersCollection.Count;

        public ObservableCollection<ArticleFolder> FoldersCollection { get; set; }
    }
}
