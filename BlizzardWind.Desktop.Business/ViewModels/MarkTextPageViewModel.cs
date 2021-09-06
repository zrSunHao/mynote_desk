using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using Microsoft.VisualBasic;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class MarkTextPageViewModel : MvxViewModel
    {
        private readonly ViewModelMediator _Mediator;
        private readonly IFileResourceService _FileResourceService;
        private Article _Article;

        public ObservableCollection<ArticleStructureModel> HeadlineCollection { get; set; }
        public ObservableCollection<MarkElement> ElementCollection { get; set; }
    }

    public partial class MarkTextPageViewModel
    {
        public MarkTextPageViewModel(ViewModelMediator mediator, IFileResourceService fileResourceService)
        {
            _Mediator = mediator;
            _FileResourceService = fileResourceService;
            ElementCollection = new ObservableCollection<MarkElement>();

            _Article = _Mediator.GetShowArticle();
            _Mediator.ArticleChangedAction += ArticleChanged;
        }

        public void OnPageLoaded()
        {
            ArticleChanged(_Article);
        }

        public async void ArticleChanged(Article article)
        {
            if (article == null)
                return;
            _Article = article;
            var parser = new MarkTextParser();
            var list = parser.GetMarkElements(_Article.Content);
            ElementCollection.Clear();
            foreach (var item in list)
            {
                if(item.Type == MarkType.txt || item.Type == MarkType.img)
                {
                    item.Content = item.KeyValue?.Name;
                    Guid fileId = Guid.Empty;
                    try
                    {
                        string idStr = item.KeyValue?.Value == null ? "" : item.KeyValue.Value;
                        fileId = Guid.Parse(idStr);
                    }
                    catch (Exception){}
                    if (fileId != Guid.Empty)
                    {
                        var filePath = await _FileResourceService.GetPathByIdAsync(fileId);
                        item.SetIFilePath(filePath);
                    }
                }
                ElementCollection.Add(item);
            }
        }
    }
}
