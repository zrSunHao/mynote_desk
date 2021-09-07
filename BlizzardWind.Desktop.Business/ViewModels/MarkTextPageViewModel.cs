using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

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
            var parser = new MarkTextParser();
            List<MarkElement> elements = parser.GetMarkElements(_Article.Content);
            ArticleUpdate(_Article, elements);
        }

        private void ArticleChanged(Article article, List<MarkElement> elements)
        {
            if (_Article != null && _Article.Id != article.Id)
                return;
            ArticleUpdate(article, elements);
        }

        private async void ArticleUpdate(Article article, List<MarkElement> elements)
        {
            if (article == null || elements == null)
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
