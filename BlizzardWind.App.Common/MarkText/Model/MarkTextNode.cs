using System.Collections.Generic;

namespace BlizzardWind.App.Common.MarkText
{
    public class MarkTextNode
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public MarkNoteElementType Type { get; set; }

        public string TypeName { get; set; }

        public List<MarkTextNode> Children { get; set; }



        public void NoteFamilyModel()
        {
            Children = new List<MarkTextNode>();
        }

        public void AddChildren(MarkTextNode model)
        {
            if (Children == null)
                Children = new List<MarkTextNode>();
            Children.Add(model);
        }
    }
}
