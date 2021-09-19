using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using System;
using System.Collections.Generic;

namespace BlizzardWind.Desktop.Business
{
    public class ViewModelMediator
    {
        private Article _Note;

        public Action<Article, List<MarkElement>> NoteChangedAction { get; set; }
        public Action<string> RouteRedirectAction { get; set; }

        public void RouteRedirect(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                return;
            if (RouteRedirectAction != null)
                RouteRedirectAction(pageName);
        }

        public void NoteChangedNotify(Article note, List<MarkElement> elements)
        {
            if(NoteChangedAction != null)
            {
                NoteChangedAction(note, elements);
            }
        }

        public void SetShowArticle(Article note)
        {
            _Note = note;
        }

        public Article GetNote()
        {
            return _Note;
        }
    }
}
