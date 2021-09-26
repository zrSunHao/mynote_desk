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

            return new MarkStandardBlock
            {
                Type = block.Type,
                Text = map.Name,
                Map = map,
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
    }
}
