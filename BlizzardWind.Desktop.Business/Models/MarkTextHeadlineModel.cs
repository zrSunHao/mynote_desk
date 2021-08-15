using System.Collections.Generic;

namespace BlizzardWind.Desktop.Business.Models
{
    public class MarkTextHeadlineModel
    {
        public string Name { get; set; }

        public List<MarkTextHeadlineModel> Children { get; set; } = new List<MarkTextHeadlineModel>();
    }
}
