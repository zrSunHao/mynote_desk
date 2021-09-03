using BlizzardWind.Desktop.Business.Models;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class ArticleFamilyPageViewModel : MvxViewModel
    {
        public ObservableCollection<ArticleFamilyModel> FamilyCollection { get; set; }
        public ObservableCollection<int> FolderCollection { get; set; }
    }

    public partial class ArticleFamilyPageViewModel
    {
        public ArticleFamilyPageViewModel()
        {
            FamilyCollection = new ObservableCollection<ArticleFamilyModel>()
            {
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
                new ArticleFamilyModel() { Folders = new List<int>() { 1, 2, 3, 4, 5, } },
            };
            FolderCollection = new ObservableCollection<int>()
            {
                1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,
            };
        }
    }
}
