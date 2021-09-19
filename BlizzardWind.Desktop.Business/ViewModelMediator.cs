using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Entities;
using System;
using System.Collections.Generic;

namespace BlizzardWind.Desktop.Business
{
    public class ViewModelMediator
    {
        private Note _Note;

        public Action<Note, List<MarkElement>> NoteChangedAction { get; set; }
        public Action<string> RouteRedirectAction { get; set; }

        public void RouteRedirect(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                return;
            if (RouteRedirectAction != null)
                RouteRedirectAction(pageName);
        }

        public void NoteChangedNotify(Note note, List<MarkElement> elements)
        {
            if(NoteChangedAction != null)
            {
                NoteChangedAction(note, elements);
            }
        }

        public void SetShowNote(Note note)
        {
            _Note = note;
        }

        public Note GetNote()
        {
            return _Note;
        }
    }
}
