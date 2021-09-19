using BlizzardWind.App.Common.MarkText;
using System.Collections.Generic;

namespace BlizzardWind.Desktop.Business.Models
{
    public class ArticleStructureModel
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public List<ArticleStructureModel> Children { get; set; }

        public void AddChildren(ArticleStructureModel model)
        {
            if(Children == null)
                Children = new List<ArticleStructureModel>();
            Children.Add(model);
        }
    }
}
