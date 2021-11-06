using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BlizzardWind.App.Common.MarkText
{
    /// <summary>
    /// 文本元素转换
    /// </summary>
    public class MarkBlockConverter
    {
        //提取【块】名称的正则
        private readonly Regex _nameRg = new(@"(?<=(\<)).*?(?=(\>))", RegexOptions.Multiline | RegexOptions.Singleline);
        //提取【单行块】值的正则
        private readonly Regex _valueRg = new(@"(?<=(\()).*?(?=(\)))", RegexOptions.Multiline | RegexOptions.Singleline);

        /// <summary>
        /// 单行文本元素转换
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public MarkStandardBlock? SingleTextFomat(MarkMaterialBlock block)
        {
            if (block == null || block.Values == null || !block.Values.Any())
                return null;

            string text = block.Values[0];
            if (block.Type == MarkNoteElementType.p) // 段落添加缩进符
                text = $"\u3000\u3000{text}";
            return new MarkStandardBlock
            {
                Type = block.Type,
                Text = text,
            };
        }

        /// <summary>
        /// 键值对文本元素转换
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public MarkStandardBlock? KeyValueTextFomat(MarkMaterialBlock block)
        {
            if (block == null || block.Values == null || !block.Values.Any())
                return null;
            string text = block.Values[0];
            var map = new MarkKeyValue
            {
                Name = _nameRg.Match(text).Value,
                Value = _valueRg.Match(text).Value
            };
            List<string> ornaments = ParseOrnaments(text);
            return new MarkStandardBlock
            {
                Type = block.Type,
                Text = map.Name,
                Map = map,
                Ornaments = ornaments.Any() ? string.Join(";", ornaments): ""
            };
        }

        /// <summary>
        /// 多段落文本元素转换
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public MarkStandardBlock? MultiParagraphTextFomat(MarkMaterialBlock block)
        {
            if (block == null || block.Values == null || !block.Values.Any())
                return null;
            var builder = new StringBuilder();
            foreach (var item in block.Values)
                builder.Append($"\u3000\u3000{item}\u000A");
            return new MarkStandardBlock
            {
                Type = block.Type,
                Text = builder.ToString().TrimEnd(),
            };
        }

        /// <summary>
        /// 列表文本元素转化
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public MarkStandardBlock? ListTextFomat(MarkMaterialBlock block)
        {
            if (block == null || block.Values == null || !block.Values.Any())
                return null;

            return new MarkStandardBlock
            {
                Type = block.Type,
                List = block.Values,
            };
        }

        private List<string> ParseOrnaments(string text)
        {
            List<string> ornaments = new List<string>();

            if (text.Contains(MarkOrnamentConsts.HorizontalCenter))
                ornaments.Add(MarkOrnamentConsts.HorizontalCenter);
            if (text.Contains(MarkOrnamentConsts.HorizontalRight))
                ornaments.Add(MarkOrnamentConsts.HorizontalRight);
            if (text.Contains(MarkOrnamentConsts.HorizontalStretch))
                ornaments.Add(MarkOrnamentConsts.HorizontalStretch);
            else
                ornaments.Add(MarkOrnamentConsts.HorizontalLeft);

            if (text.Contains(MarkOrnamentConsts.ImgUniform))
                ornaments.Add(MarkOrnamentConsts.ImgUniform);
            else
                ornaments.Add(MarkOrnamentConsts.ImgNone);

            if (text.Contains(MarkOrnamentConsts.SmallWidth))
                ornaments.Add(MarkOrnamentConsts.SmallWidth);
            else if (text.Contains(MarkOrnamentConsts.MediumWidth))
                ornaments.Add(MarkOrnamentConsts.MediumWidth);
            else if (text.Contains(MarkOrnamentConsts.LargeWidth))
                ornaments.Add(MarkOrnamentConsts.LargeWidth);
            else
                ornaments.Add(MarkOrnamentConsts.ExtraWidth);

            return ornaments;
        }
    }
}
