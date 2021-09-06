using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;

namespace BlizzardWind.Desktop.Business
{
    public class ViewModelMediator
    {
        private Article _Article;

        public Action<Article, List<MarkElement>> ArticleChangedAction { get; set; }
        public Action<string> RouteRedirectAction { get; set; }

        public void RouteRedirect(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                return;
            if (RouteRedirectAction != null)
                RouteRedirectAction(pageName);
        }

        public void ArticleChangedNotify(Article article, List<MarkElement> elements)
        {
            if(ArticleChangedAction!= null)
            {
                ArticleChangedAction(article, elements);
            }
        }

        public void SetShowArticle(Article article)
        {
            _Article = article;
        }

        public Article GetShowArticle()
        {
            return _Article;
        }
    }
}
