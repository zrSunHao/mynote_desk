using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BlizzardWind.App.Common.MarkText
{
    /// <summary>
    /// 标记文本解析器
    /// </summary>
    public class MarkTextParser
    {
        //判断是否为【块起始行】的正则 即：#*]
        private readonly Regex _senseRg = new(@"\#.*?\]");
        //提取【块起始行】标识的正则
        private readonly Regex _tagRg = new(@"(?<=\#).*?(?=\])");
        //提取【块】名称的正则
        private readonly Regex _nameRg = new(@"(?<=(\<)).*?(?=(\>))", RegexOptions.Multiline | RegexOptions.Singleline);
        //提取【单行块】值的正则
        private readonly Regex _valueRg = new(@"(?<=(\()).*?(?=(\)))", RegexOptions.Multiline | RegexOptions.Singleline);

        /// <summary>
        /// 获取标记文本显示元素
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<MarkStandardBlock> GetMarkBlocks(string text)
        {
            // 1、切割（根据换行符）
            List<string> rows = TextSplit(text);
            // 2、识别（文本块）
            List<MarkMaterialBlock> materials = BlockDistinguish(rows);
            // 3、格式化（文本块）
            List<MarkStandardBlock> blocks = BlockFormat(materials);
            return blocks;
        }


        #region 文本解析

        /// <summary>
        /// 1、切割（根据换行符）
        /// </summary>
        /// <param name="text">带解析的文本</param>
        /// <returns></returns>
        public List<string> TextSplit(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();
            List<string> strs = text.Split("\n").ToList();

            List<string> rows = new List<string>();
            foreach (var str in strs)
            {
                string row = str.Trim();
                if (!string.IsNullOrEmpty(row))
                    rows.Add(row);
            }
            return rows;
        }

        /// <summary>
        /// 2、识别（文本块）
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<MarkMaterialBlock> BlockDistinguish(List<string> rows)
        {
            List<MarkMaterialBlock> blocks = new();
            if (rows == null || !rows.Any())
                return blocks;

            MarkNoteElementType? type = null;
            List<string> rowBuffer = new List<string>();
            foreach (var row in rows)
            {
                if (row == MarkNoteSymbolConsts.END) //为【块结束行】
                {
                    if (type != null && rowBuffer != null && rowBuffer.Any())
                        blocks.Add(new MarkMaterialBlock { Type = type.Value, Values = rowBuffer });
                    rowBuffer = new List<string>();
                    type = null;
                    continue;
                }
                if (_senseRg.IsMatch(row)) //为【块起始行】
                {
                    MarkMaterialBlock block = BlockFirstRowParse(row);
                    if (block == null || block.Type == MarkNoteElementType.other)
                        continue;
                    else if (block.IsComplete) //是【单行块】
                        blocks.Add(block);
                    else //是【多行块】
                        type = block.Type;
                    continue;
                }
                rowBuffer.Add(row);
            }

            return blocks;
        }

        /// <summary>
        /// 3、格式化（文本块）
        /// </summary>
        /// <param name="materials"></param>
        /// <returns></returns>
        public List<MarkStandardBlock> BlockFormat(List<MarkMaterialBlock> materials)
        {
            List<MarkStandardBlock> blocks = new();
            if (materials == null || !materials.Any())
                return blocks;

            foreach (var material in materials)
            {
                MarkStandardBlock? block = null;
                switch (material.Type)
                {
                    case MarkNoteElementType.p:
                    case MarkNoteElementType.h1:
                    case MarkNoteElementType.h2:
                    case MarkNoteElementType.h3:
                    case MarkNoteElementType.key:       // 单行文本元素转换
                        block = SingleTextFomat(material);
                        break;
                    case MarkNoteElementType.img:
                    case MarkNoteElementType.txt:
                    case MarkNoteElementType.link:      // 键值对文本元素转换
                        block = KeyValueTextFomat(material);
                        break;
                    case MarkNoteElementType.profile:
                    case MarkNoteElementType.summary:   // 多段落文本元素转换
                        block = MultiParagraphTextFomat(material);
                        break;
                    case MarkNoteElementType.list:
                    case MarkNoteElementType.quote:     // 列表文本元素转化
                        block = ListTextFomat(material);
                        break;
                    default:
                        block = null;
                        break;
                }
                if (block != null)
                    blocks.Add(block);
            }

            return blocks;
        }

        #endregion


        #region 文本解析私有方法

        /// <summary>
        /// 块起始行识别
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private MarkMaterialBlock BlockFirstRowParse(string row)
        {
            MatchCollection rs = _tagRg.Matches(row);
            string tag = rs[0].ToString();
            if (!rs.Any()) // 判断是否提取出【块起始行】标识
            {
                MarkMaterialBlock other = new() { Type = MarkNoteElementType.other };
                other.AddValue(row);
                return other;
            }

            MarkNoteElementType type = GetElementType(tag);
            var block = new MarkMaterialBlock { Type = type };
            bool isSingle = IsSingleRowElement(type);
            if (!isSingle) //多行块起始行没有其他信息
                return new MarkMaterialBlock { Type = type };

            // 移除标识
            string value = row.Replace(@$"#{tag}]", "").Trim();
            if (!string.IsNullOrEmpty(value))
            {
                block.AddValue(value);
                block.IsComplete = true;
            }
            return block;
        }

        /// <summary>
        /// 根据符号获取元素类型
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private static MarkNoteElementType GetElementType(string symbol)
        {
            MarkNoteElementType type = symbol switch
            {
                MarkNoteSymbolConsts.H1 => MarkNoteElementType.h1,
                MarkNoteSymbolConsts.H2 => MarkNoteElementType.h2,
                MarkNoteSymbolConsts.H3 => MarkNoteElementType.h3,
                MarkNoteSymbolConsts.KEY => MarkNoteElementType.key,
                MarkNoteSymbolConsts.PROFILE => MarkNoteElementType.profile,
                MarkNoteSymbolConsts.P => MarkNoteElementType.p,
                MarkNoteSymbolConsts.IMG => MarkNoteElementType.img,
                MarkNoteSymbolConsts.TXT => MarkNoteElementType.txt,
                MarkNoteSymbolConsts.LINK => MarkNoteElementType.link,
                MarkNoteSymbolConsts.LIST => MarkNoteElementType.list,
                MarkNoteSymbolConsts.SUMMARY => MarkNoteElementType.summary,
                MarkNoteSymbolConsts.QUOTE => MarkNoteElementType.quote,
                MarkNoteSymbolConsts.END => MarkNoteElementType.end,
                _ => MarkNoteElementType.other,
            };
            return type;
        }

        /// <summary>
        /// 判断是否为单行元素
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsSingleRowElement(MarkNoteElementType type)
        {
            bool isSingle;
            switch (type)
            {
                case MarkNoteElementType.profile:
                case MarkNoteElementType.list:
                case MarkNoteElementType.summary:
                case MarkNoteElementType.quote:
                case MarkNoteElementType.other:
                    isSingle = false;
                    break;
                default:
                    isSingle = true;
                    break;
            }
            return isSingle;
        }

        #endregion


        #region 文本元素转换

        /// <summary>
        /// 单行文本元素转换
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private MarkStandardBlock? SingleTextFomat(MarkMaterialBlock block)
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
        private MarkStandardBlock? KeyValueTextFomat(MarkMaterialBlock block)
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
        private MarkStandardBlock? MultiParagraphTextFomat(MarkMaterialBlock block)
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
        private MarkStandardBlock? ListTextFomat(MarkMaterialBlock block)
        {
            if (block == null || block.Values == null || !block.Values.Any())
                return null;

            return new MarkStandardBlock
            {
                Type = block.Type,
                List = block.Values,
            };
        }

        #endregion
    }
}
