using System.Collections.Generic;
using System.Linq;

namespace BlizzardWind.App.Common.MarkText
{
    /// <summary>
    /// 标记文本的树形结构
    /// 共三层：H2/Profile/Key/Summary、H3、Leaf
    /// </summary>
    public class MarkTextTreeBuilder
    {
        private readonly List<MarkTextNode> _Nodes = new List<MarkTextNode>();
        // 最高层次结点为H2级别
        private MarkTextNode? _TopComponent = null;
        // 最近一个元素为H3
        private bool _LastIsH3 = false;

        public List<MarkTextNode> Build(List<MarkStandardBlock> blocks)
        {
            Initial();
            if (blocks == null || !blocks.Any()) return _Nodes;

            foreach (var block in blocks)
            {
                MarkTextNode component = GenerateNode(block);
                switch (block.Level)
                {
                    case MarkElementLevel.Skip:     // 高层次或无关结点，跳过
                        _LastIsH3 = false;
                        continue;
                    case MarkElementLevel.Single:   // 高层次无子元素结点,直接添加
                        SingleNode(component);
                        _LastIsH3 = false;
                        continue;
                    case MarkElementLevel.H2:       // 一级标题H2
                        H2Node(component);
                        _LastIsH3 = false;
                        continue;
                    case MarkElementLevel.H3:       // 二级标题H3
                        H3Node(component);
                        _LastIsH3 = true;
                        continue;
                    case MarkElementLevel.Leaf:     // 叶子结点
                        LeafNode(component);
                        continue;
                }
            }
            if (_TopComponent != null) _Nodes.Add(_TopComponent);
            return _Nodes;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initial()
        {
            _Nodes.Clear();
            _TopComponent = null;
            _LastIsH3 = false;
        }

        /// <summary>
        /// 生成结点
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private MarkTextNode GenerateNode(MarkStandardBlock block)
        {
            return new()
            {
                Name = block.BriefText,
                Type = block.Type,
                TypeName = block.TypeName,
                Level = block.Level,
                Children = new List<MarkTextNode>()
            };
        }

        /// <summary>
        /// 高层次无子元素结点,直接添加
        /// </summary>
        /// <param name="component"></param>
        private void SingleNode(MarkTextNode component)
        {
            if (_TopComponent != null) // 若已存在顶部结点，先添加，在处理当前节点
                _Nodes.Add(_TopComponent);
            _TopComponent = null;
            _Nodes.Add(component);
        }

        /// <summary>
        /// 一级标题H2
        /// </summary>
        /// <param name="component"></param>
        private void H2Node(MarkTextNode component)
        {
            if (_TopComponent != null) // 若已存在顶部结点，先添加，在处理当前节点
                _Nodes.Add(_TopComponent);
            _TopComponent = component;
        }

        /// <summary>
        /// 二级标题
        /// </summary>
        /// <param name="component"></param>
        private void H3Node(MarkTextNode component)
        {
            if (_TopComponent == null)
                _TopComponent = component;
            else
                _TopComponent.AddChildren(component);
        }

        /// <summary>
        /// 叶子结点
        /// </summary>
        /// <param name="component"></param>
        private void LeafNode(MarkTextNode component)
        {
            if (_TopComponent == null)
                _Nodes.Add(component);
            else if (!_LastIsH3)
                _TopComponent.AddChildren(component);
            else if (!_TopComponent.Children.Any())
                _TopComponent.AddChildren(component);
            else
            {
                int index = _TopComponent.Children.Count - 1;
                _TopComponent.Children[index].AddChildren(component);
            }
        }
    }
}
