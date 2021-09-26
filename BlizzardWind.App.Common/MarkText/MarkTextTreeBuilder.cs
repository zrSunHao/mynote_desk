using System.Collections.Generic;
using System.Linq;

namespace BlizzardWind.App.Common.MarkText
{
    public class MarkTextTreeBuilder
    {
        public List<MarkTextNode> Build(List<MarkStandardBlock> blocks)
        {
            List<MarkTextNode> nodes = new List<MarkTextNode>();
            if(blocks == null || !blocks.Any())
                return nodes;
            MarkTextNode? topComponent = null;
            bool hasTitle_2 = false;
            foreach (var el in blocks)
            {
                if (el.Level == MarkElementLevel.Skip)
                {
                    hasTitle_2 = false;
                    continue;
                }
                MarkTextNode component = new()
                {
                    Name = el.BriefText,
                    Type = el.Type,
                    TypeName = el.TypeName,
                    Level = el.Level,
                    Children = new List<MarkTextNode>()
                };
                if (component.Level == MarkElementLevel.Title_1)
                {
                    if (topComponent != null)
                        nodes.Add(topComponent);
                    topComponent = component;
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkElementLevel.Single)
                {
                    if (topComponent != null)
                        nodes.Add(topComponent);
                    topComponent = null;
                    nodes.Add(component);
                    hasTitle_2 = false;
                    continue;
                }
                if (component.Level == MarkElementLevel.Title_2)
                {
                    if (topComponent == null)
                        topComponent = component;
                    else
                        topComponent.AddChildren(component);
                    hasTitle_2 = true;
                    continue;
                }
                if (component.Level == MarkElementLevel.Leaf)
                {
                    if (topComponent == null)
                        nodes.Add(component);
                    else if (!hasTitle_2)
                        topComponent.AddChildren(component);
                    else if (!topComponent.Children.Any())
                        topComponent.AddChildren(component);
                    else
                    {
                        int index = topComponent.Children.Count - 1;
                        topComponent.Children[index].AddChildren(component);
                    }
                }
            }
            if (topComponent != null)
            {
                nodes.Add(topComponent);
            }

            return nodes;
        }
    }
}
