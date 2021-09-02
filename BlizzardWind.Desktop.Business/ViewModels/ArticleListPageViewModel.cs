﻿using BlizzardWind.Desktop.Business.Interfaces;
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
        private readonly IArticleService _articleService;

        public ObservableCollection<int> ArticleCollection { get; set; }
    }

    public partial class ArticleListPageViewModel
    {
        public ArticleListPageViewModel(IArticleService articleService)
        {
            _articleService = articleService;
            ArticleCollection = new ObservableCollection<int>() {1,2,3,4,5,6,7,8,9,10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, };
        }
    }
}