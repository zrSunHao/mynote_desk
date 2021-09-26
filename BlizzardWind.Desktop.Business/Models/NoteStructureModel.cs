﻿using BlizzardWind.App.Common.MarkText;
using System.Collections.Generic;

namespace BlizzardWind.Desktop.Business.Models
{
    public class NoteStructureModel
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public MarkNoteElementType Type { get; set; }

        public string TypeName { get; set; }

        public List<NoteStructureModel> Children { get; set; }

        public void NoteFamilyModel()
        {
            Children = new List<NoteStructureModel>();
        }

        public void AddChildren(NoteStructureModel model)
        {
            if(Children == null)
                Children = new List<NoteStructureModel>();
            Children.Add(model);
        }
    }
}
